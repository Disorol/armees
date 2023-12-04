using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using PLCSoldier.Utils;

namespace PLCSoldier.Classes
{
    internal class JsonFileWorker
    {
        private static SemaphoreSlim _valuesFileSemaphore = new SemaphoreSlim(1,1);
        private static SemaphoreSlim _typesFileSemaphore = new SemaphoreSlim(1,1);
        private static SemaphoreSlim _userFilePathSemaphore = new SemaphoreSlim(1,1);

        private static string _valuesPath = "appData/values.json";
        private static string _typesPath = "appData/types.json";
        private static string _userTypesPath = "appData/userTypes.json";

        public static bool IsJsonFileExist(string path)
        {
            if (!Directory.Exists("appData"))
            {
                return false;
            }
            else
            {
                if (File.Exists($"appData/{path}.json"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static void IsJsonFileExist(bool ForceCreate)
        {
            if (!Directory.Exists("appData"))
            {
                Directory.CreateDirectory("appData");
            }
        }

        public static List<DataValueDTO> GetValuesJson()
        {
            IsJsonFileExist(true);
            List<DataValueDTO> dataValues = new List<DataValueDTO>();

            try
            {
                _valuesFileSemaphore.Wait();

                using (FileStream fs = new FileStream(_valuesPath, FileMode.OpenOrCreate))
                {
                    if (fs.Length > 0)
                    {
                        dataValues = JsonSerializer.Deserialize<List<DataValueDTO>>(fs) ?? new List<DataValueDTO>();
                    }
                }
            }
            finally 
            {
                _valuesFileSemaphore.Release();
            }

            return dataValues;
        }

        public static async Task<List<DataValueDTO>> GetValuesJsonAsync()
        {
            IsJsonFileExist(true);
            List<DataValueDTO> dataValues = new List<DataValueDTO>();

            try
            {
                await _valuesFileSemaphore.WaitAsync(10000);

                using (FileStream fs = new FileStream(_valuesPath, FileMode.OpenOrCreate))
                {
                    if (fs.Length > 0)
                    {
                        dataValues = await JsonSerializer.DeserializeAsync<List<DataValueDTO>>(fs) ?? new List<DataValueDTO>();
                    }
                }
            }
            finally
            {
                _valuesFileSemaphore.Release();
            }

            return dataValues;
        }

        public static async Task Recreate(List<DataValue> dataValues)
        {
            if (dataValues != null)
            {
                IsJsonFileExist(true);

                try
                {
                    await _valuesFileSemaphore.WaitAsync(10000);

                    using (FileStream fileStream = new FileStream(_valuesPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await JsonSerializer.SerializeAsync(fileStream, dataValues.GetDTOs());
                    }
                }
                finally
                {
                    _valuesFileSemaphore.Release();
                }
            }
        }
        
        /// <summary>
        /// Получить содержание пользовательских типов   
        /// </summary>
        public static List<DataType> GetTypes()
        {
            List<DataType> types = new List<DataType>();
            IsJsonFileExist(true);

            try
            {
                _typesFileSemaphore.Wait(10000);

                if (File.Exists(_typesPath))
                {
                    using (var fileStream = new FileStream(_typesPath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        if (fileStream.Length > 0 && false)
                        {
                            types = JsonSerializer.Deserialize<List<DataType>>(fileStream);
                        }
                        else
                        {
                            types = DataType.GetTypes();
                            //JsonSerializer.Serialize(fileStream, types);
                        }
                    }                   
                }
                else
                {
                    types = DataType.GetTypes();
                    using (var fileStream = new FileStream(_typesPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        JsonSerializer.Serialize(fileStream, types);
                    }
                }
            }
            finally
            {
                _typesFileSemaphore.Release();
            }

            return types;
        }

        public static async Task<List<DataType>> GetTypesAsync()
        {
            List<DataType> types = new List<DataType>();
            IsJsonFileExist(true);
            
            try
            {
                await _typesFileSemaphore.WaitAsync(10000);

                if (File.Exists(_typesPath))
                {
                    using (var fileStream = new FileStream(_typesPath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        if (fileStream.Length > 0)
                        {
                            types = await JsonSerializer.DeserializeAsync<List<DataType>>(fileStream);
                        }
                        else
                        {
                            types = DataType.GetTypes();
                            await JsonSerializer.SerializeAsync(fileStream, types);

                        }
                    }
                    return types;
                }
                else
                {
                    types = DataType.GetTypes();
                    using (var fileStream = new FileStream(_typesPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await JsonSerializer.SerializeAsync(fileStream, types);
                    }
                    return types;
                }
            }
            finally
            {
                _typesFileSemaphore.Release();
            }           
        }

        /// <summary>
        /// Получить содержание типов пользовательских типов
        /// </summary>
        public static async Task<List<UserTypeContent>> GetTypesContentAsync()
        {
            List<UserTypeContent> userTypesContents = new List<UserTypeContent>();
            IsJsonFileExist(true);

            try
            {
                await _userFilePathSemaphore.WaitAsync(10000);

                if (File.Exists(_userTypesPath))
                {
                    using (var fileStream = new FileStream(_userTypesPath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        if (fileStream.Length > 0)
                        {
                            userTypesContents = await JsonSerializer.DeserializeAsync<List<UserTypeContent>>(fileStream);
                        }
                        else
                        {
                            userTypesContents = UserTypeContent.GetTypes();
                            await JsonSerializer.SerializeAsync(fileStream, userTypesContents);

                        }
                    }
                    return userTypesContents;
                }
                else
                {
                    userTypesContents = UserTypeContent.GetTypes();
                    using (var fileStream = new FileStream(_userTypesPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await JsonSerializer.SerializeAsync(fileStream, userTypesContents);
                    }
                    return userTypesContents;
                }
            }
            finally
            {
                _userFilePathSemaphore.Release();
            }           
        }
    }
}
