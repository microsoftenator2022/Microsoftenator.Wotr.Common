using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;

using Microsoftenator.Wotr.Common.Util;

using MoreLinq;

namespace Microsoftenator.Wotr.Common
{
    namespace Extensions
    {
        public static class Progression
        {
            public static void AddFeatures(
                this BlueprintProgression progression, int level, IEnumerable<IBlueprintInfo<BlueprintFeatureBase>> features) =>
                Helpers.Blueprint.Progression.AddFeatures(progression, level, features);
            public static void AddFeatures(this BlueprintProgression progression, int level,
                params IBlueprintInfo<BlueprintFeatureBase>[] features) =>
                AddFeatures(progression, level, (IEnumerable<IBlueprintInfo<BlueprintFeatureBase>>)features);

            public static void AddFeatures(
                this BlueprintProgression progression, int level, IEnumerable<BlueprintFeatureBaseReference> features) =>
                Helpers.Blueprint.Progression.AddFeatures(progression, level, features);
            public static void AddFeatures(
                this BlueprintProgression progression, int level, params BlueprintFeatureBaseReference[] features) =>
                AddFeatures(progression, level, (IEnumerable<BlueprintFeatureBaseReference>)features);

            public static void AddFeatures(this BlueprintProgression progression,
                Dictionary<int, IEnumerable<IBlueprintInfo<BlueprintFeatureBase>>> dict) =>
                Helpers.Blueprint.Progression.AddFeatures(progression, dict);

            public static BlueprintProgression ToProgression(
                this Dictionary<int, IEnumerable<IBlueprintInfo<BlueprintFeatureBase>>> dict,
                NewUnitFact<BlueprintProgression> bpi) =>
                Helpers.Blueprint.Progression.CreateFromDictionary(bpi, dict);
        }
    }
    public static partial class Helpers
    {
        public static partial class Blueprint
        {
            public static class Progression
            {
                public static Func<Action<BlueprintProgression>, BlueprintProgression> CreateWith(
                    string name, BlueprintGuid guid) =>
                    Feature.CreateWith<BlueprintProgression>(name, guid);
                public static BlueprintProgression Create(string name, BlueprintGuid guid) =>
                    CreateWith(name, guid)(Functional.Ignore);

                public static Func<Action<BlueprintProgression>, BlueprintProgression> CreateWith(
                    NewUnitFact<BlueprintProgression> bpi) =>
                    Feature.CreateWith(bpi);
                public static BlueprintProgression Create(NewUnitFact<BlueprintProgression> bpi) =>
                    CreateWith(bpi)(Functional.Ignore);

                public static void AddFeatures(
                    BlueprintProgression progression,
                    int level,
                    IEnumerable<BlueprintFeatureBaseReference> features)
                {
                    var levelEntry = progression.LevelEntries.FirstOrDefault(le => le.Level == level);

                    if (levelEntry is null)
                    {
                        levelEntry = new() { Level = level };
                        progression.LevelEntries = progression.LevelEntries.Append(levelEntry);
                    }
                    
                    levelEntry.m_Features.AddRange(features);
                }
                public static void AddFeatures(
                    BlueprintProgression progression,
                    int level,
                    IEnumerable<BlueprintFeatureBase> features) =>
                    AddFeatures(progression, level, features.Select(f => f.ToReference<BlueprintFeatureBaseReference>()));
                public static void AddFeatures(
                    BlueprintProgression progression, int level, IEnumerable<IBlueprintInfo<BlueprintFeatureBase>> features) =>
                    AddFeatures(
                        progression,
                        level,
                        features.Select(f => GetReference<BlueprintFeatureBase, BlueprintFeatureBaseReference>(f)));

                public static void AddFeature(
                    BlueprintProgression progression, int level, IBlueprintInfo<BlueprintFeatureBase> feature) =>
                    AddFeatures(progression, level, new[] { feature });

                public static void AddFeatures(BlueprintProgression progression,
                    Dictionary<int, IEnumerable<IBlueprintInfo<BlueprintFeatureBase>>> dict)
                {
                    foreach(var (level, features) in dict.Select(kvp => (kvp.Key, kvp.Value)))
                    {
                        AddFeatures(progression, level, features);
                    }
                }

                public static BlueprintProgression CreateFromDictionary(NewUnitFact<BlueprintProgression> bpi,
                    Dictionary<int, IEnumerable<IBlueprintInfo<BlueprintFeatureBase>>> dict) =>
                    CreateWith(bpi)(bp => AddFeatures(bp, dict));
            }
        }
    }
}
