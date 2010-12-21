using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;

namespace FLocal.Common.dataobjects {
	public class AnonymousUserSettings : IUserSettings {

		private readonly Account account;

		public AnonymousUserSettings(Account account) {
			this.account = account;
			//var allSkins = Skin.allSkins.ToArray();
			//this._skinId = allSkins[Util.RandomInt(0, allSkins.Length)].id;
			this._skinId = 28;
			this._modernSkinId = 2;
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
			if(this.account != null) {
				if(post.layer.name == PostLayer.NAME_HIDDEN) return false;
				if(post.poster.showPostsToUsers == User.ENUM_SHOWPOSTSTOUSERS_NONE) return false;
				if(post.poster.showPostsToUsers == User.ENUM_SHOWPOSTSTOUSERS_PRIVELEGED) return account.user.userGroup.name == UserGroup.NAME_JUDGES || account.user.userGroup.name == UserGroup.NAME_ADMINISTRATORS;
				return true;
			} else {
				if(post.poster.showPostsToUsers != User.ENUM_SHOWPOSTSTOUSERS_ALL) return false;
				if(post.layer.name != PostLayer.NAME_NORMAL) return false;
				return true;
			}
		}


		public int maxUploadImageWidth {
			get {
				return 800;
			}
		}

		public int maxUploadImageHeight {
			get {
				return 600;
			}
		}

	}
}
