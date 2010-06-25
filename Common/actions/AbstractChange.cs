using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core.DB;

namespace FLocal.Common.actions {
	abstract public class AbstractChange {

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

		protected AbstractChange(ISqlObjectTableSpec tableSpec, IEnumerable<AbstractFieldValue> data) {
			this.tableSpec = tableSpec;
			this.references = from val in data where val is ReferenceFieldValue select ((ReferenceFieldValue)val).referenced;
			this.isApplied = false;
		}

	}
}
