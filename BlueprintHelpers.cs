using System;
using System.Collections.Generic;
using System.Linq;

//using MoreLinq;

using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Components;

using Microsoftenator.Wotr.Common.Localization;
using Microsoftenator.Wotr.Common.Util;

//namespace Microsoftenator.Wotr.Common.Blueprints
//{
//    public static class Helpers
//    {
//        [Obsolete]
//        public static class Unchecked
//        {
//            public static TRef CreateBlueprintRef<TRef, T>(BlueprintGuid guid)
//                where T : BlueprintScriptableObject
//                where TRef : BlueprintReference<T>, new()
//                => new() { deserializedGuid = guid };
//        }

//        [Obsolete]
//        public static TBlueprint CreateBlueprint<TBlueprint>(string name, BlueprintGuid assetId, Action<TBlueprint> init)
//            where TBlueprint : BlueprintScriptableObject, new()
//        {
//            if(ResourcesLibrary.TryGetBlueprint<TBlueprint>(assetId) is not null) throw new ArgumentException();

//            TBlueprint bp = new()
//            {
//                AssetGuid = assetId,
//                name = name
//            };

//            init(bp);

//            ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(assetId, bp);

//            return bp;
//        }

//        [Obsolete]
//        public static TBlueprint CreateBlueprint<TBlueprint>(string name, Guid guid, Action<TBlueprint> init)
//            where TBlueprint : BlueprintScriptableObject, new()
//            => CreateBlueprint(name, new BlueprintGuid(guid), init);

//        [Obsolete]
//        public static TBlueprint CreateBlueprint<TBlueprint>(string name, Guid guid)
//            where TBlueprint : BlueprintScriptableObject, new()
//            => CreateBlueprint<TBlueprint>(name, guid, Functional.Ignore);

//        [Obsolete]
//        public static TBlueprint CreateBlueprint<TBlueprint>(NewBlueprint<TBlueprint> bpi, Action<TBlueprint> init)
//            where TBlueprint : BlueprintScriptableObject, new()
//            => CreateBlueprint<TBlueprint>(name: bpi.Name, assetId: bpi.BlueprintGuid, init: bp => { bpi.Init(bp); init(bp); });

//        [Obsolete]
//        public static TBlueprint CreateBlueprint<TBlueprint>(NewBlueprint<TBlueprint> bpi)
//            where TBlueprint : BlueprintScriptableObject, new()
//            => CreateBlueprint(bpi, init: Functional.Ignore);

//        //public static AddFeatureIfHasFact AddFeatureIfHasFact(BlueprintUnitFactReference checkedFact, BlueprintUnitFactReference feature, Action<AddFeatureIfHasFact> init)
//        //{
//        //    var component = new AddFeatureIfHasFact()
//        //    {
//        //        m_CheckedFact = checkedFact,
//        //        m_Feature = feature
//        //    };

//        //    init(component);

//        //    return component;
//        //}
//    }
//}

namespace Microsoftenator.Wotr.Common.Blueprints.Extensions
{
    public static class BlueprintExtensions
    {
        //[Obsolete]
        //public static TBlueprint Clone<TBlueprint>(
        //    this TBlueprint original,
        //    string name,
        //    Guid guid,
        //    Action<TBlueprint> init)
        //    where TBlueprint : BlueprintScriptableObject
        //{

        //    TBlueprint copy = TTT_Utils.Clone(original);

        //    copy.name = name;
        //    copy.AssetGuid = new BlueprintGuid(guid);

        //    init(copy);

        //    ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(copy.AssetGuid, copy);

        //    return copy;
        //}

        //[Obsolete]
        //public static TBlueprint Clone<TBlueprint>(
        //    this TBlueprint original,
        //    string name,
        //    string guid,
        //    Action<TBlueprint> init)
        //    where TBlueprint : BlueprintScriptableObject
        //    => original.Clone(name, Guid.Parse(guid), init);

        //[Obsolete]
        //public static TBlueprint Clone<TBlueprint>(
        //    this TBlueprint original,
        //    NewBlueprint<TBlueprint> data,
        //    Action<TBlueprint> init)
        //    where TBlueprint : BlueprintScriptableObject, new()
        //    => original.Clone(data.Name, data.Guid, init);

        //[Obsolete]
        //public static TBlueprint CloneFeature<TBlueprint>(
        //    this TBlueprint original,
        //    NewBlueprint<TBlueprint> bpi,
        //    Action<TBlueprint> init)
        //    where TBlueprint : BlueprintFeature, new()
        //{
        //    var copy = original.Clone(bpi.Name, bpi.Guid, bp => { bpi.Init(bp); init(bp); });

        //    return copy;
        //}

        public static void AddComponent<TComponent>(this BlueprintScriptableObject blueprint, TComponent component) where TComponent : BlueprintComponent
        {
            // Apparently components need a unique name
            if (String.IsNullOrEmpty(component.name))
                component.name = $"${typeof(TComponent)}${component.GetHashCode():x}";

            blueprint.ComponentsArray = blueprint.ComponentsArray.Append(component).ToArray();
        }
    }

    public static class BlueprintFeatureExtensions
    {
        public static void SetIcon(this BlueprintFeature feature, UnityEngine.Sprite? icon) => feature.m_Icon = icon;

        public static void AddComponent<TComponent>(this BlueprintFeature feat, TComponent component)
            where TComponent : BlueprintComponent => ((BlueprintUnitFact)feat).AddComponent(component);

        public static void RemoveComponents(this BlueprintFeature feat, Func<BlueprintComponent, bool> predicate)
            //=> feat.ComponentsArray = feat.ComponentsArray.Where(c => !predicate(c)).ToArray();
            => ((BlueprintUnitFact)feat).RemoveComponents(predicate);
        
        public static void AddPrerequisite<TPrerequisite>(this BlueprintFeature feat, Action<TPrerequisite> init,
            Prerequisite.GroupType group = Prerequisite.GroupType.All)
            where TPrerequisite : Prerequisite, new()
        {
            TPrerequisite p = new()
            {
                Group = group
            };

            init(p);

            feat.AddComponent(p);
        }

        public static void RemoveFeatureOnApply(this BlueprintFeature feat, BlueprintFeature blueprintToRemove)
            => feat.AddComponent(new RemoveFeatureOnApply()
            {
                m_Feature = blueprintToRemove.ToReference<BlueprintUnitFactReference>() 
            });

        public static void AddPrerequisiteFeature(this BlueprintFeature feat,
            BlueprintFeature prerequisiteFeature, Action<PrerequisiteFeature> init,
            Prerequisite.GroupType group = Prerequisite.GroupType.All, bool removeOnApply = false,
            bool addIsPrerequisiteFor = false)
        {
            feat.AddPrerequisite<PrerequisiteFeature>(p =>
            {
                p.m_Feature = prerequisiteFeature.ToReference<BlueprintFeatureReference>();

                init(p);
            }, group);

            if (addIsPrerequisiteFor)
            {
                if (prerequisiteFeature.IsPrerequisiteFor is null)
                    prerequisiteFeature.IsPrerequisiteFor = new();

                if (!prerequisiteFeature.IsPrerequisiteFor.Contains(feat.ToReference<BlueprintFeatureReference>()))
                    prerequisiteFeature.IsPrerequisiteFor.Add(feat.ToReference<BlueprintFeatureReference>());
            }

            if (removeOnApply)
                feat.RemoveFeatureOnApply(prerequisiteFeature);
        }

        public static void AddPrerequisiteNoFeature(this BlueprintFeature feat,
            BlueprintFeature prerequisiteFeature, Action<PrerequisiteNoFeature> init,
            Prerequisite.GroupType group = Prerequisite.GroupType.All)
            => feat.AddPrerequisite<PrerequisiteNoFeature>(p =>
            {
                p.m_Feature = prerequisiteFeature.ToReference<BlueprintFeatureReference>();

                init(p);
            }, group);

        public static void AddFeatureCallback<TDelegate>(this BlueprintFeature feature, TDelegate callback)
            where TDelegate : UnitFactComponentDelegate
            => feature.AddComponent(callback);
        
        public static void AddClass(this BlueprintProgression progression, BlueprintCharacterClass c, int additionalLevel = 0)
        {
            progression.m_Classes = progression.m_Classes.Append(new ()
            {
                m_Class = c.ToReference<BlueprintCharacterClassReference>(),
                AdditionalLevel = additionalLevel
            }).ToArray();
        }
    }

    public static class BlueprintFeatureSelectionExtensions
    {
        public static void AddFeature(this BlueprintFeatureSelection selection, BlueprintFeature feature,
            bool allowDuplicates = false, bool ignorePrerequisites = false)
            => AddFeature(selection, feature.ToReference<BlueprintFeatureReference>(), allowDuplicates, ignorePrerequisites);

        public static void AddFeature(this BlueprintFeatureSelection selection, BlueprintFeatureReference feature,
            bool allowDuplicates = false, bool ignorePrerequisites = false)
        {
            //BlueprintFeatureReference[] featureRefs = selection.Features;

            if (!allowDuplicates && (selection.m_Features.Contains(feature) || selection.m_AllFeatures.Contains(feature))) return;
            
            selection.IgnorePrerequisites = ignorePrerequisites;

            selection.m_Features = selection.m_Features.Append(feature).ToArray();

            selection.m_AllFeatures = selection.m_AllFeatures.Append(feature).ToArray();
        }

        public static void SetFeatures(this BlueprintFeatureSelection selection, BlueprintFeatureReference[] features, BlueprintFeatureReference[]? allFeatures = null)
        {
            selection.m_Features = features;

            selection.m_AllFeatures = allFeatures ?? features;
        }

        public static void RemoveFeature(this BlueprintFeatureSelection selection, BlueprintFeature feature)
        {
            selection.m_Features = selection.m_Features.SkipWhile(f => f.Get().AssetGuid == feature.AssetGuid).ToArray();
            selection.m_AllFeatures = selection.m_AllFeatures.SkipWhile(f => f.Get().AssetGuid == feature.AssetGuid).ToArray();
        }
    }

    public static class BlueprintRaceExtensions
    {
        public static void AddFeature(this BlueprintRace race, BlueprintFeatureBase feature)
            => race.m_Features = race.m_Features.Append(feature.ToReference<BlueprintFeatureBaseReference>()).ToArray();

        public static void AddFeatures(this BlueprintRace race, BlueprintFeatureBase[] features)
            => race.m_Features =
                race.m_Features
                    .Concat(features.Select(f => f.ToReference<BlueprintFeatureBaseReference>()))
                    .ToArray();

        public static void SetFeatures(this BlueprintRace race, BlueprintFeatureBaseReference[] features)
            => race.m_Features = features;

        public static void RemoveFeature(this BlueprintRace race, BlueprintFeatureBase feature)
        {
            var featureRef = feature.ToReference<BlueprintFeatureBaseReference>();

            race.m_Features = race.m_Features.SkipWhile(f => f.Get().AssetGuid == feature.AssetGuid).ToArray();
        }
    }

    public static class BlueprintUnitFactExtensions
    {
        public static void AddComponent<TComponent>(this BlueprintUnitFact fact, TComponent component) where TComponent : BlueprintComponent
        {
            // Apparently components need a unique name
            if (String.IsNullOrEmpty(component.name))
                component.name = $"{fact.Name}${typeof(TComponent)}${component.GetHashCode():x}";

            fact.ComponentsArray = fact.ComponentsArray.Append(component).ToArray();
        }

        public static void RemoveComponents(this BlueprintUnitFact fact, Func<BlueprintComponent, bool> predicate)
            => fact.ComponentsArray = fact.ComponentsArray.Where(c => !predicate(c)).ToArray();

        public static void SetDisplayName(this BlueprintUnitFact bp, LocalizedString? displayName)
            => bp.m_DisplayName = displayName;

        public static void SetDisplayName(this BlueprintUnitFact bp, LocalizedStringsPack strings, string text)
        {
            var key = $"{bp.name}.Name";
            strings.Add(key, text);
            bp.SetDisplayName(strings.Get(key));
        }

        public static LocalizedString GetDisplayName(this BlueprintUnitFact bp) => bp.m_DisplayName;

        public static void SetDescription(this BlueprintUnitFact bp, LocalizedString? description)
            => bp.m_Description = description;

        public static void SetDescription(this BlueprintUnitFact bp, LocalizedStringsPack strings, string text)
        {
            var key = $"{bp.name}.Description";
            strings.Add(key, text);
            bp.SetDescription(strings.Get(key));
        }

        public static LocalizedString GetDescription(this BlueprintUnitFact bp) => bp.m_Description;

        public static void CopyStringsFrom(this BlueprintUnitFact fact, BlueprintUnitFact other)
        {
            fact.m_DisplayName = other.m_DisplayName;
            fact.m_Description = other.m_DisplayName;
            fact.m_DescriptionShort = other.m_DescriptionShort;
        }
    }

    public static class BlueprintCharacterClassExtensions
    {
        public static void AddPrerequisite<TPrerequisite>(this BlueprintCharacterClass feat, Action<TPrerequisite> init,
            Prerequisite.GroupType group = Prerequisite.GroupType.All)
            where TPrerequisite : Prerequisite, new()
        {
            TPrerequisite p = new()
            {
                Group = group
            };

            init(p);

            feat.AddComponent(p);
        }

        public static void SetDisplayName(this BlueprintCharacterClass bp, LocalizedString? displayName)
            => bp.LocalizedName = displayName;
        
        public static void SetDisplayName(this BlueprintCharacterClass bp, LocalizedStringsPack strings, string text)
        {
            var key = $"{bp.name}.Name";
            strings.Add(key, text);
            bp.SetDisplayName(strings.Get(key));
        }

        public static LocalizedString GetDisplayName(this BlueprintCharacterClass bp) => bp.LocalizedName;

        public static void SetDescription(this BlueprintCharacterClass bp, LocalizedString? description)
            => bp.LocalizedDescription = description;

        public static void SetDescription(this BlueprintCharacterClass bp, LocalizedStringsPack strings, string text)
        {
            var key = $"{bp.name}.Description";
            strings.Add(key, text);
            bp.SetDescription(strings.Get(key));
        }

        public static LocalizedString GetDescription(this BlueprintCharacterClass bp) => bp.LocalizedDescription;
    }

    //public static class ContextRankConfigExtensions
    //{
    //    public static ContextRankConfig CreateContextRankConfigFeatureList(BlueprintFeatureReference[] feats)
    //        => new()
    //        {
    //            m_BaseValueType = ContextRankBaseValueType.FeatureList,
    //            m_FeatureList = feats,
    //        };

    //    public static ContextRankConfig.CustomProgressionItem CreateProgressionItem(int baseValue, int progressionValue)
    //        => new() { BaseValue = baseValue, ProgressionValue = progressionValue };

    //    public static RecalculateOnFactsChange CreateRofcComponent(BlueprintUnitFactReference[] checkedFacts)
    //        => new() { m_CheckedFacts = checkedFacts };
    //    public static void SetCustomProgression(this ContextRankConfig crc,
    //        ContextRankConfig.CustomProgressionItem[] progressionItems)
    //    {
    //        crc.m_Progression = ContextRankProgression.Custom;
    //        crc.m_CustomProgression = progressionItems;
    //    }
    //}
}