using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Core {
	public class Cache<T> {

		public static readonly Cache<T> instance = new Cache<T>();

		private Dictionary<object, T> cache;
	
		private Cache() {
			this.cache = new Dictionary<object,T>();
		}

		public T get(object id, Func<T> getter) {
			if(!this.cache.ContainsKey(id)) {
				lock(id) {
					if(!this.cache.ContainsKey(id)) {
						this.cache[id] = getter();
					}
				}
			}
			try {
 				return this.cache[id];
			} catch(KeyNotFoundException) {
				return this.get(id, getter);
			}
		}

		public void delete(object id) {
			lock(id) {
				if(this.cache.ContainsKey(id)) {
					this.cache.Remove(id);
				}
			}
		}

	}
}
