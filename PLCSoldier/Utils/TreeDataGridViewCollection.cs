using Avalonia.Controls;
using DynamicData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Utils
{
    public class ExpandableDataGridCollection<T> : IList<T>, IList, INotifyCollectionChanged
        where T : class
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private T _lastItem;
        public T LastItem
        {
            get => _lastItem;
            private set
            {
                _lastItem = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        private readonly ObservableCollection<T> _sourceCollection;
        private Func<T> _createLastItem;

        public bool IsFixedSize => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public int Count => _sourceCollection.Count + 1;

        public bool IsSynchronized => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();

        object? IList.this[int index] { get => this[index]; set => this[index] = (T) value; }

        public T this[int index]
        {
            get
            {
                if (index > _sourceCollection.Count)
                {
                    throw new IndexOutOfRangeException();
                }
                else if (index < _sourceCollection.Count)
                {
                    return _sourceCollection[index];
                }
                else return LastItem;
            }
            set
            {
                this[index] = value;
            }
        }

        public ExpandableDataGridCollection(ObservableCollection<T> sourceCollection, Func<T> createLastItem)
        {
            _sourceCollection = sourceCollection;
            _createLastItem = createLastItem;

            _lastItem = _createLastItem();

            _sourceCollection.CollectionChanged += OnSourceCollectionChanged;
        }

        private void OnSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _sourceCollection)
            {
                yield return item;
            }
            yield return LastItem;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddLastItemInTheMainCollection()
        {
            _sourceCollection.Add(LastItem);
            LastItem = _createLastItem();
        }

        // Хз, че с этим делать, мне в падлу их реализовывать
        #region Нереализованные методы

        public int Add(object? value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object? value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object? value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object? value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object? value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
