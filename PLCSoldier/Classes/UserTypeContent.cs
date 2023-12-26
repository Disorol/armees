using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PLCSoldier.Enums;

namespace PLCSoldier.Classes
{
    public class UserTypeContent
    {
        public int TypeContentId { get; set; }
        public List<DataType> AvailableTypes { get; set; }
        //тестовые данные
        public static List<UserTypeContent> GetTypes()
        {
            List<UserTypeContent> _userTypesContents = new List<UserTypeContent>()
            {
                new UserTypeContent(){TypeContentId = 4, AvailableTypes = 
                new List<DataType>(){
                    DataType.GetValueType(EDataType.BOOL)!,
                    DataType.GetValueType(EDataType.INT)!,
                }}, //CustomTitle содержит BOOL, INT
            };
            return _userTypesContents;
        }
        //получить данные из файла
        public static async Task<List<UserTypeContent>> GetValueUserTypes(int id)
        {
            List<UserTypeContent> valueTypes = new List<UserTypeContent>();
            valueTypes = await ObservableCollection.GetTypesContentAsync();
            return valueTypes.Where(x => x.TypeContentId == id).ToList();
        }
    }
}
