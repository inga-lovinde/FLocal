using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Common;
using FLocal.Common.dataobjects;
using Web.Core.DB;
using Web.Core.DB.conditions;

namespace FLocal.IISHandler.handlers.response {

	class ActiveAccountListHandler : AbstractGetHandler<FLocal.Common.URL.users.Active> {

		override protected string templateName {
			get {
				return "UserList.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			PageOuter pageOuter = PageOuter.createUnlimited(context.userSettings.usersPerPage);
			IEnumerable<Account> accounts = Account.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					Account.TableSpec.instance,
					new Web.Core.DB.conditions.ComparisonCondition(
						Account.TableSpec.instance.getColumnSpec(Account.TableSpec.FIELD_NEEDSMIGRATION),
						Web.Core.DB.conditions.ComparisonType.EQUAL,
						"0"
					),
					pageOuter
				) select int.Parse(stringId)
			);
			return new XElement[] {
				new XElement("users", 
					from account in accounts
					select account.user.exportToXmlForViewing(
						context,
						new XElement(
							"actualPosts",
							Config.instance.mainConnection.GetCountByConditions(
								Post.TableSpec.instance,
								new ComplexCondition(
									ConditionsJoinType.AND,
									new ComparisonCondition(
										Post.TableSpec.instance.getColumnSpec(Post.TableSpec.FIELD_POSTERID),
										ComparisonType.EQUAL,
										account.user.id.ToString()
									),
									new ComparisonCondition(
										Post.TableSpec.instance.getIdSpec(),
										ComparisonType.GREATEROREQUAL,
										Thread.FORMALREADMIN.ToString()
									)
								)
							)
						)
					)
				)
			};
		}

	}

}