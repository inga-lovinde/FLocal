using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Web.Core;
using Web.Core.DB;
using Web.Core.DB.conditions;

namespace FLocal.Common.dataobjects {
	public class Upload : SqlObject<Upload> {

		public const int AVATAR_MAX_FILESIZE = 80*1024;

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Uploads";
			public const string FIELD_ID = "Id";
			public const string FIELD_HASH = "MD5";
			public const string FIELD_EXTENSION = "Extension";
			public const string FIELD_SIZE = "Size";
			public const string FIELD_FILENAME = "Filename";
			public const string FIELD_UPLOADDATE = "UploadDate";
			public const string FIELD_USERID = "UserId";
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

		private string _extension;
		public string extension {
			get {
				this.LoadIfNotLoaded();
				return this._extension;
			}
		}

		private int _size;
		public int size {
			get {
				this.LoadIfNotLoaded();
				return this._size;
			}
		}

		private string _filename;
		public string filename {
			get {
				this.LoadIfNotLoaded();
				return this._filename;
			}
		}

		private DateTime _uploadDate;
		public DateTime uploadDate {
			get {
				this.LoadIfNotLoaded();
				return this._uploadDate;
			}
		}

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

		protected override void doFromHash(Dictionary<string, string> data) {
			this._hash = data[TableSpec.FIELD_HASH];
			this._extension = data[TableSpec.FIELD_EXTENSION];
			this._size = int.Parse(data[TableSpec.FIELD_SIZE]);
			this._filename = data[TableSpec.FIELD_FILENAME];
			this._uploadDate = Util.ParseDateTimeFromTimestamp(data[TableSpec.FIELD_UPLOADDATE]).Value;
			this._userId = int.Parse(data[TableSpec.FIELD_USERID]);
		}

		public XElement exportToXml(UserContext context) {
			return new XElement("upload",
				new XElement("id", this.id),
				new XElement("extension", this.extension),
				new XElement("size", this.size),
				new XElement("filename", this.filename),
				new XElement("uploadDate", this.uploadDate.ToXml()),
				new XElement("uploader", this.user.exportToXmlForViewing(context))
			);
		}

	}
}
