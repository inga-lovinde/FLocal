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

		public InsertOrUpdateChange(ISqlObjectTableSpec tableSpec, Dictionary<string, AbstractFieldValue> data, AbstractCondition condition)
			: base(tableSpec, data) {
			this.id = null;
			this.condition = condition;
		}

		public override int? getId() {
			return this.id;
		}

		public override void Lock(Transaction transaction) {
			List<string> ids = Config.instance.mainConnection.LoadIdsByConditions(this.tableSpec, this.condition, Diapasone.unlimited, new JoinSpec[0]);
			if(ids.Count > 1) {
				throw new CriticalException("Not unique");
			} else if(ids.Count == 1) {
				this.id = int.Parse(ids[0]);
				Config.instance.mainConnection.lockRow(transaction, this.tableSpec, this.id.ToString());
			} else {
				Config.instance.mainConnection.lockTable(transaction, this.tableSpec);
				ids = Config.instance.mainConnection.LoadIdsByConditions(this.tableSpec, this.condition, Diapasone.unlimited, new JoinSpec[0]);
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
			Dictionary<string, string> processedData = new Dictionary<string,string>();
			foreach(KeyValuePair<string, AbstractFieldValue> kvp in this.data) {
				processedData[kvp.Key] = kvp.Value.getStringRepresentation();
			}
			if(this.id.HasValue) {
				Config.instance.mainConnection.update(
					transaction,
					this.tableSpec,
					this.id.ToString(),
					processedData
				);
			} else {
				Config.instance.mainConnection.insert(
					transaction,
					this.tableSpec,
					processedData
				);
			}
		}

	}
}
