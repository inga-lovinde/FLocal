using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core.DB;

namespace FLocal.Common.actions {
	public class UpdateChange : AbstractChange {

		private readonly int id;
		private Dictionary<string, AbstractFieldValue> data;

		public UpdateChange(ISqlObjectTableSpec tableSpec, Dictionary<string, AbstractFieldValue> data, int id)
			: base(tableSpec, from kvp in data select kvp.Value) {
			this.id = id;
			this.data = data;
		}

		public override int? getId() {
			return this.id;
		}

		public override void Lock(Transaction transaction) {
			Config.instance.mainConnection.lockRow(transaction, this.tableSpec, this.id.ToString());
		}

		protected override void doApply(Transaction transaction) {
			Dictionary<string, string> row = Config.instance.mainConnection.LoadByIds(transaction, this.tableSpec, new List<string>() { this.id.ToString() })[0];
			Dictionary<string, string> processedData = new Dictionary<string,string>();
			foreach(KeyValuePair<string, AbstractFieldValue> kvp in this.data) {
				processedData[kvp.Key] = kvp.Value.getStringRepresentation(row[kvp.Key]);
			}
			Config.instance.mainConnection.update(
				transaction,
				this.tableSpec,
				this.id.ToString(),
				processedData
			);
		}

	}
}
