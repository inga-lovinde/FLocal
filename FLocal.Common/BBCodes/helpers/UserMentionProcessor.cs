using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.BBCodes {
	class UserMentionProcessor {

		private static string Safe(string str) {
			return System.Web.HttpUtility.HtmlEncode(str);
		}

		public static string ProcessUserMention(IPostParsingContext context, string username) {
			var user = dataobjects.User.LoadByName(username);
			context.OnUserMention(user);
			var url = new URL.users.user.Info(user.id.ToString(), null);
			return String.Format("<a class=\"separate userreference UG_{0}\" href=\"{1}\">{2}</a>", Safe(user.userGroup.name), url.canonical, Safe(user.name));
		}

	}
}
