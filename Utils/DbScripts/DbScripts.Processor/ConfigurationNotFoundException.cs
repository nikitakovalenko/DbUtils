using System;
using System.IO;

namespace DbScripts.Processor
{
    public class ConfigurationNotFoundException : Exception
    {
        public ConfigurationNotFoundException(string fileName) : base($"Cannot find configuration for moving file \"{Path.GetFileName(fileName)}\"")
        {

        }
    }
}
