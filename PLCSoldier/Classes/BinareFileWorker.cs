using MsBox.Avalonia;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PLCSoldier.Classes
{
    internal class BinaryFileWorkerLegacy
    {
        ////Существование директории и файла
        //public static bool IsBinareFileExist()
        //{
        //    if (!Directory.Exists("appData"))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        if (File.Exists("appData/readFile.dat"))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}
        ////Создание директории
        //public static void IsBinareFileExist(bool ForceCreate)
        //{
        //    if (!Directory.Exists("appData"))
        //    {
        //        Directory.CreateDirectory("appData");
        //    }
        //}
        ////получить данные из переменных
        //public static List<DataValue> GetValuesBinare()
        //{
        //    List<DataValue> dataValues = new List<DataValue>();
        //    IsBinareFileExist(true);
        //    using (BinaryReader reader = new BinaryReader(File.Open("appData/readFile.dat", FileMode.OpenOrCreate)))
        //    {
        //        while (reader.PeekChar() != -1)
        //        {
        //            DataValue dataValue = new DataValue()
        //            {
        //                Name = reader.ReadString(),
        //                TypeId = reader.ReadInt32(),
        //                Adress = reader.ReadString(),
        //                Access = reader.ReadInt32(),
        //                IsConst = reader.ReadBoolean(),
        //                ContainsIn = reader.ReadString(),
        //            };
        //            if (dataValues.Where(x => x.Name == dataValue.ContainsIn).Any())
        //            {
        //                DataValue data = dataValues.Where(x => x.Name == dataValue.ContainsIn).First();
        //                data.Childrens.Add(dataValue);
        //            }
        //            else
        //            {
        //                dataValues.Add(dataValue);
        //            }
        //        }
        //    }
        //    return dataValues;
        //}
        ////добавить данные в переменную
        //public static async Task AddValueToFileAsync(DataValue dataValue)
        //{

        //    if (string.IsNullOrWhiteSpace(dataValue.Name) || dataValue.TypeId == -1)
        //    {
        //        var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле заполнение пустое", MsBox.Avalonia.Enums.ButtonEnum.Ok);
        //        var result = await box.ShowAsync();
        //    }
        //    else
        //    {
        //        IsBinareFileExist(true);
        //        using (var fileStream = new FileStream("appData/readFile.dat", FileMode.Append, FileAccess.Write, FileShare.None))
        //        using (var writer = new BinaryWriter(fileStream))
        //        {
        //            writer.Write(dataValue.Name);
        //            writer.Write(dataValue.TypeId);
        //            writer.Write(dataValue.Adress);
        //            writer.Write(dataValue.Access);
        //            writer.Write(dataValue.IsConst);
        //            writer.Write(dataValue.ContainsIn);
        //            writer.Close();
        //        }
        //    }
        //}
        ////пересоздать данные в файле
        //public static void Recreate(List<DataValue> dataValues)
        //{
        //    if (dataValues != null)
        //    {
        //        IsBinareFileExist(true);

        //        using (BinaryWriter writer = new BinaryWriter(File.Open("appData/readFile.dat", FileMode.Create)))
        //        {
        //            foreach (DataValue item in dataValues)
        //            {
                        
        //                writer.Write(item.Name);
        //                writer.Write(item.TypeId);
        //                writer.Write(item.Adress);
        //                writer.Write(item.Access);
        //                writer.Write(item.IsConst);
        //                writer.Write(item.ContainsIn);
        //                if (item.Childrens.Count > 0)
        //                {
        //                    foreach (DataValue dataValueChildr in item.Childrens)
        //                    {
        //                        writer.Write(dataValueChildr.Name);
        //                        writer.Write(dataValueChildr.TypeId);
        //                        writer.Write(dataValueChildr.Adress);
        //                        writer.Write(dataValueChildr.Access);
        //                        writer.Write(dataValueChildr.IsConst);
        //                        writer.Write(dataValueChildr.ContainsIn);
        //                    }
        //                }
        //            }
        //            writer.Close();
        //        }
        //    }
        //}
        ////получить типы данных
        //public static List<DataType> GetTypes()
        //{
        //    List<DataType> types = new List<DataType>();
        //    List<UserTypeContent> userTypes = new List<UserTypeContent>();
        //    IsBinareFileExist(true);
        //    if (File.Exists("appData/types.dat"))
        //    {
        //        using (BinaryReader reader = new BinaryReader(File.Open("appData/types.dat", FileMode.Open)))
        //        {
        //            while (reader.PeekChar() != -1)
        //            {
        //                DataType type = new DataType()
        //                {
        //                    TypeId = reader.ReadInt32(),
        //                    Title = reader.ReadString(),
        //                    IsCustom = reader.ReadBoolean(),
        //                };
        //                types.Add(type);
        //            }
        //        }
        //        userTypes = GetTypesContent();
        //        List<int> Keys = userTypes.DistinctBy(x => x.TypeContentId).Select(x=>x.TypeContentId).ToList();
        //        foreach (int typeIdCont in Keys)
        //        {
        //            DataType type = types.Where(x => x.TypeId == typeIdCont).FirstOrDefault();
        //            if(type != null)
        //            {
        //                UserTypeContent userTypeContent = userTypes.Where(x => x.TypeContentId == typeIdCont).FirstOrDefault();
        //                userTypeContent.AvailableTypes = userTypes.Where(x => x.TypeContentId == typeIdCont).Select(x => x.TypeId).ToList();
        //                type.UserTypeContent = userTypeContent;
        //            }
        //        }
        //        return types;
        //    }
        //    else
        //    {
        //        types = DataType.GetTypes();
        //        using (BinaryWriter writer = new BinaryWriter(File.Open("appData/types.dat", FileMode.Create)))
        //        {
        //            foreach (DataType item in types)
        //            {
        //                writer.Write(item.TypeId);
        //                writer.Write(item.Title);
        //                writer.Write(item.IsCustom);
        //            }
        //            writer.Close();
        //        }
        //        return types;
        //    }
        //}
        ////получить содержание типов пользовательских типов
        //public static List<UserTypeContent> GetTypesContent()
        //{
        //    List<UserTypeContent> userTypesContents = new List<UserTypeContent>();
        //    IsBinareFileExist(true);
        //    if (File.Exists("appData/userTypes.dat"))
        //    {
        //        using (BinaryReader reader = new BinaryReader(File.Open("appData/userTypes.dat", FileMode.Open)))
        //        {
        //            while (reader.PeekChar() != -1)
        //            {
        //                UserTypeContent typesContent = new UserTypeContent()
        //                {
        //                    TypeContentId = reader.ReadInt32(),
        //                    TypeId = reader.ReadInt32(),
        //                };
        //                userTypesContents.Add(typesContent);
        //            }
        //        }
        //        return userTypesContents;
        //    }
        //    else
        //    {
        //        userTypesContents = UserTypeContent.GetTypes();
        //        using (BinaryWriter writer = new BinaryWriter(File.Open("appData/userTypes.dat", FileMode.Create)))
        //        {
        //            foreach (UserTypeContent item in userTypesContents)
        //            {
        //                writer.Write(item.TypeContentId);
        //                writer.Write(item.TypeId);
        //            }
        //            writer.Close();
        //        }
        //        return userTypesContents;
        //    }
        //}
    }
}
