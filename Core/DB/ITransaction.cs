using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.DB {

	abstract class Transaction : IDisposable {

		abstract protected void do_Commit();

		abstract protected void do_Rollback();

		private bool finalized = false;

		public void Commit() {
			lock(this) {
				this.do_Commit();
				this.finalized = true;
			}
		}

		public void Rollback() {
			lock(this) {
				this.do_Rollback();
				this.finalized = true;
			}
		}

		public void Dispose() {
			lock(this) {
				if(!this.finalized) {
					this.do_Rollback();
					this.finalized = true;
				}
			}
		}

	}

}
