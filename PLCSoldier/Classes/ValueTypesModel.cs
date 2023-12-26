using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Classes
{
    internal static class ValueTypesModel
    {
        private static IReadOnlyList<DataType>? _all;
        private static IReadOnlyList<string>? _titles;

        private static List<DataType> GetValueTypes()
        {
            return ObservableCollection.GetTypes();
        }
        public static IReadOnlyList<DataType> All => _all ??= GetValueTypes().ToList();
        public static IReadOnlyList<string> Titles => _titles ??= All.Select(x => x.Title).ToList();
    }
}
