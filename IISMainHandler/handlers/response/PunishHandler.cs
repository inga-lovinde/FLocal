using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {

	class PunishHandler : AbstractNewMessageHandler {

		override protected string templateName {
			get {
				return "PostPunish.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificNewMessageData(WebContext context) {
			Post post = Post.LoadById(int.Parse(context.requestParts[1]));

			if(!Moderator.isModerator(context.account, post.thread.board)) throw new FLocalException(context.account.id + " is not a moderator in board " + post.thread.board.id);
			if(context.account.user.id == post.poster.id) throw new FLocalException("You cannot punish your own posts");
			
			return new XElement[] {
				post.thread.board.exportToXml(context, false),
				post.thread.exportToXml(context),
				post.exportToXml(context),
				post.latestRevision.exportToXml(context),
				new XElement("layers",
					from layer in PostLayer.allLayers select layer.exportToXml(context)
				),
				new XElement("punishmentTypes",
					from punishmentType in PunishmentType.allTypes select punishmentType.exportToXml(context)
				)
			};
		}
	}

}