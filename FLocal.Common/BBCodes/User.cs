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
			return UserMentionProcessor.ProcessUserMention(context, this.DefaultOrValue);
		}

	}
}
