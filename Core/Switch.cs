using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core {

    class Switch<TResult> : List<KeyValuePair<Predicate, Func<TResult>>> {

        public void Add(Predicate checker, Func<TResult> calculator) {
            this.Add(new KeyValuePair<Predicate,Func<TResult>>(checker, calculator));
        }

        public void Add(bool checkResult, Func<TResult> calculator) {
            this.Add(() => checkResult, calculator);
        }

        public TResult Value {
            get {
                return this.Single(kvp => kvp.Key(), () => {
                    throw new FLocalException("Not found");
                }).Value();
            }
        }

    }

}
