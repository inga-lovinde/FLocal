using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Common.dataobjects;

namespace FLocal.Common.URL.my.conversations {
	public class Conversation : AbstractUrl {

		public Account interlocutor;

		public Conversation(string interlocutorId, string remainder) : base(remainder) {
			this.interlocutor = Account.LoadById(int.Parse(interlocutorId));
		}

		public override string title {
			get {
				return this.interlocutor.user.name;
			}
		}

		protected override string _canonical {
			get {
				return "/My/Conversations/Conversation/" + this.interlocutor.id + "/";
			}
		}
	}
}
