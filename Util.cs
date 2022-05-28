using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoftenator.Wotr.Common.Util
{
    public static class Functional
    {
        public static void Ignore<T>(T x) { return; }
        public static void Ignore<T1, T2>(T1 x, T2 y) { return; }

        public static T Id<T>(T x) { return x; }

        public static void IgnoreRef<T>(ref T x) { return; }
        public static void IgnoreRef<T1, T2>(ref T1 x, ref T2 y) { return; }

        public static Func<TArg1, Func<TArg2, TReturn>> Curry<TArg1, TArg2, TReturn>(Func<TArg1, TArg2, TReturn> f) => arg1 => arg2 => f(arg1, arg2);
        public static Func<TArg1, TArg2, TReturn> UnCurry<TArg1, TArg2, TReturn>(Func<TArg1, Func<TArg2, TReturn>> f) => (x, y) => f(x)(y);

        public static Func<(T1, T2), U> Tupled<T1, T2, U>(Func<T1, T2, U> f) => ((T1 x, T2 y)t) => f(t.x, t.y);
        public static Func<(T1, T2, T3), U> Tupled3<T1, T2, T3, U>(Func<T1, T2, T3, U> f) => ((T1 x, T2 y, T3 z) t) => f(t.x, t.y, t.z);

        public static Func<T1, T3> Compose<T1, T2, T3>(Func<T1, T2> f, Func<T2, T3> g) => x => g(f(x));

        public delegate TReturn PartialFunc<TParam, TReturn>(params TParam[] x);
        public static PartialFunc<TParam, TReturn> Partial<TParam, TReturn>(this PartialFunc<TParam, TReturn> f, TParam p)
        {
            TReturn partialApply(TParam[] remaining) => f(new[] { p }.Concat(remaining).ToArray());

            return partialApply;
        }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Singleton<T>(T value) { yield return value; }
        public static T Head<T>(this IEnumerable<T> s) => s.First();
        public static IEnumerable<T> Tail<T>(this IEnumerable<T> s) => s.Skip(1);
        public static (T head, IEnumerable<T> tail) Deconstruct<T>(this IEnumerable<T> s) => (s.Head(), s.Tail());

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
