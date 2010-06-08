using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FLocal.Common.dataobjects {
	public class Board : SqlObject<Board> {

		private class TableSpec : FLocal.Core.DB.ITableSpec {
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return "boards"; } }
			public string idName { get { return "id"; } }
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

		private int _lastPostId;
		public int lastPostId {
			get {
				this.LoadIfNotLoaded();
				return this._lastPostId;
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
			this._name = data["name"];
			this._description = data["comment"];
			this._lastPostId = int.Parse(data["lastPostId"]);
			this._categoryId = int.Parse(data["categoryId"]);
		}

		public XElement exportToXml() {
			return new XElement("board",
				new XElement("name", this.name),
				new XElement("description", this.description)
			);
		}

	}
}
