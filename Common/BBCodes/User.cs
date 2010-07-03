using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class User : BBCode {

		public User()
			: base("user") {
		}

		public override string Format(ITextFormatter formatter) {
			var user = dataobjects.User.LoadByName(this.Default);
			return "<a href=\"/User/" + user.id.ToString() + "/\">" + this.Safe(user.name) + "</a>";
		}

	}
}
