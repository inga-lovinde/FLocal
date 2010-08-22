using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common {
	public interface IComplexSqlObjectTableSpec : ISqlObjectTableSpec {

		void refreshSqlObjectAndRelated(int id);

	}
}
