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

		public override string Format(IPostParsingContext context, ITextFormatter formatter) {
			var user = dataobjects.User.LoadByName(this.DefaultOrValue);
			var url = new URL.users.user.Info(user.id.ToString(), null);
			return String.Format("<a class=\"separate UG_{0}\" href=\"{1}\">{2}</a>", this.Safe(user.userGroup.name), url.canonical, this.Safe(user.name));
		}

	}
}
