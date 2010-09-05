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
			return String.Format("<a class=\"separate UG_{0}\" href=\"/Users/User/{1}/Info/\">{2}</a>", this.Safe(user.userGroup.name), user.id, this.Safe(user.name));
		}

	}
}
