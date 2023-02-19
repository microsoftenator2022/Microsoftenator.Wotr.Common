using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AK.Wwise;

using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;

using Microsoftenator.Wotr.Common;
using Microsoftenator.Wotr.Common.Blueprints;
using Microsoftenator.Wotr.Common.Util;

namespace Microsoftenator.Wotr.Common
{
    namespace Extensions
    {
        public static partial class BlueprintInfo
        {
            public static TRef GetReference<TBlueprint, TRef>(this IBlueprintInfo<TBlueprint> bpi)
                where TBlueprint : SimpleBlueprint
                where TRef : BlueprintReference<TBlueprint>, new() =>
                    Helpers.Blueprint.GetReference<TBlueprint, TRef>(bpi);
        }
    }

    public static partial class Helpers
    {
        public static partial class Blueprint
        {
            public static class Unchecked
            {
                public static TRef CreateRef<TBlueprint, TRef>(BlueprintGuid guid)
                    where TBlueprint : SimpleBlueprint
                    where TRef : BlueprintReference<TBlueprint>, new()
                    => new() { deserializedGuid = guid };
            }

            public static TRef GetReference<TBlueprint, TRef>(IBlueprintInfo<TBlueprint> bpi)
                where TBlueprint : SimpleBlueprint
                where TRef : BlueprintReference<TBlueprint>, new() =>
                    Unchecked.CreateRef<TBlueprint, TRef>(bpi.BlueprintGuid);

            public static Func<Action<TBlueprint>, TBlueprint> CreateWith<TBlueprint>(NewBlueprint<TBlueprint> bpi)
                where TBlueprint : BlueprintScriptableObject, new()
            {
                if (ResourcesLibrary.TryGetBlueprint<TBlueprint>(bpi.BlueprintGuid) is not null) throw new ArgumentException();

                return init =>
                {
                    TBlueprint bp = new()
                    {
                        AssetGuid = bpi.BlueprintGuid,
                        name = bpi.Name
                    };

                    bpi.Init(bp);

                    init(bp);

                    ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(bpi.BlueprintGuid, bp);

                    return bp;
                };
            }
            public static TBlueprint Create<TBlueprint>(NewBlueprint<TBlueprint> bpi)
                where TBlueprint : BlueprintScriptableObject, new() =>
                CreateWith(bpi)(Functional.Ignore);

            public static Func<Action<TBlueprint>, TBlueprint> CreateWith<TBlueprint>(string name, BlueprintGuid guid)
                where TBlueprint : BlueprintScriptableObject, new() =>
                CreateWith(new NewBlueprint<TBlueprint>(guid, name));
            public static TBlueprint Create<TBlueprint>(string name, BlueprintGuid assetId)
                where TBlueprint : BlueprintScriptableObject, new() =>
                CreateWith<TBlueprint>(name, assetId)(Functional.Ignore);

            public static Func<Action<TBlueprint>, TBlueprint> CloneWith<TBlueprint>(
                TBlueprint original,
                string name,
                Guid guid)
                where TBlueprint : SimpleBlueprint
            {
                TBlueprint copy = TTT_Utils.Clone(original);

                copy.name = name;
                copy.AssetGuid = new BlueprintGuid(guid);
                
                return action =>
                {
                    action(copy);

                    ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(copy.AssetGuid, copy);

                    return copy;
                };
            }
            public static TBlueprint Clone<TBlueprint>(
                TBlueprint original,
                string name,
                Guid guid)
                where TBlueprint : SimpleBlueprint
                => CloneWith(original, name, guid)(Functional.Ignore);

            public static Func<Action<TBlueprint>, TBlueprint> CloneWith<TBlueprint>(
                TBlueprint original, NewBlueprint<TBlueprint> bpi) where TBlueprint : BlueprintScriptableObject, new()
                => init => CloneWith(original, bpi.Name, bpi.Guid)(bpi.Init.ContinueWith(init));
        }
    }
}
