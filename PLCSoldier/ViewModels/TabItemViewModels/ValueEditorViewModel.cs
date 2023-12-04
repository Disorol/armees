using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls.Selection;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PLCSoldier.Classes;
using PLCSoldier.Enums;
using PLCSoldier.Factories;
using PLCSoldier.Services;
using PLCSoldier.Utils;
using ReactiveUI;

namespace PLCSoldier.ViewModels.TabItemViewModels;

public partial class ValueEditorViewModel : ViewModelBase, INotifyPropertyChanged
{
    public ObservableCollection<DataType> Types { get; }
    public HierarchicalTreeDataGridSource<DataValue> Source { get; }

    private string _nameFilter = string.Empty;
    public string NameFilter
    {
        get => _nameFilter;
        set
        {
            _nameFilter = value;
            _dataValuesCollectionView.Refresh();
        }
    }

    private string _dataTypeFilter = string.Empty;
    public string DataTypeFilter
    {
        get => _dataTypeFilter;
        set
        {
            _dataTypeFilter = value;
            _dataValuesCollectionView.Refresh();
        }
    }

    private DataValueDTO _dataValueToAdd;
    public DataValueDTO DataValueToAdd
    {
        get => _dataValueToAdd;
        set => this.RaiseAndSetIfChanged(ref _dataValueToAdd, value); // изменено
    }

    private readonly ObservableCollection<DataValue> _dataValues;
    private ObservableCollectionView<DataValue> _dataValuesCollectionView;

    private ICommand _addDataValueCommand;
    public ICommand AddDataValueCommand => _addDataValueCommand ??= new RelayCommand(AddNewDataValue);

    private ICommand _copyCommand;
    public ICommand CopyCommand => _copyCommand ??= new RelayCommand<DataValue>(CopyDataValue);

    private ICommand _insertCommand;
    public ICommand InsertCommand => _insertCommand ??= new RelayCommand(InsertDataValue);

    private ICommand _deleteCommand;
    public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(() => DeleteDataValueAsync());

    public IEnumerable<DataValue> CopiedDataValues = new List<DataValue>();

    public ICommand DeleteColumnCommand => new RelayCommand(() =>
    {
        Source.Columns.RemoveAt(Source.Columns.Count - 1);
    });

    private readonly IDialogService _dialogService;
    private readonly IDataValueFactory _dataValueFactory;
    private readonly INotificationService _notificationService;

    private string _confirmation = string.Empty;

    public string Confirmation
    {
        get => _confirmation;
        set => this.RaiseAndSetIfChanged(ref _confirmation, value); // изменено
    }

    public ValueEditorViewModel(IDialogService dialogService)
    {
        _notificationService = new MessageBoxNotificationService();
        _dataValueFactory = new SafeDataValueFactory(new NotificationValidationErrorHandler(_notificationService));
        _dialogService = dialogService;

        //List<DataValueDTO> dataValuesDTOs = JsonFileWorker.GetValuesJson();
        Types = new ObservableCollection<DataType>(JsonFileWorker.GetTypes());
        _dataValues = new ObservableCollection<DataValue>();

        _dataValuesCollectionView = new ObservableCollectionView<DataValue>(_dataValues);
        _dataValuesCollectionView.AddFilter(var => var.Name.Contains(NameFilter, StringComparison.InvariantCultureIgnoreCase));
        _dataValuesCollectionView.AddFilter(var => var.ValueType.Title.Contains(DataTypeFilter, StringComparison.InvariantCultureIgnoreCase));

        LoadDataValues();

        foreach (DataValue item in _dataValues)
        {
            item.SetListToChildren(_dataValues);
        }
    
        Source = new HierarchicalTreeDataGridSource<DataValue>(_dataValuesCollectionView)
        {
            Columns =
            {
                new HierarchicalExpanderColumn<DataValue>(
                    new TextColumn<DataValue, string>("Имя переменной", x => x.Name,
                    (var, value) => var.Name = value,
                    new GridLength(100, GridUnitType.Auto),
                    new TextColumnOptions<DataValue>()
                    {
                        MinWidth = new GridLength(100)
                    }),
                    x => x.Childrens), //вывод иерархии детей переменной
                new TextColumn<DataValue, string>("Значение", x => x.StoredData.ToString(), (var, value) => var.StoredData = value, new GridLength(10, GridUnitType.Auto), new TextColumnOptions<DataValue>()
                {
                    MinWidth = new GridLength(100),
                }),
                new TemplateColumn<DataValue>("Тип данных", "DataTypeEditCell", null, new GridLength(100, GridUnitType.Auto), new TemplateColumnOptions<DataValue>()
                {
                    MinWidth = new GridLength(100),
                    CompareAscending = (x, y) =>
                    {
                        return x.ValueType.Title.CompareTo(y.ValueType.Title);
                    },
                    CompareDescending = (x, y) =>
                    {
                        return y.ValueType.Title.CompareTo(x.ValueType.Title);
                    },
                }),
                new TemplateColumn<DataValue>("Тип доступа", "AccessTypeCell", null, new GridLength(100, GridUnitType.Auto), new TemplateColumnOptions<DataValue>()
                {
                    MinWidth = new GridLength(100),
                    CompareAscending = (x, y) =>
                    {
                        return x.AccessType.CompareTo(y.AccessType);
                    },
                    CompareDescending = (x, y) =>
                    {
                        return y.AccessType.CompareTo(x.AccessType);
                    },
                }),
                new TemplateColumn<DataValue>("Константа", "IsConstCell", null, new GridLength(100, GridUnitType.Auto), new TemplateColumnOptions<DataValue>()
                {
                    MinWidth = new GridLength(0),
                    CompareAscending = (x, y) =>
                    {
                        return x.IsConst.CompareTo(y.IsConst);
                    },
                    CompareDescending = (x, y) =>
                    {
                        return y.IsConst.CompareTo(x.IsConst);
                    },
                }),
                new TemplateColumn<DataValue>("Стиль", "DisplayStyleCell", null, new GridLength(100, GridUnitType.Auto), new TemplateColumnOptions<DataValue>()
                {
                    MinWidth = new GridLength(100),
                    CompareAscending = (x, y) =>
                    {
                        return x.DisplayStyle.CompareTo(y.DisplayStyle);
                    },
                    CompareDescending = (x, y) =>
                    {
                        return y.DisplayStyle.CompareTo(x.DisplayStyle);
                    },
                }),
                new TextColumn<DataValue, string>("Описание", x => x.Description, (var, value) => var.Description = value, new GridLength(10, GridUnitType.Star), new TextColumnOptions<DataValue>()
                {
                    MinWidth = new GridLength(100),
                }),
                new TemplateColumn<DataValue>("Сохранять", "IsRetainCell", null, new GridLength(100, GridUnitType.Auto), new TemplateColumnOptions<DataValue>()
                {
                    MinWidth = new GridLength(100),
                    CompareAscending = (x, y) =>
                    {
                        return x.IsRetain.CompareTo(y.IsRetain);
                    },
                    CompareDescending = (x, y) =>
                    {
                        return y.IsRetain.CompareTo(x.IsRetain);
                    },
                }),
            }
        };
     
        ResetNewVar();
    }

    private void LoadDataValues()
    {
        var _dataValuesDTOs = DataValuesModel.All;

        foreach (var dto in _dataValuesDTOs)
        {
            AddDataValueFromDTOToMainList(dto);
        }
    }

    public async void AddNewDataValue()
    {
        AddDataValueFromDTOToMainList(DataValueToAdd);        

        await JsonFileWorker.Recreate(_dataValues.ToList());

        ResetNewVar();
    }

    private void AddDataValueFromDTOToMainList(DataValueDTO dto)
    {
        var newDataValue = CreateDataValue(dto);

        if (newDataValue == null) return;

        _dataValuesCollectionView.Add(newDataValue);
        newDataValue.SourceCollection = _dataValues;
    }

    private DataValue? CreateDataValue(DataValueDTO dto)
    {
        var dataValueCreatingResult = _dataValueFactory.CreateDataValue(dto);

        if (dataValueCreatingResult.CreatedDataValue == null)
        {
            if (dataValueCreatingResult.ErrorMessage != null)
            {
                _notificationService.Notify(dataValueCreatingResult.ErrorMessage, ENotificationType.Error);
            }
            else
            {
                _notificationService.Notify("Неизвестная ошибка", ENotificationType.Error);
            }
            return null;
        }
        
        return dataValueCreatingResult.CreatedDataValue;
    }
   
    public async void InsertDataValue()
    {
        if (Source.RowSelection == null)
        {
            return;
        }

        var selectedItem = Source.RowSelection.SelectedItem;
        var index = _dataValues.IndexOf(selectedItem!);
        foreach (var var in CopiedDataValues)
        {
            DataValue clonedVar = (var.Clone() as DataValue)!;

            var validationResult = clonedVar.Validation().CheckUniqueness(_dataValues).GetValidationResult();

            if (!validationResult.IsValid)
            {
                if (validationResult.SuggestedName != null)
                {
                    clonedVar.Name = validationResult.SuggestedName;
                }
            }

            _dataValuesCollectionView.Insert(index++, clonedVar);
        }
        await JsonFileWorker.Recreate(_dataValues.ToList());
    }

    public void CopyDataValue(DataValue var)
    {
        CopiedDataValues = new DataValue[] { var.Clone() as DataValue };
    } 

    public async Task DeleteDataValueAsync()
    {
        DataValue selection = (DataValue)((ITreeSelectionModel)Source.Selection!).SelectedItem;
        if (selection == null)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Внимание", "Выделите удаляемую переменную", MessageBox.Avalonia.Enums.ButtonEnum.Ok);
            var result = await box.ShowAsync();
        }
        else
        {
            if (selection.ContainsIn == "")
            {
                var resultDel = await _dialogService.ShowMessageBoxAsync(this,
                    $"Вы точно хотите удалить ветвь переменной {selection.ContainsIn}",
                    "Внимание.",
                    MessageBoxButton.OkCancel);
                if (resultDel == true)
                {
                    _dataValuesCollectionView.Remove(selection);
                    selection = null;
                    await JsonFileWorker.Recreate(_dataValues.ToList());
                }
            }
            else
            {
                var result = await _dialogService.ShowMessageBoxAsync(this,
                        $"Точно хотите удалить ветвь переменной {selection.ContainsIn}",
                        "Внимание.",
                        MessageBoxButton.OkCancel);
                if (result == true)
                {
                    DataValue dataValue = _dataValues.Where(x => x.Name == selection.ContainsIn).FirstOrDefault();
                    if (dataValue != null)
                    {
                        _dataValuesCollectionView.Remove(dataValue);
                        selection = null;
                        await JsonFileWorker.Recreate(_dataValues.ToList());
                    }
                }
            }
        }
    }

    private void ResetNewVar()
    {
        DataValueToAdd = new DataValueDTO();
    }
}
