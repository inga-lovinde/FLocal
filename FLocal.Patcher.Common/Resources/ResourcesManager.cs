using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Patcher;

namespace FLocal.Patcher.Common.Resources {
	static class ResourcesManager {

		public class XmlUrlResolver : System.Xml.XmlUrlResolver {
			public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn) {
				//Assembly assembly = Assembly.GetExecutingAssembly();
				//return assembly.GetManifestResourceStream(this.GetType(),Path.GetFileName(absoluteUri.AbsolutePath));
				//				throw new ApplicationException(Path.GetFileName(absoluteUri.AbsolutePath));
				try {
					return GetResource(Path.GetFileName(absoluteUri.AbsolutePath)); //note that we ignore all folders structure
				} catch(ResourceNotFoundException) {
					throw new XmlResourceNotFoundException(absoluteUri);
				}
			}
		}

		public static Stream GetResource(string name) {
			var result = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(ResourcesManager), name);
			if(result == null) {
				throw new ResourceNotFoundException(name);
			}
			return result;
		}

		private static readonly string Namespace = typeof(ResourcesManager).Namespace + ".";

		public static IEnumerable<string> GetResourcesList() {
			return
				from rawName in Assembly.GetExecutingAssembly().GetManifestResourceNames()
				where rawName.StartsWith(Namespace)
				select rawName.Substring(Namespace.Length);
		}

	}
}
