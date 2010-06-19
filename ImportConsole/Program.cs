﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Importer;
using FLocal.Common.actions;
using FLocal.Common.dataobjects;
using FLocal.Core;
using System.Configuration;
using NConsoler;

namespace FLocal.ImportConsole {
	class Program {
		public static void Main(string[] args) {

			if(!FLocal.Common.Config.isInitialized) {
				lock(typeof(FLocal.Common.Config)) {
					if(!FLocal.Common.Config.isInitialized) {
						FLocal.Common.Config.Init(ConfigurationManager.AppSettings);
					}
				}
				Consolery.Run(typeof(Program), args);
			}
		}

		[Action]
		public static void ImportUsers() {

			for(int i=1; i<800; i++) {
				Console.Write("[" + i + "]");
				foreach(string userName in ShallerGateway.getUserNames(i)) {
					try {
						User.LoadByName(userName);
						Console.Write("-");
					} catch(NotFoundInDBException) {
						Dictionary<string, string> userData = ShallerGateway.getUserInfo(userName);
						AbstractChange addUser = new InsertChange(
							User.TableSpec.instance,
							new Dictionary<string, AbstractFieldValue>() {
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
							new Dictionary<string,AbstractFieldValue>() {
								{ Account.TableSpec.FIELD_NAME, new ScalarFieldValue(userName.ToLower()) },
								{ Account.TableSpec.FIELD_NEEDSMIGRATION, new ScalarFieldValue("1") },
								{ Account.TableSpec.FIELD_PASSWORDHASH, new ScalarFieldValue("*") },
								{ Account.TableSpec.FIELD_USERID, new ReferenceFieldValue(addUser) },
							}
						);
						ChangeSetUtil.ApplyChanges(addUser, addAccount);
						Console.Write(".");
					}
				}
			}
		}

		[Action]
		public static void ProcessUpload(string pathToUpload) {
			throw new NotImplementedException();
		}
	}
}
