using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;
using FLocal.Core.DB;

namespace FLocal.Common.actions {

	/// <summary>
	/// Note that you should create ChangeSet instances outside of transactions they're using!
	/// Otherwise, there will be a 100% DB deadlock in Dispose (or data in registry won't update)
	/// </summary>
	internal class ChangeSet : IDisposable {

		private static readonly object tablesLockOrder_locker = new object();
		private static IEnumerable<string> tablesLockOrder {
			get {
				return Cache<IEnumerable<string>>.instance.get(
					tablesLockOrder_locker,
					() => new List<string>() {
						dataobjects.Account.TableSpec.TABLE,
						dataobjects.User.TableSpec.TABLE,
						dataobjects.Board.TableSpec.TABLE,
						dataobjects.Thread.TableSpec.TABLE,
						dataobjects.Post.TableSpec.TABLE,
						dataobjects.Thread.ReadMarkerTableSpec.TABLE,
						dataobjects.Session.TableSpec.TABLE,
					}
				);
			}
		}

		private object locker = new object();

		private bool isAdding;
		private bool isProcessing;
		private bool isProcessed;

		private Dictionary<string, HashSet<AbstractChange>> changesByTable;

		public ChangeSet() {
			this.isAdding = false;
			this.isProcessing = false;
			this.isProcessed = false;
			this.changesByTable = new Dictionary<string,HashSet<AbstractChange>>();
			foreach(string table in tablesLockOrder) {
				this.changesByTable[table] = new HashSet<AbstractChange>();
			}
		}

		public void Add(AbstractChange change) {
			lock(locker) {
				if(this.isAdding) throw new CriticalException("Cannot add inside add");
				if(this.isProcessing || this.isProcessed) throw new CriticalException("ChangeSet is readonly");
				this.isAdding = true;
			}
			if(!this.changesByTable.ContainsKey(change.tableSpec.name)) throw new CriticalException("Table not supported");
			this.changesByTable[change.tableSpec.name].Add(change);
			this.isAdding = false;
			foreach(AbstractChange referencedChange in change.references) {
				this.Add(referencedChange);
			}
		}

		private void ApplyToChange(Transaction transaction, AbstractChange change) {
			foreach(AbstractChange referenced in change.references) {
				if(!referenced.getId().HasValue) {
					this.ApplyToChange(transaction, referenced);
				}
			}
			change.Apply(transaction);
		}

		public void Apply(Transaction transaction) {
			lock(this.locker) {
				if(this.isAdding) throw new CriticalException("Cannot process while adding");
				if(this.isProcessing || this.isProcessed) throw new CriticalException("ChangeSet is already processed");
				this.isProcessing = true;
			}
			foreach(string table in tablesLockOrder) {
				foreach(AbstractChange change in (from AbstractChange _change in this.changesByTable[table] orderby _change.getId() select _change)) {
					change.Lock(transaction);
				}
			}
			foreach(KeyValuePair<string, HashSet<AbstractChange>> kvp in this.changesByTable) {
				foreach(AbstractChange change in kvp.Value) {
					this.ApplyToChange(transaction, change);
				}
			}
			this.isProcessed = true;
		}

		public void Dispose() {
			//if(!this.isProcessed) throw new CriticalException("ChangeSet is not processed yet");
			foreach(KeyValuePair<string, HashSet<AbstractChange>> kvp in this.changesByTable) {
				foreach(AbstractChange change in kvp.Value) {
					if(change.getId().HasValue) {
						change.tableSpec.refreshSqlObject(change.getId().Value);
					} //otherwise we're disposing because of sql error or something, so we should show real cause of problem, not "id is null"
				}
			}
		}

	}
}
