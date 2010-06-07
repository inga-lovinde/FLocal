using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core {

	public abstract class IDataObject<TKey, TData>
        where TData : IDataObject<TKey, TData>, new()
        where TKey : struct {

        //static TData LoadById(TKey id);

        //static TData CreateByIdFromRegistry(TKey id);

        internal abstract void CreateByIdFromRegistry(TKey id, bool forLoadingFromHash);

        //TKey GetId();

        public abstract TKey id {
            get;
        }

		internal abstract void markAsDeletedFromRegistry();

    }

}
