using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSoldier.Classes;

namespace PLCSoldier.Utils.CollectionFilter
{
    public class TreeSourceItemsFilter<T> : ICollectionFilter<T>
        where T : class
    {
        private List<Func<T, bool>> _filters = new List<Func<T, bool>>();
        private HierarchicalTreeDataGridSource<T> _source; 
        private IEnumerable<T> _sourceCollection;

        public TreeSourceItemsFilter(HierarchicalTreeDataGridSource<T> source, IEnumerable<T> sourceCollection)
        {
            _source = source;
            _sourceCollection = sourceCollection;
        }

        public void AddFilter(Func<T, bool> predicate)
        {
            _filters.Add(predicate);
        }

        public void RemoveFilter(Func<T, bool> predicate)
        {
            _filters.Remove(predicate);
        }

        public IEnumerable<T> Filter()
        {

            return _source.Items = new ObservableCollection<T>(_sourceCollection.Where(item =>
            {
                foreach (var filter in _filters)
                {
                    if (!filter(item))
                        return false;
                }
                return true;
            })); 
        }
    }
}
