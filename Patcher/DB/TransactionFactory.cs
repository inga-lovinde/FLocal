using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.DB
{
	static class TransactionFactory
	{

	
		private static Transaction Create(IDBTraits DbDriver, string ConnectionString)
		{
			return new Transaction(DbDriver, ConnectionString);
		}
		
		public static Transaction Create(Context context)
		{
			return Create(context.DbDriver, context.config.ConnectionString);
		}

	}
}
