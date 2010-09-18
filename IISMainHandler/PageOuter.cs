﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core.DB;
using System.Xml.Linq;
using FLocal.Core;

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
			string[] requestParts = url.remainder.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			int offset = 0;
			bool reversed = (requestParts.Length > (offset+1)) && (requestParts[offset+1].ToLower() == "reversed");
			if(requestParts.Length > offset) {
				if(requestParts[offset].ToLower() == "all") {
					return new PageOuter(perPage, reversed);
				} else if(Char.IsDigit(requestParts[offset][0])) {
					return new PageOuter(long.Parse(requestParts[offset]), perPage, perPage, reversed);
				} else {
					return new PageOuter(customAction[requestParts[offset][0]](requestParts[offset].Substring(1)), perPage, perPage, reversed);
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
				new XElement("total", this.total),
				new XElement("perPage", this.perPage),
				new XElement("isEmpty", (this.perPage >= this.total).ToPlainString())
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
