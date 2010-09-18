using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.forum {
	public class AllThreads : AbstractUrl {

		public AllThreads(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "All threads";
			}
		}

		protected override string _canonical {
			get {
				return "/Forum/AllThreads/";
			}
		}

	}
}
