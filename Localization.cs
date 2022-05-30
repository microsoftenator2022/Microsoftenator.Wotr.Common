using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Kingmaker.Blueprints.Facts;
using Kingmaker.Localization;
using Kingmaker.Localization.Shared;

using Microsoftenator.Wotr.Common.Localization.Extensions;
using Microsoftenator.Wotr.Common.Util;

namespace Microsoftenator.Wotr.Common.Localization
{
    public class LocalizedStringsPack //: IDictionary<string, string> //, IDictionary<string, LocalizedString>
    {
        public readonly Locale? Locale;

        internal readonly Dictionary<string, string> Strings = new();
        private Dictionary<string, string> LoadedStrings = new();

        //public IEnumerable<LocalizedString> LocalizedStrings
        //    => LoadedStrings.Select(LocalizationHelpers.LocalizedString);

        public void Add(string key, string text) 
        {
            Strings[key] = text;
        }

        public LocalizedString? Get(string key)
        {
            if (!LoadedStrings.ContainsKey(key))
            { 
                if(!Strings.ContainsKey(key))
                    return null;

                LoadNew();
            }

            return LocalizationHelpers.LocalizedString(key);
        }

        public void LoadNew()
        {
            var newStrings = Strings.Where(s => !LoadedStrings.Contains(s)).ToDictionary();

            LocalizationManager.CurrentPack.AddStrings(ToLocalizationPack(Locale, newStrings));

            LoadedStrings = Strings;
        }

        public void LoadAll()
        {
            LocalizationManager.CurrentPack.AddStrings(ToLocalizationPack());

            LoadedStrings = Strings;
        }

        public LocalizedStringsPack(Locale? locale = null)
        {
            Locale = locale;
        }

        internal static LocalizationPack ToLocalizationPack(Locale? locale, Dictionary<string, string> strings)
        {
            return new LocalizationPack()
            {
                Locale = locale ?? LocalizationManager.CurrentLocale,
                m_Strings = strings.MapValues(value => new LocalizationPack.StringEntry { Text = value })
            };
        }

        public LocalizationPack ToLocalizationPack() => ToLocalizationPack(Locale, Strings);

        #region IDictionary<string, string>
        //void IDictionary<string, string>.Add(string key, string value)
        //{
        //    Strings.Add(key, value);
        //    Loaded = false;
        //}

        //void ICollection<KeyValuePair<string, string>>.Clear()
        //{
        //    Strings.Clear();
        //    Loaded = false;
        //}

        //bool IDictionary<string, string>.Remove(string key) => Strings.Remove(key);
        //string IDictionary<string, string>.this[string key]
        //{
        //    get => Strings[key];
        //    set => ((IDictionary<string, string>)this).Add(key, value);
        //}

        //ICollection<string> IDictionary<string, string>.Keys => Strings.Keys;

        //ICollection<string> IDictionary<string, string>.Values => Strings.Values;

        //int ICollection<KeyValuePair<string, string>>.Count => Strings.Count;

        //bool ICollection<KeyValuePair<string, string>>.IsReadOnly
        //    => ((ICollection<KeyValuePair<string, string>>)Strings).IsReadOnly;

        //void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item)
        //    => ((IDictionary<string, string>)this).Add(item.Key, item.Value);

        //bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item)
        //    => ((ICollection<KeyValuePair<string, string>>)Strings).Contains(item);

        //bool IDictionary<string, string>.ContainsKey(string key) => Strings.ContainsKey(key);
        //void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        //    => ((ICollection<KeyValuePair<string, string>>)Strings).CopyTo(array, arrayIndex);

        //IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
        //    => Strings.GetEnumerator();

        //bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item)
        //    => ((IDictionary<string, string>)this).Remove(item.Key);

        //bool IDictionary<string, string>.TryGetValue(string key, out string value)
        //    => Strings.TryGetValue(key, out value);

        //IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<KeyValuePair<string, string>>)this).GetEnumerator();
        #endregion

        //public IReadOnlyDictionary<string, LocalizedString> LocalizedStrings
        //    => Strings.Keys.ToDictionary
        //    (
        //        keySelector: Functional.Id,
        //        elementSelector: LocalizationHelpers.LocalizedString
        //    );

        #region IDictionary<string, LocalizedString>
        //LocalizedString IDictionary<string, LocalizedString>.this[string key]
        //{
        //    get => LocalizedStrings[key];
        //    set => throw new InvalidOperationException();
        //}

        //ICollection<string> IDictionary<string, LocalizedString>.Keys => LocalizedStrings.Keys;
        //ICollection<LocalizedString> IDictionary<string, LocalizedString>.Values => LocalizedStrings.Values;
        //int ICollection<KeyValuePair<string, LocalizedString>>.Count => LocalizedStrings.Count;
        //bool ICollection<KeyValuePair<string, LocalizedString>>.IsReadOnly => true;
        //void IDictionary<string, LocalizedString>.Add(string key, LocalizedString value)
        //    => throw new InvalidOperationException();

        //void ICollection<KeyValuePair<string, LocalizedString>>.Add(KeyValuePair<string, LocalizedString> item)
        //    => throw new InvalidOperationException();

        //void ICollection<KeyValuePair<string, LocalizedString>>.Clear() => throw new InvalidOperationException();
        //bool ICollection<KeyValuePair<string, LocalizedString>>.Contains(KeyValuePair<string, LocalizedString> item)
        //    => LocalizedStrings.Contains(item);

        //bool IDictionary<string, LocalizedString>.ContainsKey(string key) => LocalizedStrings.ContainsKey(key);
        //void ICollection<KeyValuePair<string, LocalizedString>>.CopyTo(KeyValuePair<string, LocalizedString>[] array, int arrayIndex)
        //    => LocalizedStrings.CopyTo(array, arrayIndex);

        //IEnumerator<KeyValuePair<string, LocalizedString>> IEnumerable<KeyValuePair<string, LocalizedString>>.GetEnumerator()
        //    => LocalizedStrings.GetEnumerator();

        //bool IDictionary<string, LocalizedString>.Remove(string key) => throw new InvalidOperationException();
        //bool ICollection<KeyValuePair<string, LocalizedString>>.Remove(KeyValuePair<string, LocalizedString> item)
        //    => throw new InvalidOperationException();

        //bool IDictionary<string, LocalizedString>.TryGetValue(string key, out LocalizedString value)
        //    => LocalizedStrings.TryGetValue(key, out value);

        //IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)LocalizedStrings).GetEnumerator();
        #endregion
    }

    public static class LocalizationHelpers
    {
        public static LocalizationPack.StringEntry CreateStringEntry(string text) => new() { Text = text };
        
        public static LocalizationPack CreateLocalizationPack(Dictionary<string, LocalizationPack.StringEntry> strings)
            => new() { m_Strings =  strings};
        
        public static LocalizedString LocalizedString(string key) => new() { m_Key = key };

//#if DEBUG
//        [Obsolete]
//#endif
//        public static LocalizedString DefineString(string key, string text)
//        {
//            LocalizationManager.CurrentPack.AddString(key, text);

//            return LocalizedString(key);
//        }

        //public static LocalizedString DefineString(this LocalizedStringsPack pack, string key, string text)
        //    => pack.Add(key, text);
    }
}

namespace Microsoftenator.Wotr.Common.Localization.Extensions
{
    public static class LocalizationExtensions
    {
        public static LocalizedString GetDisplayNameLocalizedString(this BlueprintUnitFact fact) => fact.m_DisplayName;
        public static LocalizedString GetDescriptionLocalizedString(this BlueprintUnitFact fact) => fact.m_Description;

        //[Obsolete]
        //public static LocalizedString CopyWith(this LocalizedString ls, string key, Func<string, string> mutator)
        //    => LocalizationHelpers.DefineString(key, mutator(LocalizationManager.CurrentPack.GetText(ls)));

        //[Obsolete]
        //public static LocalizedString Copy(this LocalizedString ls, string key) => ls.CopyWith(key, Functional.Id);

        public static void AddStrings(this LocalizationPack pack, IEnumerable<(string key, string text)> strings)
        {
            var dict = new Dictionary<string, LocalizationPack.StringEntry>();

            foreach ((var key, var text) in strings)
            {
                dict[key] = LocalizationHelpers.CreateStringEntry(text);
            }

            pack.AddStrings(LocalizationHelpers.CreateLocalizationPack(dict));
        }

        public static void AddString(this LocalizationPack pack, string key, string text)
            => pack.AddStrings(new[] { (key, text) });
    }
}
