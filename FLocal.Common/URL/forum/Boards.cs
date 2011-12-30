using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.forum {
	public class Boards : AbstractUrl {

		public Boards(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "Boards";
			}
		}

		protected override string _canonical {
			get {
				return "/Forum/Boards/";
			}
		}
	}
}
