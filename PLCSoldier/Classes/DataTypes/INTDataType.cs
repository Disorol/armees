using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSoldier.Enums;

namespace PLCSoldier.Classes
{
    public class INTDataType : DataType
    {
        public override EDataType EDataType => EDataType.INT;

        public override string Title => "INT";

        public override string DefaultValue => "0";

        public override string SetValue(string? value)
        {
            if (short.TryParse(value, out short parsedValue))
            {
                return value;
            }
            else
            {
                throw new ArgumentException($"Значение {value} не соответствует типу INT");
            }
        }

        public override bool IsValueValid(string? value)
        {
            return short.TryParse(value, out _);
        }
    }
}
