using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Common.dataobjects;

namespace FLocal.Common.URL.my.conversations {
	public class Reply : AbstractUrl {

		public PMMessage pm;

		public Reply(string pmId, string remainder) : base(remainder) {
			this.pm = PMMessage.LoadById(int.Parse(pmId));
		}

		public override string title {
			get {
				return this.pm.title;
			}
		}

		protected override string _canonical {
			get {
				return "/My/Conversations/Reply/" + this.pm.id + "/";
			}
		}
	}
}
