using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using FLocal.Core;

namespace FLocal.MySQLConnector {
	class Transaction : Core.DB.Transaction {

		internal MySqlConnection sqlconnection;
		internal MySqlTransaction sqltransaction;

		public bool finalizedImpl {
			get;
			private set;
		}

		public Transaction(Connection connection, System.Data.IsolationLevel iso) : base() {
			this.sqlconnection = connection.createConnection();
			try {
				//for some reason, call to BeginTransaction with IsolationLevel set fails somewhere deep in mysql library
				this.sqltransaction = this.sqlconnection.BeginTransaction();
			} catch(Exception e) {
				this.close();
				throw e;
			}
		}

		protected override void do_Commit() {
			lock(this) {
				if(this.finalizedImpl) throw new CriticalException("Already finalized");
				this.sqltransaction.Commit();
				this.close();
			}
		}

		protected override void do_Rollback() {
			lock(this) {
				if(this.finalizedImpl) throw new CriticalException("Already finalized");
				this.sqltransaction.Rollback();
				this.close();
			}
		}

		private void close() {
			this.sqlconnection.Close();
			this.sqlconnection.Dispose();
			this.finalizedImpl = true;
		}

	}
}
