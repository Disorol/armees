//using Newtonsoft.Json;
using System.Collections.Generic;
using PLCSoldier.Enums;

namespace PLCSoldier.Classes
{
    public abstract class DataType
    {
        public static DataType DefaultType => new BOOLDataType();

        public abstract EDataType EDataType { get; }
        public abstract string Title { get; }
        public abstract string DefaultValue { get; }
        private UserTypeContent UserTypeContent { get; set; }

        public abstract string SetValue(string? value);
        public abstract bool IsValueValid(string? value);

        //тестовые данные
        public static List<DataType> GetTypes()
        {
            List<DataType> _types = new List<DataType>()
            {
                new BOOLDataType(),
                new INTDataType(),
                new DINTDataType(),
                new REALDataType(),
            };
            return _types;
        }
        //получить тип данных по id
        public static DataType? GetValueType(EDataType dataType)
        {
            switch (dataType)
            {
                case EDataType.BOOL:
                    return new BOOLDataType();
                case EDataType.INT:
                    return new INTDataType();
                case EDataType.DINT:
                    return new DINTDataType();
                case EDataType.REAL:
                    return new REALDataType();
                //case EDataType.CustomTitle:
                //    return null;
                default:
                    return null;
            }
        }
    }
}
