using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoftenator.Wotr.Common.Util
{
    public class Deferred<T, U>
    {
        private readonly Func<T, U> State;
        private Action<T> Prefix = Functional.Ignore;
        private Action<T> Postfix = Functional.Ignore;
        
        private Deferred(Func<T, U> state) => State = state;

        public Func<T, U> Return => x =>
        {
            Prefix(x);

            var y = State(x);

            Postfix(x);

            return y;
        };

        public Deferred<T, TNext> Add<TNext>(Func<U, TNext> f) => new(Functional.Compose(State, f));

        public Deferred<T, U> AddPrefix(Action<T> f) => new(State) { Prefix = this.Prefix.ContinueWith(f) };
        public Deferred<T, U> AddPostfixOnInput(Action<T> f) => new(State) { Postfix = this.Postfix.ContinueWith(f) };
    }
}
