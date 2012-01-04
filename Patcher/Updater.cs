using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using NConsoler;
using Web.Core;
using Patcher.Data;
using Patcher.Data.Patch;
using Patcher.DB;
using System.IO;

namespace Patcher
{
	public sealed class Updater
	{

		private const string STATUS_INSTALLING = "installing";
		private const string STATUS_INSTALLED = "installed";
	
		private readonly Context context;

		public Updater(IUpdateParams updateParams, IInteractiveConsole console) {
			this.context = new Context(updateParams, console);
		}

		private int Wrap(Action action) {
			bool success = false;
			try {
				action();
				success = true;
			} catch(Exception e) {
				this.context.console.Report(e.ToString());
			}
			if(this.context.console.IsInteractive) {
				this.context.console.Report("Press any key to exit...");
				this.context.console.WaitForUserAction();
			}
			return success ? 0 : 1;
		}

		private void InstallPatch(PatchId patchId)
		{
		
			AbstractPatch patch = AbstractPatch.LoadById(patchId, this.context);
			
			if(!patch.DoesSupportEnvironment(this.context.EnvironmentName))
			{
				this.context.console.Report(" (skipping because of unsupported environment) ");
				return;
			}

			using(var transaction = this.context.CreateTransaction())
			{

				bool patchExistsInDB;
				XDocument rollbackData;
				
				var patchDBData = transaction.ExecuteReader(
					string.Format(
						"select {1} from {0} where {2} = {3} and {4} = {5}",
						transaction.EscapeName(this.context.PatchesTable),
						transaction.EscapeName("STATUS"),
						transaction.EscapeName("VERSION"),
						transaction.MarkParam("pversion"),
						transaction.EscapeName("NAME"),
						transaction.MarkParam("pname")
					),
					new Dictionary<string, object>
					{
						{ "pversion", patchId.version },
						{ "pname", patchId.name },
					}
				).ToList();

				if(patch is PersistentPatch)
				{
					bool reinstall;
				
					if(patchDBData.Any())
					{
						var patchInfo = patchDBData.Single();
						switch(patchInfo["STATUS"])
						{
							case STATUS_INSTALLED:
								throw new ApplicationException("Patch is already installed");
							case STATUS_INSTALLING:
								reinstall = true;
								break;
							default:
								throw new ApplicationException(string.Format("Unknown status {0}", patchInfo["STATUS"]));
						}
					} else
					{
						reinstall = false;
					}
				
					if(reinstall)
					{
						patchExistsInDB = true;
					}

					throw new NotImplementedException("Persistent patch installation is not implemented yet");
				} else
				{
					if(patchDBData.Any())
					{
						throw new ApplicationException("Patch is already installed");
					}

					patchExistsInDB = false;
					rollbackData = patch.Apply(transaction);
				}
				
				//System.Threading.Thread.Sleep(1000);
				int affectedRows = transaction.ExecuteNonQuery(
					String.Format(
						patchExistsInDB
							? "update {0} set {5} = {6}, {7} = {8} where {1} = {2} and {3} = {4}"
							: "insert into {0}({1}, {3}, {5}, {7}) values({2}, {4}, {6}, {8})"
						,
						transaction.EscapeName(this.context.PatchesTable),
						transaction.EscapeName("VERSION"),
						transaction.MarkParam("pversion"),
						transaction.EscapeName("NAME"),
						transaction.MarkParam("pname"),
						transaction.EscapeName("ROLLBACK_DATA"),
						transaction.MarkParam("prollbackdata"),
						transaction.EscapeName("STATUS"),
						transaction.MarkParam("pstatus")
					),
					new Dictionary<string, object>
					{
						{ "pversion", patchId.version },
						{ "pname", patchId.name },
						{ "prollbackdata", rollbackData.ToString() },
						{ "pstatus", STATUS_INSTALLED },
					}
				);
				if(affectedRows != 1)
				{
					throw new ApplicationException("Unable to install patch; are you trying to run two patchers simultaneously?");
				}
				transaction.Commit();
			}
		}
		
		private void UninstallPatch(PatchId patchId)
		{
			using(var transaction = this.context.CreateTransaction())
			{
				AbstractPatch patch = AbstractPatch.LoadById(patchId, context);
				var patchInstallInfo = transaction.ExecuteReader(
					String.Format(
						"select {1} from {0} where {2} = {4} and {3} = {5} for update",
						transaction.EscapeName(this.context.PatchesTable),
						transaction.EscapeName("ROLLBACK_DATA"),
						transaction.EscapeName("VERSION"),
						transaction.EscapeName("NAME"),
						transaction.MarkParam("pversion"),
						transaction.MarkParam("pname")
					),
					new Dictionary<string, object>
					{
						{ "pversion", patchId.version },
						{ "pname", patchId.name },
					}
				).Single();
				patch.Rollback(transaction, XDocument.Parse(patchInstallInfo["ROLLBACK_DATA"]));
				int affectedRows = transaction.ExecuteNonQuery(
					String.Format(
						"delete from {0} where {1} = {3} and {2} = {4}",
						transaction.EscapeName(this.context.PatchesTable),
						transaction.EscapeName("VERSION"),
						transaction.EscapeName("NAME"),
						transaction.MarkParam("pversion"),
						transaction.MarkParam("pname")
					),
					new Dictionary<string, object>
					{
						{ "pversion", patchId.version },
						{ "pname", patchId.name },
					}
				);
				if(affectedRows != 1)
				{
					throw new ApplicationException("Unable to install patch; are you trying to run two patchers simultaneously?");
				}
				transaction.Commit();
			}
		}
		
		private int _Apply(bool firstPatchOnly)
		{
			return Wrap(() => {
				this.context.console.Report("begin");
				List<PatchId> patchesToInstall;
				List<PatchId> inputPatches = context.getPatchesList();
				foreach (var patch in inputPatches)
				{
					this.context.console.Report("Found patch number {0:version}; name is {0:name}", patch);
				}
				using (Transaction transaction = this.context.CreateTransaction())
				{
					patchesToInstall = (
											//We should install patches that didn't finished installing in the first place
	                                 		from row in transaction.ExecuteReader(
	                                 			string.Format(
	                                 				"select {1}, {2} from {0} where {3} = {4} for update",
	                                 				transaction.EscapeName(this.context.PatchesTable),
	                                 				transaction.EscapeName("VERSION"),
	                                 				transaction.EscapeName("NAME"),
	                                 				transaction.EscapeName("STATUS"),
	                                 				transaction.MarkParam("pstatus")
	                                 			),
	                                 			new Dictionary<string, object>
	                                 			{
	                                 				{ "pstatus", STATUS_INSTALLING },
	                                 			}
	                                 		)
		                                 	let patch = new PatchId(int.Parse(row["VERSION"]), row["NAME"])
	                                 		orderby patch ascending
	                                 		select patch
										)
										.Concat(
											//Then are going all other patches from resources that did not begin installing
											Util.RemoveExtra(
												inputPatches,
		                                 		from row in transaction.ExecuteReader(
		                                 			string.Format(
		                                 				"select {1}, {2} from {0} for update",
		                                 				transaction.EscapeName(this.context.PatchesTable),
		                                 				transaction.EscapeName("VERSION"),
		                                 				transaction.EscapeName("NAME")
		                                 			)
		                                 		)
			                                 	let patch = new PatchId(int.Parse(row["VERSION"]), row["NAME"])
		                                 		orderby patch ascending
		                                 		select patch
											)
										)
										.ToList();
				}
				if(firstPatchOnly)
				{
					patchesToInstall = patchesToInstall.RemoveAllButFirst();
				}
				foreach (var patch in patchesToInstall)
				{
					this.context.console.Report("Going to install patch number {0:version}; name is {0:name}", patch);
				}
				if (patchesToInstall.Count == 0)
				{
					this.context.console.Report("No patches to install");
				} else
				{
					this.context.console.Report("Installing {0} patches...", patchesToInstall.Count);
					foreach (var patch in patchesToInstall)
					{
						this.context.console.Report("Installing patch #{0:version} from {0:name}...", patch);
						InstallPatch(patch);
						this.context.console.Report("done!");
					}
					this.context.console.Report("Finished installing patches");
				}
			});
		}

		public int ApplyAll() {
			return this._Apply(false);
		}

		public int ApplyFirstPatch() {
			return this._Apply(true);
		}
		
		private int _Rollback(bool lastPatchOnly)
		{
			return Wrap(() => {
				this.context.console.Report("begin");
				List<PatchId> patchesToRemove;
				using (Transaction transaction = this.context.CreateTransaction())
				{

					var patchesInstalling = (
		                                 		from row in transaction.ExecuteReader(
		                                 			string.Format(
		                                 				"select {1}, {2} from {0} where {3} = {4} for update",
		                                 				transaction.EscapeName(this.context.PatchesTable),
		                                 				transaction.EscapeName("VERSION"),
		                                 				transaction.EscapeName("NAME"),
		                                 				transaction.EscapeName("STATUS"),
		                                 				transaction.MarkParam("pstatus")
		                                 			),
		                                 			new Dictionary<string, object>
		                                 			{
		                                 				{ "pstatus", STATUS_INSTALLING },
		                                 			}
		                                 		)
			                                 	let patch = new PatchId(int.Parse(row["VERSION"]), row["NAME"])
		                                 		orderby patch ascending
		                                 		select patch
					                        ).ToList();
					if(patchesInstalling.Any())
					{
						foreach(var patchInstalling in patchesInstalling)
						{
							this.context.console.Report("Patch #{0:version} from {0:name} is already installing", patchInstalling);
						}
						throw new ApplicationException("Cannot uninstall patches while there are not installed ones");
					}
				
					patchesToRemove = (
										from row in transaction.ExecuteReader(
											string.Format(
												"select {1}, {2} from {0} order by {3} desc for update",
												transaction.EscapeName(this.context.PatchesTable),
												transaction.EscapeName("VERSION"),
												transaction.EscapeName("NAME"),
												transaction.EscapeName("INSTALL_DATE")
											)
										)
										let patch = new PatchId(int.Parse(row["VERSION"]), row["NAME"])
										select patch
									  ).ToList();
				}
				if(lastPatchOnly)
				{
					patchesToRemove = patchesToRemove.RemoveAllButFirst();
				}
				foreach (var patch in patchesToRemove)
				{
					this.context.console.Report("Going to uninstall patch number {0:version}; name is {0:name}", patch);
				}
				if (patchesToRemove.Count == 0)
				{
					this.context.console.Report("No patches to uninstall");
				} else
				{
					this.context.console.Report("Uninstalling {0} patches...", patchesToRemove.Count);
					foreach (var patch in patchesToRemove)
					{
						this.context.console.Report("Uninstalling patch #{0:version} from {0:name}...", patch);
						UninstallPatch(patch);
						this.context.console.Report("done!");
					}
					this.context.console.Report("Finished uninstalling patches");
				}
			});
		}

		public int RollbackAll() {
			return this._Rollback(false);
		}

		public int RollbackLastPatch() {
			return this._Rollback(true);
		}

	}
}
