using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSoldier.Enums;

namespace PLCSoldier.Classes
{
    public class REALDataType : DataType
    {
        public override EDataType EDataType => EDataType.REAL;

        public override string Title => "REAL";

        public override string DefaultValue => "0";

        public override string SetValue(string? value)
        {
            if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
            {
                return value.Replace(',','.');
            }
            else
            {
                throw new ArgumentException($"Значение {value} не соответствует типу REAL");
            }
        }

        public override bool IsValueValid(string? value)
        {
            return float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        }
    }
}
