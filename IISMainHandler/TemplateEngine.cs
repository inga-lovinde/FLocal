using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace FLocal.IISHandler {
	class TemplateEngine {

		private class TemplateCacher {

			public static TemplateCacher instance = new TemplateCacher();

			private object locker = new object();

			private Dictionary<string, XslCompiledTransform> cache = new Dictionary<string,XslCompiledTransform>();

			public XslCompiledTransform getCompiledTransform(string templateName) {
				if(!this.cache.ContainsKey(templateName)) {
					lock(this.locker) {
						if(!this.cache.ContainsKey(templateName)) {
							XslCompiledTransform xslt = new XslCompiledTransform();
							xslt.Load(FLocal.Common.Config.instance.dataDir + "Templates" + FLocal.Common.Config.instance.DirSeparator + templateName);
							this.cache[templateName] = xslt;
						}
					}
				}
				return this.cache[templateName];
			}

		}

		public static string Compile(string templateName, XDocument data) {
			StringBuilder builder = new StringBuilder();
			using(XmlWriter writer = XmlWriter.Create(builder, new XmlWriterSettings { Indent = false })) {
				using(XmlReader reader = data.CreateReader()) {
					TemplateCacher.instance.getCompiledTransform(templateName).Transform(reader, writer);
				}
			}
			return builder.ToString();
		}

	}
}
