using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core {

	public class Config<T> where T : Config<T> {

		private static T _instance;

		public static T instance {
			get {
				if(_instance == null) throw new FLocalException("not initialized");
				return _instance;
			}
		}

		protected Config() {
		}

		protected static void doInit(Func<T> configCreator) {
			if(_instance != null) throw new FLocalException("already initialized");
			lock(_instance) {
				if(_instance != null) throw new FLocalException("already initialized");
				_instance = configCreator();
			}
		}

	}
}
