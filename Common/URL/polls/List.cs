using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.polls {
	public class List : AbstractUrl {

		public List(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "Polls";
			}
		}

		protected override string _canonical {
			get {
				return "/Polls/List/";
			}
		}
	}
}
