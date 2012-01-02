using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core.DB;
using System.Xml.Linq;
using Web.Core;

namespace FLocal.IISHandler {
	class PageOuter : Diapasone {

		public readonly long perPage;

		public readonly bool reversed;

		public bool ascendingDirection {
			get {
				return !this.reversed;
			}
		}

		public bool descendingDirection {
			get {
				return this.reversed;
			}
		}

		private PageOuter(long start, long count, long perPage, bool reversed)
			: base(start, count) {
			this.perPage = perPage;
			this.reversed = reversed;
		}

		private PageOuter(long perPage, bool reversed)
			: base(0, -1) {
			this.perPage = perPage;
			this.reversed = reversed;
		}

		public static PageOuter createUnlimited(long perPage) {
			return new PageOuter(perPage, false);
		}

		public static PageOuter create(long perPage, long total) {
			PageOuter result = new PageOuter(0, perPage, perPage, false);
			result.total = total;
			return result;
		}

		public static PageOuter createFromUrl(FLocal.Common.URL.AbstractUrl url, long perPage, Dictionary<char, Func<string, long>> customAction) {
			if(url.remainder.Contains("/")) throw new WrongUrlException();
			string[] requestParts = url.remainder.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
			bool reversed = (requestParts.Length > 1) && (requestParts[1].ToLower() == "reversed");
			if(requestParts.Length > 0) {
				if(requestParts[0].ToLower() == "all") {
					return new PageOuter(perPage, reversed);
				} else if(Char.IsDigit(requestParts[0][0])) {
					return new PageOuter(long.Parse(requestParts[0]), perPage, perPage, reversed);
				} else {
					return new PageOuter(customAction[requestParts[0][0]](requestParts[0].Substring(1)), perPage, perPage, reversed);
				}
			} else {
				return new PageOuter(0, perPage, perPage, reversed);
			}
		}

		public static PageOuter createFromUrl(FLocal.Common.URL.AbstractUrl url, long perPage) {
			return createFromUrl(url, perPage, new Dictionary<char, Func<string, long>>());
		}

		public XElement exportToXml(int left, int current, int right) {
			XElement result = new XElement("pageOuter",
				new XElement("isReversed", this.reversed.ToPlainString()),
				new XElement("unlimited", (this.count < 1).ToPlainString()),
				new XElement("start", this.start),
				new XElement("count", this.count),
				new XElement("total", this.total.Value),
				new XElement("perPage", this.perPage),
				new XElement("isEmpty", (this.perPage >= this.total.Value).ToPlainString())
			);
			if(this.count > 0) {
				if(this.start + this.count < this.total.Value) {
					result.Add(new XElement("next", this.start + this.count));
				}
			}
			HashSet<long> pages = new HashSet<long>();
			for(long i=0; i<left; i++) {
				pages.Add(i*this.perPage);
			}
			{
				long last = this.total.Value - 1;
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
				from page in pages where (page >= 0) && (page < this.total.Value) orderby page select new XElement("page", page)
			));
			return result;
		}

	}
}
