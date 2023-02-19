using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Microsoftenator.Wotr.Common.Blueprints;
using Microsoftenator.Wotr.Common.Util;
using Kingmaker.Blueprints.JsonSystem.Converters;
using UnityEngine;

namespace Microsoftenator.Wotr.Common
{
    public static partial class Helpers
    {
        public static Sprite GetSprite(string assetId, long fileId) =>
            (Sprite)UnityObjectConverter.AssetList.Get(assetId, fileId);

        public static partial class Blueprint
        {
            public static class Feature
            {
                public static Func<Action<TBlueprint>, TBlueprint> CreateWith<TBlueprint>(NewUnitFact<TBlueprint> bpi)
                    where TBlueprint : BlueprintFeature, new() =>
                    Blueprint.CreateWith(bpi);
                public static TBlueprint Create<TBlueprint>(NewUnitFact<TBlueprint> bpi)
                    where TBlueprint : BlueprintFeature, new() =>
                    CreateWith(bpi)(Functional.Ignore);

                public static Func<Action<TBlueprint>, TBlueprint> CreateWith<TBlueprint>(string name, BlueprintGuid assetId)
                    where TBlueprint : BlueprintFeature, new() =>
                    CreateWith(new NewUnitFact<TBlueprint>(assetId, name));
                public static TBlueprint Create<TBlueprint>(string name, BlueprintGuid assetId)
                    where TBlueprint : BlueprintFeature, new() =>
                    CreateWith<TBlueprint>(name, assetId)(Functional.Ignore);
            }
        }
    }
}
