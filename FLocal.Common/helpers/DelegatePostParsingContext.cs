using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Common.dataobjects;

namespace FLocal.Common.helpers {
	class DelegatePostParsingContext : BBCodes.IPostParsingContext {

		private readonly Action<User> onUserMention;

		public DelegatePostParsingContext(Action<User> onUserMention) {
			this.onUserMention = onUserMention;
		}

		#region IPostParsingContext Members

		public void OnUserMention(User user) {
			this.onUserMention(user);
		}

		#endregion

	}
}
