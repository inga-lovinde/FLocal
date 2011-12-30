using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;
using FLocal.Migration.Gateway;
using FLocal.Common;
using FLocal.Common.dataobjects;
using FLocal.Common.actions;

namespace FLocal.Migration.Console {
	class UsersImporter {

		public static void ImportUsers() {

			for(int i=1; i<800; i++) {
				System.Console.Write("[" + i + "]");
				foreach(string userName in ShallerGateway.getUserNames(i)) {
					User user;
					try {
						User.LoadByName(userName);
						System.Console.Write("-");
					} catch(NotFoundInDBException) {
						Dictionary<string, string> userData = ShallerGateway.getUserInfo(userName);
						AbstractChange addUser = new InsertChange(
							User.TableSpec.instance,
							new Dictionary<string, AbstractFieldValue> {
								{ User.TableSpec.FIELD_NAME, new ScalarFieldValue(userName) },
								{ User.TableSpec.FIELD_REGDATE, new ScalarFieldValue(DateTime.Parse(userData["regDate"]).ToUTCString()) },
								{ User.TableSpec.FIELD_LOCATION, new ScalarFieldValue(userData["location"]) },
								{ User.TableSpec.FIELD_SHOWPOSTSTOUSERS, new ScalarFieldValue("All") },
								{ User.TableSpec.FIELD_SIGNATURE, new ScalarFieldValue(userData["signature"]) },
								{ User.TableSpec.FIELD_TITLE, new ScalarFieldValue(userData["title"]) },
								{ User.TableSpec.FIELD_TOTALPOSTS, new ScalarFieldValue("0") },
								{ User.TableSpec.FIELD_USERGROUPID, new ScalarFieldValue("1") },
								{ User.TableSpec.FIELD_BIOGRAPHY, new ScalarFieldValue(userData["biography"]) },
							}
						);
						AbstractChange addAccount = new InsertChange(
							Account.TableSpec.instance,
							new Dictionary<string,AbstractFieldValue> {
								{ Account.TableSpec.FIELD_NAME, new ScalarFieldValue(userName.ToLower()) },
								{ Account.TableSpec.FIELD_NEEDSMIGRATION, new ScalarFieldValue("1") },
								{ Account.TableSpec.FIELD_PASSWORDHASH, new ScalarFieldValue("*") },
								{ Account.TableSpec.FIELD_USERID, new ReferenceFieldValue(addUser) },
							}
						);
						AbstractChange addIndicator = new InsertChange(
							AccountIndicator.TableSpec.instance,
							new Dictionary<string,AbstractFieldValue> {
								{ AccountIndicator.TableSpec.FIELD_ACCOUNTID, new ReferenceFieldValue(addAccount) },
								{ AccountIndicator.TableSpec.FIELD_PRIVATEMESSAGES, new ScalarFieldValue("0") },
								{ AccountIndicator.TableSpec.FIELD_UNREADPRIVATEMESSAGES, new ScalarFieldValue("0") },
							}
						);
						ChangeSetUtil.ApplyChanges(addUser, addAccount, addIndicator);

						user = User.LoadById(addUser.getId().Value);
						System.Console.Write(".");

						if(userData["avatar"] != null && userData["avatar"] != "") {
							Upload avatar;
							string[] nameParts = userData["avatar"].Split('.');
							if(nameParts.Length != 2) throw new FLocalException("wrong avatar filename '" + userData["avatar"] + "'");
							int oldAvatarId = int.Parse(nameParts[0]);
							FileInfo avatarInfo = ShallerGateway.getFileInfo("user/" + userData["avatar"]);
							try {
								avatar = UploadManager.UploadFile(avatarInfo.dataStream, avatarInfo.fileName, avatarInfo.lastModified, user, 900000 + oldAvatarId);
							} catch(UploadManager.AlreadyUploadedException e) {
								avatar = Upload.LoadById(e.uploadId);
							}
							ChangeSetUtil.ApplyChanges(
								new UpdateChange(
									User.TableSpec.instance,
									new Dictionary<string,AbstractFieldValue> {
										{ User.TableSpec.FIELD_AVATARID, new ScalarFieldValue(avatar.id.ToString()) }
									},
									user.id
								)
							);
							System.Console.Write("a");
						}
					}

				}
			}
		}

	}
}