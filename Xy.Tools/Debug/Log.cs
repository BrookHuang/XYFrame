using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Xy.Tools.Debug {
    public class Log {
        public enum LogType {
            Event,
            Error,
            Report
        }

        private static System.Diagnostics.Stopwatch _stopWatch = new System.Diagnostics.Stopwatch();
        private static bool _isInWorkflow = false;
        private static long _timeHold = 0;
        private static long _ticksHold = 0;

        public static void StartWorkflowLog() {
            _isInWorkflow = true;
            _stopWatch.Start();
        }

        public static void EndWorkflowLog() {
            _stopWatch.Reset();
            _isInWorkflow = false;
            _timeHold = 0;
            _ticksHold = 0;
        }

        public static void WriteEventLog(string eventInfo) {
            WriteLog(LogType.Event, eventInfo);
        }

        public static void WriteErrorLog(string errorInfo) {
            WriteLog(LogType.Error, errorInfo);
        }

        public static void WriteReportLog(string errorInfo) {
            WriteLog(LogType.Report, errorInfo);
        }

        public static void WriteLog(LogType type, string log) {
            if(_isInWorkflow){
                _stopWatch.Stop();
            }
            StringBuilder _sb = new StringBuilder();
            _sb.Append("===========================================================================================");
            _sb.Append(DateTime.Now.ToString("HH:mm:ss.fff"));
            if (_isInWorkflow) {
                _sb.Append(" |");
                _sb.Append(" " + _stopWatch.ElapsedMilliseconds);
                _sb.Append(" (+" + (_stopWatch.ElapsedMilliseconds - _timeHold) + ")");
                _sb.Append(" |");
                _sb.Append(" " + _stopWatch.ElapsedTicks);
                _sb.Append(" (+" + (_stopWatch.ElapsedTicks - _ticksHold) + ")");
            }
            _sb.AppendLine();
            _sb.AppendLine(log);
            string filepath;
            filepath = Xy.AppSetting.LogDir + type.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

            System.IO.FileStream _fs = System.IO.File.Open(filepath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            try {
                byte[] _log = System.Text.Encoding.UTF8.GetBytes(_sb.ToString());
                _fs.Write(_log, 0, _log.Length);
                _fs.Flush();
            } finally {
                _fs.Close();
            }
            if (_isInWorkflow) {
                _timeHold = _stopWatch.ElapsedMilliseconds;
                _ticksHold = _stopWatch.ElapsedTicks;
                _stopWatch.Start();
            }
        }
    }
}
