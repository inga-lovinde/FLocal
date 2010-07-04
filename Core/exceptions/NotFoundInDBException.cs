using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core {

    public partial class NotFoundInDBException : FLocalException {

        public NotFoundInDBException() : base("Object not found in DB") { }

		public NotFoundInDBException(DB.ITableSpec tableSpec, string id) : base("Object " + tableSpec.name + "[" + tableSpec.idName + "=" + id + "] not found in db") {}

    }

}
