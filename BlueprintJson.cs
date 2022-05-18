using System;
using System.Collections.Generic;
using System.Linq;

using Kingmaker.Blueprints;

using Microsoftenator.Wotr.Common.Blueprints;

using Newtonsoft.Json;

namespace Microsoftenator.Wotr.Common.Blueprints.Json
{
    //[JsonObject]
    //public struct BlueprintInfoJson
    //{
    //    [JsonProperty(Required = Required.Always)]
    //    public string? Guid;
    //    [JsonProperty(Required = Required.Always)]
    //    public string? Name;

    //    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    //    public string? DisplayName;
    //    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    //    public string? Description;

    //    public static IEnumerable<BlueprintInfo<BlueprintScriptableObject>> DeserializeAll(string json)
    //        => JsonConvert.DeserializeObject<IEnumerable<BlueprintInfoJson>>(json)
    //            .Select(bpj => bpj.FromJson<BlueprintScriptableObject>());
    //    public static string SerializeAll<T>(IEnumerable<BlueprintInfo<T>> blueprints) where T : BlueprintScriptableObject
    //        => JsonConvert.SerializeObject(blueprints.Select(bp => bp.ToJson()), Formatting.Indented);
    //}

    //internal static partial class Extensions
    //{
    //    internal static BlueprintInfo<T> FromJson<T>(this BlueprintInfoJson bpj) where T : BlueprintScriptableObject
    //        => new(bpj);
    //    internal static BlueprintInfoJson ToJson<T>(this BlueprintInfo<T> bpi) where T : BlueprintScriptableObject
    //        => new() { Guid = bpi.GuidString, Name = bpi.Name, DisplayName = bpi.DisplayName, Description = bpi.Description };
    //}
}
