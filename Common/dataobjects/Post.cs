using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;
using FLocal.Common;
using FLocal.Common.actions;

namespace FLocal.Common.dataobjects {
	public class Post : SqlObject<Post> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Posts";
			public const string FIELD_ID = "Id";
			public const string FIELD_POSTERID = "PosterId";
			public const string FIELD_POSTDATE = "PostDate";
			public const string FIELD_LASTCHANGEDATE = "LastChangeDate";
			public const string FIELD_REVISION = "Revision";
			public const string FIELD_LAYERID = "LayerId";
			public const string FIELD_TITLE = "Title";
			public const string FIELD_BODY = "Body";
			public const string FIELD_THREADID = "ThreadId";
			public const string FIELD_PARENTPOSTID = "ParentPostId";
			public const string FIELD_TOTALPUNISHMENTS = "TotalPunishments";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private int _posterId;
		public int posterId {
			get {
				this.LoadIfNotLoaded();
				return this._posterId;
			}
		}
		public User poster {
			get {
				this.LoadIfNotLoaded();
				return User.LoadById(this.posterId);
			}
		}

		private DateTime _postDate;
		public DateTime postDate {
			get {
				this.LoadIfNotLoaded();
				return this._postDate;
			}
		}

		private DateTime _lastChangeDate;
		public DateTime lastChangeDate {
			get {
				this.LoadIfNotLoaded();
				return this._lastChangeDate;
			}
		}

		private int? _revision;
		public int? revision {
			get {
				this.LoadIfNotLoaded();
				return this._revision;
			}
		}
		public Revision latestRevision {
			get {
				return Revision.LoadById(
					int.Parse(
						Config.instance.mainConnection.LoadIdsByConditions(
							Revision.TableSpec.instance,
							new ComplexCondition(
								ConditionsJoinType.AND,
								new ComparisonCondition(
									Revision.TableSpec.instance.getColumnSpec(Revision.TableSpec.FIELD_POSTID),
									ComparisonType.EQUAL,
									this.id.ToString()
								),
								new ComparisonCondition(
									Revision.TableSpec.instance.getColumnSpec(Revision.TableSpec.FIELD_NUMBER),
									ComparisonType.EQUAL,
									this.revision.Value.ToString()
								)
							),
							Diapasone.unlimited
						).Single()
					)
				);
			}
		}
		
		private int _layerId;
		public int layerId {
			get {
				this.LoadIfNotLoaded();
				return this._layerId;
			}
		}
		public PostLayer layer {
			get {
				return PostLayer.LoadById(this.layerId);
			}
		}

		private string _title;
		public string title {
			get {
				this.LoadIfNotLoaded();
				return this._title;
			}
		}

		private string _body;
		public string body {
			get {
				this.LoadIfNotLoaded();
				return this._body;
			}
		}
		public string bodyShort {
			get {
				return this.body.Replace("<br />", Util.EOL).Replace("<br/>", Util.EOL).PHPSubstring(0, 1000);
			}
		}

		private int _threadId;
		public int threadId {
			get {
				this.LoadIfNotLoaded();
				return this._threadId;
			}
		}
		public Thread thread {
			get {
				return Thread.LoadById(this.threadId);
			}
		}

		private int? _parentPostId;
		public int? parentPostId {
			get {
				this.LoadIfNotLoaded();
				return this._parentPostId;
			}
		}
		public Post parentPost {
			get {
				return Post.LoadById(this.parentPostId.Value);
			}
		}

		private int _totalPunishments;
		public int totalPunishments {
			get {
				this.LoadIfNotLoaded();
				return this._totalPunishments;
			}
		}

		private readonly object punishments_Locker = new object();
		public IEnumerable<Punishment> punishments {
			get {
				return
					from id in Cache<IEnumerable<int>>.instance.get(
						this.punishments_Locker,
						() => {
							IEnumerable<int> ids = (from stringId in Config.instance.mainConnection.LoadIdsByConditions(
								Punishment.TableSpec.instance,
								new ComparisonCondition(
									Punishment.TableSpec.instance.getColumnSpec(Punishment.TableSpec.FIELD_POSTID),
									ComparisonType.EQUAL,
									this.id.ToString()
								),
								Diapasone.unlimited
							) select int.Parse(stringId)).ToList();
							Punishment.LoadByIds(ids);
							return ids;
						}
					)
					let punishment = Punishment.LoadById(id)
					orderby punishment.id
					select punishment;
			}
		}
		internal void punishments_Reset() {
			Cache<IEnumerable<int>>.instance.delete(this.punishments_Locker);
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._posterId = int.Parse(data[TableSpec.FIELD_POSTERID]);
			this._postDate = Util.ParseDateTimeFromTimestamp(data[TableSpec.FIELD_POSTDATE]).Value;
			this._lastChangeDate = Util.ParseDateTimeFromTimestamp(data[TableSpec.FIELD_LASTCHANGEDATE]).Value;
			this._revision = Util.ParseInt(data[TableSpec.FIELD_REVISION]);
			this._layerId = int.Parse(data[TableSpec.FIELD_LAYERID]);
			this._title = data[TableSpec.FIELD_TITLE];
			this._body = data[TableSpec.FIELD_BODY];
			this._threadId = int.Parse(data[TableSpec.FIELD_THREADID]);
			this._parentPostId = Util.ParseInt(data[TableSpec.FIELD_PARENTPOSTID]);
			this._totalPunishments = Util.ParseInt(data[TableSpec.FIELD_TOTALPUNISHMENTS]).GetValueOrDefault(0);
		}

		public XElement exportToXmlSimpleWithParent(UserContext context) {
			return new XElement("post",
				new XElement("id", this.id),
				new XElement("name", this.title),
				new XElement("parent", this.thread.exportToXmlSimpleWithParent(context))
			);
		}

		public XElement exportToXmlBase(UserContext context) {
			return new XElement("post",
				new XElement("id", this.id),
				new XElement("poster", this.poster.exportToXmlForViewing(context)),
				new XElement("bodyShort", context.isPostVisible(this) ? this.bodyShort : ""),
				new XElement("title", this.title)
			);
		}

		private XNode XMLBody(UserContext context) {
			return XElement.Parse("<body>" + context.outputParams.preprocessBodyIntermediate(this.body) + "</body>", LoadOptions.PreserveWhitespace);
		}

		public XElement exportToXml(UserContext context, params XElement[] additional) {
			
			if(!context.isPostVisible(this)) return null;

			XElement result = new XElement("post",
				new XElement("id", this.id),
				new XElement("poster",
					this.poster.exportToXmlForViewing(
						context,
						new XElement("isModerator", Moderator.isModerator(this.poster, this.thread.board).ToPlainString())
					)
				),
				new XElement("postDate", this.postDate.ToXml()),
				new XElement("layerId", this.layerId),
				new XElement("layerName", this.layer.name),
				new XElement("title", this.title),
				new XElement("body", context.outputParams.preprocessBodyIntermediate(this.body)),
				//this.XMLBody(context),
				new XElement("bodyShort", this.bodyShort),
				new XElement("threadId", this.threadId),
				new XElement("isPunishmentEnabled", ((context.account != null) && Moderator.isModerator(context.account, this.thread.board)).ToPlainString()),
				new XElement("isOwner", ((context.account != null) && (this.poster.id == context.account.user.id)).ToPlainString()),
				new XElement(
					"specific",
					new XElement(
						"changeInfo",
						new XElement("lastChangeDate", this.lastChangeDate.ToXml()),
						new XElement("revision", this.revision.ToString())
					)
				)
			);
			if(this.totalPunishments > 0) {
				result.Add(from punishment in punishments select new XElement("specific", punishment.exportToXml(context)));
			}
			if(this.parentPostId.HasValue) {
				result.Add(new XElement("parentPost", this.parentPost.exportToXmlBase(context)));
			}
			if(additional.Length > 0) {
				result.Add(additional);
			}
			return result;
		}

		public Post Reply(User poster, string title, string body, PostLayer desiredLayer) {
			return this.Reply(poster, title, body, desiredLayer, DateTime.Now, null);
		}
		public Post Reply(User poster, string title, string body, PostLayer desiredLayer, DateTime date, int? forcedPostId) {

			if(this.thread.isLocked && poster.name != Config.instance.AdminUserName) {
				throw new FLocalException("thread locked");
			}

			PostLayer actualLayer = poster.getActualLayer(this.thread.board, desiredLayer);

			var changes = Thread.getNewPostChanges(this.thread.board, this.threadId, this, poster, actualLayer, title, body, date, forcedPostId);
			ChangeSetUtil.ApplyChanges(changes.Value.ToArray());
			
			return Post.LoadById(changes.Key.getId().Value);
		}

		private readonly object Edit_locker = new object(); //TODO: move locking to DB
		public void Edit(User user, string newTitle, string newBody, PostLayer newDesiredLayer) {
			if(this.poster.id != user.id) {
				throw new AccessViolationException();
			}
			PostLayer actualLayer = poster.getActualLayer(this.thread.board, newDesiredLayer);
			if(actualLayer.id < this.layer.id) {
				actualLayer = this.layer;
			}
			lock(this.Edit_locker) {
				List<AbstractChange> changes = new List<AbstractChange> {
					new InsertChange(
						Revision.TableSpec.instance,
						new Dictionary<string, AbstractFieldValue> {
							{ Revision.TableSpec.FIELD_POSTID, new ScalarFieldValue(this.id.ToString()) },
							{ Revision.TableSpec.FIELD_CHANGEDATE, new ScalarFieldValue(DateTime.Now.ToUTCString()) },
							{ Revision.TableSpec.FIELD_TITLE, new ScalarFieldValue(newTitle) },
							{ Revision.TableSpec.FIELD_BODY, new ScalarFieldValue(newBody) },
							{ Revision.TableSpec.FIELD_NUMBER, new ScalarFieldValue((this.revision + 1).ToString()) },
						}
					),
					new UpdateChange(
						TableSpec.instance,
						new Dictionary<string, AbstractFieldValue> {
							{ TableSpec.FIELD_TITLE, new ScalarFieldValue(newTitle) },
							{ TableSpec.FIELD_BODY, new ScalarFieldValue(UBBParser.UBBToIntermediate(newBody)) },
							{ TableSpec.FIELD_LASTCHANGEDATE, new ScalarFieldValue(DateTime.Now.ToUTCString()) },
							{ TableSpec.FIELD_REVISION, new IncrementFieldValue() },
							{ TableSpec.FIELD_LAYERID, new ScalarFieldValue(actualLayer.id.ToString()) },
						},
						this.id
					)
				};
				if(this.thread.firstPost.id == this.id) {
					changes.Add(
						new UpdateChange(
							Thread.TableSpec.instance,
							new Dictionary<string,AbstractFieldValue> {
								{ TableSpec.FIELD_TITLE, new ScalarFieldValue(newTitle) },
							},
							this.thread.id
						)
					);
				}
				ChangeSetUtil.ApplyChanges(changes.ToArray());
			}
		}

		public void Punish(Account account, PunishmentType type, string comment) {

			if(!Moderator.isModerator(account, this.thread.board)) throw new FLocalException(account.id + " is not a moderator in board " + this.thread.board.id);

			if(account.user.id == this.poster.id) throw new FLocalException("You cannot punish your own posts");
	
			ChangeSetUtil.ApplyChanges(
				new UpdateChange(
					TableSpec.instance,
					new Dictionary<string,AbstractFieldValue> {
						{ TableSpec.FIELD_TOTALPUNISHMENTS, new IncrementFieldValue() },
					},
					this.id
				),
				new InsertChange(
					Punishment.TableSpec.instance,
					new Dictionary<string,AbstractFieldValue> {
						{ Punishment.TableSpec.FIELD_POSTID, new ScalarFieldValue(this.id.ToString()) },
						{ Punishment.TableSpec.FIELD_OWNERID, new ScalarFieldValue(this.poster.id.ToString()) },
						{ Punishment.TableSpec.FIELD_ORIGINALBOARDID, new ScalarFieldValue(this.thread.board.id.ToString()) },
						{ Punishment.TableSpec.FIELD_MODERATORID, new ScalarFieldValue(account.id.ToString()) },
						{ Punishment.TableSpec.FIELD_PUNISHMENTDATE, new ScalarFieldValue(DateTime.Now.ToUTCString()) },
						{ Punishment.TableSpec.FIELD_PUNISHMENTTYPE, new ScalarFieldValue(type.id.ToString()) },
						{ Punishment.TableSpec.FIELD_ISWITHDRAWED, new ScalarFieldValue("0") },
						{ Punishment.TableSpec.FIELD_COMMENT, new ScalarFieldValue(comment) },
					}
				)
			);

			Account posterAccount = null;
			try {
				posterAccount = Account.LoadByUser(this.poster);
			} catch(NotFoundInDBException) {
			}
			
			if(posterAccount != null) {
				PMMessage newMessage = PMConversation.SendPMMessage(
					account,
					posterAccount,
					this.title,
					type.description + "\r\n" + this.id
				);
				newMessage.conversation.markAsRead(account, newMessage, newMessage);
			}
		}

	}
}
