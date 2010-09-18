using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Common;
using FLocal.Common.dataobjects;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.IISHandler.handlers.response {

	class AllThreadsHandler : AbstractGetHandler<FLocal.Common.URL.forum.AllThreads> {

		override protected string templateName {
			get {
				return "AllThreads.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			PageOuter pageOuter = PageOuter.createFromUrl(this.url, context.userSettings.threadsPerPage);
			IEnumerable<Thread> threads = Thread.LoadByIds(
				from stringId
				in Config.instance.mainConnection.LoadIdsByConditions(
					Thread.TableSpec.instance,
					new ComparisonCondition(
						Thread.TableSpec.instance.getColumnSpec(Thread.TableSpec.FIELD_LASTPOSTID),
						ComparisonType.GREATEROREQUAL,
						Thread.FORMALREADMIN.ToString()
					),
					pageOuter,
					new JoinSpec[0],
					new SortSpec[] {
						new SortSpec(
							Thread.TableSpec.instance.getColumnSpec(Thread.TableSpec.FIELD_LASTPOSTID),
							pageOuter.descendingDirection
						)
					}
				)
				select int.Parse(stringId)
			);

			XElement[] result = new XElement[] {
				new XElement("threads",
					from thread in threads select thread.exportToXml(context),
					pageOuter.exportToXml(1, 5, 1)
				)
			};

			return result;
		}

	}

}