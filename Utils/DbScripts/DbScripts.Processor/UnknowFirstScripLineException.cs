using System;

namespace DbScripts.Processor
{
    public class UnknowFirstScripLineException : Exception
    {
        public UnknowFirstScripLineException(string line) : base($"Unknow first script line \"{line}\"")
        {

        }
    }
}
