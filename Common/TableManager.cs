using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;

namespace FLocal.Common {
	public static class TableManager {

		private static List<ISqlObjectTableSpec> _TABLES = new List<ISqlObjectTableSpec> {
			dataobjects.Account.TableSpec.instance,
			dataobjects.AccountIndicator.TableSpec.instance,
			dataobjects.AccountSettings.TableSpec.instance,
			dataobjects.AvatarsSettings.TableSpec.instance,
			dataobjects.Board.TableSpec.instance,
			dataobjects.Category.TableSpec.instance,
			dataobjects.Invite.TableSpec.instance,
			dataobjects.LocalNetwork.TableSpec.instance,
			dataobjects.Moderator.TableSpec.instance,
			dataobjects.PMConversation.TableSpec.instance,
			dataobjects.PMMessage.TableSpec.instance,
			dataobjects.Poll.TableSpec.instance,
			dataobjects.Post.TableSpec.instance,
			dataobjects.PostLayer.TableSpec.instance,
			dataobjects.Punishment.TableSpec.instance,
			dataobjects.PunishmentLayerChange.TableSpec.instance,
			dataobjects.PunishmentTransfer.TableSpec.instance,
			dataobjects.PunishmentType.TableSpec.instance,
			dataobjects.Restriction.TableSpec.instance,
			dataobjects.QuickLink.TableSpec.instance,
			dataobjects.Revision.TableSpec.instance,
			dataobjects.Session.TableSpec.instance,
			dataobjects.Skin.TableSpec.instance,
			dataobjects.TexImage.TableSpec.instance,
			dataobjects.Thread.TableSpec.instance,
			dataobjects.Upload.TableSpec.instance,
			dataobjects.User.TableSpec.instance,
			dataobjects.UserGroup.TableSpec.instance,
		};

		public static Dictionary<string, ISqlObjectTableSpec> TABLES = (from table in _TABLES select new KeyValuePair<string, ISqlObjectTableSpec>(table.name, table)).ToDictionary();

	}
}
