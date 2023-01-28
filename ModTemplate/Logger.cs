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
        protected readonly UnityModManager.ModEntry.ModLogger? logger;

        public Logger(UnityModManager.ModEntry.ModLogger? logger) => this.logger = logger;

        public static Logger Null { get; } = new Logger(null);

        public bool IsNull => logger is null;

        protected virtual Action<T> MaybeLogger<T>(Func<UnityModManager.ModEntry.ModLogger, Action<T>> mapped)
        {
            if (logger is null) return Functional.Ignore;

            return mapped(logger);
        }

        protected Action<string> MaybeLogger(Func<UnityModManager.ModEntry.ModLogger, Action<string>> mapped) =>
            MaybeLogger<string>(mapped);

        public virtual Action<string> Debug =>
#if DEBUG
            MaybeLogger(log => s => log.Log($"[DEBUG] {s}"));
#else
            Functional.Ignore;
#endif

        public virtual Action<string> Info => MaybeLogger(log => log.Log);
        public virtual Action<string> Warning => MaybeLogger(log => log.Warning);
        public virtual Action<string> Error => MaybeLogger(log => log.Error);
        public virtual Action<string> Critical => MaybeLogger(log => log.Critical);
    }
}