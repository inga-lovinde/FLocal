using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Common.dataobjects;

namespace FLocal.Common.URL.forum.board.thread.post {
	public class Show : Abstract {

		public Show(string postId, string remainder) : base(postId, remainder) {
		}

		protected override string _canonical {
			get {
				return base._canonical + "Show/";
			}
		}

	}

}
