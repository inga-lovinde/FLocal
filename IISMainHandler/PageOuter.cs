using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core.DB;
using System.Xml.Linq;
using FLocal.Core;

namespace FLocal.IISHandler {
	class PageOuter : Diapasone {

		public readonly long perPage;

		private PageOuter(long start, long count, long perPage)
			: base(start, count) {
			this.perPage = perPage;
		}

		private PageOuter(long perPage)
			: base(0, -1) {
			this.perPage = perPage;
		}

		public static PageOuter create(long perPage, long total) {
			PageOuter result = new PageOuter(0, perPage, perPage);
			result.total = total;
			return result;
		}

		public static PageOuter createFromGet(string[] requestParts, long perPage, Dictionary<char, Func<long>> customAction, int offset) {
			if(requestParts.Length > offset) {
				if(requestParts[offset].ToLower() == "all") {
					return new PageOuter(perPage);
				} else if(Char.IsDigit(requestParts[offset][0])) {
					return new PageOuter(long.Parse(requestParts[offset]), perPage, perPage);
				} else {
					return new PageOuter(customAction[requestParts[offset][0]](), perPage, perPage);
				}
			} else {
				return new PageOuter(0, perPage, perPage);
			}
		}

		/*public static PageOuter createFromGet(string[] requestParts, long perPage, Dictionary<char, Func<long>> customAction) {
			return createFromGet(requestParts, perPage, customAction, 2);
		}*/

		public static PageOuter createFromGet(string[] requestParts, long perPage, int offset) {
			return createFromGet(requestParts, perPage, new Dictionary<char, Func<long>>(), offset);
		}

		/*public static PageOuter createFromGet(string[] requestParts, long perPage) {
			return createFromGet(requestParts, perPage, new Dictionary<char,Func<long>>());
		}*/

		public XElement exportToXml(int left, int current, int right) {
			XElement result = new XElement("pageOuter",
				new XElement("unlimited", (this.count < 1).ToPlainString()),
				new XElement("start", this.start),
				new XElement("count", this.count),
				new XElement("total", this.total),
				new XElement("perPage", this.perPage)
			);
			if(this.count > 0) {
				if(this.start + this.count < this.total) {
					result.Add(new XElement("next", this.start + this.count));
				}
			}
			HashSet<long> pages = new HashSet<long>();
			for(long i=0; i<left; i++) {
				pages.Add(i*this.perPage);
			}
			{
				long last = this.total - 1;
				long totalFloor = last - (last % this.perPage);
				for(long i=0; i<left; i++) {
					pages.Add(totalFloor - i*this.perPage);
				}
			}
			{
				long startFloor = this.start - (this.start % this.perPage);
				for(long i=current; i>-current; i--) {
					pages.Add(startFloor + i*this.perPage);
				}
			}
			pages.Add(this.start);
			result.Add(new XElement("pages",
				from page in pages where (page >= 0) && (page < this.total) orderby page select new XElement("page", page)
			));
			return result;
		}

	}
}
