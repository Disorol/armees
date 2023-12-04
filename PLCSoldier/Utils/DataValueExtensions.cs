using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSoldier.Classes;

namespace PLCSoldier.Utils
{
    public static class DataValueExtensions
    {
        public static DataValueNameValidator Validation(this DataValue dataValue)
        {
            return new DataValueNameValidator(dataValue.Name);
        }

        public static IEnumerable<DataValueDTO> GetDTOs(this IEnumerable<DataValue> dataValues)
        {
            foreach (var dataValue in dataValues)
            {
                yield return dataValue.CreateDTO();
            }
        }
    }
}
