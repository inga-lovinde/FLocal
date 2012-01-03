using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Web.Core;
using Patcher.Data.Command;
using Patcher.DB;

namespace Patcher.Data.Patch
{
	abstract class AbstractPatch
	{
	
		private static class Checker
		{
			private static XmlSchemaSet LoadXsd()
			{
				XmlSchemaSet schemaSet = new XmlSchemaSet();
				using(Stream xsdStream = Resources.ResourcesManager.GetResource("IPatch.xsd"))
				{
					using(XmlReader xsdReader = XmlReader.Create(xsdStream))
					{
						schemaSet.Add("", xsdReader);
					}
				}
				schemaSet.Compile();
				return schemaSet;
			}

			private static readonly XmlSchemaSet Xsd = LoadXsd();
			
			public static void Check(XDocument document)
			{
				document.Validate(Xsd, (sender, e) => { throw new ApplicationException("Intermediate XML validation failed: " + e.Message, e.Exception); });
			}

		}
		
		public static AbstractPatch LoadById(PatchId id, Context context)
		{
			return Cache<AbstractPatch>.instance.get(new KeyValuePair<PatchId, Context>(id, context), () => _LoadById(id, context));
		}
	
		private static AbstractPatch _LoadById(PatchId id, Context context)
		{
			XDocument data;
			using(Stream xmlStream = context.loadPatch(id))
			{
				using(XmlReader reader = XmlReader.Create(xmlStream))
				{
					data = XDocument.Load(reader);
				}
			}
			Checker.Check(data);
			XElement version = data.Root.Element("version");
			string number = version.Element("number").Value;
			string author = version.Element("author").Value;
			if((number != id.version.ToString()) || (author != id.name))
			{
				throw new ApplicationException(string.Format("Versions mismatch on patch #{0} from {1} (got #{2} from {3})", id.version, id.name, number, author));
			}

			HashSet<string> restrictToEnvironments;
			if(data.Root.Element("restrictToEnvironments") != null)
			{
				restrictToEnvironments = new HashSet<string>(from elem in data.Root.Element("restrictToEnvironments").Elements() select elem.Value);
			} else
			{
				restrictToEnvironments = new HashSet<string>();
			}

			XElement commandSet;
			bool isStrictCommandSet;
			{
				XElement strictCommandSet = data.Root.Element("strictCommandSet");
				XElement looseCommandSet = data.Root.Element("looseCommandSet");
				if (strictCommandSet != null && looseCommandSet == null)
				{
					commandSet = strictCommandSet;
					isStrictCommandSet = true;
				} else if (strictCommandSet == null && looseCommandSet != null)
				{
					commandSet = looseCommandSet;
					isStrictCommandSet = false;
				} else if (strictCommandSet != null && looseCommandSet != null)
				{
					throw new ApplicationException("Malformed XML: both strictCommandSet and looseCommandSet found");
				} else
				{
					throw new ApplicationException("Malformed XML: no CommandSet found");
				}
			}

			var commands = (from elem in commandSet.Elements() select AbstractCommand.Create(elem)).ToArray();

			if(isStrictCommandSet)
			{
				var isPersistent = commands.OfType<AbstractPersistentCommand>().Any();
				if (isPersistent)
				{
					if (commands.Length != 1)
					{
						throw new ApplicationException("More than one persistent command");
					}
					if(context.DbDriver.IsDDLTransactional)
					{
						return new AtomicPatch(commands, true, restrictToEnvironments, context);
					} else
					{
						return new PersistentPatch(commands[0], restrictToEnvironments, context);
					}
				}
				else
				{
					return new AtomicPatch(commands, true, restrictToEnvironments, context);
				}
			} else
			{
				return new AtomicPatch(commands, false, restrictToEnvironments, context);
			}
		}

		private readonly HashSet<string> restrictToEnvironments;

		protected readonly Context context;

		protected AbstractPatch(HashSet<string> restrictToEnvironments, Context context)
		{
			this.restrictToEnvironments = restrictToEnvironments;
			this.context = context;
		}

		public abstract XDocument Apply(Transaction transaction);

		public abstract void Rollback(Transaction transaction, XDocument rollbackInfo);
		
		public bool DoesSupportEnvironment(string environmentName)
		{
			if(restrictToEnvironments.Any())
			{
				return restrictToEnvironments.Contains(environmentName);
			} else
			{
				return true;
			}
		}
	
	}
}
