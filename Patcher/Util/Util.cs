using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Patcher
{
	static class Util
	{
	
		private static bool RemoveExtra_MoveNext<T>(IEnumerator<T> enumerator) where T : IComparable<T>
		{
			T previous = enumerator.Current;
			if(!enumerator.MoveNext())
			{
				return false;
			}
			T current = enumerator.Current;
			if(previous.CompareTo(current) >= 0)
			{
				throw new ApplicationException("Expecting ordered list");
			}
			return true;
		}
	
		private static bool RemoveExtra_ForwardUntil<T>(IEnumerator<T> enumerator, T val) where T : IComparable<T>
		{
			                                             	while (enumerator.MoveNext())
			                                             	{
			                                             		if (val.CompareTo(enumerator.Current) <= 0)
			                                             		{
			                                             			return true;
			                                             		}
			                                             	}
			                                             	return false;
		}
	
		//Note that this method expects unique values in both orderedSource and orderedToRemove; i.e. { 1, 2, 2, 3 } would be incorrect input value
		public static IEnumerable<T> RemoveExtra<T>(IEnumerable<T> orderedSource, IEnumerable<T> orderedToRemove) where T : IComparable<T>
		{

			using(IEnumerator<T> sourceEnumerator = orderedSource.GetEnumerator(), removeEnumerator = orderedToRemove.GetEnumerator())
			{
				//rourceRemains: whether there are some elements left in source
				//removeRemains: whether there are some elements left in toRemove
				//Here we position both enumerators on the first element (if exists)
				bool sourceRemains = sourceEnumerator.MoveNext();
				bool removeRemains = removeEnumerator.MoveNext();
				
				while(sourceRemains && removeRemains)
				//In the beginning of each iteration, current element of source has not been processed
				//and current element of toRemove is not less than previous element of source (if exists)
				{
					int comparisonResult = sourceEnumerator.Current.CompareTo(removeEnumerator.Current);
					
					if(comparisonResult < 0)
					//If current element of toRemove is greater than current element of source, we should return current element of source and move to the next element of source
					{
						yield return sourceEnumerator.Current;
						sourceRemains = RemoveExtra_MoveNext(sourceEnumerator);
					} else if(comparisonResult > 0)
					//If current element of toRemove is less than current element of source, we should move to the first element of toRemove which is not less than current element of source, and then repeat the iteration
					{
						removeRemains = RemoveExtra_ForwardUntil(removeEnumerator, sourceEnumerator.Current);
					} else if(comparisonResult == 0)
					//If current element of toRemove equals to current element of source, we should skip current element of source
					//Also, there is no point in comparing next element of source with the current element of toRemove, because former is definitely larger than the latter
					{
						sourceRemains = RemoveExtra_MoveNext(sourceEnumerator);
						removeRemains = RemoveExtra_MoveNext(removeEnumerator);
					} else
					{
						throw new ApplicationException("Wrong comparison result " + comparisonResult);
					}
				}
				
				//If there is no elements left in toRemove, then either there were no elements in toRemove or last element of toRemove was less than current element of source
				//So we can output all remaining elements of source
				while(sourceRemains)
				{
					yield return sourceEnumerator.Current;
					sourceRemains = sourceEnumerator.MoveNext();
				}
			}
		}
	
		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
		{
			return source.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		}
		
		public static List<T> RemoveAllButFirst<T>(this List<T> list)
		{
			List<T> result = new List<T>();
			if(list.Count != 0)
			{
				result.Add(list.First());
			}
			return result;
		}
		
		public static object GetValue(this DbDataReader reader, string column)
		{
			return reader.GetValue(reader.GetOrdinal(column));
		}

		public static IEnumerable<T> ConcatScalar<T>(this IEnumerable<T> source, params T[] toAdd)
		{
			foreach(T elem in source) {
				yield return elem;
			}
			foreach(T elem in toAdd)
			{
				if(elem != null)
				{
					yield return elem;
				}
			}
		}

	}
}
