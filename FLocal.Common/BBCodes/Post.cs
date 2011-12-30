using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class Post : AbstractLocalLink {

		public Post()
			: base("post") {
		}

		protected override FLocal.Common.URL.AbstractUrl url {
			get {
				return new URL.forum.board.thread.post.Show(this.DefaultOrValue, null);
			}
		}

	}
}
