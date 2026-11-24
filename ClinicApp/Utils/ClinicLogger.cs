using System;
using System.IO;
using System.Text;

namespace ClinicApp.Utils
{
    public class ClinicLogger
    {
        private readonly string _logPath;
        public ClinicLogger(string logPath = "clinic.log")
        {
            _logPath = logPath;
        }

        public void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }
        public void LogWarning(string message)
        {
            WriteLog("WARN", message);
        }
        public void LogError(string message)
        {
            WriteLog("ERROR", message);
        }

        public string[] GetLastLines(int n)
        {
            if (n <= 0 || !File.Exists(_logPath))
            {
                return new string[0];
            }
            string[] lines = File.ReadAllLines(_logPath, Encoding.UTF8);
            int skip = lines.Length - n;
            if (skip < 0)
            {
                skip = 0;
            }
            int resultCount = lines.Length - skip;
            string[] result = new string[resultCount];
            for (int i = 0; i < resultCount; i++)
            {
                result[i] = lines[skip + i];
            }
            return result;
        }
        public void Clear()
        {
            File.WriteAllText(_logPath, string.Empty, Encoding.UTF8);
        }
        public bool Exists()
        {
            return File.Exists(_logPath);
        }
        private void WriteLog(string level, string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string line = $"[{timestamp}] [{level,-5}] {message}{Environment.NewLine}";
            File.AppendAllText(_logPath, line, Encoding.UTF8);
        }
    }
}