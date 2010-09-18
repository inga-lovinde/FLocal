using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL {
	public class Robots : AbstractUrl {

		public Robots() : base("") {
		}

		public override string title {
			get {
				return "robots.txt";
			}
		}

		protected override string _canonical {
			get {
				return "/robots.txt";
			}
		}
	}
}
