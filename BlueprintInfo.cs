﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Blueprints;

using Microsoftenator.Wotr.Common.Blueprints.Extensions;
using Microsoftenator.Wotr.Common.Localization;
using Microsoftenator.Wotr.Common.Util;

namespace Microsoftenator.Wotr.Common
{
    public interface IBlueprintInfo<out TBlueprint> where TBlueprint : SimpleBlueprint
    {
        BlueprintGuid BlueprintGuid { get; }
        TBlueprint? TryGetBlueprint();
        TBlueprint GetBlueprint();
    }

    public abstract class BlueprintInfo
    {
        public string GuidString => BlueprintGuid.ToString();
        internal Guid Guid => BlueprintGuid.m_Guid;
        public BlueprintGuid BlueprintGuid { get; private set; }
        internal TRef GetBlueprintRefInternal<TRef, T>()
            where T : BlueprintScriptableObject
            where TRef : BlueprintReference<T>, new()
            => new() { deserializedGuid = BlueprintGuid };

        public virtual T? TryGetBlueprint<T>() where T : BlueprintScriptableObject
            => ResourcesLibrary.TryGetBlueprint<T>(BlueprintGuid);

        public abstract string Name { get; }
        public abstract Type BlueprintType { get; }

        internal BlueprintInfo(string guidString) : this(BlueprintGuid.Parse(guidString)) { }

        internal BlueprintInfo(BlueprintGuid guid)
        {
            BlueprintGuid = guid;
        }

        public virtual bool IsEnabled { get; }

        public virtual IEnumerable<BlueprintInfo> Dependencies { get; } = new List<BlueprintInfo>();

        public virtual bool CanBeEnabled => Dependencies.All(bpi => bpi.IsEnabled);
    }

    public abstract class BlueprintInfo<T> : BlueprintInfo, IBlueprintInfo<T> where T : BlueprintScriptableObject
    {
        internal BlueprintInfo(string guidString) : base(guidString) { }

        internal BlueprintInfo(BlueprintGuid guid) : base(guid) { }
        
        public virtual T? TryGetBlueprint() => TryGetBlueprint<T>();
        public virtual T GetBlueprint() => TryGetBlueprint() ?? throw new NullReferenceException();

        internal virtual bool Enabled { get; set; }
        public override bool IsEnabled => this.Enabled;
    }

    public class NewBlueprint<T> : BlueprintInfo<T> where T : BlueprintScriptableObject, new()
    {
        public virtual Action<T> Init { get; set; }

        public NewBlueprint(BlueprintGuid guid, string name) : base(guid)
        {
            this.name = name;
            Init = Functional.Ignore;
        }

        public NewBlueprint(string guidString, string name) : this(BlueprintGuid.Parse(guidString), name) { }
        
        private readonly string name;
        public override string Name => name;

        private readonly Type blueprintType = typeof(T);
        public override Type BlueprintType => blueprintType;
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
                fact.m_DisplayName = Helpers.Localization.LocalizedString.Empty;
                fact.m_Description = Helpers.Localization.LocalizedString.Empty;
                fact.m_DescriptionShort = Helpers.Localization.LocalizedString.Empty;

                DisplayName = getDisplayName();
                Description = getDescription();

                if (DisplayName is not null) fact.SetDisplayName(DisplayName);
                if (Description is not null) fact.SetDescription(Description);
            }

            if (bp is BlueprintFeature feature)
            {
                feature.IsClassFeature = true;
            }

            if (bp is BlueprintAbility ability)
            {
                ability.LocalizedDuration = Helpers.Localization.LocalizedString.Empty;
                ability.LocalizedSavingThrow = Helpers.Localization.LocalizedString.Empty;
                ability.MaterialComponent = new();
            }
        }
        
        public override Action<T> Init
        {
            get => bp => { InitDefaults(bp); base.Init(bp); };
            set => base.Init = value;
        }

        public NewUnitFact(BlueprintGuid guid, string name) : base(guid, name) { }
        public NewUnitFact(string guidString, string name) : base(BlueprintGuid.Parse(guidString), name) { }

        public NewUnitFact(
            BlueprintGuid guid,
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

        public NewUnitFact(
            string guidString,
            string name,
            LocalizedStringsPack strings,
            string? displayName = null,
            string? description = null) : this(BlueprintGuid.Parse(guidString), name, strings, displayName, description) { }
    }

    public sealed class OwlcatBlueprint<T> : BlueprintInfo<T> where T : BlueprintScriptableObject
    {
        public OwlcatBlueprint(BlueprintGuid guid) : base(guid) { }
        public OwlcatBlueprint(string guidString) : this(BlueprintGuid.Parse(guidString)) { }
        public override string Name => base.TryGetBlueprint<T>()?.name ?? throw new NullReferenceException();

        //public T GetBlueprint() => base.TryGetBlueprintInternal<T>() ?? throw new NullReferenceException();

        private readonly Type blueprintType = typeof(T);
        public override Type BlueprintType => blueprintType;
    }

    //public static class BlueprintInfoExtensions
    //{
    //    public static TRef GetBlueprintRef<T, U, TRef>(this BlueprintInfo<T> obj)
    //        where T : U
    //        where U : BlueprintScriptableObject
    //        where TRef : BlueprintReference<U>, new()
    //        => obj.GetBlueprintRefInternal<TRef, U>();

    //    public static U? GetBlueprint<T, U>(this BlueprintInfo<T> obj)
    //        where T : U
    //        where U : BlueprintScriptableObject, new()
    //        => obj.GetBlueprint();
    //}
}
