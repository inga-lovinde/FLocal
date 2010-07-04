using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core {
    class Registry<TKey, TData>
        where TData : IDataObject<TKey, TData>, new()
        where TKey : struct {

        internal static readonly Registry<TKey, TData> instance = new Registry<TKey,TData>();

        private Dictionary<TKey, TData> storage;

        private Dictionary<TKey, object> locks;

        private Registry() {
            this.storage = new Dictionary<TKey,TData>();
            this.locks = new Dictionary<TKey, object>();
        }
        
        internal TData Get(TKey id, bool forLoadingFromHash) {
            if(!this.locks.ContainsKey(id)) {
				lock(this.locks) {
					if(!this.locks.ContainsKey(id)) {
						this.locks.Add(id, new object());
					}
				}
			}
            lock(this.locks[id]) {
				if(!this.storage.ContainsKey(id)) {

					//this.storage[id] = IDataObject<TKey, TData>.CreateByIdFromRegistry(id);
					//this.storage[id] = TData.CreateByIdFromRegistry(id);
					TData obj = new TData();
					obj.CreateByIdFromRegistry(id, forLoadingFromHash);
					this.storage[id] = obj;
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
			foreach(TKey id in idsToDelete) {
				lock(this.locks[id]) {
					IDataObject<TKey, TData> obj = null;
					lock(this.locks) {
						if(this.storage.ContainsKey(id)) {
							obj = this.storage[id];
							this.storage.Remove(id);
						}
					}
					if(obj != null) {
						obj.markAsDeletedFromRegistry();
					}
				}
			}
		}

    }

}
