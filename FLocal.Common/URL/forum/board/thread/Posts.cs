using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Common.dataobjects;

namespace FLocal.Common.URL.forum.board.thread {
	public class Posts : Abstract {

		public Posts(string threadId, string remainder) : base(threadId, remainder) {
		}

		protected override string _canonical {
			get {
				return base._canonical + "Posts/";
			}
		}
	}
}
