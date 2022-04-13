using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DbScripts.Processor
{
    public class FileProcessor
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public FileProcessor(
            IConfiguration configuration,
            ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void Process(string folderPath)
        {
            var files = Directory.GetFiles(folderPath);

            foreach(var file in files)
            {
                ProcessFile(file, folderPath);
            }
        }

        private void ProcessFile(string fileName, string folderPath)
        {
            try
            {
                if (NeedRemoveFile(fileName))
                {
                    File.Delete(fileName);
                    _logger.Info($"Delete {Path.GetFileName(fileName).ToUpper()}");
                }

                var targetFile = GetTargetFile(fileName);
                var targetDirectory = Path.GetDirectoryName(targetFile);
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }
                File.Move(fileName, targetFile, true);

                _logger.Info($"{Path.GetFileName(fileName).ToUpper()} moved to {targetFile.Replace(folderPath, string.Empty).ToUpper()}");
            }
            catch(ConfigurationNotFoundException ex)
            {
                _logger.Warning(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private bool NeedRemoveFile(string fileName)
        {
            return _configuration.DeletePatterns.Any(dp => Regex.IsMatch(fileName, dp));
        }

        private string GetTargetFile(string fileName)
        {
            var moveParams = _configuration.MoveParams.FirstOrDefault(p => Regex.IsMatch(fileName, p.Pattern)) ?? throw new ConfigurationNotFoundException(fileName);

            return Path.Combine(
                Path.GetDirectoryName(fileName),
                moveParams.TargetFolderName,
                Regex.Split(Path.GetFileName(fileName), moveParams.Pattern)[0] + Path.GetExtension(fileName)                
                );
            
        }
    }
}
