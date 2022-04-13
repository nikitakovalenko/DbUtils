using System;

namespace DbScripts.Processor
{
    public class ConsoleLogger : ILogger
    {
        public void Error(string message)
        {
            WriteLogMessage("Error", message);
        }

        public void Info(string message)
        {
            WriteLogMessage("Info", message);
        }

        public void Warning(string message)
        {
            WriteLogMessage("Warning", message);
        }

        private void WriteLogMessage(string type, string message)
        {
            Console.WriteLine($"{type} {DateTime.Now.ToString("HH:mm:ss ffff")}: {message}");
        }
    }
}
