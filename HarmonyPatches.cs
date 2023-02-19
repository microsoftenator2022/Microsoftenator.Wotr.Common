using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;

namespace Microsoftenator.Wotr.Common
{
    public static class HarmonyPatches
    {
        internal static bool Patched = false;

        internal static Harmony? HarmonyInstance;

        public static void ApplyAll()
        {
            if (Patched) return;

            Patched = true;

            HarmonyInstance ??= new Harmony("Microsoftenator.Wotr.Common");

            HarmonyInstance.PatchAll();
        }

        public static void UnpatchAll()
        {
            if (!Patched) return;

            Patched = false;

            HarmonyInstance?.UnpatchAll();
        }
    }
}
