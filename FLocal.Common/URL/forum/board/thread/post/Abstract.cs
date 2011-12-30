using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Common.dataobjects;

namespace FLocal.Common.URL.forum.board.thread.post {
	public abstract class Abstract : thread.Abstract {

		private static int extractPostId(string postId) {
			return int.Parse(postId);
		}

		public readonly Post post;

		public Abstract(int postId, string remainder)
		: base(Post.LoadById(postId).threadId, remainder) {
			this.post = Post.LoadById(postId);
		}
		public Abstract(string postId, string remainder) : this(extractPostId(postId), remainder) {
		}

		public override string title {
			get {
				return this.post.title;
			}
		}

		protected override string _canonical {
			get {
				return base._canonical + "Post/" + post.id + "/";
			}
		}

	}

}
