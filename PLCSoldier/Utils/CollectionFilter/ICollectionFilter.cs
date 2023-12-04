using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Utils.CollectionFilter
{
    public interface ICollectionFilter<T>
    {
        IEnumerable<T> Filter();
        void AddFilter(Func<T, bool> predicate);
    }
}
