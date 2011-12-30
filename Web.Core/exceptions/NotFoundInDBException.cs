using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core.DB;

namespace Web.Core {

    public partial class NotFoundInDBException : FLocalException {

        public NotFoundInDBException() : base("Object not found in DB") { }

		public NotFoundInDBException(ColumnSpec columnSpec, string value) : base("Object " + columnSpec.table.name + "[" + columnSpec.name + "=" + value + "] not found in db") {}

		public NotFoundInDBException(ITableSpec tableSpec, string id) : this(tableSpec.getIdSpec(), id) {}

    }

}
