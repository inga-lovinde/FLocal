using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core.DB;

namespace FLocal.Common {
	public interface ISqlObjectTableSpec : ITableSpec {
		void refreshSqlObject(int id);
	}
}
