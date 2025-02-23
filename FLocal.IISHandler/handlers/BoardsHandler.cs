﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Common;
using FLocal.Common.dataobjects;
using Web.Core;
using Web.Core.DB;
using Web.Core.DB.conditions;

namespace FLocal.IISHandler.handlers {

	class BoardsHandler : AbstractGetHandler<FLocal.Common.URL.forum.Boards> {

		override protected string templateName {
			get {
				return "Boards.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			return new XElement[] {
				new XElement("categories", from category in Category.allCategories select category.exportToXmlForMainPage(context)),
				new XElement("totalRegistered", Config.instance.mainConnection.GetCountByConditions(User.TableSpec.instance, new EmptyCondition())),
				new XElement("activity",
					new XElement("threshold", Config.instance.ActivityThreshold.ToString()),
					new XElement("sessions", Config.instance.mainConnection.GetCountByConditions(
						Session.TableSpec.instance,
						new ComparisonCondition(
							Session.TableSpec.instance.getColumnSpec(Session.TableSpec.FIELD_LASTHUMANACTIVITY),
							ComparisonType.GREATEROREQUAL,
							DateTime.Now.Subtract(Config.instance.ActivityThreshold).ToUTCString()
						)
					))
				),
				new XElement("currentDate", DateTime.Now.ToXml()),
			};
		}

	}

}