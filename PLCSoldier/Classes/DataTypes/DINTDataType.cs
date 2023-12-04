using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSoldier.Enums;

namespace PLCSoldier.Classes
{
    public class DINTDataType : DataType
    {
        public override EDataType EDataType => EDataType.DINT;

        public override string Title => "DINT";

        public override string DefaultValue => "0";

        public override bool IsValueValid(string? value)
        {
            return int.TryParse(value, out _);
        }

        public override string SetValue(string? value)
        {
            if (int.TryParse(value, out int parsedValue))
            {
                return value;
            }
            else
            {
                throw new ArgumentException($"Значение {value} не соответствует типу DINT");
            }
        }
    }
}
