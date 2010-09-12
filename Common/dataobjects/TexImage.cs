using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;
using FLocal.Common.actions;
using FLocal.TexCompiler;

namespace FLocal.Common.dataobjects {
	public class TexImage : SqlObject<TexImage> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "TexImages";
			public const string FIELD_ID = "Id";
			public const string FIELD_HASH = "MD5";
			public const string FIELD_TEXT = "Text";
			public const string FIELD_UPLOADID = "UploadId";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private string _hash;
		public string hash {
			get {
				this.LoadIfNotLoaded();
				return this._hash;
			}
		}

		private string _text;
		public string text {
			get {
				this.LoadIfNotLoaded();
				return this._text;
			}
		}

		private int _uploadId;
		public int uploadId {
			get {
				this.LoadIfNotLoaded();
				return this._uploadId;
			}
		}
		public Upload upload {
			get {
				return Upload.LoadById(this.uploadId);
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._hash = data[TableSpec.FIELD_HASH];
			this._text = data[TableSpec.FIELD_TEXT];
			this._uploadId = int.Parse(data[TableSpec.FIELD_UPLOADID]);
		}

		public XElement exportToXml(UserContext context) {
			return new XElement("texImage",
				new XElement("id", this.id),
				new XElement("hash", this.hash),
				new XElement("text", this.text),
				this.upload.exportToXml(context)
			);
		}

		public static TexImage Save(string text) {
			string hash = Util.md5(text);
			try {
				return TexImage.LoadById(
					int.Parse(
						Config.instance.mainConnection.LoadIdByField(
							TableSpec.instance.getColumnSpec(TableSpec.FIELD_HASH),
							hash
						)
					)
				);
			} catch(NotFoundInDBException) {
			}

			User uploader = User.LoadByName("Mr. TeX compiler");
			Upload file = UploadManager.SafeUploadFile(Compiler.GetPngStream(text), "tex-" + hash + ".png", uploader);
			InsertOrUpdateChange change = new InsertOrUpdateChange(
				TableSpec.instance,
				new Dictionary<string,AbstractFieldValue> {
					{ TableSpec.FIELD_HASH, new ScalarFieldValue(hash) },
					{ TableSpec.FIELD_TEXT, new ScalarFieldValue(text) },
					{ TableSpec.FIELD_UPLOADID, new ScalarFieldValue(file.id.ToString()) },
				},
				new Dictionary<string,AbstractFieldValue>(),
				new ComparisonCondition(
					TableSpec.instance.getColumnSpec(TableSpec.FIELD_HASH),
					ComparisonType.EQUAL,
					hash
				)
			);
			ChangeSetUtil.ApplyChanges(change);
			return TexImage.LoadById(change.getId().Value);
		}

	}
}
