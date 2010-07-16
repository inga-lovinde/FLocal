using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FLocal.Common {
	abstract public class UserContext {
	
		public Common.Config config {
			get {
				return Common.Config.instance;
			}
		}

		abstract public IOutputParams outputParams {
			get;
		}

		abstract public string formatDateTime(DateTime dateTime);

		abstract public XElement formatTotalPosts(long posts);

		/// <summary>
		/// May be null
		/// </summary>
		abstract public dataobjects.Account account {
			get;
		}

		abstract public bool isPostVisible(dataobjects.Post post);

	}

	public static class UserContext_Extensions {
		
		/*public static string ToString(this DateTime dateTime, UserContext context) {
			return context.formatDateTime(dateTime);
		}*/

		public static XElement ToXml(this DateTime dateTime) {
			return new XElement("date",
				new XElement("year", dateTime.Year),
				new XElement("month", dateTime.Month),
				new XElement("mday", dateTime.Day),
				new XElement("hour", dateTime.Hour),
				new XElement("minute", dateTime.Minute),
				new XElement("second", dateTime.Second),
				new XElement("ticks", dateTime.Ticks)
			);
		}

	}

}
