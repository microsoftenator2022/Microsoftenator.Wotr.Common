using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using Microsoftenator.Wotr.Common.Blueprints;
using Microsoftenator.Wotr.Common.Util;
using UnityModManagerNet;

namespace Microsoftenator.Wotr.Common.ModTemplate
{
    public abstract class ModBase
    {
        protected UnityModManager.ModEntry? modEntry;

        //private Action<UnityModManager.ModEntry> onGUI = Functional.Ignore;
        //private Action<UnityModManager.ModEntry> onSaveGUI = Functional.Ignore;
        private Func<UnityModManager.ModEntry, bool> onUnload = (_) => true;

        public virtual UnityModManager.ModEntry ModEntry
        {
            get => modEntry ?? throw new NullReferenceException();
            set
            {
                modEntry = value;

                modEntry.OnUnload = this.onUnload;
                modEntry.OnToggle = this.OnToggle;
                //modEntry.OnGUI = this.OnGUI;
                //modEntry.OnSaveGUI = this.OnSaveGUI;
            }
        }

        public virtual Logger Log { get; protected set; } = new();

        public virtual bool OnLoad(UnityModManager.ModEntry modEntry, Harmony? harmony = null, bool harmonyPatch = false)
        {
            Log.ModLogger = modEntry.Logger;

            Log.Debug($"{nameof(ModBase)}.{nameof(OnLoad)}");

            ModEntry = modEntry;
            
            if(harmony is not null)
                Harmony = harmony;

            if (harmonyPatch)
            {
                Harmony ??= new Harmony(ModEntry.Info.Id);

                var onUnload = this.onUnload;
                OnUnload = (me) =>
                {
                    Harmony?.UnpatchAll(ModEntry.Info.Id);

                    return onUnload(me) && true;
                };

                Harmony?.PatchAll();
            }

            return true;
        }

        public virtual Func<UnityModManager.ModEntry, bool> OnUnload
        {
            get => onUnload;
            set
            {
                onUnload = value;

                if(modEntry is not null)
                    modEntry.OnUnload = onUnload;
            }
        }

        public virtual Func<UnityModManager.ModEntry, bool, bool> OnToggle { get; set; } = (_, _) => true;

        public virtual Harmony? Harmony { get; set; }

        //public virtual Action<UnityModManager.ModEntry> OnGUI
        //{
        //    get => onGUI;
        //    set
        //    {
        //        onGUI = value;

        //        if (modEntry is not null)
        //            modEntry.OnGUI = onGUI;
        //    }
        //}

        //public virtual Action<UnityModManager.ModEntry> OnSaveGUI
        //{
        //    get => onSaveGUI;
        //    set
        //    {
        //        onSaveGUI = value;

        //        if (modEntry is not null)
        //            modEntry.OnSaveGUI = onSaveGUI;
        //    }
        //}
    }

    //    static class Main
    //    {
    //        internal class Logger
    //        {
    //            private readonly UnityModManager.ModEntry.ModLogger? logger;

    //            internal Logger(UnityModManager.ModEntry.ModLogger? logger) => this.logger = logger;

    //            public Action<string> Debug =>
    //#if DEBUG
    //                MaybeLogger(log => s => log.Log($"[DEBUG] {s}"));
    //#else
    //                Functional.Ignore;
    //#endif
    //            private Action<T> MaybeLogger<T>(Func<UnityModManager.ModEntry.ModLogger, Action<T>> mapped)
    //            {
    //                if (logger is null) return Functional.Ignore;

    //                return mapped(logger);
    //            }

    //            private Action<string> MaybeLogger(Func<UnityModManager.ModEntry.ModLogger, Action<string>> mapped) =>
    //                MaybeLogger<string>(mapped);


    //            public Action<string> Info => MaybeLogger(log => log.Log);
    //            public Action<string> Warning => MaybeLogger(log => log.Warning);
    //            public Action<string> Error => MaybeLogger(log => log.Error);
    //            public Action<string> Critical => MaybeLogger(log => log.Critical);
    //        }

    //        internal static UnityModManager.ModEntry? ModEntry { get; private set; }

    //        internal static readonly int SharedModVersion = 0;
    //        internal static Func<IEnumerable<BlueprintInfo>, bool> AddSharedBlueprints { get; private set; } = _ => false;

    //        static internal Logger Log { get; private set; } = new(null);
    //        internal static bool Enabled { get; private set; } = false;

    //        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
    //        {
    //            Log?.Debug($"{nameof(Main)}.{nameof(OnToggle)}({value})");

    //            Enabled = value;
    //            return true;
    //        }

    //        static bool Load(UnityModManager.ModEntry modEntry)
    //        {

    //            Log = new(modEntry.Logger);

    //            Log.Debug($"{nameof(Main)}.{nameof(Load)}");

    //            ModEntry = modEntry;

    //            var harmony = new Harmony(modEntry.Info.Id);

    //            SharedMods.Register(modEntry.Info.Id, SharedModVersion);
    //            AddSharedBlueprints = blueprints => SharedMods.AddBlueprints(modEntry.Info.Id, SharedModVersion, blueprints);

    //            harmony.PatchAll();

    //            return true;
    //        }
    //    }
}
