using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Common.dataobjects;

namespace FLocal.Common.URL.forum.board {
	public abstract class Abstract : AbstractUrl {

		private static int extractBoardId(string boardId) {
			return int.Parse(boardId.Contains('-') ? boardId.Substring(0, boardId.IndexOf('-')) : boardId);
		}

		public readonly Board board;

		public Abstract(int boardId, string remainder) : base(remainder) {
			this.board = Board.LoadById(boardId);
		}
		public Abstract(string boardId, string remainder) : this(extractBoardId(boardId), remainder) {
		}

		public override string title {
			get {
				return this.board.name;
			}
		}

		protected override string _canonical {
			get {
				return "/Forum/Board/" + this.board.id + "-" + this.board.nameTranslit + "/";
			}
		}
	}
}
