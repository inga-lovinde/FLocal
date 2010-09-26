using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;

namespace FLocal.Common.dataobjects {
	public class AnonymousUserSettings : IUserSettings {

		public AnonymousUserSettings() {
			var allSkins = Skin.allSkins.ToArray();
			//this._skinId = allSkins[Util.RandomInt(0, allSkins.Length)].id;
			this._skinId = 28;
			this._modernSkinId = 1;
			this._machicharaId = 2;
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

		private int _modernSkinId;
		public ModernSkin modernSkin {
			get {
				return ModernSkin.LoadById(this._modernSkinId);
			}
		}

		private int _machicharaId;
		public Machichara machichara {
			get {
				return Machichara.LoadById(this._machicharaId);
			}
		}

		public bool isPostVisible(Post post) {
			if(post.poster.showPostsToUsers != User.ENUM_SHOWPOSTSTOUSERS_ALL) return false;
			if(post.layer.name != PostLayer.NAME_NORMAL) return false;
			return true;
		}
		
	}
}
