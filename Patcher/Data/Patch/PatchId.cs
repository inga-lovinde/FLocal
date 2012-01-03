using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Patch
{
	public class PatchId : IComparable<PatchId>, IComparable, IFormattable
	{
		
		public readonly int version;
		public readonly string name;
		
		public PatchId(int version, string name)
		{
			this.version = version;
			this.name = name;
		}

		private int _CompareTo(PatchId other)
		{
			int result = this.version.CompareTo(other.version);
			if(result == 0)
			{
				result = this.name.CompareTo(other.name);
			}
			return result;
		}

		int IComparable<PatchId>.CompareTo(PatchId other)
		{
			return this._CompareTo(other);
		}

		int IComparable.CompareTo(object obj)
		{
			if(obj is PatchId)
			{
				return this._CompareTo((PatchId)obj);
			}
			throw new ApplicationException("Cannot compare PatchId to " + obj.GetType());
		}
		
		public string ToString(string format, IFormatProvider formatProvider)
		{
		
			if(String.IsNullOrEmpty(format)) return this.ToString();
		
			switch(format.ToLower())
			{
				case "version":
					return this.version.ToString(formatProvider);
				case "name":
					return this.name.ToString(formatProvider);
				default:
					throw new ApplicationException(String.Format("Unknown format {0}", format));
			}
		}
	}
}
