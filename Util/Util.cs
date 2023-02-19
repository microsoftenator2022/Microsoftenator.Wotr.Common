using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoftenator.Wotr.Common.Util
{
    public static class ArrayExtensions
    {
        public static T[] Append<T>(this T[] array, T value) => Enumerable.Append(array, value).ToArray();
    }

    public static class EnumerableExtensions
    {
        public static T Head<T>(this IEnumerable<T> s) => s.First();
        public static IEnumerable<T> Tail<T>(this IEnumerable<T> s) => s.Skip(1);
        public static (T head, IEnumerable<T> tail) Deconstruct<T>(this IEnumerable<T> s) => (s.Head(), s.Tail());

        public static void Deconstruct<T>(this IEnumerable<T> s, out T head, out IEnumerable<T> tail)
        {
            head = s.First();
            tail = s.Skip(1);
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> pairs)
            => Enumerable.ToDictionary(pairs, keySelector: item => item.Key, elementSelector: item => item.Value);

        public static Dictionary<TKeyOut, TValueOut> Map<TKeyIn, TValueIn, TKeyOut, TValueOut>(
            this IDictionary<TKeyIn, TValueIn> source,
            Func<TKeyIn, TKeyOut> keyMapper,
            Func<TValueIn, TValueOut> valueMapper)
            => Enumerable.ToDictionary
            (
                source: source,
                keySelector: item => keyMapper(item.Key),
                elementSelector: item => valueMapper(item.Value)
            );

        public static Dictionary<TKey, TValueOut> MapValues<TKey, TValueIn, TValueOut>(
            this IDictionary<TKey, TValueIn> source, Func<TValueIn, TValueOut> mapper)
            => source.Map(Functional.Id, mapper);
    }

    public static class TTT_Utils
    {
        public static T Clone<T>(T original) where T : notnull
            => (T)(TabletopTweaks.Core.Utilities.ObjectDeepCopier.Clone(original) ?? throw new NullReferenceException());
    }
}
