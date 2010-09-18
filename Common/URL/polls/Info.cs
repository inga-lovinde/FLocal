using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Common.dataobjects;

namespace FLocal.Common.URL.polls {
	public class Info : AbstractUrl {

		public readonly Poll poll;

		public Info(string pollId, string remainder) : base(remainder) {
			this.poll = Poll.LoadById(int.Parse(pollId));
		}

		public override string title {
			get {
				return this.poll.title;
			}
		}

		protected override string _canonical {
			get {
				return "/Polls/Info/" + this.poll.id + "/";
			}
		}
	}
}
