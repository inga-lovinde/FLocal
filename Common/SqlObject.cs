using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;
using FLocal.Core.DB;

namespace FLocal.Common {
	abstract public class SqlObject<TKey, T> : Core.DataObject<TKey, T>
		where T : SqlObject<TKey, T>, new()
		where TKey : struct {

		protected SqlObject() : base() {
		}

		abstract protected ISqlObjectTableSpec table {
			get;
		}

		private volatile bool _isLoaded = false;
		protected bool isLoaded {
			get {
				return this._isLoaded;
			}
			private set {
				this._isLoaded = value;
			}
		}

		private volatile object lockFiller = new object();
		private volatile object lockInitializer = new object();

		abstract protected void doFromHash(Dictionary<string, string> data);

		/// <summary>
		/// Note that this method does not updates isLoaded field!
		/// </summary>
		/// <param name="data"></param>
		private void fromHash(Dictionary<string, string> data) {
			lock(this.lockFiller) {
				if(data[this.table.idName] != this.id.ToString()) {
					throw new CriticalException("Id mismatch");
				}
				this.doFromHash(data);
			}
		}

		/// <summary>
		/// Note that this method does not updates isLoaded field!
		/// </summary>
		/// <param name="data"></param>
		private void doLoad() {
			this.fromHash(Config.instance.mainConnection.LoadById(this.table, this.id.ToString()));
		}

		private void Load() {
			lock(this.lockInitializer) {
				if(this.isLoaded) throw new CriticalException("already initialized");
				this.doLoad();
				this.isLoaded = true;
			}
		}

		protected void LoadFromHash(Dictionary<string, string> data) {
			lock(this.lockInitializer) {
				if(this.isLoaded) throw new CriticalException("already initialized");
				this.fromHash(data);
				this.isLoaded = true;
			}
		}

		protected void LoadIfNotLoaded() {
			if(!this.isLoaded) {
				lock(this.lockInitializer) {
					if(!this.isLoaded) {
						this.doLoad();
						this.isLoaded = true;
					}
				}
			}
		}

		public void ReLoad() {
			lock(this.lockInitializer) {
				this.doLoad();
				this.isLoaded = true;
			}
		}

		/*protected override void AfterCreate(bool forLoadingFromHash) {
			base.AfterCreate(forLoadingFromHash);
			if(!forLoadingFromHash) this.LoadIfNotLoaded();
		}*/

		protected static void Refresh(TKey id) {
			Dictionary<TKey, T> objects = LoadByIdsForLoadingFromHash(new List<TKey>() { id });
			objects[id].ReLoad();
		}

	}

	abstract public class SqlObject<T> : SqlObject<int, T>, IComparable<T> where T : SqlObject<T>, new() {

		public static List<T> LoadByIds(IEnumerable<int> ids) {

			Dictionary<int, T> rawRes = LoadByIdsForLoadingFromHash(ids);
			
			List<int> idsToQuery = new List<int>();
			foreach(int id in ids) {
				if(!rawRes[id].isLoaded) {
					idsToQuery.Add(id);
				}
			}

			List<int> loadedIds = new List<int>();
			if(idsToQuery.Count > 0) {
				ITableSpec table = rawRes[idsToQuery[0]].table;
				List<Dictionary<string, string>> rawData = Config.instance.mainConnection.LoadByIds(table, new List<string>(from int id in idsToQuery select id.ToString()));
				foreach(Dictionary<string, string> row in rawData) {
					int id = int.Parse(row[table.idName]);
					loadedIds.Add(id);
					if(!rawRes.ContainsKey(id)) throw new CriticalException("wrong id");
					rawRes[id].LoadFromHash(row);
				}
			}

			List<T> res = new List<T>();
			foreach(int id in ids) {
				if(!rawRes[id].isLoaded) {
					throw new CriticalException("#" + id + " not loaded (all ids (" + ids.ToPrintableString() + "), idsToQuery (" + idsToQuery.ToPrintableString() + "), loaded ids (" + loadedIds.ToPrintableString() + ")");
				}
				res.Add(rawRes[id]);
			}

			return res;
		}

		int IComparable<T>.CompareTo(T other) {
			if(other.id > this.id) {
				return -1;
			} else if(other.id == this.id) {
				return 0;
			} else {
				return 1;
			}
		}

	}

}
