using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.DB
{
	
	/// <summary>
	/// Ideally, this class should never be used.
	/// It allows patches designed for really transactional DBs (e.g. PostgreSQL) to be applied to Oracle DB.
	/// Note that such patches should never be applied automatically to Oracle; manual installation by developer required, because, if something goes wrong, it leaves a system in an incorrect state.
	/// </summary>
	class OracleFakeTransactionalDBTraits : OracleDBTraits, IDBTraits
	{

		public new static readonly IDBTraits instance = new OracleFakeTransactionalDBTraits();
		
		protected OracleFakeTransactionalDBTraits() : base()
		{
		}
		
		bool IDBTraits.IsDDLTransactional
		{
			get { return true; }
		}
		
	}
}
