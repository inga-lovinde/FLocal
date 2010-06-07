using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace FLocal.Core {

	public abstract class Config<T> : IDisposable where T : Config<T> {

		private static T _instance = null;

		public static T instance {
			get {
				if(_instance == null) throw new FLocalException("not initialized");
				return _instance;
			}
		}

		private NameValueCollection data;

		public string AppInfo {
			get {
				return data["AppInfo"];
			}
		}

		protected Config(NameValueCollection data) {
			this.data = data;
		}

		protected static void doInit(Func<T> configCreator) {
			if(_instance != null) throw new FLocalException("already initialized");
			lock(typeof(Config<T>)) {
				if(_instance != null) throw new FLocalException("already initialized");
				_instance = configCreator();
			}
		}

		protected static void doReInit(Func<T> configCreator) {
			lock(typeof(Config<T>)) {
				_instance = configCreator();
			}
		}

		public static bool isInitialized {
			get {
				return _instance != null;
			}
		}

		public virtual void Dispose() {
		}

	}
}
