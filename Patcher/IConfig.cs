using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Data.Patch;

namespace Patcher
{
	public interface IConfig
	{

		string EnvironmentName
		{
			get;
		}

		string DbDriverName
		{
			get;
		}

		string ConnectionString
		{
			get;
		}

		string PatchesTable
		{
			get;
		}

	}
}
