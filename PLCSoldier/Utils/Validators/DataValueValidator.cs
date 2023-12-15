using Microsoft.CodeAnalysis;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using PLCSoldier.Classes;

namespace PLCSoldier.Utils
{
    public class DataValueNameValidator
    {
        private string? _errorMessage;
        private bool _isValid = true;

        private string? _suggestedName;

        public DataValueNameValidator(string? name)
        {
            _suggestedName = name;
        }

        public DataValueNameValidator Reset(string newName)
        {
            _suggestedName = newName;
            _errorMessage = null;
            _isValid = true;

            return this;
        }

        /// <param name="otherValues">Список переменных, в которых содержится переменная</param>
        public DataValueNameValidator CheckUniqueness(IEnumerable<DataValue>? otherValues)
        {
            // Если предлагаемое имя равно null, значит либо модель не прошла валидацию на предыдущих этапах,
            // либо изначальное имя переменной было null. В обоих случаях модель не проходит валидацию
            if (_suggestedName == null)
            {
                _isValid = false;
                return this;
            }

            // Если переменная не состоит в списке, то и негде проверять уникальность имени
            if (otherValues == null)
                return this;
           
            string originalName = _suggestedName;
            for (int i = 1; otherValues.Any(x => x.Name.ToLower() == _suggestedName.ToLower()); i++)
            {
                _isValid = false;
                _suggestedName = originalName + $"_{i}";
            }

            return this;
        }

        public DataValueNameValidator CheckIsNullOrWhiteSpace()
        {
            if (string.IsNullOrWhiteSpace(_suggestedName))
            {
                _isValid = false;
                _suggestedName = null;
                _errorMessage ??= Properties.Resources.WarningVariableName;
            }

            return this;
        }

        public DataValueNameValidator TrimName()
        {
            if (_suggestedName == null)
            {
                _isValid = false;
                return this;
            }

            string buffer = _suggestedName;
            _suggestedName = _suggestedName.Trim();

            if (buffer != _suggestedName)
                _isValid = false;
                           
            return this;
        }

        public DataValueNameValidator CheckNameFormat()
        {
            if (_suggestedName == null)
            {
                _isValid = false;
                return this;
            }

            if (!Regex.IsMatch(_suggestedName, @"^[A-Za-z][0-9A-Za-z]*([_.][0-9A-Za-z]+)*$") 
                /*|| !Regex.IsMatch(_dataValue.Name, @"(?!^\d+$)^.+$")*/)
            {
                _suggestedName = null;
                _isValid = false;
                _errorMessage ??= Properties.Resources.ErrorIncorrectFormat;
            }

            return this;
        }

        public DataValueNameValidator ExecuteFullNameValidation(IEnumerable<DataValue>? otherValues)
        {
            return TrimName()
                .CheckIsNullOrWhiteSpace()
                .CheckNameFormat()
                .CheckUniqueness(otherValues);
        }       

        public DataValueValidationResult GetValidationResult()
        {
            return new DataValueValidationResult(_isValid, _suggestedName, _errorMessage);
        }
    }

    /// <summary>
    /// Результат валидации объекта DataValue
    /// </summary>
    /// <param name="IsValid">Валидна ли текущая модель</param>
    /// <param name="SuggestedName">Предлагаемое имя, проходящее все указанные проверки</param>
    public record DataValueValidationResult(bool IsValid, string? SuggestedName, string? ErrorMessage);
}
