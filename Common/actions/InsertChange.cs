using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core.DB;

namespace FLocal.Common.actions {
	class InsertChange : AbstractChange {

		private int? id;

		public InsertChange(ISqlObjectTableSpec tableSpec, Dictionary<string, AbstractFieldValue> data, int id)
			: base(tableSpec, data) {
			this.id = null;
		}
	
		public override int? getId() {
			return this.id;
		}

		public override void Lock(Transaction transaction) {
			Config.instance.mainConnection.lockTable(transaction, this.tableSpec);
		}

		protected override void doApply(Transaction transaction) {
			Dictionary<string, string> processedData = new Dictionary<string,string>();
			foreach(KeyValuePair<string, AbstractFieldValue> kvp in this.data) {
				processedData[kvp.Key] = kvp.Value.getStringRepresentation();
			}
			this.id = int.Parse(Config.instance.mainConnection.insert(
				transaction,
				this.tableSpec,
				processedData
			));
		}

	}
}
