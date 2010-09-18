﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.forum {
	public class AllPosts : AbstractUrl {

		public AllPosts(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "All posts";
			}
		}

		protected override string _canonical {
			get {
				return "/Forum/AllPosts/";
			}
		}

	}
}
