using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Common.dataobjects;

namespace FLocal.Common.URL.forum.board.thread {
	public abstract class Abstract : board.Abstract {

		private static int extractThreadId(string threadId) {
			return int.Parse(threadId.Contains('-') ? threadId.Substring(0, threadId.IndexOf('-')) : threadId);
		}

		public readonly Thread thread;

		public Abstract(int threadId, string remainder)
		: base(Thread.LoadById(threadId).boardId, remainder) {
			this.thread = Thread.LoadById(threadId);
		}
		public Abstract(string threadId, string remainder) : this(extractThreadId(threadId), remainder) {
		}

		public override string title {
			get {
				return this.thread.title;
			}
		}

		protected override string _canonical {
			get {
				return base._canonical + "Thread/" + this.thread.id + "-" + this.thread.titleTranslit + "/";
			}
		}
	}
}
