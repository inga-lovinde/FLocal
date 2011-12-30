using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.upload {
	public class List : AbstractUrl {

		public List(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "Uploads list";
			}
		}

		protected override string _canonical {
			get {
				return "/Upload/List/";
			}
		}
	}
}
