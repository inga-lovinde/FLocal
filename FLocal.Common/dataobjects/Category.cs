using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;
using Web.Core.DB;
using System.Xml.Linq;

namespace FLocal.Common.dataobjects {
	public class Category : SqlObject<Category> {

		public class TableSpec : IComplexSqlObjectTableSpec {
			public const string TABLE = "Categories";
			public const string FIELD_ID = "Id";
			public const string FIELD_SORTORDER = "SortOrder";
			public const string FIELD_NAME = "Name";

			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
			public void refreshSqlObjectAndRelated(int id) {
				Refresh(id);
				LoadById(id).subBoards_Reset();
			}
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private string _name;
		public string name {
			get {
				this.LoadIfNotLoaded();
				return this._name;
			}
		}

		private int _sortOrder;
		public int sortOrder {
			get {
				this.LoadIfNotLoaded();
				return this._sortOrder;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._name = data[TableSpec.FIELD_NAME];
			this._sortOrder = int.Parse(data[TableSpec.FIELD_SORTORDER]);
		}

		private static readonly object allCategories_Locker = new object();
		public static IEnumerable<Category> allCategories {
			get {
				return
					from id in Cache<IEnumerable<int>>.instance.get(
						allCategories_Locker,
						() => {
							IEnumerable<int> ids = (from stringId in Config.instance.mainConnection.LoadIdsByConditions(
								TableSpec.instance,
								new Web.Core.DB.conditions.EmptyCondition(),
								Diapasone.unlimited
							) select int.Parse(stringId)).ToList();
							Category.LoadByIds(ids);
							return ids;
						}
					)
					let category = Category.LoadById(id)
					orderby category.sortOrder, category.id
					select category;
			}
		}
		internal static void allCategories_Reset() {
			Cache<IEnumerable<int>>.instance.delete(allCategories_Locker);
		}

		private readonly object subBoards_Locker = new object();
		public IEnumerable<Board> subBoards {
			get {
				return
					from id in Cache<IEnumerable<int>>.instance.get(
						this.subBoards_Locker,
						() => {
							IEnumerable<int> ids = (from stringId in Config.instance.mainConnection.LoadIdsByConditions(
								Board.TableSpec.instance,
								new Web.Core.DB.conditions.ComparisonCondition(
									Board.TableSpec.instance.getColumnSpec(Board.TableSpec.FIELD_CATEGORYID),
									Web.Core.DB.conditions.ComparisonType.EQUAL,
									this.id.ToString()
								),
								Diapasone.unlimited
							) select int.Parse(stringId)).ToList();
							Board.LoadByIds(ids);
							return ids;
						}
					)
					let board = Board.LoadById(id)
					where board.sortOrder >= 0
					orderby board.sortOrder, board.id
					select board;
			}
		}
		internal void subBoards_Reset() {
			Cache<IEnumerable<int>>.instance.delete(this.subBoards_Locker);
		}

		public XElement exportToXmlSimple(UserContext context) {
			return new XElement("category",
				new XElement("id", this.id),
				new XElement("name", this.name)
			);
		}

		public XElement exportToXmlForMainPage(UserContext context) {
			return new XElement("category",
				new XElement("id", this.id),
				new XElement("name", this.name),
				new XElement("sortOrder", this.sortOrder),
				new XElement("boards", from board in this.subBoards select board.exportToXml(context, Board.SubboardsOptions.FirstLevel))
			);
		}

		public XElement exportToXmlForTree(UserContext context) {
			return new XElement("category",
				new XElement("id", this.id),
				new XElement("name", this.name),
				new XElement("sortOrder", this.sortOrder),
				new XElement("boards", from board in this.subBoards select board.exportToXmlSimple(context, Board.SubboardsOptions.AllLevels))
			);
		}

	}
}
