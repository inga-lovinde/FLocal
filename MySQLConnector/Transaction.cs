using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;
using System.Data.Common;

namespace FLocal.MySQLConnector {
	class Transaction : Core.DB.Transaction {

		internal Connection connection;
		internal DbConnection sqlconnection;
		internal DbTransaction sqltransaction;

		public bool finalizedImpl {
			get;
			private set;
		}

		public Transaction(Connection connection, System.Data.IsolationLevel iso) : base() {
			this.connection = connection;
			this.sqlconnection = connection.createConnection();
			try {
				if(connection.traits.supportsIsolationLevel()) {
					this.sqltransaction = this.sqlconnection.BeginTransaction(iso);
				} else {
					this.sqltransaction = this.sqlconnection.BeginTransaction();
				}
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
			this.connection.RemoveTransaction(this);
		}

	}
}
