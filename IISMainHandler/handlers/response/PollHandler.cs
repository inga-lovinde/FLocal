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

	class PollHandler : AbstractGetHandler {

		override protected string templateName {
			get {
				return "Poll.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			Poll poll = Poll.LoadById(int.Parse(context.requestParts[1]));
			return new XElement[] {
				poll.exportToXmlWithVotes(context)
			};
		}

	}

}