using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;

namespace FLocal.Common.dataobjects {
	public class AnonymousUserSettings : IUserSettings {

		public AnonymousUserSettings() {
			var allSkins = Skin.allSkins.ToArray();
			this._skinId = allSkins[Util.RandomInt(0, allSkins.Length)].id;
		}

		public int threadsPerPage {
			get {
				return 40;
			}
		}

		public int postsPerPage {
			get {
				return 20;
			}
		}

		public int uploadsPerPage {
			get {
				return 50; //some pictures won't properly load when there are 100 pictures per page
			}
		}

		public int usersPerPage {
			get {
				return 50;
			}
		}

		private int _skinId;
		public Skin skin {
			get {
				return Skin.LoadById(this._skinId);
			}
		}

	}
}
