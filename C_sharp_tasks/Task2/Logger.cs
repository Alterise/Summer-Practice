using System;
using System.IO;

namespace Task2
{
    public class Logger
    {
        private readonly StreamWriter _file;

        public enum Severity
        {
            Trace,
            Debug,
            Information,
            Warning,
            Error,
            Critical
        }
        
        public Logger(string filename)
        {
            _file = new(filename);
            _file.AutoFlush = true;
        }
        
        public void Add(Severity severity, string data)
        {
            _file.WriteLine("[" +  DateTime.Now + "] [" + severity + "]: " + data);
        }
    }
}