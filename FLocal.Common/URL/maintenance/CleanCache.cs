using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.maintenance {
	public class CleanCache : AbstractUrl {

		public CleanCache(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "Maintenance :: Clean cache";
			}
		}

		protected override string _canonical {
			get {
				return "/Maintenance/CleanCache/";
			}
		}

	}
}
