using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.DB {
	public class ColumnOrValue {

		private bool _isColumn;

		private ColumnSpec _column;

		private string _value;

		private ColumnOrValue() { }

		public static ColumnOrValue createColumn(ColumnSpec column) {
			return new ColumnOrValue{_isColumn = true, _column = column};
		}

		public static ColumnOrValue createValue(string value) {
			return new ColumnOrValue{_isColumn = false, _value = value};
		}

		public bool isColumn {
			get {
				return this._isColumn;
			}
		}

		public ColumnSpec column {
			get {
				if(!this.isColumn) throw new CriticalException("Not a column");
				return this._column;
			}
		}

		public string value {
			get {
				if(this.isColumn) throw new CriticalException("not a value");
				return this._value;
			}
		}

	}
}
