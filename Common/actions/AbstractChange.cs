using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core.DB;

namespace FLocal.Common.actions {
	abstract class AbstractChange {

		private bool isApplied;

		abstract public int? getId();

		abstract public void Lock(Transaction transaction);

		abstract protected void doApply(Transaction transaction);

		public void Apply(Transaction transaction) {
			if(!this.isApplied) {
				this.doApply(transaction);
				this.isApplied = true;
			}
		}

		public readonly IEnumerable<AbstractChange> references;

		public readonly ISqlObjectTableSpec tableSpec;

		protected readonly Dictionary<string, AbstractFieldValue> data;

		protected AbstractChange(ISqlObjectTableSpec tableSpec, Dictionary<string, AbstractFieldValue> data) {
			this.tableSpec = tableSpec;
			this.data = data;
			this.references = from kvp in data where kvp.Value is ReferenceFieldValue select ((ReferenceFieldValue)kvp.Value).referenced;
			this.isApplied = false;
		}

	}
}
