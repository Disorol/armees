using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSoldier.Enums;

namespace PLCSoldier.Classes
{
    public class BOOLDataType : DataType
    {
        public override string Title => "BOOL";

        public override EDataType EDataType => EDataType.BOOL;

        public override string DefaultValue => "0";

        public override string SetValue(string? value)
        {
            if (value?.ToLower() == "true" || value == "1")
            {
                return "1";
            }
            if (value?.ToLower() == "false" || value == "0")
            {
                return "0";
            };

            throw new ArgumentException($"Значение {value} не соответствует типу BOOL");
        }

        public override bool IsValueValid(string? value)
        {
            value = value?.ToLower();

            if (value == "true" || value == "false" || value == "1" || value == "0")
                return true;
            else
                return false;
        }
    }
}
