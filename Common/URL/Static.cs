using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL {
	public class Static : AbstractUrl {

		public Static(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return this.remainder;
			}
		}

		protected override string _canonical {
			get {
				return "/static/";
			}
		}
	}
}
