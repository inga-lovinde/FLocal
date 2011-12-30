using System;
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

namespace FLocal.IISHandler.handlers.response {

	class PollHandler : AbstractGetHandler<FLocal.Common.URL.polls.Info> {

		override protected string templateName {
			get {
				return "Poll.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			Poll poll = this.url.poll;
			return new XElement[] {
				poll.exportToXmlWithVotes(context)
			};
		}

	}

}