using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FLocal.Core {
	
	public static class ExtensionMethods {

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> kvps) {

            Dictionary<TKey, TValue> res = new Dictionary<TKey,TValue>();
            foreach(KeyValuePair<TKey, TValue> kvp in kvps) {
                res[kvp.Key] = kvp.Value;
            }
            return res;
        }

        public static TDestinationDictionary
            Convert<TSourceDictionary, TDestinationDictionary, TKey, TSource, TDestination>
            (this TSourceDictionary dict, Func<TSource, TDestination> converter)
            where TSourceDictionary : IDictionary<TKey, TSource>
            where TDestinationDictionary : IDictionary<TKey, TDestination>, new()
            {

                TDestinationDictionary res = new TDestinationDictionary();
                foreach(KeyValuePair<TKey, TSource> kvp in dict) {
                    res[kvp.Key] = converter(kvp.Value);
                }
                return res;
        }

        public delegate TResult RefFunc<T, TResult>(ref T arg);

        public static RefFunc<T, T> getProperty<T>(this object obj, Func<T> creator) {
            return (ref T arg) => {
                if(arg == null) {
                    arg = creator();
                }
                return arg;
            };
        }

        public static T getProperty<T>(this object obj, ref T arg, Func<T> creator) {
            return obj.getProperty<T>(creator)(ref arg);
        }

        public static string ToPrintableString(this bool val) {
            return val ? "Enabled" : "Disabled";
        }

		public static string ToPlainString(this bool val) {
			return val ? "true" : "false";
		}

		public static string ToPrintableString<T>(this IEnumerable<T> list) {
			return string.Join(",", (from elem in list select elem.ToString()).ToArray());
		}

        public static string ToDBString(this bool val) {
            return val ? "1" : "0";
        }

        public static T Safe<T>(this T obj, T defaultValue) {
            if(obj == null) {
                return defaultValue;
            } else {
                return obj;
            }
        }

        public static HashSet<T> Clone<T>(this HashSet<T> obj) {
            return new HashSet<T>(obj);
        }

        /// <summary>
        /// Note that actions from todoArr can be executed in any order!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="todoArr"></param>
        /// <returns></returns>
        public static T Tee<T>(this T obj, params Action<T>[] todoArr) {
            foreach(Action<T> todo in todoArr) todo(obj);
            return obj;
        }

        public static T Tee<T>(this T obj, Action todo) {
            todo();
            return obj;
        }

        private static List<TAttribute> GetAttributeArray<TAttribute>(this System.Reflection.PropertyInfo property) where TAttribute : Attribute {
            return new List<TAttribute>(from attribute in property.GetCustomAttributes(true) where /*Tie(attribute, obj => Console.WriteLine(obj.GetType().FullName))*/ attribute is TAttribute select (TAttribute)attribute);
        }

        public static TAttribute GetAttribute<TAttribute>(this System.Reflection.PropertyInfo property) where TAttribute : Attribute {
            return property.GetAttributeArray<TAttribute>().Single();
        }

        public static bool HasAttribute<TAttribute>(this System.Reflection.PropertyInfo property) where TAttribute : Attribute {
            return property.GetAttributeArray<TAttribute>().Count > 0;
        }

        public static TResult Invoke<TResult>(this System.Reflection.MethodBase method, object obj, params object[] args) {
            return (TResult)method.Invoke(obj, args);
        }

        public static void Invoke(this System.Reflection.MethodBase method, object obj, params object[] args) {
            method.Invoke(obj, args);
        }

        public static Dictionary<TKey, TResult> Replace<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dict, Func<TValue, TResult> Replacer) {
            return new List<KeyValuePair<TKey, TResult>>(
                from kvp in dict
                select new KeyValuePair<TKey, TResult>(kvp.Key, Replacer(kvp.Value))
            ).ToDictionary();
        }

        public static Dictionary<TKey, TValue> Replace<TKey, TValue>(this IDictionary<TKey, TValue> dict, Func<TValue, TValue> Replacer) {
            return dict.Replace<TKey, TValue, TValue>(Replacer);
        }

        public static T Single<T>(this IEnumerable<T> list, Predicate<T> checker, Func<T> onNotFound) {
            try {
                return list.Single(checker.ToFunc());
            } catch(InvalidOperationException) {
                return onNotFound();
            }
        }

        public static IEnumerable<string> Enquote(this IEnumerable<string> strings) {
            return from str in strings select str.Enquote();
        }

        public static TResult Check<TSource, TResult>(this TSource obj, Predicate<TSource> checker, Func<TSource, TResult> returnOnFail, TResult returnOnSuccess) {
            if(!checker(obj)) {
                return returnOnFail(obj);
            } else {
                return returnOnSuccess;
            }
        }

        public static TResult Check<TSource, TResult>(this TSource obj, Predicate<TSource> checker, Func<TSource, Exception> onFail, Func<TSource, TResult> returnOnSuccess) {
            if(!checker(obj)) {
                throw onFail(obj);
            } else {
                return returnOnSuccess(obj);
            }
        }
        
        public static void Check<T>(this T obj, Predicate<T> checker, Func<T, Exception> onFail) {
            if(!checker(obj)) {
                throw onFail(obj);
            }
        }

        public static bool Empty<T>(this IEnumerable<T> list) {
            return list.Count() == 0;
        }

        /// <summary>
        /// Checks arg given against the predicate given; in case of type mismatch returns false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        /// <param name="checker"></param>
        /// <returns></returns>
        public static bool TypedLoosePredicate<T>(this object arg, Predicate<T> checker) {
            return (arg is T) && checker((T)arg);
        }

        public static Func<object, string> SafeToStringConvertor() {
            return obj => obj == null ? "" : obj.ToString();
        }

        public static T Recurse<T>(this T obj, Func<T, T> step, Predicate<T> checker) {
            //Debugger.Debug("recurse(" + typeof(T).FullName + ")");
            int i=0;
            while(checker(obj)) {
                //Debugger.Debug("Iteration " + i.ToString());
                obj = step(obj);
                i++;
            }
            //Debugger.Debug("endrecurse(" + typeof(T).Name + ")");
            return obj;
        }

        public static T[] Slice<T>(this T[] obj, int offset, int length) {
            if(offset<0) throw new IndexOutOfRangeException();
            if(offset+length > obj.Length) throw new IndexOutOfRangeException();
            T[] res = new T[length];
            Array.Copy(obj, offset, res, 0, length);
            return res;
        }

        public static T[] Slice<T>(this T[] arr, int offset) {
            return arr.Slice(offset, arr.Length-offset);
        }

        public static T TryValues<T>(this T obj, params Func<T>[] trials) {
            //Debugger.Debug("TryValues(" + typeof(T).FullName + ")");
            return new KeyValuePair<T, Func<T>[]>(obj, trials).Recurse(
                kvp => new KeyValuePair<T, Func<T>[]>(kvp.Value[0](), kvp.Value.Slice(1)),
                kvp => !kvp.Value.Empty() && kvp.Key == null
            ).Key;
        }

        public static IEnumerable<T> Replace<T>(this IEnumerable<T> list, Func<T, T> replacer) {
            return from elem in list select replacer(elem);
        }

        internal static string ToStringOnFail(this bool test, Lazy<string> onFail) {
            return test ? "" : onFail();
        }

        public static string Join(this IEnumerable<string> strings, string separator) {
            return String.Join(separator, strings.ToArray());
        }

        public static IEnumerable<T> GetSorted<T>(this IEnumerable<T> list) {
            return from elem in list orderby elem select elem;
        }

        public static Int64 ToInt64(this decimal number) {
            return decimal.ToInt64(number);
        }

		public static string ToUTCString(this DateTime date) {
			return date.ToUniversalTime().ToString("u");
		}

		public static IEnumerable<T> ToSequence<T>(this T obj, Func<T, IEnumerable<T>> nextElementsGenerator) where T : class {
			yield return obj;
			foreach(var sub in nextElementsGenerator(obj)) {
				foreach(var subsub in sub.ToSequence(nextElementsGenerator)) {
					yield return subsub;
				}
			}
		}

		public static T Last<T>(this List<T> list) {
			return list[list.Count-1];
		}

		public static IEnumerable<T> Union<T>(this IEnumerable<T> enumerable, params T[] second) {
			return enumerable.Union((IEnumerable<T>)second);
		}

		/// <summary>
		/// Code taken from http://msdn.microsoft.com/en-us/library/system.io.stream.write.aspx
		/// </summary>
		/// <param name="input"></param>
		/// <param name="output"></param>
		public static void WriteTo(this Stream input, Stream output) {
			const int size = 4096;
			byte[] bytes = new byte[4096];
			int numBytes;
			while((numBytes = input.Read(bytes, 0, size)) > 0)
				output.Write(bytes, 0, numBytes);
		}

    }
}
