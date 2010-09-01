using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class Post : BBCode {

		public Post()
			: base("post") {
		}

		public override string Format(ITextFormatter formatter) {
			var post = dataobjects.Post.LoadById(int.Parse(this.DefaultOrValue));
			var name = this.Safe(post.title);
			if(this.Default != null) {
				name = this.GetInnerHTML(formatter);
			}
			return "<a href=\"/Post/" + post.id.ToString() + "/\">" + name + "</a>";
		}

	}
}
