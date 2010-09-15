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
				new XElement("ticks", dateTime.Ticks),
				new XElement("rss", dateTime.ToString("o"))
			);
		}

		public static XElement ToXml(this TimeSpan timeSpan) {
			return new XElement("timeSpan",
				new XElement("days", (long)timeSpan.TotalDays),
				new XElement("hours", timeSpan.Hours),
				new XElement("minutes", timeSpan.Minutes),
				new XElement("seconds", timeSpan.Seconds),
				new XElement("ticks", timeSpan.Ticks)
			);
		}

		public static Guid GetGuid(this Exception exception) {
			return Core.Cache<Guid>.instance.get(exception, () => Guid.NewGuid());
		}

		public static XElement ToXml(this Exception exception) {
			return new XElement("exception",
				new XElement("type", exception.GetType().FullName),
				new XElement("message", exception.Message),
				new XElement("trace", exception.StackTrace),
				new XElement("guid", exception.GetGuid().ToString())
			);
		}

	}

}
