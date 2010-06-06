using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.DB {
	public interface IDBConnection : IDisposable {

		List<Dictionary<string, string>> LoadByIds(ITableSpec table, List<string> ids);

		List<string> LoadIdsByConditions(ITableSpec table, conditions.AbstractCondition conditions, Diapasone diapasone, JoinSpec[] joins, SortSpec[] sorts);

		Transaction beginTransaction(System.Data.IsolationLevel iso);

		void lockTable(Transaction transaction, ITableSpec table);

		void lockRow(Transaction transaction, ITableSpec table, string id);

		void update(Transaction transaction, ITableSpec table, string id, Dictionary<string, string> data);

		string insert(Transaction transaction, ITableSpec table, Dictionary<string, string> data);

		void delete(Transaction transaction, ITableSpec table, string id); //do we really need this?

	}

	public static class IDBConnectionExtensions {

		public static List<string> LoadIdsByConditions(this IDBConnection connection, ITableSpec table, conditions.AbstractCondition conditions, Diapasone diapasone, params JoinSpec[] joins) {
			return connection.LoadIdsByConditions(table, conditions, diapasone, joins, new SortSpec[] { new SortSpec(table.getIdSpec(), true) });
		}

		public static Transaction beginTransaction(this IDBConnection connection) {
			return connection.beginTransaction(System.Data.IsolationLevel.Snapshot);
		}

		public static Dictionary<string, string> LoadById(this IDBConnection connection, ITableSpec table, string id) {
			List<Dictionary<string, string>> rows = connection.LoadByIds(table, new List<string> { id });
			if(rows.Count < 1) {
				throw new NotFoundInDBException();
			}
			if(rows.Count > 1) {
				throw new CriticalException(rows.Count + " objects with specified id");
			}
			return rows[0];
		}
	
	}

}
