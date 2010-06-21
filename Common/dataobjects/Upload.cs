using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.Common.dataobjects {
	public class Upload : SqlObject<Upload> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Uploads";
			public const string FIELD_ID = "Id";
			public const string FIELD_HASH = "MD5";
			public const string FIELD_EXTENSION = "Extension";
			public const string FIELD_SIZE = "Size";
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

		private DateTime _uploadDate;
		public DateTime uploadDate {
			get {
				this.LoadIfNotLoaded();
				return this._uploadDate;
			}
		}

		private int? _userId;
		public int? userId {
			get {
				this.LoadIfNotLoaded();
				return this._userId;
			}
		}
		public User user {
			get {
				return User.LoadById(this.userId.Value);
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._hash = data[TableSpec.FIELD_HASH];
			this._extension = data[TableSpec.FIELD_EXTENSION];
			this._size = int.Parse(data[TableSpec.FIELD_SIZE]);
			this._uploadDate = Util.ParseDateTimeFromTimestamp(data[TableSpec.FIELD_UPLOADDATE]).Value;
			this._userId = Util.ParseInt(data[TableSpec.FIELD_USERID]);
		}

	}
}
