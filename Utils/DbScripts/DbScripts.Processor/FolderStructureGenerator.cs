using System.IO;

namespace DbScripts.Processor
{
    public class FolderStructureGenerator
    {
        private readonly IConfiguration _configuration;

        public FolderStructureGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Generate(string parentDirectory)
        {
            foreach(var directory in _configuration.DbObjectFolders)
            {
                var targetDir = Path.Combine(parentDirectory, directory);
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                var keepFileName = Path.Combine(targetDir, ".keep");

                if (!File.Exists(keepFileName))
                {
                    File.Create(keepFileName);
                }
            }
        }
    }
}
