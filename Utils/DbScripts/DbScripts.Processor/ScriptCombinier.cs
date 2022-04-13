using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DbScripts.Processor
{
    public class ScriptCombinier
    {
        private readonly ILogger _logger;

        public ScriptCombinier(ILogger logger)
        {
            _logger = logger;
        }

        public void Combine(string scriptsTargetDirectory)
        {
            var files = Directory.GetFiles(scriptsTargetDirectory);

            foreach (var file in files)
            try
            {
                MoveFile(scriptsTargetDirectory, file, "(\\w*).Constraint.sql", ".Table.sql");
                MoveFile(scriptsTargetDirectory, file, "(\\w*).AlterRole.sql", ".DatabaseRole.sql");
                MoveFile(scriptsTargetDirectory, file, ".Index.sql", ".Table.sql", ExtractTableNameFromIndexScript);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void MoveFile(string scriptsTargetDirectory, string file, string sourceScriptFileNamePattern, string targetScriptFileNamePattern, Func<string, string> getObjectName = null)
        {
            var match = Regex.Match(file, sourceScriptFileNamePattern);

            if (match.Success)
            {
                var objectName = getObjectName == null
                    ? match.Groups[1].Value
                    : getObjectName(file);

                var targetFileName = Path.Combine(scriptsTargetDirectory, objectName + targetScriptFileNamePattern);

                if (!File.Exists(targetFileName))
                {
                    throw new FileNotFoundException($"Cannot find script \"{targetFileName}\" to combain with \"{file}\"");
                }

                using (var outputStream = File.OpenWrite(targetFileName))
                using (var inputStream = File.OpenRead(file))
                {
                    outputStream.Position = outputStream.Length - 1;
                    inputStream.CopyTo(outputStream);
                }
                File.Delete(file);
                _logger.Info($"The script {file} has been added to {targetFileName}");
            }
        }

        private string ExtractTableNameFromIndexScript(string file)
        {
            var fileText = File.ReadAllText(file);

            var match = Regex.Match(fileText, @"CREATE.*ON\s\[.*].\[(\w*)]");

            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                throw new Exception($"Cannot extract table name for index {Path.GetFileName(file)}");
            }
        }
    }
}
