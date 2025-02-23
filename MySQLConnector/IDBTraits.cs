﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Web.Core.DB;

namespace MySQLConnector {
	public interface IDBTraits {

		DbConnection createConnection(string connectionString);

		long LastInsertId(DbCommand command, ITableSpec table);

		string escapeIdentifier(string identifier);

		string markParam(string param);

		bool supportsIsolationLevel();

	}
}
