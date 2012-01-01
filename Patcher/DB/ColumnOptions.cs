using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.DB
{
	class ColumnOptions : IEquatable<ColumnOptions>
	{

		public readonly string type;
		public readonly string defaultValue;
		public readonly bool isNotNull;

		private string _simpleType
		{
			get
			{
				//for better compatibility with size specifiers we strip anything except for plain type name here
				string stripped;
				if(type.Contains('('))
				{
					stripped = type.Substring(0, type.IndexOf('('));
				}
				else
				{
					stripped = type;
				}
				return stripped.ToLower();
			}
		}
		
		public ColumnOptions(string type, string defaultValue, bool isNotNull)
		{
			this.type = type;
			this.defaultValue = defaultValue;
			this.isNotNull = isNotNull;
		}


		#region IEquatable<ColumnOptions> Members

		public bool Equals(ColumnOptions other)
		{
			return (this._simpleType == other._simpleType) && (this.defaultValue == other.defaultValue) && (this.isNotNull == other.isNotNull);
		}

		#endregion
	}
}
