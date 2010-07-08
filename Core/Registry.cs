using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core {
    class Registry<TKey, TData>
        where TData : IDataObject<TKey, TData>, new()
        where TKey : struct {

        internal static readonly Registry<TKey, TData> instance = new Registry<TKey,TData>();

        private volatile Dictionary<TKey, TData> storage;

        private volatile Dictionary<TKey, object> locks;

        private Registry() {
            this.storage = new Dictionary<TKey,TData>();
            this.locks = new Dictionary<TKey, object>();
        }
        
        internal TData Get(TKey id, bool forLoadingFromHash) {
			if(!this.storage.ContainsKey(id)) {
				lock(this.locks) {
					if(!this.storage.ContainsKey(id)) {
						TData obj = new TData();
						obj.CreateByIdFromRegistry(id, forLoadingFromHash);
						this.storage[id] = obj;
					}
				}
			}

			lock(this.locks) {
				if(this.storage.ContainsKey(id)) {
					return this.storage[id];
				}
			}

			return this.Get(id, forLoadingFromHash);
        }

        public bool IsCached(TKey id) {
            return this.storage.ContainsKey(id);
        }

		internal void Clear() {
			lock(this.locks) {
				this.storage.Clear();
			}
		}

		internal void Delete(TKey[] idsToDelete) {
			lock(this.locks) {
				foreach(TKey id in idsToDelete) {
					IDataObject<TKey, TData> obj = null;
					if(this.storage.ContainsKey(id)) {
						obj = this.storage[id];
						this.storage.Remove(id);
						obj.markAsDeletedFromRegistry();
					}
				}
			}
		}

    }

}
