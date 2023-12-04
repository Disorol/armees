using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PLCSoldier.Utils
{
    public class ObservableCollectionView<T> : INotifyCollectionChanged, IEnumerable<T>, IList, IList<T>
        where T : class
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private readonly IList<T> _sourceCollection;

        private List<Func<T, bool>> _filters = new List<Func<T, bool>>();

        public bool IsFixedSize => false;

        public bool IsReadOnly => false;

        public int Count
        {
            get
            {
                int count = 0;

                using (var enumerator = GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool IsSynchronized => false;

        public object SyncRoot => null;

        T IList<T>.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public object? this[int index] 
        {
            get
            {
                int curIndex = 0;
                using (var enumerator = GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if (curIndex == index)
                            return enumerator.Current;

                        curIndex++;
                    }
                }
                    
                throw new ArgumentOutOfRangeException();
            }
            set => this[index] = value; 
        }

        public ObservableCollectionView(IList<T> sourceCollection)
        {
            _sourceCollection = sourceCollection;
        }

        public void Refresh()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void AddFilter(Func<T, bool> predicate)
        {
            _filters.Add(predicate);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _sourceCollection)
            {
                bool isValid = true;
                foreach (var filter in _filters)
                {
                    if (!filter(item))
                    {
                        isValid = false;
                        break;
                    }
                }
                if (isValid)
                {
                    yield return item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Add(object? value)
        {
            if (value is not T)
                return -1;

            Add((T)value);

            return _sourceCollection.Count - 1;
        }

        public void Clear()
        {
            _sourceCollection.Clear();

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(object? value)
        {
            if (value is not T)
                return false;

            return _sourceCollection.Contains((T)value);
        }

        public int IndexOf(object? value)
        {
            if (value is not T)
            {
                return -1;
            }
            else return IndexOf((T)value);
        }

        public void Insert(int index, object? value)
        {
            if (value is not T)
                return;

            _sourceCollection.Insert(index, (T) value);

            int visualIndex = IndexOf(value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, visualIndex));
        }

        public void Remove(object? value)
        {
            if (value is not T)
                return;

            Remove((T)value);
        }

        public void RemoveAt(int index)
        {
            if (index >= _sourceCollection.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            int visualIndex = IndexOf(_sourceCollection.ElementAt(index));

            _sourceCollection.RemoveAt(index);

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, visualIndex));
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item)
        {
            int counter = 0;

            using (var enumerator = GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current == item)
                    {
                        return counter;
                    }

                    counter++;
                }

                return -1;
            }
        }

        public void Insert(int index, T item)
        {
            _sourceCollection.Insert(index, item);

            int visualIndex = IndexOf(item);
            if (visualIndex != -1)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, visualIndex));
        }

        public void Add(T item)
        {
            _sourceCollection.Add(item);

            int visualIndex = IndexOf(item);
            if (visualIndex != -1)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, visualIndex));
        }

        public bool Contains(T item)
        {
            return _sourceCollection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _sourceCollection.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            int visualIndex = IndexOf(item);

            bool isRemoved = _sourceCollection.Remove(item);

            if (visualIndex != -1)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, visualIndex));

            return isRemoved;
        }
    }
}
