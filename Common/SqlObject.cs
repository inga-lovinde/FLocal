using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;
using FLocal.Core.DB;

namespace FLocal.Common {
	abstract public class SqlObject<T> : Core.DataObject<int, T> where T : SqlObject<T>, new() {

		protected SqlObject() : base() {
		}

		abstract protected ITableSpec table {
			get;
		}

		private bool isLoaded = false;

		private object lockFiller = new object();
		private object lockInitializer = new object();

		abstract protected void doFromHash(Dictionary<string, string> data);

		protected void fromHash(Dictionary<string, string> data) {
			lock(this.lockFiller) {
				if(data[this.table.idName] != this.id.ToString()) {
					throw new CriticalException("Id mismatch");
				}
				this.doFromHash(data);
			}
		}

		private void doLoad() {
			this.fromHash(Config.instance.mainConnection.LoadById(this.table, this.id.ToString()));
		}

		protected void Load() {
			lock(this.lockInitializer) {
				if(this.isLoaded) throw new CriticalException("already initialized");
				this.doLoad();
				this.isLoaded = true;
			}
		}

		protected void LoadIfNotLoaded() {
			lock(this.lockInitializer) {
				if(!this.isLoaded) {
					this.doLoad();
					this.isLoaded = true;
				}
			}
		}

		public void ReLoad() {
			this.doLoad();
		}

		protected override void AfterCreate(bool forLoadingFromHash) {
			base.AfterCreate(forLoadingFromHash);
			if(!forLoadingFromHash) this.Load();
		}

		public static List<T> LoadByIds(List<int> ids) {

			Dictionary<int, T> rawRes = LoadByIdsForLoadingFromHash(ids);
			
			List<int> idsToQuery = new List<int>();
			foreach(int id in ids) {
				if(!rawRes[id].isLoaded) {
					idsToQuery.Add(id);
				}
			}

			if(idsToQuery.Count > 0) {
				ITableSpec table = rawRes[idsToQuery[0]].table;
				List<Dictionary<string, string>> rawData = Config.instance.mainConnection.LoadByIds(table, new List<string>(from int id in idsToQuery select id.ToString()));
				foreach(Dictionary<string, string> row in rawData) {
					int id = int.Parse(row[table.idName]);
					if(!rawRes.ContainsKey(id)) throw new CriticalException("wrong id");
					rawRes[id].fromHash(row);
				}
			}

			List<T> res = new List<T>();
			foreach(int id in ids) {
				if(!rawRes[id].isLoaded) throw new CriticalException("not loaded");
				res.Add(rawRes[id]);
			}

			return res;
		}

	}
}
