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
				if(this.revision.HasValue) {
					return this.latestRevision.body.PHPSubstring(0, 300);
				}
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
				new XElement("bodyShort", context.isPostVisible(this) == PostVisibilityEnum.VISIBLE ? this.bodyShort : ""),
				new XElement("title", this.title)
			);
		}

		private XNode XMLBody(UserContext context) {
			return XElement.Parse("<body>" + context.outputParams.preprocessBodyIntermediate(this.body) + "</body>", LoadOptions.PreserveWhitespace);
		}

		public XElement exportToXml(UserContext context, params XElement[] additional) {
			
			XElement result = null;

			switch(context.isPostVisible(this)) {
				case PostVisibilityEnum.UNVISIBLE:
					return null;
				case PostVisibilityEnum.HIDDEN:
					result = new XElement("post",
						new XElement("hidden"),
						new XElement("id", this.id)
					);
					break;
				case PostVisibilityEnum.VISIBLE:
					result = new XElement("post",
						new XElement("id", this.id),
						new XElement("poster",
							this.poster.exportToXmlForViewing(
								context,
								new XElement("isModerator", Moderator.isModerator(this.poster, this.thread).ToPlainString()),
								new XElement("isAdministrator", (this.thread.board.administrator.userId == this.poster.id).ToPlainString())
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
						new XElement("isPunishmentEnabled", ((context.account != null) && Moderator.isModerator(context.account, this.thread)).ToPlainString()),
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
					break;
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

			Post newPost;
			lock(this.thread.locker) {

				var changes = Thread.getNewPostChanges(this.thread.board, this.threadId, this, poster, actualLayer, title, body, date, forcedPostId);
				ChangeSetUtil.ApplyChanges(changes.Value.ToArray());
				
				newPost = Post.LoadById(changes.Key.getId().Value);
			}
			return newPost;
		}

		private readonly object Edit_locker = new object(); //TODO: move locking to DB
		public void Edit(User user, string newTitle, string newBody, PostLayer newDesiredLayer) {
			if(this.poster.id != user.id) {
				throw new AccessDeniedException();
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

		private IEnumerable<Post> subPosts {
			get {
				return Post.LoadByIds(
					from stringId in Config.instance.mainConnection.LoadIdsByConditions(
						TableSpec.instance,
						new ComparisonCondition(
							TableSpec.instance.getColumnSpec(TableSpec.FIELD_PARENTPOSTID),
							ComparisonType.EQUAL,
							this.id.ToString()
						),
						Diapasone.unlimited
					) select int.Parse(stringId)
				);
			}
		}

		private readonly object Punish_Locker = new object();
		public void Punish(Account account, PunishmentType type, string comment, PunishmentTransfer.NewTransferInfo? transferInfo, PunishmentLayerChange.NewLayerChangeInfo? layerChangeInfo) {

			if(string.IsNullOrEmpty(comment)) throw new FLocalException("Comment is empty");

			if(!Moderator.isModerator(account, this.thread)) throw new FLocalException(account.id + " is not a moderator in board " + this.thread.board.id);

			if(!Moderator.isTrueModerator(account, this.thread.board)) {
				if(type.weight != 0) throw new FLocalException("You cannot set punishments with weight != 0");
				if(transferInfo.HasValue && !transferInfo.Value.newBoard.isTransferTarget) throw new FLocalException("You cannot transfer in '" + transferInfo.Value.newBoard.name + "'");
			}

			lock(this.Punish_Locker) {

				lock(this.thread.locker) {

					IEnumerable<AbstractChange> changes = (
						from punishment in this.punishments
						select (AbstractChange)new UpdateChange(
							Punishment.TableSpec.instance,
							new Dictionary<string,AbstractFieldValue> {
								{ Punishment.TableSpec.FIELD_ISWITHDRAWED, new ScalarFieldValue("1") },
							},
							punishment.id
						)
					);

					InsertChange layerChangeInsert = null;
					if(layerChangeInfo.HasValue) {
						var _layerChangeInfo = layerChangeInfo.Value;

						if(_layerChangeInfo.newLayer.name == PostLayer.NAME_HIDDEN) throw new FLocalException("You cannot hide posts");
						
						layerChangeInsert = new InsertChange(
							PunishmentLayerChange.TableSpec.instance,
							new Dictionary<string,AbstractFieldValue> {
								{ PunishmentLayerChange.TableSpec.FIELD_OLDLAYERID, new ScalarFieldValue(this.layerId.ToString()) },
								{ PunishmentLayerChange.TableSpec.FIELD_NEWLAYERID, new ScalarFieldValue(_layerChangeInfo.newLayerId.ToString()) },
								{ PunishmentLayerChange.TableSpec.FIELD_ISSUBTHREADCHANGE, new ScalarFieldValue(_layerChangeInfo.isSubthreadChange.ToDBString()) },
							}
						);
						changes.Union(layerChangeInsert);

						List<Post> postsAffected;
						if(_layerChangeInfo.isSubthreadChange) {
							postsAffected = this.ToSequence(post => post.subPosts).OrderBy(post => post.id).ToList();
						} else {
							postsAffected = new List<Post>();
							postsAffected.Add(this);
						}

						changes = changes.Union(
							from post in postsAffected
							select (AbstractChange)new UpdateChange(
								Post.TableSpec.instance,
								new Dictionary<string,AbstractFieldValue> {
									{ Post.TableSpec.FIELD_LAYERID, new ScalarFieldValue(_layerChangeInfo.newLayerId.ToString()) },
								},
								post.id
							)
						);

					}

					InsertChange transferInsert = null;
					if(transferInfo.HasValue) {

						var _transferInfo = transferInfo.Value;

						transferInsert = new InsertChange(
							PunishmentTransfer.TableSpec.instance,
							new Dictionary<string,AbstractFieldValue> {
								{ PunishmentTransfer.TableSpec.FIELD_OLDBOARDID, new ScalarFieldValue(this.thread.boardId.ToString()) },
								{ PunishmentTransfer.TableSpec.FIELD_NEWBOARDID, new ScalarFieldValue(_transferInfo.newBoardId.ToString()) },
								{ PunishmentTransfer.TableSpec.FIELD_ISSUBTHREADTRANSFER, new ScalarFieldValue(_transferInfo.isSubthreadTransfer.ToDBString()) },
								{ PunishmentTransfer.TableSpec.FIELD_OLDPARENTPOSTID, new ScalarFieldValue(this.parentPostId.HasValue ? this.parentPostId.ToString() : null) },
							}
						);
						changes = changes.Union(transferInsert);

						Post lastAffectedPost;
						int totalAffectedPosts;

						if(!this.parentPostId.HasValue) {
							if(!_transferInfo.isSubthreadTransfer) {
								throw new FLocalException("You cannot move the first post in thread");
							} else {
								lastAffectedPost = this.thread.lastPost;
								totalAffectedPosts = this.thread.totalPosts;
								changes = changes.Union(
									new UpdateChange(
										Thread.TableSpec.instance,
										new Dictionary<string,AbstractFieldValue> {
											{ Thread.TableSpec.FIELD_BOARDID, new ScalarFieldValue(_transferInfo.newBoardId.ToString()) },
										},
										this.thread.id
									)
								);
							}
						} else {

							List<Post> postsAffected;
							if(_transferInfo.isSubthreadTransfer) {
								postsAffected = this.ToSequence(post => post.subPosts).OrderBy(post => post.id).ToList();
							} else {
								postsAffected = new List<Post>();
								postsAffected.Add(this);
							}

							lastAffectedPost = postsAffected.Last();
							totalAffectedPosts = postsAffected.Count;

							InsertChange threadCreate = new InsertChange(
								Thread.TableSpec.instance,
								new Dictionary<string,AbstractFieldValue> {
									{ Thread.TableSpec.FIELD_BOARDID, new ScalarFieldValue(_transferInfo.newBoardId.ToString()) },
									{ Thread.TableSpec.FIELD_FIRSTPOSTID, new ScalarFieldValue(this.id.ToString()) },
									{ Thread.TableSpec.FIELD_ISANNOUNCEMENT, new ScalarFieldValue("0") },
									{ Thread.TableSpec.FIELD_ISLOCKED, new ScalarFieldValue("0") },
									{ Thread.TableSpec.FIELD_LASTPOSTDATE, new ScalarFieldValue(lastAffectedPost.postDate.ToUTCString()) },
									{ Thread.TableSpec.FIELD_LASTPOSTID, new ScalarFieldValue(lastAffectedPost.id.ToString()) },
									{ Thread.TableSpec.FIELD_TITLE, new ScalarFieldValue(this.title) },
									{ Thread.TableSpec.FIELD_TOPICSTARTERID, new ScalarFieldValue(this.posterId.ToString()) },
									{ Thread.TableSpec.FIELD_TOTALPOSTS, new ScalarFieldValue(totalAffectedPosts.ToString()) },
									{ Thread.TableSpec.FIELD_TOTALVIEWS, new ScalarFieldValue("0") },
								}
							);
							changes = changes.Union(threadCreate);

							changes = changes.Union(
								from post in postsAffected
								select (AbstractChange)new UpdateChange(
									TableSpec.instance,
									new Dictionary<string,AbstractFieldValue> {
										{ TableSpec.FIELD_THREADID, new ReferenceFieldValue(threadCreate) },
									},
									post.id
								)
							);

							if(!_transferInfo.isSubthreadTransfer) {
								changes = changes.Union(
									from post in this.subPosts
									select (AbstractChange)new UpdateChange(
										TableSpec.instance,
										new Dictionary<string,AbstractFieldValue> {
											{ TableSpec.FIELD_PARENTPOSTID, new ScalarFieldValue(this.parentPostId.ToString()) },
										},
										post.id
									)
								);
							}

						}

						changes = changes.Union(
							from board in this.thread.board.boardAndParents
							select (AbstractChange)new UpdateChange(
								Board.TableSpec.instance,
								new Dictionary<string,AbstractFieldValue> {
									{ Board.TableSpec.FIELD_TOTALPOSTS, new IncrementFieldValue(IncrementFieldValue.DECREMENTOR_CUSTOM(totalAffectedPosts)) },
									{ Board.TableSpec.FIELD_TOTALTHREADS, new IncrementFieldValue(IncrementFieldValue.DECREMENTOR_CUSTOM(this.parentPostId.HasValue ? 0 : 1)) },
								},
								board.id
							)
						);

						changes = changes.Union(
							from board in _transferInfo.newBoard.boardAndParents
							select (AbstractChange)new UpdateChange(
								Board.TableSpec.instance,
								new Dictionary<string,AbstractFieldValue> {
									{ Board.TableSpec.FIELD_TOTALPOSTS, new IncrementFieldValue(IncrementFieldValue.INCREMENTOR_CUSTOM(totalAffectedPosts)) },
									{ Board.TableSpec.FIELD_TOTALTHREADS, new IncrementFieldValue() },
									{ Board.TableSpec.FIELD_LASTPOSTID, new IncrementFieldValue(IncrementFieldValue.GREATEST(lastAffectedPost.id)) },
								},
								board.id
							)
						);

						changes = changes.Union(
							new UpdateChange(
								TableSpec.instance,
								new Dictionary<string,AbstractFieldValue> {
									{ TableSpec.FIELD_PARENTPOSTID, new ScalarFieldValue(null) },
								},
								this.id
							)
						);

						if(this.parentPostId.HasValue) {
							changes = changes.Union(
								new UpdateChange(
									Thread.TableSpec.instance,
									new Dictionary<string,AbstractFieldValue> {
										{ Thread.TableSpec.FIELD_TOTALPOSTS, new IncrementFieldValue(IncrementFieldValue.DECREMENTOR_CUSTOM(totalAffectedPosts)) },
									},
									this.threadId
								)
							);
						}
					}

					changes = changes.Union(
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
								{ Punishment.TableSpec.FIELD_EXPIRES, new ScalarFieldValue(DateTime.Now.Add(type.timeSpan).ToUTCString()) },
								{ Punishment.TableSpec.FIELD_TRANSFERID, (transferInsert != null) ? (AbstractFieldValue)new ReferenceFieldValue(transferInsert) : (AbstractFieldValue)new ScalarFieldValue(null) },
								{ Punishment.TableSpec.FIELD_LAYERCHANGEID, (layerChangeInsert != null) ? (AbstractFieldValue)new ReferenceFieldValue(layerChangeInsert) : (AbstractFieldValue)new ScalarFieldValue(null) },
							}
						)
					);

					ChangeSetUtil.ApplyChanges(changes.ToArray());

					this.punishments_Reset();

					Account posterAccount = null;
					try {
						posterAccount = Account.LoadByUser(this.poster);
					} catch(NotFoundInDBException) {
					}
					
					if((posterAccount != null) && (posterAccount.id != account.id) && !posterAccount.needsMigration) {
						PMMessage newMessage = PMConversation.SendPMMessage(
							account,
							posterAccount,
							this.title,
							String.Format("{0}{3}[post]{2}[/post]{3}{1}", type.description, comment, this.id, Util.EOL)
						);
						newMessage.conversation.markAsRead(account, newMessage, newMessage);
					}

					HashSet<int> punishmentsBoards = new HashSet<int>(from punishment in this.punishments select punishment.originalBoardId);
					foreach(int boardId in punishmentsBoards) {
						Restriction.RecalculateRestrictions(Board.LoadById(boardId), this.poster);
					}
				}
			}
		}

	}
}
