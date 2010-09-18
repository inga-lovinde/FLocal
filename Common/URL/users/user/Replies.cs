using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.users.user {
	public class Replies : Abstract {

		public Replies(string userId, string remainder) : base(userId, remainder) {
		}

		protected override string _canonical {
			get {
				return "/Users/User/" + this.user.id + "/Replies/";
			}
		}
	}
}
