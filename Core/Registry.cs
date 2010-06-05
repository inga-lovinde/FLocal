using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core {
    class Registry<TKey, TData>
        where TData : IDataObject<TKey, TData>, new()
        where TKey : struct {

        public static readonly Registry<TKey, TData> instance = new Registry<TKey,TData>();

        private Dictionary<TKey, TData> storage;

        private HashSet<TKey> locks;

        protected Registry() {
            this.storage = new Dictionary<TKey,TData>();
            this.locks = new HashSet<TKey>();
        }
        
        public TData Get(TKey id) {
            if(this.locks.Contains(id)) throw new CriticalException("Locked");
            if(!this.storage.ContainsKey(id)) {
                try {
                    this.locks.Add(id);

                    //this.storage[id] = IDataObject<TKey, TData>.CreateByIdFromRegistry(id);
                    //this.storage[id] = TData.CreateByIdFromRegistry(id);
                    TData obj = new TData();
                    obj.CreateByIdFromRegistry(id);
                    this.storage[id] = obj;

                    this.locks.Remove(id);
                } catch(FLocalException e) {
                    this.locks.Remove(id);
                    throw e;
                }
            }
            return this.storage[id];
        }

        public bool IsCached(TKey id) {
            if(this.locks.Contains(id)) throw new CriticalException("locked");
            return this.storage.ContainsKey(id);
        }

    }

}
