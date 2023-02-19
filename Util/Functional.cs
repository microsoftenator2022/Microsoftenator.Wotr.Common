using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoftenator.Wotr.Common.Util
{
    public static class Functional
    {
        public static void Ignore<T>(T x) { return; }
        public static void Ignore<T1, T2>(T1 x, T2 y) { return; }

        public static T Identity<T>(T x) => x;
        public static T Id<T>(T x) => Identity(x);

        public static void IgnoreRef<T>(ref T x) { return; }
        public static void IgnoreRef<T1, T2>(ref T1 x, ref T2 y) { return; }

        public static Func<TArg1, Func<TArg2, TReturn>> Curry<TArg1, TArg2, TReturn>(Func<TArg1, TArg2, TReturn> f) => arg1 => arg2 => f(arg1, arg2);
        public static Func<TArg1, TArg2, TReturn> UnCurry<TArg1, TArg2, TReturn>(Func<TArg1, Func<TArg2, TReturn>> f) => (x, y) => f(x)(y);

        public static Func<(T1, T2), U> Tupled<T1, T2, U>(Func<T1, T2, U> f) => ((T1 x, T2 y) t) => f(t.x, t.y);
        public static Func<(T1, T2, T3), U> Tupled3<T1, T2, T3, U>(Func<T1, T2, T3, U> f) => ((T1 x, T2 y, T3 z) t) => f(t.x, t.y, t.z);

        public static Func<T1, T3> Compose<T1, T2, T3>(Func<T1, T2> f, Func<T2, T3> g) => x => g(f(x));

        public delegate TReturn PartialFunc<TParam, TReturn>(params TParam[] x);
        public static PartialFunc<TParam, TReturn> Partial<TParam, TReturn>(this PartialFunc<TParam, TReturn> f, TParam p)
        {
            TReturn partialApply(TParam[] remaining) => f(new[] { p }.Concat(remaining).ToArray());

            return partialApply;
        }

        internal static Action<T> ContinueWith<T>(this Action<T> f, Action<T> g) => x => { f(x); g(x); };
        internal static Func<T, U> ContinueWith<T, U>(this Action<T> f, Func<T, U> g) => x => { f(x); return g(x); };
        internal static Func<T, U> ContinueWith<T, U>(this Func<T, U> f, Action<U> g) => x => { var y = f(x); g(y); return y; };
        internal static Func<T, T> Extract<T>(this Action<T> f) => f.ContinueWith(Id);

        public static class Enumerable
        {
            public static IEnumerable<T> Singleton<T>(T value) { yield return value; }
        }
    }
}
