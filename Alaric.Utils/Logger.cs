using System;
using System.IO;
using System.Text;

namespace Alaric.Utils
{
    /// <summary>  
    /// A class provides basic Log service.
    /// </summary>  
    [Serializable]
    public class Logger
    {
        /// <summary>
        /// The severity of the information.
        /// </summary>
        public enum LogLevel
        {
            Infos,
            Warns,
            Error
        };

        private readonly StringBuilder _logStringBuilder = new StringBuilder();

        private string _time = "";

        private string _fileName;

        private string _fileDirectory = "";

        private void GetTime()
        {
            _time = DateTime.Now.ToString();
        }

        private static string TimeStringFormat(string timeString)
        {
            char[] timeChar = timeString.ToCharArray();
            if (timeChar[5] != '0')
            {
                timeChar[4] = '0';
            }
            else
            {
                timeChar[4] = (char)0;
            }
            if (timeString[7] != '0')
            {
                timeChar[6] = '0';
            }
            else
            {
                timeChar[6] = (char)0;
            }

            timeString = new string(timeChar);
            string result = (timeString.Replace(' ', '-').Replace(":", "")).Substring(2);
            return result;
        }

        private void Initialize(string logFileName,string customLogFileDirection)
        {
            if (string.IsNullOrEmpty(logFileName))
            {
                throw new Exception("Logger is not initialized.");
            }
            if (!customLogFileDirection.EndsWith(@"\"))
            {
                GetTime();
                _fileDirectory = customLogFileDirection + @"\logs\";
            }
            else
            {
                _fileDirectory = customLogFileDirection;
            }
            _fileName = logFileName + "." + TimeStringFormat(_time) + ".log";
        }

        /// <summary>
        /// Initialize a logger which saves the log files to default path.
        /// </summary>
        /// <param name="logFileName">The prefix of the log file.</param>
        public Logger(string logFileName)
        {
            Initialize(logFileName, Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Initialize a logger which saves the log files to specified path.
        /// </summary>
        /// <param name="logFileName">The prefix of the log file.</param>
        /// <param name="customeLogFileDirection">The specified path.</param>
        public Logger(string logFileName,string customeLogFileDirection)
        {
            Initialize(logFileName, customeLogFileDirection);
        }

        ~Logger()
        {
            if (!Directory.Exists(_fileDirectory))
            {
                Directory.CreateDirectory(_fileDirectory);
            }
            File.AppendAllText(_fileDirectory + _fileName, _logStringBuilder.ToString());
        }

        /// <summary>
        /// Logs a Exception.
        /// </summary>
        /// <param name="exception">The Exception to be logged.</param>
        public virtual void Write(Exception exception)
        {
            Write(exception.ToString(), "Exception", LogLevel.Error);
        }

        /// <summary>
        /// Log as you wanted to.
        /// </summary>
        /// <param name="logInfo">The information you want to record.</param>
        /// <param name="logObject"></param>
        /// <param name="level">The severity of the information.</param>
        public virtual void Write(string logInfo,string logObject, LogLevel level)
        {
            GetTime();
            _logStringBuilder.Append("[");
            _logStringBuilder.Append(_time);
            _logStringBuilder.Append("] [" + logObject + "] [");
            _logStringBuilder.Append(Utilities.EnumToString(level));
            _logStringBuilder.Append("]:");
            _logStringBuilder.AppendLine(logInfo);
#if DEBUG
            Console.WriteLine("[" + _time + "] [" + logObject + "] [" + Utilities.EnumToString(level) + "]:" + logInfo);
#endif
        }

        /// <summary>
        /// Returns the content of the Logger.
        /// </summary>
        /// <returns>String Logs</returns>
        public override string ToString()
        {
            return _logStringBuilder.ToString();
        }
    }
}
