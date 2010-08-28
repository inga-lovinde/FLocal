using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Common;
using FLocal.Common.dataobjects;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.IISHandler.handlers.response {

	class LocalNetworksListHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "LocalNetworks.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			IEnumerable<LocalNetwork> localNetworks = LocalNetwork.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					LocalNetwork.TableSpec.instance,
					new Core.DB.conditions.EmptyCondition(),
					Diapasone.unlimited
				) select int.Parse(stringId)
			);
			return new XElement[] {
				new XElement("localNetworks", from localNetwork in localNetworks select localNetwork.exportToXml(context)),
			};
		}

	}

}