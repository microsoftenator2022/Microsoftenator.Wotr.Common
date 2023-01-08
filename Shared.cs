using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoftenator.Wotr.Common.Blueprints;
using Microsoftenator.Wotr.Common.Util;

namespace Microsoftenator.Wotr.Common
{
    public static class SharedMods
    {
        internal static readonly Dictionary<string, (int version, ICollection<BlueprintInfo> blueprints)> ModInfo =
            ModInfo ?? new();
        
        public static bool Register(string sharedModName, int version, IEnumerable<BlueprintInfo> blueprints)
        {
            if(ModInfo.TryGetValue(sharedModName, out var modInfo) && modInfo.version >= version) return false;

            ModInfo[sharedModName] = (version, blueprints.ToList());

            return true;
        }

        public static bool Register(string sharedModName, IEnumerable<BlueprintInfo> blueprints) =>
            Register(sharedModName, 0, blueprints);

        public static bool Register(string sharedModName, int version = 0) =>
            Register(sharedModName, version, Enumerable.Empty<BlueprintInfo>());

        public static bool AddBlueprints(string sharedModName, int version, IEnumerable<BlueprintInfo> newBlueprints)
        {
            if (ModInfo.TryGetValue(sharedModName, out var modInfo) && modInfo.version > version) return false;

            ModInfo[sharedModName] =
                (version, blueprints: new [] 
                {
                    modInfo.blueprints,
                    newBlueprints.Where(bp => !modInfo.blueprints.Any(bp2 => bp.Guid == bp2.Guid))
                }.SelectMany(Functional.Id).ToList());

            return true;
        }

        public static bool AddBlueprints(string sharedModName, IEnumerable<BlueprintInfo> newBlueprints) =>
            AddBlueprints(sharedModName, 0, newBlueprints);
    }
}
