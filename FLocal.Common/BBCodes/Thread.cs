using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class Thread : AbstractLocalLink {

		public Thread()
			: base("thread") {
		}

		protected override FLocal.Common.URL.AbstractUrl url {
			get {
				return new URL.forum.board.thread.Posts(this.DefaultOrValue, null);
			}
		}

	}
}
