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
	public class Poll : SqlObject<Poll> {

		private class PollOptionInfo {
			public string name;
			public int votes;
			public List<int> voters;
			public bool selected;
		}

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Polls";
			public const string FIELD_ID = "Id";
			public const string FIELD_TITLE = "Title";
			public const string FIELD_ISDETAILED = "IsDetailed";
			public const string FIELD_ISMULTIOPTION = "IsMultiOption";
			public const string FIELD_OPTIONS = "Options";
			public const string FIELD_POSTERID = "PosterId";
			public const string FIELD_POSTDATE = "PostDate";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		public class Vote : SqlObject<Vote> {

			public class TableSpec : ISqlObjectTableSpec {
				public const string TABLE = "Votes";
				public const string FIELD_ID = "Id";
				public const string FIELD_POLLID = "PollId";
				public const string FIELD_USERID = "UserId";
				public const string FIELD_VOTEINFO = "VoteInfo";
				public static readonly TableSpec instance = new TableSpec();
				public string name { get { return TABLE; }}
				public string idName { get { return FIELD_ID; }}
				public void refreshSqlObject(int id) { Refresh(id); }
			}

			protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

			private int _userId;
			public int userId {
				get {
					this.LoadIfNotLoaded();
					return this._userId;
				}
			}
			public User user {
				get {
					return User.LoadById(this.userId);
				}
			}

			private HashSet<int> _options;
			public HashSet<int> options {
				get {
					this.LoadIfNotLoaded();
					return this._options;
				}
			}

			protected override void doFromHash(Dictionary<string, string> data) {
				this._userId = int.Parse(data[TableSpec.FIELD_USERID]);
				this._options = new HashSet<int>(from elem in XElement.Parse(data[TableSpec.FIELD_VOTEINFO]).Descendants("vote") select int.Parse(elem.Attribute("optionId").Value));
			}

		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private string _title;
		public string title {
			get {
				this.LoadIfNotLoaded();
				return this._title;
			}
		}

		private bool _isDetailed;
		public bool isDetailed {
			get {
				this.LoadIfNotLoaded();
				return this._isDetailed;
			}
		}

		private bool _isMultiOption;
		public bool isMultiOption {
			get {
				this.LoadIfNotLoaded();
				return this._isMultiOption;
			}
		}

		private Dictionary<int, string> _options;
		public Dictionary<int, string> options {
			get {
				this.LoadIfNotLoaded();
				return this._options;
			}
		}

		private int _posterId;
		public int posterId {
			get {
				this.LoadIfNotLoaded();
				return this._posterId;
			}
		}
		public User poster {
			get {
				return User.LoadById(this._posterId);
			}
		}

		private DateTime _postDate;
		public DateTime postDate {
			get {
				this.LoadIfNotLoaded();
				return this._postDate;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._title = data[TableSpec.FIELD_TITLE];
			this._isDetailed = Util.string2bool(data[TableSpec.FIELD_ISDETAILED]);
			this._isMultiOption = Util.string2bool(data[TableSpec.FIELD_ISMULTIOPTION]);
			this._options = (from elem in XElement.Parse(data[TableSpec.FIELD_OPTIONS]).Descendants("option") select new KeyValuePair<int, string>(int.Parse(elem.Attribute("id").Value), elem.Attribute("name").Value)).ToDictionary();
			this._posterId = int.Parse(data[TableSpec.FIELD_POSTERID]);
			this._postDate = Util.ParseDateTimeFromTimestamp(data[TableSpec.FIELD_POSTDATE]).Value;
		}

		public XElement exportToXml(UserContext context) {
			return new XElement("poll",
				new XElement("id", this.id),
				new XElement("title", this.title),
				new XElement("isDetailed", this.isDetailed.ToPlainString()),
				new XElement("isMultiOption", this.isMultiOption.ToPlainString()),
				new XElement(
					"options",
					from kvp in this.options
					select new XElement(
						"option",
						new XElement("id", kvp.Key),
						new XElement("name", kvp.Value)
					)
				),
				new XElement("poster", this.poster.exportToXmlForViewing(context)),
				new XElement("postDate", this.postDate.ToXml())
			);
		}

		private Dictionary<int, PollOptionInfo> GetOptionsWithVotes(User user) {
			Dictionary<int, PollOptionInfo> result = this.options.ToDictionary(kvp => kvp.Key, kvp => new PollOptionInfo { name = kvp.Value, votes = 0, voters = new List<int>(), selected = false });
			foreach(
				var vote
				in
				Vote.LoadByIds(
					from stringId in Config.instance.mainConnection.LoadIdsByConditions(
						Vote.TableSpec.instance,
						new ComparisonCondition(
							Vote.TableSpec.instance.getColumnSpec(Vote.TableSpec.FIELD_POLLID),
							ComparisonType.EQUAL,
							this.id.ToString()
						),
						Diapasone.unlimited
					) select int.Parse(stringId)
				)
			) {
				foreach(int optionId in vote.options) {
					result[optionId].votes += 1;
					if(this.isDetailed) {
						result[optionId].voters.Add(vote.userId);
					}
				}
				if(user != null && vote.user.id == user.id) {
					foreach(int optionId in vote.options) {
						result[optionId].selected = true;
					}
				}
			}
			return result;
		}

		public XElement exportToXmlWithVotes(UserContext context) {
			return new XElement("poll",
				new XElement("id", this.id),
				new XElement("title", this.title),
				new XElement("isDetailed", this.isDetailed.ToPlainString()),
				new XElement("isMultiOption", this.isMultiOption.ToPlainString()),
				new XElement(
					"options",
					from kvp in this.GetOptionsWithVotes((context.account != null) ? context.account.user : null)
					select new XElement(
						"option",
						new XElement("id", kvp.Key),
						new XElement("name", kvp.Value.name),
						new XElement("isSelected", kvp.Value.selected),
						new XElement("votes", kvp.Value.votes),
						new XElement(
							"voters",
							from userId in kvp.Value.voters select User.LoadById(userId).exportToXmlForViewing(context)
						)
					),
					new XElement(
						"total",
						Config.instance.mainConnection.GetCountByConditions(
							Vote.TableSpec.instance,
							new ComparisonCondition(
								Vote.TableSpec.instance.getColumnSpec(Vote.TableSpec.FIELD_POLLID),
								ComparisonType.EQUAL,
								this.id.ToString()
							)
						)
					)
				),
				new XElement("poster", this.poster.exportToXmlForViewing(context)),
				new XElement("postDate", this.postDate.ToXml())
			);
		}

		public void GiveVote(User user, HashSet<int> options) {
			foreach(int option in options) {
				if(!this.options.ContainsKey(option)) throw new CriticalException("invalid option");
			}
			AbstractFieldValue voteInfo = new ScalarFieldValue(
				new XElement("votes",
					from option in options select new XElement("vote", new XAttribute("optionId", option))
				).ToString()
			);
			ChangeSetUtil.ApplyChanges(
				new InsertOrUpdateChange(
					Vote.TableSpec.instance,
					new Dictionary<string,AbstractFieldValue> {
						{ Vote.TableSpec.FIELD_POLLID, new ScalarFieldValue(this.id.ToString()) },
						{ Vote.TableSpec.FIELD_USERID, new ScalarFieldValue(user.id.ToString()) },
						{ Vote.TableSpec.FIELD_VOTEINFO, voteInfo }
					},
					new Dictionary<string,AbstractFieldValue> {
						{ Vote.TableSpec.FIELD_VOTEINFO, voteInfo },
					},
					new ComplexCondition(
						ConditionsJoinType.AND,
						new ComparisonCondition(
							Vote.TableSpec.instance.getColumnSpec(Vote.TableSpec.FIELD_POLLID),
							ComparisonType.EQUAL,
							this.id.ToString()
						),
						new ComparisonCondition(
							Vote.TableSpec.instance.getColumnSpec(Vote.TableSpec.FIELD_USERID),
							ComparisonType.EQUAL,
							user.id.ToString()
						)
					)
				)
			);
		}

		public static Poll Create(User poster, bool isDetailed, bool isMultiOption, string titleUbb, List<string> optionsUbb) {
			List<XElement> options = new List<XElement>();
			for(int i=0; i<optionsUbb.Count; i++) {
				options.Add(
					new XElement(
						"option",
						new XAttribute("id", i+1),
						new XAttribute("name", UBBParser.UBBToIntermediate(optionsUbb[i]))
					)
				);
			}
			AbstractChange pollInsert = new InsertChange(
				TableSpec.instance,
				new Dictionary<string,AbstractFieldValue> {
					{ TableSpec.FIELD_ISDETAILED, new ScalarFieldValue(isDetailed ? "1" : "0") },
					{ TableSpec.FIELD_ISMULTIOPTION, new ScalarFieldValue(isMultiOption ? "1" : "0") },
					{ TableSpec.FIELD_POSTERID, new ScalarFieldValue(poster.id.ToString()) },
					{ TableSpec.FIELD_POSTDATE, new ScalarFieldValue(DateTime.Now.ToUTCString()) },
					{ TableSpec.FIELD_TITLE, new ScalarFieldValue(UBBParser.UBBToIntermediate(titleUbb)) },
					{ TableSpec.FIELD_OPTIONS, new ScalarFieldValue((new XElement("options", options)).ToString()) },
				}
			);
			ChangeSetUtil.ApplyChanges(pollInsert);
			return Poll.LoadById(pollInsert.getId().Value);
		}

	}
}
