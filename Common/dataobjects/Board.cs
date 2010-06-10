using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FLocal.Common.dataobjects {
	public class Board : SqlObject<Board> {

		private class TableSpec : FLocal.Core.DB.ITableSpec {
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return "Boards"; } }
			public string idName { get { return "Id"; } }
		}

		protected override FLocal.Core.DB.ITableSpec table { get { return TableSpec.instance; } }

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

		private int? _lastPostId;
		public int lastPostId {
			get {
				this.LoadIfNotLoaded();
				return this._lastPostId.Value;
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

		protected override void doFromHash(Dictionary<string, string> data) {
			this._name = data["Name"];
			this._description = data["Comment"];
			if(data["LastPostId"] != "") {
				this._lastPostId = int.Parse(data["LastPostId"]);
			} else {
				this._lastPostId = null;
			}
			this._categoryId = int.Parse(data["CategoryId"]);
		}

		public XElement exportToXml() {
			return new XElement("board",
				new XElement("name", this.name),
				new XElement("description", this.description)
			);
		}

	}
}
