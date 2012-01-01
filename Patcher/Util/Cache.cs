using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher
{
	class Cache<TKey, TValue>
	{

		public static readonly Cache<TKey, TValue> instance = new Cache<TKey, TValue>();
		
		private readonly Dictionary<TKey, TValue> storage = new Dictionary<TKey, TValue>();
		private Cache()
		{
		}
		
		public TValue GetValue(TKey key, Func<TValue> creator)
		{
			if(!this.storage.ContainsKey(key))
			{
				lock(this.storage)
				{
					if(!this.storage.ContainsKey(key))
					{
						this.storage[key] = creator();
					}
				}
			}
			return this.storage[key];
		}
	
	}
}
