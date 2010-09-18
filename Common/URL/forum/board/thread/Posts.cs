using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Common.dataobjects;

namespace FLocal.Common.URL.forum.board.thread {
	public class Posts : AbstractUrl {

		public readonly dataobjects.Thread thread;

		public Posts(string threadId, string remainder) : base(remainder) {
			this.thread = dataobjects.Thread.LoadById(int.Parse(threadId));
		}

		public override string title {
			get {
				return this.thread.title;
			}
		}

		protected override string _canonical {
			get {
				return "/Forum/Board/" + this.thread.boardId + "/Thread/" + this.thread.id + "/Posts/";
			}
		}
	}
}
