using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.polls {
	public class New : AbstractUrl {

		public New(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "New poll";
			}
		}

		protected override string _canonical {
			get {
				return "/Polls/New/";
			}
		}
	}
}
