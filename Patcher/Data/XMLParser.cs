using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data
{
	static class XMLParser
	{
	
		private static ForeignKeyConstraint.ReferentialAction ParseReferentialAction(XElement element)
		{
			if(element == null) {
				return ForeignKeyConstraint.ReferentialAction.NoAction;
			}
			switch(element.Value)
			{
				case "noAction":
					return ForeignKeyConstraint.ReferentialAction.NoAction;
				case "cascade":
					return ForeignKeyConstraint.ReferentialAction.Cascade;
				case "setNull":
					return ForeignKeyConstraint.ReferentialAction.SetNull;
				case "setDefault":
					return ForeignKeyConstraint.ReferentialAction.SetDefault;
				default:
					throw new FormattableException("Unknown referential action {0}", element.Value);
			}
		}
	
		public static AbstractConstraint ParseConstraint(XElement element)
		{
			XElement[] children = element.Elements().ToArray();
			if(children.Length != 3)
			{
				throw new FormattableException("Wrong children count");
			}
			
			if(children[0].Name != "table")
			{
				throw new FormattableException("Expected element name {0}, got {1}", "table", children[0].Name);
			}
			string table = children[0].Value;
			
			if(children[1].Name != "constraintName")
			{
				throw new FormattableException("Expected element name {0}, got {1}", "constraintName", children[1].Name);
			}
			string constraintName = children[1].Value;

			XElement specific = children[2];
			
			switch(specific.Name.ToString())
			{
				case "foreignKey":
					return new ForeignKeyConstraint(
						table,
						constraintName,
						specific.Element("column").Value,
						specific.Element("referencedTable").Value,
						ParseReferentialAction(specific.Element("onUpdate")),
						ParseReferentialAction(specific.Element("onDelete"))
					);
				case "unique":
					return new UniqueConstraint(table, constraintName, new HashSet<string>(from elem in specific.Elements("column") select elem.Value));
				case "check":
					return new CheckConstraint(table, constraintName, specific.Element("condition").Value);
				default:
					throw new FormattableException("Unknown constraint type {0}", children[2].Name);
			}
		}

		public static ColumnOptions ParseColumnOptions(XElement element)
		{
			return new ColumnOptions(
				element.Element("type").Value,
				element.Element("defaultValue") != null ? element.Element("defaultValue").Value : null,
				element.Element("isNotNull") != null
			);
		}

		public static ColumnReference ParseColumnReference(XElement element) {
			return new ColumnReference(
				element.Element("table").Value,
				element.Element("column").Value
			);
		}

		private static ColumnDescription ParseColumnDescription(XElement element, string table)
		{
			return new ColumnDescription(
				new ColumnReference(
					table,
					element.Element("column").Value
				),
				ParseColumnOptions(element)
			);
		}

		public static TableDescription ParseTableDescription(XElement element) {
			string table = element.Element("table").Value;
			return new TableDescription(
				table,
				ParseColumnDescription(element.Element("primaryKey"), table),
				from elem in element.Elements("column") select ParseColumnDescription(elem, table)
			);
		}

	}
}
