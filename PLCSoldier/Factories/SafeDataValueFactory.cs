using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSoldier.Classes;
using PLCSoldier.Services;
using PLCSoldier.Utils;

namespace PLCSoldier.Factories
{
    public class SafeDataValueFactory : IDataValueFactory
    {
        private IValidationErrorHandler _validationErrorHandler;

        /// <param name="validationErrorHandler">Сервис уведомлений, который будет присвоен созданным переменным</param>
        public SafeDataValueFactory(IValidationErrorHandler validationErrorHandler)
        {
            _validationErrorHandler = validationErrorHandler;
        }

        public DataValueCreationResult CreateDataValue(DataValueDTO dataValueDTO)
        {
            return CheckType(dataValueDTO);
        }

        private DataValueCreationResult CheckType(DataValueDTO dataValueDTO)
        {
            DataType? dataValueType = DataValuesModel.GetDataType(dataValueDTO.Type);

            if (dataValueType == null)
            {
                return new DataValueCreationResult(IsCreated: false, CreatedDataValue: null, ErrorMessage: 
                    $"Не удалось создать экземпляр переменной: тип {dataValueDTO.Type} не найден");
            }
            else return CreateInstance(dataValueDTO, dataValueType);
        }

        private DataValueCreationResult CreateInstance(DataValueDTO dataValueDTO, DataType dataType)
        {
            // Добавить инициализацию детей

            try
            {
                var instance = new DataValue(dataValueDTO.Name!, 
                    dataType.EDataType,
                    new ExceptionValidationErrorHandler())
                {
                    AccessType = dataValueDTO.AccessType,
                    Address = dataValueDTO.Address ?? string.Empty,
                    Description = dataValueDTO.Description ?? string.Empty,
                    IsConst = dataValueDTO.IsConst,
                    DisplayStyle = dataValueDTO.DisplayStyle,
                    IsRetain = dataValueDTO.IsRetain,
                    StoredData = dataValueDTO.Value!
                };

                instance.SetValidationErrorHandler(_validationErrorHandler);
                return new DataValueCreationResult(IsCreated: true, CreatedDataValue: instance, ErrorMessage: null);
            }
            catch (Exception ex)
            {
                return new DataValueCreationResult(IsCreated: false, CreatedDataValue: null, ErrorMessage: ex.Message);
            }           
        }
    }

    public record DataValueCreationResult(bool IsCreated, DataValue? CreatedDataValue, string? ErrorMessage);
}
