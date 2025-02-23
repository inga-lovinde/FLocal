﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Web.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {

	class PunishHandler : AbstractNewMessageHandler<FLocal.Common.URL.forum.board.thread.post.Punish> {

		override protected string templateName {
			get {
				return "PostPunish.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificNewMessageData(WebContext context) {
			Post post = this.url.post;

			if(!Moderator.isModerator(context.account, post.thread)) throw new FLocalException(context.account.id + " is not a moderator in board " + post.thread.board.id);
			
			return new XElement[] {
				new XElement("currentLocation", post.exportToXmlSimpleWithParent(context)),
				post.thread.board.exportToXml(context, Board.SubboardsOptions.None),
				post.thread.exportToXml(context),
				post.exportToXml(context),
				new XElement("layers",
					from layer in PostLayer.allLayers where layer.name != PostLayer.NAME_HIDDEN select layer.exportToXml(context)
				),
				new XElement("categories",
					from category in Category.allCategories select
					category.exportToXmlForTree(context)
				),
				new XElement("punishmentTypes",
					from punishmentType in PunishmentType.allTypes select punishmentType.exportToXml(context)
				),
				new XElement("isTrueModerator", Moderator.isTrueModerator(context.account, post.thread.board).ToPlainString())
			};
		}
	}
	
}