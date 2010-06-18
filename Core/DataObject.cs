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

        public override TKey id {
            get {
                if(this.isNewObject) {
                    throw new ObjectDoesntHaveAnIdException();
                }
                return this._id.Value;
            }
        }

        private bool isNewObject {
            get {
                return !this._id.HasValue;
            }
        }

        private bool isJustCreated;

        protected DataObject() {
            Debug.Assert(this is T);
            this.isJustCreated = true;
            this._id = null;
        }

        public static T LoadById(TKey id) {
            return registry.Get(id, false);
        }

		protected static Dictionary<TKey, T> LoadByIdsForLoadingFromHash(IEnumerable<TKey> ids) {
			Dictionary<TKey, T> res = new Dictionary<TKey,T>();
			foreach(TKey id in ids) {
				res[id] = registry.Get(id, true);
			}
			return res;
		}

        protected virtual void AfterCreate(bool forLoadingFromHash) { }

        internal override void CreateByIdFromRegistry(TKey id, bool forLoadingFromHash) {
            if(!this.isJustCreated) throw new CriticalException("Object already has an id");
            this._id = id;
            this.isJustCreated = false;
            this.AfterCreate(forLoadingFromHash);
        }

		internal override void markAsDeletedFromRegistry() {
		}

        private static Registry<TKey, T> registry {
            get {
                return Registry<TKey, T>.instance;
            }
        }

		protected void deleteFromRegistry() {
			registry.Delete(new TKey[] { this.id });
		}

    }

}
