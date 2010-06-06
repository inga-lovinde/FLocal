using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.DB {
	interface IDBConnection {

		Dictionary<string, string> LoadById(ITableSpec table, int id);
		Dictionary<string, string>[] LoadByIds(ITableSpec table, int[] ids);

		int[] LoadIdsByConditions(ITableSpec table, conditions.AbstractCondition conditions, Diapasone diapasone, JoinSpec[] joins, SortSpec[] sorts);

		Transaction beginTransaction();

		ILock lockTable(Transaction transaction, ITableSpec table);

		ILock lockRow(Transaction transaction, ITableSpec table, int id);

		void update(Transaction transaction, ITableSpec table, int id, Dictionary<string, string> data);

		void delete(Transaction transaction, ITableSpec table, int id); //do we really need this?

	}

	static class IDBConnectionExtensions {

		public static int[] LoadIdsByConditions(this IDBConnection connection, ITableSpec table, conditions.AbstractCondition conditions, Diapasone diapasone, params JoinSpec[] joins) {
			return connection.LoadIdsByConditions(table, conditions, diapasone, joins, new SortSpec[] { new SortSpec(table.getIdSpec(), true) });
		}

	}

}
