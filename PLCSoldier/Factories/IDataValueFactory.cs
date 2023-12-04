using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSoldier.Classes;

namespace PLCSoldier.Factories
{
    public interface IDataValueFactory
    {
        DataValueCreationResult CreateDataValue(DataValueDTO dataValueDTO);
    }
}
