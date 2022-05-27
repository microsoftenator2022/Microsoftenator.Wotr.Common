using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Localization;

using Microsoftenator.Wotr.Common.Blueprints.Extensions;
using Microsoftenator.Wotr.Common.Localization;
using Microsoftenator.Wotr.Common.Util;

namespace Microsoftenator.Wotr.Common.Blueprints
{
    [Obsolete]
    public class BlueprintInfo<T> where T : BlueprintScriptableObject
    {
        public readonly string GuidString;
        public readonly Guid Guid;
        public readonly string Name;
        public readonly string? DisplayName;
        public readonly string? Description;

        public BlueprintInfo(string guid, (string name, string? displayName, string? description) info)
            : this(guid, info.name, info.displayName, info.description) { }

        public BlueprintInfo(string guid, string name, string? displayName, string? description)
        {
            //if (guid is null || name is null)
            //    throw new NullReferenceException();

            GuidString = guid;
            Guid = Guid.Parse(GuidString);
            Name = name;
            DisplayName = displayName;
            Description = description;
        }

        //internal BlueprintInfo(BlueprintInfoJson bpj) : this(bpj.Guid, bpj.Name, bpj.DisplayName, bpj.Description) { }

        //public BlueprintInfo<U> Cast<U>() where U : T
        //    => new(name: Name, guid: GuidString, displayName: DisplayName, description: Description);
        public T GetBlueprint() => ResourcesLibrary.TryGetBlueprint<T>(GuidString);
        public U? GetBlueprint<U>() where U : T => GetBlueprint() as U;
    }

    public abstract class BlueprintInfoAbstract
    {
        public string GuidString { get; private set; }
        internal Guid Guid => Guid.Parse(GuidString);
        public BlueprintGuid BlueprintGuid => new(Guid);
        internal TRef GetBlueprintRefInternal<TRef, T>()
            where T : BlueprintScriptableObject
            where TRef : BlueprintReference<T>, new()
            => new() { deserializedGuid = BlueprintGuid };

        internal T? TryGetBlueprintInternal<T>() where T : BlueprintScriptableObject
            => new BlueprintReference<T>() { deserializedGuid = BlueprintGuid }.Get();

        public abstract string Name { get; }
        public abstract Type BlueprintType { get; }

        internal BlueprintInfoAbstract(string guidString)
        {
            GuidString = guidString;
        }
    }

    public abstract class BlueprintInfoAbstract<T> : BlueprintInfoAbstract where T : BlueprintScriptableObject
    {
        internal BlueprintInfoAbstract(string guidString) : base(guidString) { }

        public virtual TRef GetBlueprintRef<TRef>() where TRef : BlueprintReference<T>, new()
            => GetBlueprintRefInternal<TRef, T>();

        public BlueprintReference<T> GetBlueprintRef() => GetBlueprintRefInternal<BlueprintReference<T>, T>();

        public virtual T? TryGetBlueprint() => TryGetBlueprintInternal<T>();

        //public abstract BlueprintInfoAbstract<U> As<U>() where U : T;

        //public override Type BlueprintType => typeof(T);
    }

    public sealed class NewBlueprint<T> : BlueprintInfoAbstract<T> where T : BlueprintScriptableObject, new()
    {
        private readonly string name;
        public override string Name => name;

        public LocalizedString? DisplayName { get; set; }
        public LocalizedString? Description { get; set; }

        private void InitFact(T bp)
        {
            if (bp is BlueprintUnitFact fact)
            {
                if (DisplayName is not null) fact.SetDisplayName(DisplayName);
                if (Description is not null) fact.SetDescription(Description);
            }
        }

        private Action<T> init;
        public Action<T> Init
        {
            get => init;
            set => init = bp => { InitFact(bp); init(bp); };
        }
        public NewBlueprint(string guid, string name) : base(guid)
        {
            this.name = name;
            this.init = InitFact;
        }

        public NewBlueprint(
            string guid,
            string name,
            LocalizedStringsPack strings,
            string? displayName = null,
            string? description = null)
            : this(guid, name)
        {
            if (displayName is not null) DisplayName = strings.Add($"{name}.Name", displayName);
            if (description is not null) Description = strings.Add($"{name}.Description", description);
        }

        public T GetBlueprint() => base.TryGetBlueprintInternal<T>() ?? throw new NullReferenceException();

        private readonly Type blueprintType = typeof(T);
        public override Type BlueprintType => blueprintType;
    }

    public sealed class OwlcatBlueprint<T> : BlueprintInfoAbstract<T> where T : BlueprintScriptableObject
    {
        public OwlcatBlueprint(string guid) : base(guid) { }
        public override string Name => base.TryGetBlueprintInternal<T>()?.name ?? throw new NullReferenceException();

        public T GetBlueprint() => base.TryGetBlueprintInternal<T>() ?? throw new NullReferenceException();

        private readonly Type blueprintType = typeof(T);
        public override Type BlueprintType => blueprintType;
    }

    public static class BlueprintInfoExtensions
    {
        public static TRef GetBlueprintRef<T, U, TRef>(this NewBlueprint<T> obj)
            where T : U, new()
            where U : BlueprintScriptableObject, new()
            where TRef : BlueprintReference<U>, new()
            => obj.GetBlueprintRefInternal<TRef, U>();

        public static TRef GetBlueprintRef<T, U, TRef>(this OwlcatBlueprint<T> obj)
            where T : U
            where U : BlueprintScriptableObject
            where TRef : BlueprintReference<U>, new()
            => obj.GetBlueprintRefInternal<TRef, U>();

        public static U GetBlueprint<T, U>(this NewBlueprint<T> obj)
            where T : U, new()
            where U : BlueprintScriptableObject, new()
            => obj.GetBlueprint();

        public static U? TryGetBlueprint<T, U>(this OwlcatBlueprint<T> obj)
            where T : U
            where U : BlueprintScriptableObject
            => obj.TryGetBlueprintInternal<T>();
    }
}
