using System;
using System.IO;

namespace LoggerService
{
    public class FileLogger : ISLogger
    {
        private readonly FileLoggerConfiguration _fileLoggerConfiguration;

        public FileLogger(FileLoggerConfiguration fileLoggerConfiguration)
        {
            _fileLoggerConfiguration = fileLoggerConfiguration;
        }

        public void ClearLog()
        {
            if (File.Exists(_fileLoggerConfiguration.Path))
            {
                File.WriteAllText(_fileLoggerConfiguration.Path, String.Empty);
            }
        }

        public void Log(string message)
        {
            using (StreamWriter streamWriter = File.AppendText(_fileLoggerConfiguration.Path))
            {
                streamWriter.WriteLine(message);
                streamWriter.Close();
            }
        }

    }
}
