using DbScripts.Processor;
using System;

namespace DbScripts
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileProcessor = new FileProcessor(new InMemoryConfiguration(), new ConsoleLogger());
            var scriptSeparator = new ScriptSeparator(new ConsoleLogger());
            var scriptsCombinier = new ScriptCombinier(new ConsoleLogger());
            var folderStructureGenerator = new FolderStructureGenerator(new InMemoryConfiguration());

            var dbScriptFileName = args[0];
            var directory = args[1];

            folderStructureGenerator.Generate(directory);
            scriptSeparator.ExtractSeparateScripts(directory, dbScriptFileName);
            scriptsCombinier.Combine(directory);
            fileProcessor.Process(directory);

            Console.ReadLine();
        }
    }
}
