using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.my {
	public class Avatars : AbstractUrl {

		public Avatars(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "Avatars settings";
			}
		}

		protected override string _canonical {
			get {
				return "/My/Avatars/";
			}
		}
	}
}
