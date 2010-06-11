using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;

namespace FLocal.Common.dataobjects {
	public class Board : SqlObject<Board> {

		public class TableSpec : FLocal.Core.DB.ITableSpec {
			public const string TABLE = "Boards";
			public const string FIELD_ID = "Id";
			public const string FIELD_SORTORDER = "SortOrder";
			public const string FIELD_CATEGORYID = "CategoryId";
			public const string FIELD_LASTPOSTID = "LastPostId";
			public const string FIELD_TOTALPOSTS = "TotalPosts";
			public const string FIELD_TOTALTHREADS = "TotalThreads";
			public const string FIELD_NAME = "Name";
			public const string FIELD_DESCRIPTION = "Comment";
			public const string FIELD_PARENTBOARDID = "ParentBoardId";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
		}

		protected override FLocal.Core.DB.ITableSpec table { get { return TableSpec.instance; } }

		private int _sortOrder;
		public int sortOrder {
			get {
				this.LoadIfNotLoaded();
				return this._sortOrder;
			}
		}
		
		private int? _categoryId;
		public int? categoryId {
			get {
				this.LoadIfNotLoaded();
				return this._categoryId;
			}
		}
		public Category category {
			get {
				return Category.LoadById(this.categoryId.Value);
			}
		}

		private int? _lastPostId;
		public int? lastPostId {
			get {
				this.LoadIfNotLoaded();
				return this._lastPostId;
			}
		}

		private int _totalPosts;
		public int totalPosts {
			get {
				this.LoadIfNotLoaded();
				return this._totalPosts;
			}
		}

		private int _totalThreads;
		public int totalThreads {
			get {
				this.LoadIfNotLoaded();
				return this._totalThreads;
			}
		}

		private string _name;
		public string name {
			get {
				this.LoadIfNotLoaded();
				return this._name;
			}
		}

		private string _description;
		public string description {
			get {
				this.LoadIfNotLoaded();
				return this._description;
			}
		}

		private int? _parentBoardId;
		public int? parentBoardId {
			get {
				this.LoadIfNotLoaded();
				return this._parentBoardId;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._sortOrder = int.Parse(data[TableSpec.FIELD_SORTORDER]);
			this._categoryId = Util.ParseInt(data[TableSpec.FIELD_CATEGORYID]);
			this._lastPostId = Util.ParseInt(data[TableSpec.FIELD_LASTPOSTID]);
			this._totalPosts = int.Parse(data[TableSpec.FIELD_TOTALPOSTS]);
			this._totalThreads = int.Parse(data[TableSpec.FIELD_TOTALTHREADS]);
			this._name = data[TableSpec.FIELD_NAME];
			this._description = data[TableSpec.FIELD_DESCRIPTION];
			this._parentBoardId = Util.ParseInt(data[TableSpec.FIELD_PARENTBOARDID]);
		}

		private readonly object subBoards_Locker = new object();
		public IEnumerable<Board> subBoards {
			get {
				return
					from id in Cache<IEnumerable<int>>.instance.get(
						this.subBoards_Locker,
						() => {
							IEnumerable<int> ids = from stringId in Config.instance.mainConnection.LoadIdsByConditions(
								TableSpec.instance,
								new FLocal.Core.DB.conditions.ComparisonCondition(
									TableSpec.instance.getColumnSpec(Board.TableSpec.FIELD_PARENTBOARDID),
									FLocal.Core.DB.conditions.ComparisonType.EQUAL,
									this.id.ToString()
								),
								Diapasone.unlimited,
								new JoinSpec[0]
							) select int.Parse(stringId);
							Board.LoadByIds(ids);
							return ids;
						}
					)
					let board = Board.LoadById(id)
					orderby board.sortOrder, board.id
					select board;
			}
		}

		private bool hasNewPosts() {
			return Core.Util.RandomInt(0, 1000) < 500;
		}

		private XElement exportLastPostInfo() {
			if(!this.lastPostId.HasValue) {
				return new XElement("none");
			} else {
				throw new NotImplementedException();
			}
		}

		public XElement exportToXmlForMainPage(UserContext context) {
			XElement result = new XElement("board",
				new XElement("id", this.id),
				new XElement("sortOrder", this.sortOrder),
				new XElement("categoryId", this.categoryId),
				new XElement("totalPosts", this.totalPosts),
				new XElement("totalThreads", this.totalThreads),
				new XElement("name", this.name),
				new XElement("description", this.description),
				new XElement("hasNewPosts", this.hasNewPosts() ? "true" : "false"),
				new XElement("lastPostInfo", this.exportLastPostInfo())
			);

			if(!this.parentBoardId.HasValue) {
				result.Add(new XElement("subBoards",
					from board in this.subBoards select board.exportToXmlForMainPage(context)
				));
			}

			return result;
		}

	}
}
