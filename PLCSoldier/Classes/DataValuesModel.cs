using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSoldier.Enums;

namespace PLCSoldier.Classes
{
    public static class DataValuesModel
    {
        private static IReadOnlyList<DataValueDTO>? _all;
        private static IReadOnlyList<string>? _titles;
        private static IEnumerable<string>? _accessTypes, _dataTypes, _displayStyles;

        private static List<DataValueDTO> GetValueTypes()
        {
            return JsonFileWorker.GetValuesJson();
        }
        private static List<DataType> GetDataTypes()
        {
            return JsonFileWorker.GetTypes();
        }
        public static IReadOnlyList<DataValueDTO> All => GetValueTypes().ToList();
        public static IReadOnlyList<string> Titles => GetDataTypes().Select(x=>x.Title).ToList();
        public static IEnumerable<string> AccessTypes => _accessTypes ??= Array.ConvertAll(Enum.GetValues<EAccessType>(), type => type.ToString());
        public static IEnumerable<string> DataTypes => _dataTypes ??= new string[] { "BOOL", "INT", "DINT", "REAL" };
        public static IEnumerable<string> DisplayStyles => _displayStyles ??= Array.ConvertAll(Enum.GetValues<EDisplayStyle>(), type => type.ToString());
        public static DataType? GetDataType(string name)
        {
            // Добавить проверку на случай, если имя будет равняться CustomDataType

            Type? requiredType = Type.GetType("PLCSoldier.Classes." + name + "DataType");

            if (requiredType != null)
            {
                return Activator.CreateInstance(requiredType) as DataType;
            }
            
            // Дополнительный поиск требуемого класса для случая CustomDataType

            return null;
        }
    }
}
