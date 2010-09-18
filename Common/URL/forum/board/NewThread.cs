using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Common.dataobjects;

namespace FLocal.Common.URL.forum.board {
	public class NewThread : AbstractUrl {

		public readonly Board board;

		public NewThread(string boardId, string remainder) : base(remainder) {
			this.board = Board.LoadById(int.Parse(boardId));
		}

		public override string title {
			get {
				return this.board.name;
			}
		}

		protected override string _canonical {
			get {
				return "/Forum/Board/" + this.board.id + "/NewThread/";
			}
		}
	}
}
