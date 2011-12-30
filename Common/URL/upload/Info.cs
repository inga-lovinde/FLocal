using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;
using FLocal.Common.dataobjects;

namespace FLocal.Common.URL.upload {
	public class Info : AbstractUrl {

		public readonly Upload upload;

		public Info(string uploadId, string remainder) : base(remainder) {
			this.upload = Upload.LoadById(int.Parse(uploadId));
		}

		public override string title {
			get {
				return this.upload.filename;
			}
		}

		protected override string _canonical {
			get {
				return "/Upload/Info/" + this.upload.id + "/";
			}
		}
	}
}
