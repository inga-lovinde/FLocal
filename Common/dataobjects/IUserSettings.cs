﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FLocal.Common.dataobjects {
	public interface IUserSettings {

		int threadsPerPage {
			get;
		}

		int postsPerPage {
			get;
		}

		int uploadsPerPage {
			get;
		}

		int usersPerPage {
			get;
		}

		Skin skin {
			get;
		}

		ModernSkin modernSkin {
			get;
		}

		Machichara machichara {
			get;
		}

		bool isPostVisible(Post post);

	}

	public static class IUserSettings_Extension {

		public static XElement exportToXml(this IUserSettings settings, UserContext context) {
			return new XElement("settings",
				new XElement("postsPerPage", settings.postsPerPage),
				new XElement("threadsPerPage", settings.threadsPerPage),
				new XElement("usersPerPage", settings.usersPerPage),
				new XElement("uploadsPerPage", settings.uploadsPerPage),
				new XElement("skinId", settings.skin.id),
				new XElement("modernSkinId", settings.modernSkin.id),
				new XElement("machicharaId", settings.machichara.id)
			);
		}

	}
}
