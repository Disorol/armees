using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSoldier.Enums;

namespace PLCSoldier.Classes
{
    public class DataValueDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Value { get; set; } = DataType.DefaultType.DefaultValue;
        public string Type { get; set; } = DataType.DefaultType.EDataType.ToString();
        public EAccessType AccessType { get; set; } = EAccessType.Read;
        public string? Address { get; set; }
        public bool IsConst { get; set; } = false;
        public EDisplayStyle DisplayStyle { get; set; } = EDisplayStyle.Decimal;
        public bool IsRetain { get; set; } = false;
    }
}
