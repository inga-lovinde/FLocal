using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.IO;
using System.Globalization;
using Web.Core;

namespace FLocal.IISHandler {
	class TemplateEngine {

		public static class EncodingInfo {

			public class XmlEncoderReplacementFallback : EncoderFallback {

				private readonly Encoding originalEncoding;

				public XmlEncoderReplacementFallback(Encoding encoding) {
					this.originalEncoding = encoding;
				}

				public override EncoderFallbackBuffer CreateFallbackBuffer() {
					return new XmlEncoderReplacementFallbackBuffer(this.originalEncoding);
				}

				public override int MaxCharCount {
					get {
						return 20;
					}
				}

			}

			private class XmlEncoderReplacementFallbackBuffer : EncoderFallbackBuffer {

				const int SurHighStart = 0xd800;
				const int SurHighEnd   = 0xdbff; 
				const int SurLowStart  = 0xdc00; 
				const int SurLowEnd    = 0xdfff;

				int fallbackIndex = -1;
				string currentReplacement = "";

				private readonly Encoding originalEncoding;

				public XmlEncoderReplacementFallbackBuffer(Encoding originalEncoding) {
					this.originalEncoding = originalEncoding;
				}

				private bool Fallback(int ch) {
					this.currentReplacement = string.Format(CultureInfo.InvariantCulture, "&#x{0:X};", ch);
					return this.currentReplacement.Length > 0;
				}

				public override bool Fallback(char highChar, char lowChar, int index) {
					if (!char.IsSurrogatePair(highChar, lowChar)) {
						throw new CriticalException("Invalid surrogate pair: " + (int)lowChar + (int)highChar); 
					}
					return this.Fallback(char.ConvertToUtf32(highChar, lowChar));
				}

				public override bool Fallback(char ch, int index) {
					if (char.IsSurrogate(ch)) {
						throw new FLocalException("Wrong char: " + (int)ch);
					} 
					return this.Fallback(ch);
				}

				public override char GetNextChar() {
					this.fallbackIndex++;
					return this.currentReplacement[this.fallbackIndex];
				}

				public override bool MovePrevious() {
					if(this.fallbackIndex > 0) {
						this.fallbackIndex--;
						return true;
					} else {
						return false;
					}
				}

				public override int Remaining {
					get {
						return this.currentReplacement.Length - this.fallbackIndex - 1;
					}
				}

				public override void Reset() {
					this.fallbackIndex = -1;
					this.currentReplacement = "";
					base.Reset();
				}
			}

		}

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

		public static void WriteCompiled(string templateName, XDocument data, TextWriter outStream) {
			//var fallback = new EncodingInfo.XmlEncoderReplacementFallback(outStream.Encoding);
			using(XmlWriter writer = XmlWriter.Create(outStream, new XmlWriterSettings { Indent = false })) {
				using(XmlReader reader = data.CreateReader()) {
					TemplateCacher.instance.getCompiledTransform(templateName).Transform(reader, writer);
				}
			}
		}

	}
}
