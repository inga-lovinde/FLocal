using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Common.dataobjects;

namespace FLocal.Common.URL.users.user {
	public abstract class Abstract : AbstractUrl {

		public readonly User user;

		public Abstract(string userId, string remainder) : base(remainder) {
			int iUserId;
			if(int.TryParse(userId, out iUserId)) {
				this.user = User.LoadById(iUserId);
			} else {
				this.user = User.LoadByName(userId);
			}
		}

		public override string title {
			get {
				return this.user.name;
			}
		}

	}
}
