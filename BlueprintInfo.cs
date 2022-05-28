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
    //[Obsolete]
    //public class BlueprintInfo<T> where T : BlueprintScriptableObject
    //{
    //    public readonly string GuidString;
    //    public readonly Guid Guid;
    //    public readonly string Name;
    //    public readonly string? DisplayName;
    //    public readonly string? Description;

    //    public BlueprintInfo(string guid, (string name, string? displayName, string? description) info)
    //        : this(guid, info.name, info.displayName, info.description) { }

    //    public BlueprintInfo(string guid, string name, string? displayName, string? description)
    //    {
    //        //if (guid is null || name is null)
    //        //    throw new NullReferenceException();

    //        GuidString = guid;
    //        Guid = Guid.Parse(GuidString);
    //        Name = name;
    //        DisplayName = displayName;
    //        Description = description;
    //    }

    //    //internal BlueprintInfo(BlueprintInfoJson bpj) : this(bpj.Guid, bpj.Name, bpj.DisplayName, bpj.Description) { }

    //    //public BlueprintInfo<U> Cast<U>() where U : T
    //    //    => new(name: Name, guid: GuidString, displayName: DisplayName, description: Description);
    //    public T GetBlueprint() => ResourcesLibrary.TryGetBlueprint<T>(GuidString);
    //    public U? GetBlueprint<U>() where U : T => GetBlueprint() as U;
    //}

    public abstract class BlueprintInfo
    {
        public string GuidString { get; private set; }
        internal Guid Guid => Guid.Parse(GuidString);
        public BlueprintGuid BlueprintGuid => new(Guid);
        //internal TRef GetBlueprintRefInternal<TRef, T>()
        //    where T : BlueprintScriptableObject
        //    where TRef : BlueprintReference<T>, new()
        //    => new() { guid = GuidString };

        internal T? TryGetBlueprintInternal<T>() where T : BlueprintScriptableObject
            => ResourcesLibrary.TryGetBlueprint<T>(BlueprintGuid);

        public abstract string Name { get; }
        public abstract Type BlueprintType { get; }

        internal BlueprintInfo(string guidString)
        {
            GuidString = guidString;
        }
    }

    public abstract class BlueprintInfoAbstract<T> : BlueprintInfo where T : BlueprintScriptableObject
    {
        internal BlueprintInfoAbstract(string guidString) : base(guidString) { }

        //public virtual TRef GetBlueprintRef<TRef>() where TRef : BlueprintReference<T>, new()
        //    => GetBlueprintRefInternal<TRef, T>();

        //public BlueprintReference<T> GetBlueprintRef() => GetBlueprintRefInternal<BlueprintReference<T>, T>();

        public virtual T? TryGetBlueprint() => TryGetBlueprintInternal<T>();
        public virtual T GetBlueprint() => TryGetBlueprint() ?? throw new NullReferenceException();

        //public abstract BlueprintInfoAbstract<U> As<U>() where U : T;

        //public override Type BlueprintType => typeof(T);
    }

    public class NewBlueprint<T> : BlueprintInfoAbstract<T> where T : BlueprintScriptableObject, new()
    {
        

        //private Func<LocalizedString?> getDisplayName = () => null;
        //public LocalizedString? DisplayName { get; set; }

        //private Func<LocalizedString?> getDescription = () => null;
        //public LocalizedString? Description { get; set; }

        //internal void InitDefaults(T bp)
        //{
        //    if (bp is BlueprintUnitFact fact)
        //    {
        //        DisplayName = getDisplayName();
        //        Description = getDescription();

        //        if (DisplayName is not null) fact.SetDisplayName(DisplayName);
        //        if (Description is not null) fact.SetDescription(Description);
        //    }
        //}

        //private Action<T> init = Functional.Ignore;
        public virtual Action<T> Init { get; set; }

        public NewBlueprint(string guid, string name) : base(guid)
        {
            this.name = name;
            Init = Functional.Ignore;
        }

        private readonly string name;
        public override string Name => name;

        //public T GetBlueprint() => base.TryGetBlueprintInternal<T>() ?? throw new NullReferenceException();

        private readonly Type blueprintType = typeof(T);
        public override Type BlueprintType => blueprintType;

        //public NewBlueprint(
        //    string guid,
        //    string name,
        //    LocalizedStringsPack strings,
        //    string? displayName = null,
        //    string? description = null)
        //    : this(guid, name)
        //{
        //    //if(displayName is not null)
        //    //{
        //    //    var key = $"{name}.Name";
        //    //    strings.Add(key, displayName);
        //    //    getDisplayName = () => strings.Get(key);
        //    //}

        //    //if(description is not null) 
        //    //{
        //    //    var key = $"{name}.Description";
        //    //    strings.Add(key, description);
        //    //    getDescription = () => strings.Get(key);
        //    //}
        //}
    }

    public class NewUnitFact<T> : NewBlueprint<T>
            where T : BlueprintUnitFact, new()
    {
        private readonly Func<LocalizedString?> getDisplayName = () => null;
        public LocalizedString? DisplayName { get; set; }

        private readonly Func<LocalizedString?> getDescription = () => null;
        public LocalizedString? Description { get; set; }

        internal void InitDefaults(T bp)
        {
            if (bp is BlueprintUnitFact fact)
            {
                DisplayName = getDisplayName();
                Description = getDescription();

                if (DisplayName is not null) fact.SetDisplayName(DisplayName);
                if (Description is not null) fact.SetDescription(Description);
            }
        }
        
        public override Action<T> Init
        {
            get => bp => { InitDefaults(bp); base.Init(bp); };
            set => base.Init = value;
        }

        public NewUnitFact(string guid, string name) : base(guid, name) { }

        public NewUnitFact(
            string guid,
            string name,
            LocalizedStringsPack strings,
            string? displayName = null,
            string? description = null) : base(guid, name)
        {
            if (displayName is not null)
            {
                var key = $"{name}.Name";
                strings.Add(key, displayName);
                getDisplayName = () => strings.Get(key);
            }

            if (description is not null)
            {
                var key = $"{name}.Description";
                strings.Add(key, description);
                getDescription = () => strings.Get(key);
            }
        }
    }

    public sealed class OwlcatBlueprint<T> : BlueprintInfoAbstract<T> where T : BlueprintScriptableObject
    {
        public OwlcatBlueprint(string guid) : base(guid) { }
        public override string Name => base.TryGetBlueprintInternal<T>()?.name ?? throw new NullReferenceException();

        //public T GetBlueprint() => base.TryGetBlueprintInternal<T>() ?? throw new NullReferenceException();

        private readonly Type blueprintType = typeof(T);
        public override Type BlueprintType => blueprintType;
    }

    public static class BlueprintInfoExtensions
    {
        //public static TRef GetBlueprintRef<T, U, TRef>(this NewBlueprint<T> obj)
        //    where T : U, new()
        //    where U : BlueprintScriptableObject, new()
        //    where TRef : BlueprintReference<U>, new()
        //    => obj.GetBlueprintRefInternal<TRef, U>();

        //public static TRef GetBlueprintRef<T, U, TRef>(this OwlcatBlueprint<T> obj)
        //    where T : U
        //    where U : BlueprintScriptableObject
        //    where TRef : BlueprintReference<U>, new()
        //    => obj.GetBlueprintRefInternal<TRef, U>();

        public static U? GetBlueprint<T, U>(this NewBlueprint<T> obj)
            where T : U, new()
            where U : BlueprintScriptableObject, new()
            => obj.GetBlueprint();

        public static U? GetBlueprint<T, U>(this OwlcatBlueprint<T> obj)
            where T : U
            where U : BlueprintScriptableObject
            => obj.GetBlueprint();
    }
}
