using CommunityToolkit.Mvvm.ComponentModel;
using HanumanInstitute.MvvmDialogs.Avalonia;
using MsBox.Avalonia;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PLCSoldier.Enums;
using PLCSoldier.Services;
using PLCSoldier.Utils;

namespace PLCSoldier.Classes
{
    public class DataValue : ObservableObject, ICloneable
    {
        public event Action<object>? ValueChanged;

        public static EDataType DefaultDataType = EDataType.BOOL;

        public string Name
        {
            get => _name;
            set
            {
                string lastName = _name;

                var validationResult = _validator.Reset(value)
                    .ExecuteFullNameValidation(SourceCollection?.Where(var => var != this))
                    .GetValidationResult();

                if (validationResult.IsValid || validationResult.SuggestedName != null)
                {
                    if (validationResult.IsValid)
                    {
                        _name = value;
                    }
                    else if (validationResult.SuggestedName != null)
                    {
                        _name = validationResult.SuggestedName;
                    }

                    if (lastName != _name)
                    {
                        OnPropertyChanged();
                        _ = SaveChanges();
                    }
                }
                else if (validationResult.ErrorMessage != null)
                {
                    _validationErrorHandler.Handle(validationResult.ErrorMessage);
                }
                else
                {
                    _validationErrorHandler.Handle("Произошла какая-то совсем уж неописуемая ошибка");
                }
            }
        }

        public EDataType EVarType
        {
            get { return _eDataType; }
            set
            {
                if (_childrens.Count > 0 && SourceCollection != null)
                {
                    _validationErrorHandler.Handle("Нельзя менять тип данных для родительской переменной");
                    return;
                }

                if (_containsIn != "")
                {
                    _validationErrorHandler.Handle("Нельзя менять тип данных для дочерней переменной");
                    return;
                }

                var requiredValueType = DataType.GetValueType(value);

                if (requiredValueType == null)
                {
                    _validationErrorHandler.Handle("Указанный тип переменной не существует");
                    return;
                }

                _eDataType = value;
                ValueType = requiredValueType;

                OnPropertyChanged();
                _ = SaveChanges();
            }
        }

        public DataType ValueType
        {
            get => _valueType;
            private set
            {
                _valueType = value;
                StoredData = value.DefaultValue;

                OnPropertyChanged();
            }
        }

        public string StoredData
        {
            get { return _storedData; }
            set
            {
                try
                {
                    _storedData = ValueType.SetValue(value);

                    OnPropertyChanged();
                    ValueChanged?.Invoke(_storedData);
                    _ = SaveChanges();
                }
                catch (Exception ex)
                {
                    _validationErrorHandler.Handle(ex.Message);
                }
            }
        }

        public ObservableCollection<DataValue> Childrens
        {
            get { return _childrens; }
            set
            {
                _childrens = value;
                OnPropertyChanged();
                _ = SaveChanges();
            }
        }
        
        public string Description
        {
            get => _description;
            set 
            { 
                _description = value; 
                OnPropertyChanged(); 
                _ = SaveChanges(); 
            }
        }
        public EAccessType AccessType
        {
            get => _accessType;
            set
            {
                _accessType = value;
                OnPropertyChanged();
                _ = SaveChanges();
            }
        }
             
        public EDisplayStyle DisplayStyle
        {
            get => _displayStyle;
            set 
            { 
                _displayStyle = value; 
                OnPropertyChanged();
                _ = SaveChanges();
            }
        }
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged();
                _ = SaveChanges();
            }
        }
        public bool IsConst
        {
            get { return _isConst; }
            set
            {
                _isConst = value;
                OnPropertyChanged();
                _ = SaveChanges();
            }
        }
        public string ContainsIn
        {
            get { return _containsIn; }
            set
            {
                _containsIn = value;
                OnPropertyChanged();
            }
        }
        public bool IsRetain
        {
            get => _isRetain;
            set
            {
                _isRetain = value;
                OnPropertyChanged();
                _ = SaveChanges();
            }
        }

        public IEnumerable<DataValue>? SourceCollection
        {
            get => _sourceCollection;
            set
            {
                _sourceCollection = value;
                
                // Обновление имени на случай, если в списке уже есть переменная с таким именем
                Name = _name;
            }
        }

        private string _name = "";
        private EDataType _eDataType = EDataType.BOOL;
        private string _address = "";
        private bool _isConst = false;       
        private string _containsIn = ""; //находится в переменной под именем
        private string _storedData = string.Empty;
        private ObservableCollection<DataValue> _childrens = new ObservableCollection<DataValue>();
        private DataType _valueType;
        private string _description = string.Empty;
        private EDisplayStyle _displayStyle;
        private EAccessType _accessType;
        private bool _isRetain;
        private IEnumerable<DataValue>? _sourceCollection;

        private DataValueNameValidator _validator;
        private IValidationErrorHandler _validationErrorHandler;

        public DataValue(string name, 
            EDataType eDataType,
            IValidationErrorHandler validationErrorHandler)
        {
            _validator = new DataValueNameValidator(name);
            _validationErrorHandler = validationErrorHandler;

            Name = name;
            EVarType = eDataType;
        }

        public void SetValidationErrorHandler(IValidationErrorHandler validationErrorHandler)
        {
            _validationErrorHandler = validationErrorHandler;
        }

        public void SetListToChildren(ObservableCollection<DataValue> dataValuesMain)
        {
            SourceCollection = dataValuesMain;
            foreach (DataValue item in _childrens)
            {
                item.SetListToChildren(dataValuesMain);
            }
        }
        public bool CheckDublicateInChildr(string fountString, List<DataValue> searchable)
        {
            if (searchable.Where(x => x.Name == fountString).Any())
            {
                return false;
            }
            return true;
        }
        public void TotalRename(ObservableCollection<DataValue> Childrens, string newName) 
        {
            foreach (DataValue child in Childrens)
            {
                if(child._childrens.Count() != 0)
                {
                    TotalRename(child._childrens, newName);
                    child._containsIn = newName;
                }
                else
                {
                    child._containsIn = newName;
                }
            }
        }

        public object Clone()
        {
            return new DataValue(Name, EVarType, _validationErrorHandler)
            {
                Description = Description,
                StoredData = StoredData,
                AccessType = AccessType,
                Address = Address,
                DisplayStyle = DisplayStyle,
                ContainsIn = ContainsIn,
                IsConst = IsConst,
            };
        }

        public DataValueDTO CreateDTO()
        {
            return new DataValueDTO()
            {
                AccessType = AccessType,
                Address = Address,
                DisplayStyle = DisplayStyle,
                Description = Description,
                IsConst = IsConst,
                IsRetain = IsRetain,
                Name = Name,
                Type = EVarType.ToString(),
                Value = StoredData,
            };
        }

        private async Task SaveChanges()
        {
            if (SourceCollection != null)
                await JsonFileWorker.Recreate(SourceCollection.ToList());
        }
    }
}
