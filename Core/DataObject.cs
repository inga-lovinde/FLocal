using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FLocal.Core {

	abstract public class DataObject<TKey, T> : IDataObject<TKey, T>
        where T : DataObject<TKey, T>, new()
        where TKey : struct {

        private TKey? _id;

        public TKey id {
            get {
                if(this.isNewObject) {
                    throw new ObjectDoesntHaveAnIdException();
                }
                return this._id.Value;
            }
        }

        public bool isNewObject {
            get {
                return !this._id.HasValue;
            }
        }

        private bool isJustCreated;

        public DataObject() {
            Debug.Assert(this is T);
            this.isJustCreated = true;
            this._id = null;
        }

        public static T LoadById(TKey id) {
            return registry.Get(id);
        }

        protected virtual void AfterCreate() { }

        public void CreateByIdFromRegistry(TKey id) {
            if(!this.isJustCreated) throw new CriticalException("Object already has an id");
            this._id = id;
            this.isJustCreated = false;
            this.AfterCreate();
        }

        private static Registry<TKey, T> registry {
            get {
                return Registry<TKey, T>.instance;
            }
        }

    }

}
