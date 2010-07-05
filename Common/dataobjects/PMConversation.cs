using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;
using FLocal.Common.actions;

namespace FLocal.Common.dataobjects {
	public class PMConversation : SqlObject<PMConversation> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "PMConversations";
			public const string FIELD_ID = "Id";
			public const string FIELD_OWNERID = "OwnerAccountId";
			public const string FIELD_INTERLOCUTORID = "InterlocutorAccountId";
			public const string FIELD_TOTALMESSAGES = "TotalMessages";
			public const string FIELD_LASTMESSAGEID = "LastMessageId";
			public const string FIELD_LASTMESSAGEDATE = "LastMessageDate";
			public const string FIELD_LASTREADMESSAGEID = "LastReadMessageId";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private int _ownerId;
		public int ownerId {
			get {
				this.LoadIfNotLoaded();
				return this._ownerId;
			}
		}
		public Account owner {
			get {
				return Account.LoadById(this.ownerId);
			}
		}

		private int _interlocutorId;
		public int interlocutorId {
			get {
				this.LoadIfNotLoaded();
				return this._interlocutorId;
			}
		}
		public Account interlocutor {
			get {
				return Account.LoadById(this.interlocutorId);
			}
		}

		private int _totalMessages;
		public int totalMessages {
			get {
				this.LoadIfNotLoaded();
				return this._totalMessages;
			}
		}

		private int _lastMessageId;
		public int lastMessageId {
			get {
				this.LoadIfNotLoaded();
				return this._lastMessageId;
			}
		}
		public PMMessage lastMessage {
			get {
				return PMMessage.LoadById(this.lastMessageId);
			}
		}

		private DateTime _lastMessageDate;
		public DateTime lastMessageDate {
			get {
				this.LoadIfNotLoaded();
				return this._lastMessageDate;
			}
		}
		
		private int? _lastReadMessageId;
		public int? lastReadMessageId {
			get {
				this.LoadIfNotLoaded();
				return this._lastReadMessageId;
			}
		}
		public PMMessage lastReadMessage {
			get {
				return PMMessage.LoadById(this.lastReadMessageId.Value);
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._ownerId = int.Parse(data[TableSpec.FIELD_OWNERID]);
			this._interlocutorId = int.Parse(data[TableSpec.FIELD_INTERLOCUTORID]);
			this._totalMessages = int.Parse(data[TableSpec.FIELD_TOTALMESSAGES]);
			this._lastMessageId = int.Parse(data[TableSpec.FIELD_LASTMESSAGEID]);
			this._lastMessageDate = new DateTime(long.Parse(data[TableSpec.FIELD_LASTMESSAGEDATE]));
			this._lastReadMessageId = Util.ParseInt(data[TableSpec.FIELD_LASTREADMESSAGEID]);
		}

		public XElement exportToXmlSimpleWithParent(UserContext context) {
			return new XElement("conversation",
				new XElement("id", this.id),
				new XElement("name", this.interlocutor.user.name)
			);
		}

		public XElement exportToXml(UserContext context, bool includeFirstPost, params XElement[] additional) {
			if((context.account == null) || (context.account.id != this.owner.id)) throw new AccessViolationException();
			XElement result = new XElement("conversation",
				new XElement("id", this.id),
				new XElement("owner", this.owner.exportToXml(context)),
				new XElement("interlocutor", this.interlocutor.exportToXml(context)),
				new XElement("totalMessages", this.totalMessages),
				new XElement("lastMessageId", this.lastMessageId),
				new XElement("lastMessageDate", this.lastMessageDate.ToXml()),
				new XElement("lastReadMessageId", this.lastReadMessageId),
				new XElement("afterLastRead", this.lastReadMessageId + 1),
				new XElement(
					"totalNewMessages",
					Config.instance.mainConnection.GetCountByConditions(
						PMMessage.TableSpec.instance,
						new ComplexCondition(
							ConditionsJoinType.AND,
							new ComparisonCondition(
								PMMessage.TableSpec.instance.getColumnSpec(PMMessage.TableSpec.FIELD_OWNERID),
								ComparisonType.EQUAL,
								this.ownerId.ToString()
							),
							new ComparisonCondition(
								PMMessage.TableSpec.instance.getColumnSpec(PMMessage.TableSpec.FIELD_INTERLOCUTORID),
								ComparisonType.EQUAL,
								this.interlocutorId.ToString()
							),
							new ComparisonCondition(
								PMMessage.TableSpec.instance.getIdSpec(),
								ComparisonType.GREATERTHAN,
								(this.lastReadMessageId.HasValue ? this.lastReadMessageId.Value : 0).ToString()
							)
						)
					)
				),
				context.formatTotalPosts(this.totalMessages)
			);
			if(additional.Length > 0) {
				result.Add(additional);
			}
			return result;
		}

		public IEnumerable<PMMessage> getMessages(Diapasone diapasone, UserContext context) {
			return PMMessage.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					PMMessage.TableSpec.instance,
					new ComplexCondition(
						ConditionsJoinType.AND,
						new ComparisonCondition(
							PMMessage.TableSpec.instance.getColumnSpec(PMMessage.TableSpec.FIELD_OWNERID),
							ComparisonType.EQUAL,
							this.ownerId.ToString()
						),
						new ComparisonCondition(
							PMMessage.TableSpec.instance.getColumnSpec(PMMessage.TableSpec.FIELD_INTERLOCUTORID),
							ComparisonType.EQUAL,
							this.interlocutorId.ToString()
						)
					),
					diapasone,
					new JoinSpec[0],
					new SortSpec[] {
						new SortSpec(
							PMMessage.TableSpec.instance.getIdSpec(),
							true
						),
					}
				) select int.Parse(stringId)
			);
		}

		public void markAsRead(Account account, PMMessage minMessage, PMMessage maxMessage) {
			if(this.ownerId != account.id) throw new AccessViolationException();
			ChangeSetUtil.ApplyChanges(new AbstractChange[] {
				new UpdateChange(
					TableSpec.instance,
					new Dictionary<string,AbstractFieldValue> {
						{
							TableSpec.FIELD_LASTREADMESSAGEID,
							new IncrementFieldValue(
								s => {
									if((s == null) || (s == "")) {
										s = "0"; //workaround
									}
									if(maxMessage.id < int.Parse(s)) {
										return (s == "0") ? null : s; //if some newer posts were already read
									}
									long count = Config.instance.mainConnection.GetCountByConditions(
										PMMessage.TableSpec.instance,
										new ComplexCondition(
											ConditionsJoinType.AND,
											new ComparisonCondition(
												PMMessage.TableSpec.instance.getColumnSpec(PMMessage.TableSpec.FIELD_OWNERID),
												ComparisonType.EQUAL,
												this.ownerId.ToString()
											),
											new ComparisonCondition(
												PMMessage.TableSpec.instance.getColumnSpec(PMMessage.TableSpec.FIELD_INTERLOCUTORID),
												ComparisonType.EQUAL,
												this.interlocutorId.ToString()
											),
											new ComparisonCondition(
												PMMessage.TableSpec.instance.getIdSpec(),
												ComparisonType.GREATERTHAN,
												s
											),
											new ComparisonCondition(
												PMMessage.TableSpec.instance.getIdSpec(),
												ComparisonType.LESSTHAN,
												minMessage.id.ToString()
											)
										)
									);
									if(count > 0) {
										return (s == "0") ? null : s; //if there are some unread posts earlier than minPost
									} else {
										return maxMessage.id.ToString();
									}
								}
							)
						}
					},
					this.id
				)
			});
		}

		public static IEnumerable<PMConversation> getConversations(Account owner, Diapasone diapasone) {
			return LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					TableSpec.instance,
					new ComparisonCondition(
						TableSpec.instance.getColumnSpec(TableSpec.FIELD_OWNERID),
						ComparisonType.EQUAL,
						owner.id.ToString()
					),
					diapasone,
					new JoinSpec[0],
					new SortSpec[] {
						new SortSpec(
							TableSpec.instance.getColumnSpec(TableSpec.FIELD_LASTMESSAGEID),
							false
						)
					}
				) select int.Parse(stringId)
			);
		}

		public static PMConversation LoadByAccounts(Account owner, Account interlocutor) {
			return LoadById(
					int.Parse(Config.instance.mainConnection.LoadIdsByConditions(
						TableSpec.instance,
						new ComplexCondition(
							ConditionsJoinType.AND,
							new ComparisonCondition(
								TableSpec.instance.getColumnSpec(TableSpec.FIELD_OWNERID),
								ComparisonType.EQUAL,
								owner.id.ToString()
							),
							new ComparisonCondition(
								TableSpec.instance.getColumnSpec(TableSpec.FIELD_INTERLOCUTORID),
								ComparisonType.EQUAL,
								interlocutor.id.ToString()
							)
						),
						Diapasone.unlimited
					).Single())
				);
		}

		public static PMMessage SendPMMessage(Account sender, Account receiver, string title, string bodyUBB) {
			string bodyIntermediate = UBBParser.UBBToIntermediate(bodyUBB);
			AbstractChange insertPmReceiver = new InsertChange(
				PMMessage.TableSpec.instance,
				new Dictionary<string,AbstractFieldValue> {
					{ PMMessage.TableSpec.FIELD_OWNERID, new ScalarFieldValue(receiver.id.ToString()) },
					{ PMMessage.TableSpec.FIELD_INTERLOCUTORID, new ScalarFieldValue(sender.id.ToString()) },
					{ PMMessage.TableSpec.FIELD_DIRECTION, new ScalarFieldValue(PMMessage.ENUM_DIRECTION_INCOMING) },
					{ PMMessage.TableSpec.FIELD_POSTDATE, new ScalarFieldValue(DateTime.Now.ToUTCString()) },
					{ PMMessage.TableSpec.FIELD_TITLE, new ScalarFieldValue(title) },
					{ PMMessage.TableSpec.FIELD_BODY, new ScalarFieldValue(bodyIntermediate) },
					{ PMMessage.TableSpec.FIELD_BODYUBB, new ScalarFieldValue(bodyUBB) },
					{ PMMessage.TableSpec.FIELD_INCOMINGPMID, new ScalarFieldValue(null) },
					{ PMMessage.TableSpec.FIELD_ISREAD, new ScalarFieldValue("0") },
				}
			);
			AbstractChange insertPmSender = new InsertChange(
				PMMessage.TableSpec.instance,
				new Dictionary<string,AbstractFieldValue> {
					{ PMMessage.TableSpec.FIELD_OWNERID, new ScalarFieldValue(sender.id.ToString()) },
					{ PMMessage.TableSpec.FIELD_INTERLOCUTORID, new ScalarFieldValue(receiver.id.ToString()) },
					{ PMMessage.TableSpec.FIELD_DIRECTION, new ScalarFieldValue(PMMessage.ENUM_DIRECTION_OUTGOING) },
					{ PMMessage.TableSpec.FIELD_POSTDATE, new ScalarFieldValue(DateTime.Now.ToUTCString()) },
					{ PMMessage.TableSpec.FIELD_TITLE, new ScalarFieldValue(title) },
					{ PMMessage.TableSpec.FIELD_BODY, new ScalarFieldValue(bodyIntermediate) },
					{ PMMessage.TableSpec.FIELD_BODYUBB, new ScalarFieldValue(bodyUBB) },
					{ PMMessage.TableSpec.FIELD_INCOMINGPMID, new ReferenceFieldValue(insertPmReceiver) },
					{ PMMessage.TableSpec.FIELD_ISREAD, new ScalarFieldValue("1") },
				}
			);
			AbstractChange updateConversationSender = new InsertOrUpdateChange(
				TableSpec.instance,
				new Dictionary<string,AbstractFieldValue> {
					{ TableSpec.FIELD_OWNERID, new ScalarFieldValue(sender.id.ToString()) },
					{ TableSpec.FIELD_INTERLOCUTORID, new ScalarFieldValue(receiver.id.ToString()) },
					{ TableSpec.FIELD_TOTALMESSAGES, new ScalarFieldValue("1") },
					{ TableSpec.FIELD_LASTMESSAGEID, new ReferenceFieldValue(insertPmSender) },
					{ TableSpec.FIELD_LASTMESSAGEDATE, new ScalarFieldValue(DateTime.Now.ToUTCString()) },
					{ TableSpec.FIELD_LASTREADMESSAGEID, new ReferenceFieldValue(insertPmSender) },
				},
				new Dictionary<string,AbstractFieldValue> {
					{ TableSpec.FIELD_TOTALMESSAGES, new IncrementFieldValue() },
					{ TableSpec.FIELD_LASTMESSAGEID, new TwoWayReferenceFieldValue(insertPmSender, TwoWayReferenceFieldValue.GREATEST) },
					{ TableSpec.FIELD_LASTMESSAGEDATE, new ScalarFieldValue(DateTime.Now.ToUTCString()) },
				},
				new ComplexCondition(
					ConditionsJoinType.AND,
					new ComparisonCondition(
						TableSpec.instance.getColumnSpec(TableSpec.FIELD_OWNERID),
						ComparisonType.EQUAL,
						sender.id.ToString()
					),
					new ComparisonCondition(
						TableSpec.instance.getColumnSpec(TableSpec.FIELD_INTERLOCUTORID),
						ComparisonType.EQUAL,
						receiver.id.ToString()
					)
				)
			);
			AbstractChange updateConversationReceiver = new InsertOrUpdateChange(
				TableSpec.instance,
				new Dictionary<string,AbstractFieldValue> {
					{ TableSpec.FIELD_OWNERID, new ScalarFieldValue(receiver.id.ToString()) },
					{ TableSpec.FIELD_INTERLOCUTORID, new ScalarFieldValue(sender.id.ToString()) },
					{ TableSpec.FIELD_TOTALMESSAGES, new ScalarFieldValue("1") },
					{ TableSpec.FIELD_LASTMESSAGEID, new ReferenceFieldValue(insertPmReceiver) },
					{ TableSpec.FIELD_LASTMESSAGEDATE, new ScalarFieldValue(DateTime.Now.ToUTCString()) },
					{ TableSpec.FIELD_LASTREADMESSAGEID, new ScalarFieldValue(null) },
				},
				new Dictionary<string,AbstractFieldValue> {
					{ TableSpec.FIELD_TOTALMESSAGES, new IncrementFieldValue() },
					{ TableSpec.FIELD_LASTMESSAGEID, new TwoWayReferenceFieldValue(insertPmReceiver, TwoWayReferenceFieldValue.GREATEST) },
					{ TableSpec.FIELD_LASTMESSAGEDATE, new ScalarFieldValue(DateTime.Now.ToUTCString()) },
				},
				new ComplexCondition(
					ConditionsJoinType.AND,
					new ComparisonCondition(
						TableSpec.instance.getColumnSpec(TableSpec.FIELD_OWNERID),
						ComparisonType.EQUAL,
						receiver.id.ToString()
					),
					new ComparisonCondition(
						TableSpec.instance.getColumnSpec(TableSpec.FIELD_INTERLOCUTORID),
						ComparisonType.EQUAL,
						sender.id.ToString()
					)
				)
			);
			AbstractChange updateIndicatorReceiver = new UpdateChange(
				AccountIndicator.TableSpec.instance,
				new Dictionary<string,AbstractFieldValue> {
					{ AccountIndicator.TableSpec.FIELD_PRIVATEMESSAGES, new IncrementFieldValue() },
					{ AccountIndicator.TableSpec.FIELD_UNREADPRIVATEMESSAGES, new IncrementFieldValue() },
				},
				AccountIndicator.LoadByAccount(receiver).id
			);
			ChangeSetUtil.ApplyChanges(
				insertPmReceiver,
				insertPmSender,
				updateConversationReceiver,
				updateConversationSender,
				updateIndicatorReceiver
			);
			return PMMessage.LoadById(insertPmSender.getId().Value);
		}

	}

}
