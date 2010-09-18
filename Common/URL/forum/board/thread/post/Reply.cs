using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Common.dataobjects;

namespace FLocal.Common.URL.forum.board.thread.post {
	public class Reply : AbstractUrl {

		public Post post;

		public Reply(string postId, string remainder) : base(remainder) {
			this.post = Post.LoadById(int.Parse(postId));
		}

		public override string title {
			get {
				return post.title;
			}
		}

		protected override string _canonical {
			get {
				return "/Forum/Board/" + this.post.thread.boardId + "/Thread/" + this.post.threadId + "/Post/" + post.id + "/Reply/";
			}
		}

	}

}
