using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core {

    public class Util {

        private static bool? _isRunningOnMono;
        
        public static bool isRunningOnMono {
            get {
                if(!_isRunningOnMono.HasValue) {
                    _isRunningOnMono = (Type.GetType("Mono.Runtime") != null);
                }
                return _isRunningOnMono.Value;
            }
        }

        public static bool string2bool(string _val) {
            switch(_val.ToLower()) {
                case "y":
                case "yes":
                case "true":
                case "enabled":
                case "on":
                case "1":
                    return true;
                case "n":
                case "no":
                case "false":
                case "disabled":
                case "off":
                case "0":
                    return false;
                default:
                    throw new FLocalException("Cannot parse string '" + _val + "'; try to use 'n' or 'y'");
            }
        }

        public static T string2enum<T>(string value) {
            return (T)System.Enum.Parse(typeof(T), value);
        }

		public static void CheckEmail(string email) {
            if(!email.Contains('@') || !email.Contains('.')) throw new FLocalException("Wrong email '" + email + "'");
        }

        public static Predicate<string> PhoneValidator {
            get {
                return phone => {
                    return true;
                };
            }
        }

        public static T EnumParse<T>(string name) {
            return (T)Enum.Parse(typeof(T), name, true);
        }

        public static IEnumerable<int> Range(int start, int count, int step) {
            if(step == 0) {
                throw new FLocalException("Step could not be equal to zero");
            }
            for(int i=0; i<count; i++) {
                yield return start + (i*count);
            }
        }

        public static IEnumerable<int> Range(int start, int count) {
            return Range(start, count, 1);
        }

        public static IEnumerable<T> CreateList<T>(int length, Func<int, T> creator) {
            return from i in Range(0, length) select creator(i);
        }

        public static IEnumerable<T> CreateList<T>(int length, Func<T> creator) {
            return CreateList<T>(length, num => creator());
        }

        private static readonly Random RandomGenerator = new Random();

        public static int RandomInt(int gethan, int lessthan) {
            return RandomGenerator.Next(gethan, lessthan);
        }

        public enum RandomSource {
            LETTERS,
            LETTERS_DIGITS,
            CYRILLIC,
        }

        private static Dictionary<RandomSource, string> randoms = new Dictionary<RandomSource,string> {
            { RandomSource.LETTERS, "abcdefghijklmnopqrstuvwxyz" },
            { RandomSource.LETTERS_DIGITS, "abcdefghijklmnopqrstuvwxyz0123456789" },
            { RandomSource.CYRILLIC, "абвгдеёжзийклмнопрстуфхцчшщьыъэюя" },
        };
        
        public static string RandomString(int length, RandomSource source) {
            return new String(
                CreateList(length, (
                    (Func<string, char>)(str => str[RandomInt(0, str.Length)])
                ).Curry(
                    randoms[source]
                )).ToArray()
            );
        }

        public static string RandomString(int length) {
            return RandomString(length, RandomSource.LETTERS);
        }

		public static int? ParseInt(string raw) {
			if(raw == "") {
				return null;
			} else {
				return int.Parse(raw);
			}
		}

		public static DateTime? ParseDateTimeFromTimestamp(string raw) {
			if(raw == "") {
				return null;
			} else {
				long timestamp = long.Parse(raw);
				if(timestamp < 1) {
					return null;
				} else {
					return new DateTime(timestamp);
				}
			}
		}

		public static string EOL {
			get {
				return new string(new char[] { (char)0x0d, (char)0x0a });
			}
		}

		/// <summary>
		/// Code sample from http://msdn.microsoft.com/en-us/library/system.security.cryptography.md5.aspx
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string md5(string input) {
			System.Security.Cryptography.HashAlgorithm md5Hasher = System.Security.Cryptography.MD5.Create();

			// Convert the input string to a byte array and compute the hash.
			byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

			// Create a new Stringbuilder to collect the bytes
			// and create a string.
			StringBuilder sBuilder = new StringBuilder();

			// Loop through each byte of the hashed data 
			// and format each one as a hexadecimal string.
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			// Return the hexadecimal string.
			return sBuilder.ToString();
		}

		public static bool throws<TException>(Action action) where TException : Exception {
			try {
				action();
				return false;
			} catch(TException) {
				return true;
			}
		}

    }

}
