using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.DB {

	abstract public class Transaction : IDisposable {

		protected Transaction() {
			this.finalized = false;
		}

		abstract protected void do_Commit();

		abstract protected void do_Rollback();

		public bool finalized {
			get;
			private set;
		}

		public void Commit() {
			lock(this) {
				if(this.finalized) throw new CriticalException("Already finalized");
				this.do_Commit();
				this.finalized = true;
			}
		}

		public void Rollback() {
			lock(this) {
				if(this.finalized) throw new CriticalException("Already finalized");
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
