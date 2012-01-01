using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using NConsoler;
using Patcher.Data;
using Patcher.Data.Patch;
using Patcher.DB;
using System.IO;

namespace Patcher
{
	public sealed class ConsoleProcessor
	{

		private const string STATUS_INSTALLING = "installing";
		private const string STATUS_INSTALLED = "installed";
	
		private static readonly Regex PatchName = new Regex("^Patch_(?<version>[01-9]+)_(?<author>[a-z]+)\\.xml$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

		private readonly Context context;

		private ConsoleProcessor(Context context)
		{
			this.context = context;
		}
	
		public static int Process(IConfig config, Func<IEnumerable<PatchId>> getPatchesList, Func<PatchId, Stream> loadPatch, string[] args)
		{
			using(new CultureReplacementWrapper(System.Globalization.CultureInfo.InvariantCulture))
			{
				return Consolery.RunInstance(new ConsoleProcessor(new Context(config, getPatchesList, loadPatch)), args);
			}
		}

		private static int Wrap(bool interactive, Action action)
		{
			bool success = false;
			try
			{
				action();
				success = true;
			} catch(Exception e)
			{
				Console.WriteLine();
				Console.WriteLine(e);
			}
			if(interactive)
			{
				Console.WriteLine("Press any key to exit...");
				Console.ReadKey(true);
			}
			return success ? 0 : 1;
		}
		
		private void InstallPatch(PatchId patchId)
		{
		
			AbstractPatch patch = AbstractPatch.LoadById(patchId, this.context);
			
			if(!patch.DoesSupportEnvironment(context.config.EnvironmentName))
			{
				Console.Write(" (skipping because of unsupported environment) ");
				return;
			}

			using(var transaction = TransactionFactory.Create(context))
			{

				bool patchExistsInDB;
				XDocument rollbackData;
				
				var patchDBData = transaction.ExecuteReader(
					string.Format(
						"select {1} from {0} where {2} = {3} and {4} = {5}",
						transaction.EscapeName(context.config.PatchesTable),
						transaction.EscapeName("STATUS"),
						transaction.EscapeName("VERSION"),
						transaction.MarkParam("pversion"),
						transaction.EscapeName("AUTHOR"),
						transaction.MarkParam("pauthor")
					),
					new Dictionary<string, object>
					{
						{ "pversion", patchId.version },
						{ "pauthor", patchId.author },
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
						transaction.EscapeName(context.config.PatchesTable),
						transaction.EscapeName("VERSION"),
						transaction.MarkParam("pversion"),
						transaction.EscapeName("AUTHOR"),
						transaction.MarkParam("pauthor"),
						transaction.EscapeName("ROLLBACK_DATA"),
						transaction.MarkParam("prollbackdata"),
						transaction.EscapeName("STATUS"),
						transaction.MarkParam("pstatus")
					),
					new Dictionary<string, object>
					{
						{ "pversion", patchId.version },
						{ "pauthor", patchId.author },
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
			using(var transaction = TransactionFactory.Create(context))
			{
				AbstractPatch patch = AbstractPatch.LoadById(patchId, context);
				var patchInstallInfo = transaction.ExecuteReader(
					String.Format(
						"select {1} from {0} where {2} = {4} and {3} = {5} for update",
						context.config.PatchesTable,
						"ROLLBACK_DATA",
						"VERSION",
						"AUTHOR",
						transaction.MarkParam("pversion"),
						transaction.MarkParam("pauthor")
					),
					new Dictionary<string, object>
					{
						{ "pversion", patchId.version },
						{ "pauthor", patchId.author },
					}
				).Single();
				patch.Rollback(transaction, XDocument.Parse(patchInstallInfo["ROLLBACK_DATA"]));
				System.Threading.Thread.Sleep(1000);
				int affectedRows = transaction.ExecuteNonQuery(
					String.Format(
						"delete from {0} where {1} = {3} and {2} = {4}",
						transaction.EscapeName(context.config.PatchesTable),
						transaction.EscapeName("VERSION"),
						transaction.EscapeName("AUTHOR"),
						transaction.MarkParam("pversion"),
						transaction.MarkParam("pauthor")
					),
					new Dictionary<string, object>
					{
						{ "pversion", patchId.version },
						{ "pauthor", patchId.author },
					}
				);
				if(affectedRows != 1)
				{
					throw new ApplicationException("Unable to install patch; are you trying to run two patchers simultaneously?");
				}
				transaction.Commit();
			}
		}
		
		[Action]
		public int Apply([Optional(false)] bool firstPatchOnly, [Optional(true)] bool interactive)
		{
			return Wrap(interactive, () => {
				Console.WriteLine("begin");
				List<PatchId> patchesToInstall;
				List<PatchId> inputPatches = context.getPatchesList();
				foreach (var patch in inputPatches)
				{
					Console.WriteLine("Found patch number {0:version}; author is {0:author}", patch);
				}
				using (Transaction transaction = TransactionFactory.Create(context))
				{
					patchesToInstall = (
											//We should install patches that didn't finished installing in the first place
	                                 		from row in transaction.ExecuteReader(
	                                 			string.Format(
	                                 				"select {1}, {2} from {0} where {3} = {4} for update",
	                                 				transaction.EscapeName(context.config.PatchesTable),
	                                 				transaction.EscapeName("VERSION"),
	                                 				transaction.EscapeName("AUTHOR"),
	                                 				transaction.EscapeName("STATUS"),
	                                 				transaction.MarkParam("pstatus")
	                                 			),
	                                 			new Dictionary<string, object>
	                                 			{
	                                 				{ "pstatus", STATUS_INSTALLING },
	                                 			}
	                                 		)
		                                 	let patch = new PatchId(int.Parse(row["VERSION"]), row["AUTHOR"])
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
		                                 				transaction.EscapeName(context.config.PatchesTable),
		                                 				transaction.EscapeName("VERSION"),
		                                 				transaction.EscapeName("AUTHOR")
		                                 			)
		                                 		)
			                                 	let patch = new PatchId(int.Parse(row["VERSION"]), row["AUTHOR"])
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
					Console.WriteLine("Going to install patch number {0:version}; author is {0:author}", patch);
				}
				if (patchesToInstall.Count == 0)
				{
					Console.WriteLine("No patches to install");
				} else
				{
					bool shouldInstall = true;
					if (interactive)
					{
						Console.WriteLine("Press Enter to install {0} patches or any key to exit", patchesToInstall.Count);
						var info = Console.ReadKey(true);
						if (info.Key != ConsoleKey.Enter) shouldInstall = false;
					}
					if (shouldInstall)
					{
						Console.WriteLine("Installing {0} patches...", patchesToInstall.Count);
						foreach (var patch in patchesToInstall)
						{
							Console.Write("Installing patch #{0:version} from {0:author}...", patch);
							InstallPatch(patch);
							Console.WriteLine("done!");
						}
						Console.WriteLine("Finished installing patches");
					} else
					{
						Console.WriteLine("Installation cancelled");
					}
				}
			});
		}
		
		[Action]
		public int Rollback([Optional(false)] bool lastPatchOnly, [Optional(true)] bool interactive)
		{
			return Wrap(interactive, () => {
				Console.WriteLine("begin");
				List<PatchId> patchesToRemove;
				using (Transaction transaction = TransactionFactory.Create(context))
				{

					var patchesInstalling = (
		                                 		from row in transaction.ExecuteReader(
		                                 			string.Format(
		                                 				"select {1}, {2} from {0} where {3} = {4} for update",
		                                 				transaction.EscapeName(context.config.PatchesTable),
		                                 				transaction.EscapeName("VERSION"),
		                                 				transaction.EscapeName("AUTHOR"),
		                                 				transaction.EscapeName("STATUS"),
		                                 				transaction.MarkParam("pstatus")
		                                 			),
		                                 			new Dictionary<string, object>
		                                 			{
		                                 				{ "pstatus", STATUS_INSTALLING },
		                                 			}
		                                 		)
			                                 	let patch = new PatchId(int.Parse(row["VERSION"]), row["AUTHOR"])
		                                 		orderby patch ascending
		                                 		select patch
					                        ).ToList();
					if(patchesInstalling.Any())
					{
						foreach(var patchInstalling in patchesInstalling)
						{
							Console.WriteLine("Patch #{0:version} from {0:author} is already installing", patchInstalling);
						}
						throw new ApplicationException("Cannot uninstall patches while there are not installed ones");
					}
				
					patchesToRemove = (
										from row in transaction.ExecuteReader(
											string.Format(
												"select {1}, {2} from {0} for update order by {3} desc",
												transaction.EscapeName(context.config.PatchesTable),
												transaction.EscapeName("VERSION"),
												transaction.EscapeName("AUTHOR"),
												transaction.EscapeName("INSTALL_DATE")
											)
										)
										let patch = new PatchId(int.Parse(row["VERSION"]), row["AUTHOR"])
										select patch
									  ).ToList();
				}
				if(lastPatchOnly)
				{
					patchesToRemove = patchesToRemove.RemoveAllButFirst();
				}
				foreach (var patch in patchesToRemove)
				{
					Console.WriteLine("Going to uninstall patch number {0:version}; author is {0:author}", patch);
				}
				if (patchesToRemove.Count == 0)
				{
					Console.WriteLine("No patches to uninstall");
				} else
				{
					bool shouldInstall = true;
					if (interactive)
					{
						Console.WriteLine("Press Enter to uninstall {0} patches or any key to exit", patchesToRemove.Count);
						var info = Console.ReadKey(true);
						if (info.Key != ConsoleKey.Enter) shouldInstall = false;
					}
					if (shouldInstall)
					{
						Console.WriteLine("Uninstalling {0} patches...", patchesToRemove.Count);
						foreach (var patch in patchesToRemove)
						{
							Console.Write("Uninstalling patch #{0:version} from {0:author}...", patch);
							UninstallPatch(patch);
							Console.WriteLine("done!");
						}
						Console.WriteLine("Finished uninstalling patches");
					} else
					{
						Console.WriteLine("Uninstallation cancelled");
					}
				}
			});
		}

	}
}
