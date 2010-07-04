using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core {
	public static class RegistryCleaner {

		public static void CleanRegistry<TKey, TData>() where TKey : struct where TData : IDataObject<TKey, TData>, new() {
			Registry<TKey, TData>.instance.Clear();
		}

	}
}
