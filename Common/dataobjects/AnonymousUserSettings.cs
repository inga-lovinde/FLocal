using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;

namespace FLocal.Common.dataobjects {
	public class AnonymousUserSettings : IUserSettings {

		private readonly Account account;

		public AnonymousUserSettings(Account account) {
			this.account = account;
			//var allSkins = Skin.allSkins.ToArray();
			//this._skinId = allSkins[Util.RandomInt(0, allSkins.Length)].id;
			//this._skinId = 28;
			this._skinId = Skin.LoadByName(Config.instance.DefaultLegacySkin).id;
			this._modernSkinId = ModernSkin.LoadByName(Config.instance.DefaultModernSkin).id;
			this._machicharaId = Machichara.LoadByName(Config.instance.DefaultMachichara).id;
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

		public PostVisibilityEnum isPostVisible(Post post) {
			if(this.account != null) {
				if(post.layer.name == PostLayer.NAME_HIDDEN) return PostVisibilityEnum.HIDDEN;
				if(post.poster.showPostsToUsers == User.ENUM_SHOWPOSTSTOUSERS_NONE) return PostVisibilityEnum.HIDDEN;
				if(post.poster.showPostsToUsers == User.ENUM_SHOWPOSTSTOUSERS_PRIVELEGED) return (account.user.userGroup.name == UserGroup.NAME_JUDGES || account.user.userGroup.name == UserGroup.NAME_ADMINISTRATORS) ? PostVisibilityEnum.VISIBLE : PostVisibilityEnum.HIDDEN;
				return PostVisibilityEnum.VISIBLE;
			} else {
				if(post.poster.showPostsToUsers != User.ENUM_SHOWPOSTSTOUSERS_ALL) return PostVisibilityEnum.HIDDEN;
				if(post.layer.name != PostLayer.NAME_NORMAL) return PostVisibilityEnum.HIDDEN;
				return PostVisibilityEnum.VISIBLE;
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
