using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core.DB;

namespace FLocal.Common.actions {
	public class InsertChange : AbstractChange {

		private int? id;

		private Dictionary<string, AbstractFieldValue> data;

		public InsertChange(ISqlObjectTableSpec tableSpec, Dictionary<string, AbstractFieldValue> data)
			: base(tableSpec, from kvp in data select kvp.Value) {
			this.id = null;
			this.data = data;
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
			if(processedData.ContainsKey(this.tableSpec.idName)) {
				this.id = int.Parse(processedData[this.tableSpec.idName]);
			}
		}

	}
}
