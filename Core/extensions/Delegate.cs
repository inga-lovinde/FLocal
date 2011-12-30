using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Core {
    
	static class DelegateExtension {

        public static Func<T, bool> ToFunc<T>(this Predicate<T> p) {
            return arg => p(arg);
        }

        public static Func<TResult> Curry<T1, TResult>(this Func<T1, TResult> func, T1 arg1) {
            return () => func(arg1);
        }

        public static Action Curry<T1>(this Action<T1> action, T1 arg1) {
            return () => action(arg1);
        }

        public static Func<TResult> Curry<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 arg1, T2 arg2) {
            return () => func(arg1, arg2);
        }

        public static Action Curry<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2) {
            return () => action(arg1, arg2);
        }

        public static Func<T2, TResult> LCurry<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 arg1) {
            return arg2 => func(arg1, arg2);
        }

        public static Action<T2> LCurry<T1, T2>(this Action<T1, T2> action, T1 arg1) {
            return arg2 => action(arg1, arg2);
        }

        public static Func<T1, TResult> RCurry<T1, T2, TResult>(this Func<T1, T2, TResult> func, T2 arg2) {
            return arg1 => func(arg1, arg2);
        }

        public static Action<T1> RCurry<T1, T2>(this Action<T1, T2> action, T2 arg2) {
            return arg1 => action(arg1, arg2);
        }

    }
}
