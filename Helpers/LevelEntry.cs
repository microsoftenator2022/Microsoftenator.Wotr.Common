using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;

using Microsoftenator.Wotr.Common.Blueprints;
using Microsoftenator.Wotr.Common.Extensions;

namespace Microsoftenator.Wotr.Common
{
    public static partial class Helpers
    {
        public static partial class Components
        {
            public static class LevelEntry
            {
                public static Kingmaker.Blueprints.Classes.LevelEntry Create(
                    int level, IEnumerable<IBlueprintInfo<BlueprintFeatureBase>> features) =>
                    new()
                    {
                        Level = level,
                        m_Features =
                            features
                                .Select(f => f.GetReference<BlueprintFeatureBase, BlueprintFeatureBaseReference>())
                                .ToList()
                    };
                public static Kingmaker.Blueprints.Classes.LevelEntry Create(
                    int level, IBlueprintInfo<BlueprintFeatureBase> feature) =>
                    Create(level, new[] { feature });
            }
        }
    }
}
