using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core {

	interface IDataObject<TKey, TData> /*: IDataObject<TKey>*/
        where TData : IDataObject<TKey, TData>, new()
        where TKey : struct {

        //static TData LoadById(TKey id);

        //static TData CreateByIdFromRegistry(TKey id);

        void CreateByIdFromRegistry(TKey id);

        //TKey GetId();

        TKey id {
            get;
        }

    }

}
