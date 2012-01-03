using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.DB
{
	static class TransactionFactory
	{

	
		public static Transaction Create(string DbDriverName, string ConnectionString)
		{
			return new Transaction(DBTraitsFactory.GetTraits(DbDriverName), ConnectionString);
		}
		
	}
}
