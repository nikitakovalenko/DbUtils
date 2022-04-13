using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DbScripts.Processor
{
    public class ScriptSeparator
    {
        private readonly ILogger _logger;

        public ScriptSeparator(ILogger logger)
        {
            _logger = logger;
        }

        public void ExtractSeparateScripts(string scriptsTargetDirectory, string fileName)
        {
            try
            {
                ExtractSeparateScriptsInternal(scriptsTargetDirectory, fileName);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void ExtractSeparateScriptsInternal(string scriptsTargetDirectory, string fileName)
        {
            var scrpitStrings = new StringBuilder();
            var scriptFileName = string.Empty;

            using var fs = new FileStream(path: fileName, mode: FileMode.Open, access: FileAccess.Read);
            using var reader = new StreamReader(fs);
            while (!reader.EndOfStream)
            {
                var curStr = reader.ReadLine();

                if (IsFirstScriptRow(curStr))
                {
                    var newScriptFileName = GenerateScriptFileName(scriptsTargetDirectory, GetObjectTypeAndName(curStr));

                    if (!scriptFileName.Equals(newScriptFileName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(scriptFileName))
                        {
                            GenerateScriptFile(scrpitStrings, scriptFileName);
                            scrpitStrings.Clear();
                        }

                        scriptFileName = newScriptFileName;
                    }
                }

                if (!string.IsNullOrEmpty(scriptFileName))
                {
                    scrpitStrings.AppendLine(curStr);
                }
            }
            GenerateScriptFile(scrpitStrings, scriptFileName);
        }

        private bool IsFirstScriptRow(string source)
        {
            return Regex.IsMatch(source, @"\/[*]{6} Object.*[*]{6}\/|^ALTER TABLE|^ALTER ROLE|EXEC sys.sp_addextendedproperty");
        }

        private Tuple<string, string> GetObjectTypeAndName(string source)
        {
            if (Regex.IsMatch(source, @"^ALTER TABLE"))
            {
                return new Tuple<string, string>("Constraint", Regex.Match(source, @"]\.\[(\w*)]").Groups[1].Value);
            }
            if (Regex.IsMatch(source, @"^ALTER ROLE"))
            {
                return new Tuple<string, string>("AlterRole", Regex.Match(source, @"\[(\w*)]").Groups[1].Value);
            }
            else if (Regex.IsMatch(source, @"EXEC sys.sp_addextendedproperty"))
            {
                return new Tuple<string, string>("ExtendedProperties", "ExtendedProperties");
            }
            else if (Regex.IsMatch(source, @"\/[*]{6} Object.*[*]{6}\/"))
            {
                var match = Regex.Match(source, @"Object:\W*(\w*)\W*\S*]\.\[(\w*)]");
                if (match.Groups.Count > 1)
                {
                    return new Tuple<string, string>(match.Groups[1].Value, match.Groups[2].Value);
                }
                else
                {
                    match = Regex.Match(source, @"Object:\W*(\w*)\W*\[(\S*)]");
                    return new Tuple<string, string>(match.Groups[1].Value, match.Groups[2].Value.Replace('\\', '_'));
                }
            }

            throw new UnknowFirstScripLineException(source);
            
        }

        private string GenerateScriptFileName(string scriptsTargetDirectory, Tuple<string, string> objectTypeAndName)
        {
            return Path.Combine(scriptsTargetDirectory, $"{ objectTypeAndName.Item2}.{objectTypeAndName.Item1}.sql");
        }

        private void GenerateScriptFile(StringBuilder scriptStrings, string scriptFileName)
        {
            using (var stream = File.OpenWrite(scriptFileName))
            {
                stream.Position = stream.Length > 0 ? stream.Length - 1 : 0;

                using var writter = new StreamWriter(stream, Encoding.UTF8);
                writter.WriteLine(scriptStrings.ToString());
                writter.WriteLine();
            }

            _logger.Info($"Generate new script {Path.GetFileName(scriptFileName)}");
        }
    }
}
