using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoftenator.Wotr.Common.Util;
using UnityModManagerNet;

namespace Microsoftenator.Wotr.Common.ModTemplate
{
    public class Logger
    {
        public enum LogEntryType
        {
            Debug,
            Info,
            Warning,
            Error,
            Critical
        }

        protected internal virtual UnityModManager.ModEntry.ModLogger? ModLogger { get; set; }

        private Logger(UnityModManager.ModEntry.ModLogger? logger, IEnumerable<(LogEntryType, string)>? preloadLogs = null)
        {
            this.ModLogger = logger;

            if(preloadLogs is not null)
                foreach(var log in preloadLogs)
                    this.PreLoadLogs.Add(log);
        }

        protected Logger(UnityModManager.ModEntry.ModLogger? logger, Logger? other = null) : this(logger, other?.PreLoadLogs) { }

        public Logger() : this(null, (Logger?)null) { }
        
        public bool IsNull => ModLogger is null;

        protected readonly List<(LogEntryType logType, string message)> PreLoadLogs = new();

        protected void ReplayLogs()
        {
            //if (logger is null) return;

            var logs = new (LogEntryType logType, string message)[PreLoadLogs.Count];

            PreLoadLogs.CopyTo(logs);
            PreLoadLogs.Clear();

            foreach (var (logType, message) in logs)
            {
                Log(logType, message);
            }
        }

        protected virtual void Log(LogEntryType logType, string message)
        {
            if (ModLogger is null) 
            {
                PreLoadLogs.Add((logType, message));
                return;
            }

            if (PreLoadLogs.Count > 0)
                ReplayLogs();

            switch (logType)
            {
                case LogEntryType.Debug:
                    ModLogger.Log($"[DEBUG] {message}");
                    break;
                case LogEntryType.Info:
                    ModLogger.Log(message);
                    break;
                case LogEntryType.Warning:
                    ModLogger.Warning(message);
                    break;
                case LogEntryType.Error:
                    ModLogger.Error(message);
                    break;
                case LogEntryType.Critical:
                    ModLogger.Critical(message);
                    break;
            }
        }

        public virtual void Debug(string message) => Log(LogEntryType.Debug, message);
        public virtual void Info(string message) => Log(LogEntryType.Info, message);
        public virtual void Warning(string message) => Log(LogEntryType.Warning, message);
        public virtual void Error(string message) => Log(LogEntryType.Error, message);
        public virtual void Critical(string message) => Log(LogEntryType.Critical, message);
    }
}