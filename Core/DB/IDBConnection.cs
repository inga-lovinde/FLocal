using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core.DB.conditions;

namespace FLocal.Core.DB {
	public interface IDBConnection : IDisposable {

		List<Dictionary<string, string>> LoadByIds(ITableSpec table, List<string> ids);

		List<string> LoadIdsByConditions(ITableSpec table, AbstractCondition conditions, Diapasone diapasone, JoinSpec[] joins, SortSpec[] sorts, bool allowHugeLists);

		long GetCountByConditions(ITableSpec table, AbstractCondition conditions, params JoinSpec[] joins);

		Transaction beginTransaction(System.Data.IsolationLevel iso);

		void lockTable(Transaction transaction, ITableSpec table);

		void lockRow(Transaction transaction, ITableSpec table, string id);

		List<Dictionary<string, string>> LoadByIds(Transaction transaction, ITableSpec table, List<string> ids);

		List<string> LoadIdsByConditions(Transaction transaction, ITableSpec table, AbstractCondition conditions, Diapasone diapasone, JoinSpec[] joins, SortSpec[] sorts, bool allowHugeLists);

		void update(Transaction transaction, ITableSpec table, string id, Dictionary<string, string> data);

		string insert(Transaction transaction, ITableSpec table, Dictionary<string, string> data);

		void delete(Transaction transaction, ITableSpec table, string id); //do we really need this?

	}

	public static class IDBConnectionExtensions {

		public static List<string> LoadIdsByConditions(this IDBConnection connection, ITableSpec table, AbstractCondition conditions, Diapasone diapasone, JoinSpec[] joins, SortSpec[] sorts) {
			return connection.LoadIdsByConditions(table, conditions, diapasone, joins, sorts, false);
		}

		public static List<string> LoadIdsByConditions(this IDBConnection connection, ITableSpec table, AbstractCondition conditions, Diapasone diapasone, params JoinSpec[] joins) {
			return connection.LoadIdsByConditions(table, conditions, diapasone, joins, new SortSpec[] { new SortSpec(table.getIdSpec(), true) });
		}

		public static Transaction beginTransaction(this IDBConnection connection) {
			return connection.beginTransaction(System.Data.IsolationLevel.ReadCommitted);
		}

		public static Dictionary<string, string> LoadById(this IDBConnection connection, ITableSpec table, string id) {
			List<Dictionary<string, string>> rows = connection.LoadByIds(table, new List<string> { id });
			if(rows.Count < 1) {
				throw new NotFoundInDBException(table, id);
			}
			if(rows.Count > 1) {
				throw new CriticalException(rows.Count + " objects with specified id");
			}
			return rows[0];
		}

		public static string LoadIdByField(this IDBConnection connection, ColumnSpec column, string value) {
			List<string> ids = connection.LoadIdsByConditions(
				column.table,
				new ComparisonCondition(
					column,
					ComparisonType.EQUAL,
					value
				),
				Diapasone.unlimited
			);
			if(ids.Count > 1) {
				throw new CriticalException("not unique");
			} else if(ids.Count == 1) {
				return ids[0];
			} else {
				throw new NotFoundInDBException(column, value);
			}
		}
	
	}

}
