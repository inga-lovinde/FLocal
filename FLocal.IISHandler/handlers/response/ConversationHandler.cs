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

namespace FLocal.IISHandler.handlers.response {

	class ConversationHandler : AbstractGetHandler<FLocal.Common.URL.my.conversations.Conversation> {

		override protected string templateName {
			get {
				return "Conversation.xslt";
			}
		}

		override protected IEnumerable<XElement> getSpecificData(WebContext context) {
			Account interlocutor = this.url.interlocutor;
			PMConversation conversation = PMConversation.LoadByAccounts(context.session.account, interlocutor);
			PageOuter pageOuter = PageOuter.createFromUrl(
				this.url,
				context.userSettings.postsPerPage,
				new Dictionary<char, Func<string, long>> {
					{
						'p',
						s => Config.instance.mainConnection.GetCountByConditions(
							PMMessage.TableSpec.instance,
							new ComplexCondition(
								ConditionsJoinType.AND,
								new ComparisonCondition(
									PMMessage.TableSpec.instance.getColumnSpec(PMMessage.TableSpec.FIELD_OWNERID),
									ComparisonType.EQUAL,
									context.session.account.id.ToString()
								),
								new ComparisonCondition(
									PMMessage.TableSpec.instance.getColumnSpec(PMMessage.TableSpec.FIELD_INTERLOCUTORID),
									ComparisonType.EQUAL,
									interlocutor.id.ToString()
								),
								new ComparisonCondition(
									PMMessage.TableSpec.instance.getIdSpec(),
									ComparisonType.LESSTHAN,
									int.Parse(s).ToString()
								)
							)
						)
					}
				}
			);
			IEnumerable<PMMessage> messages = conversation.getMessages(pageOuter, context, pageOuter.ascendingDirection);

			XElement[] result = new XElement[] {
				conversation.exportToXml(context, false),
				new XElement("messages",
					from message in messages select message.exportToXml(context),
					pageOuter.exportToXml(2, 5, 2)
				)
			};

			if(messages.Count() > 0) {
				conversation.markAsRead(context.session.account, messages.Min(), messages.Max());
			}

			foreach(var message in messages) {
				message.MarkAsRead(context.session.account);
			}

			return result;
		}

	}

}