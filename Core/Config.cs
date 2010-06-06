using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core {

	class Config<T> where T : Config<T> {

		private static Config<T> _instance;

		public static Config<T> instance {
			get {
				if(_instance == null) throw new FLocalException("not initialized");
				return _instance;
			}
			private set {
				lock(_instance) {
					if(_instance != null) throw new FLocalException("already initialized");
					_instance = value;
				}
			}
		}

		private Config() {
		}

		public static void doInit(Config<T> config) {
			_instance = config;
		}

	}
}
