using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Web {
	class DBIsOutdatedException : ApplicationException {

		public DBIsOutdatedException()
			: base("DB is outdated") {
		}

	}
}
