using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.Common.actions {
	class InsertOrUpdateChange : AbstractChange {

		private int? id;

		private AbstractCondition condition;

		private Dictionary<string, AbstractFieldValue> dataToInsert;
		private Dictionary<string, AbstractFieldValue> dataToUpdate;

		public InsertOrUpdateChange(ISqlObjectTableSpec tableSpec, Dictionary<string, AbstractFieldValue> dataToInsert, Dictionary<string, AbstractFieldValue> dataToUpdate, AbstractCondition condition)
			: base(tableSpec, (from kvp in dataToInsert select kvp.Value).Union(from kvp in dataToUpdate select kvp.Value)) {
			this.id = null;
			this.condition = condition;
			this.dataToInsert = dataToInsert;
			this.dataToUpdate = dataToUpdate;
		}

		public override int? getId() {
			return this.id;
		}

		public override void Lock(Transaction transaction) {
			List<string> ids = Config.instance.mainConnection.LoadIdsByConditions(transaction, this.tableSpec, this.condition, Diapasone.unlimited, new JoinSpec[0], new SortSpec[0], false);
			if(ids.Count > 1) {
				throw new CriticalException("Not unique");
			} else if(ids.Count == 1) {
				this.id = int.Parse(ids[0]);
				Config.instance.mainConnection.lockRow(transaction, this.tableSpec, this.id.ToString());
			} else {
				Config.instance.mainConnection.lockTable(transaction, this.tableSpec);
				ids = Config.instance.mainConnection.LoadIdsByConditions(transaction, this.tableSpec, this.condition, Diapasone.unlimited, new JoinSpec[0], new SortSpec[0], false);
				if(ids.Count > 1) {
					throw new CriticalException("Not unique");
				} else if(ids.Count == 1) {
					this.id = int.Parse(ids[0]);
				} else {
					this.id = null;
				}
			}
		}

		protected override void doApply(Transaction transaction) {
			if(this.id.HasValue) {
				Dictionary<string, string> row = Config.instance.mainConnection.LoadByIds(transaction, this.tableSpec, new List<string>() { this.id.ToString() })[0];
				Dictionary<string, string> processedData = new Dictionary<string,string>();
				foreach(KeyValuePair<string, AbstractFieldValue> kvp in this.dataToUpdate) {
					processedData[kvp.Key] = kvp.Value.getStringRepresentation(row[kvp.Key]);
				}
				Config.instance.mainConnection.update(
					transaction,
					this.tableSpec,
					this.id.ToString(),
					processedData
				);
			} else {
				Dictionary<string, string> processedData = new Dictionary<string,string>();
				foreach(KeyValuePair<string, AbstractFieldValue> kvp in this.dataToInsert) {
					processedData[kvp.Key] = kvp.Value.getStringRepresentation();
				}
				Config.instance.mainConnection.insert(
					transaction,
					this.tableSpec,
					processedData
				);
			}
		}

	}
}
