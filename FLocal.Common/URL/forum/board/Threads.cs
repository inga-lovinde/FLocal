using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Common.dataobjects;

namespace FLocal.Common.URL.forum.board {
	public class Threads : Abstract {

		public Threads(string boardId, string remainder) : base(boardId, remainder) {
		}

		protected override string _canonical {
			get {
				return base._canonical + "Threads/";
			}
		}
	}
}
