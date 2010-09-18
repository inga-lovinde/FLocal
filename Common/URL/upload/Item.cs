using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;
using FLocal.Common.dataobjects;

namespace FLocal.Common.URL.upload {
	public class Item : AbstractUrl {

		public readonly Upload upload;

		public Item(string uploadId, string remainder) : base(remainder) {
			int iUploadId;
			if(!int.TryParse(uploadId, out iUploadId)) {
				string[] parts = uploadId.Split('.');
				if(parts.Length != 2) throw new FLocalException("wrong url");
				if(parts[0].Substring(0, 4).ToLower() != "file") throw new FLocalException("wrong url");
				int rawFileNum = int.Parse(parts[0].Substring(4));
				switch(parts[1].ToLower()) {
					case "jpg":
						iUploadId = rawFileNum;
						break;
					case "gif":
						iUploadId = 500000 + rawFileNum;
						break;
					case "png":
						iUploadId = 600000 + rawFileNum;
						break;
					default:
						throw new FLocalException("wrong url");
				}
			}
			this.upload = Upload.LoadById(iUploadId);
		}

		public override string title {
			get {
				return this.upload.filename;
			}
		}

		protected override string _canonical {
			get {
				return "/Upload/Item/" + this.upload.id + "/";
			}
		}
	}
}
