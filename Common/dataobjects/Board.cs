using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

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
		
		private int _categoryId;
		public int categoryId {
			get {
				this.LoadIfNotLoaded();
				return this._categoryId;
			}
		}
		public Category category {
			get {
				return Category.LoadById(this.categoryId);
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

		protected override void doFromHash(Dictionary<string, string> data) {
			this._sortOrder = int.Parse(data[TableSpec.FIELD_SORTORDER]);
			this._categoryId = int.Parse(data[TableSpec.FIELD_CATEGORYID]);
			if(data[TableSpec.FIELD_LASTPOSTID] != "") {
				this._lastPostId = int.Parse(data[TableSpec.FIELD_LASTPOSTID]);
			} else {
				this._lastPostId = null;
			}
			this._totalPosts = int.Parse(data[TableSpec.FIELD_TOTALPOSTS]);
			this._totalThreads = int.Parse(data[TableSpec.FIELD_TOTALTHREADS]);
			this._name = data[TableSpec.FIELD_NAME];
			this._description = data[TableSpec.FIELD_DESCRIPTION];
		}

		public XElement exportToXmlForMainPage() {
			return new XElement("board",
				new XElement("id", this.id),
				new XElement("sortOrder", this.sortOrder),
				new XElement("categoryId", this.categoryId),
				new XElement("lastPostId", this.lastPostId),
				new XElement("totalPosts", this.totalPosts),
				new XElement("totalThreads", this.totalThreads),
				new XElement("name", this.name),
				new XElement("description", this.description)
			);
		}

	}
}
