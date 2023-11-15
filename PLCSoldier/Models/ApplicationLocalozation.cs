using Avalonia.Controls.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Models
{
    public class ApplicationLocalozation
    {
        /*
            0 - russian
            1 - english
        */

        public static List<string> ApplicationTitle = new List<string> { "ПЛК Армеец", "PLC Soldier" };

        public static Dictionary<string, List<string>> TopMenuTranslations = new Dictionary<string, List<string>>
        {
            {"File", new List<string>() { "Файл", "File" } },
            {"New project", new List<string>() { "Новый проект", "New project" } },
            {"Open project", new List<string>() { "Открыть проект", "Open project" } },
            {"Settings", new List<string>() { "Настройки", "Settings" } },
            {"Language", new List<string>() { "Язык", "Language" } },
            {"Russian", new List<string>() { "Русский", "Russian" } },
            {"English", new List<string>() { "Английский", "English" } },
            {"Exit", new List<string>() { "Выход", "Exit" } },
            {"Edit", new List<string>() { "Редактировать", "Edit" } },
            {"View", new List<string>() { "Вид", "View" } },
            {"Logical organizer", new List<string>() { "Логический органайзер", "Logical organizer" } },
            {"Hardware organizer", new List<string>() { "Аппаратный органайзер", "Hardware organizer" } },
            {"Errors", new List<string>() { "Ошибки", "Errors" } },
            {"Search results", new List<string>() { "Поиск результатов", "Search results" } },
            {"Watch", new List<string>() { "Просмотр", "Watch" } },
            {"Workspace", new List<string>() { "Рабочая область", "Workspace" } },
            {"Property", new List<string>() { "Свойства", "Property" } },
            {"Search", new List<string>() { "Поиск", "Search" } },
            {"Logic", new List<string>() { "Логика", "Logic" } },
            {"Communications", new List<string>() { "Коммуникации", "Communications" } },
            {"Tools", new List<string>() { "Инструменты", "Tools" } },
            {"Window", new List<string>() { "Окно", "Window" } },
            {"Help", new List<string>() { "Помощь", "Help" } },
        };

        public static Dictionary<int, List<BottomTabItem>> BottomItemsTranslations = new Dictionary<int, List<BottomTabItem>>
        {
            {0, new List<BottomTabItem>() { new BottomTabItem() { Content = "какой-то текст", Header = "Ошибки" }, new BottomTabItem() { Content = "some text", Header = "Errors" }  } },
            {1, new List<BottomTabItem>() { new BottomTabItem() { Content = "какой-то текст", Header = "Поиск результатов" }, new BottomTabItem() { Content = "some text", Header = "Search results" }  } },
            {2, new List<BottomTabItem>() { new BottomTabItem() { Content = "какой-то текст", Header = "Просмотр" }, new BottomTabItem() { Content = "some text", Header = "Watch" }  } },
        };

        public static Dictionary<int, List<LeftUpperTabItem>> LeftUpperItemsTranslations = new Dictionary<int, List<LeftUpperTabItem>>
        {
            {0, new List<LeftUpperTabItem>() { new LeftUpperTabItem() { Header = "Логический органайзер" }, new LeftUpperTabItem() { Header = "Logical organizer" }  } },
        };

        public static Dictionary<int, List<LeftBottomTabItem>> LeftBottomItemsTranslations = new Dictionary<int, List<LeftBottomTabItem>>
        {
            {0, new List<LeftBottomTabItem>() { new LeftBottomTabItem() { Header = "Аппаратный органайзер", Content = "какой-то текст" }, new LeftBottomTabItem() { Header = "Hardware organizer", Content = "some text" }  } },
        };

        public static Dictionary<int, List<FarRightTabItem>> FarRightItemsTranslations = new Dictionary<int, List<FarRightTabItem>>
        {
            {0, new List<FarRightTabItem>() { new FarRightTabItem() { Content = "какой-то текст", Header = "Свойства" }, new FarRightTabItem() { Content = "some text", Header = "Property" }  } },
        };

        public static Dictionary<int, List<CentralTabItem>> CentralItemsTranslations = new Dictionary<int, List<CentralTabItem>>
        {
            {0, new List<CentralTabItem>() { new CentralTabItem() { Content = "какой-то текст", Header = "Рабочая область" }, new CentralTabItem() { Content = "some text", Header = "Workspace" }  } },
            {1, new List<CentralTabItem>() { new CentralTabItem() { Header = "Редактор переменных" }, new CentralTabItem() { Header = "Variable editor" }  } },
        };

        public static List<ToolToolTip> ToolToolTipTranslations = new List<ToolToolTip>()
        {
            new ToolToolTip()
            {
                TooltipA = "Всплывающая подсказка 1",
                TooltipB = "Всплывающая подсказка 2",
                TooltipC = "Всплывающая подсказка 3",
                TooltipD = "Всплывающая подсказка 4",
                TooltipE = "Всплывающая подсказка 5",
                TooltipF = "Всплывающая подсказка 6",
                TooltipG = "Всплывающая подсказка 7",
                TooltipH = "Всплывающая подсказка 8",
                TooltipI = "Всплывающая подсказка 9"
            },

            new ToolToolTip()
            {
                TooltipA = "Popup hint 1",
                TooltipB = "Popup hint 2",
                TooltipC = "Popup hint 3",
                TooltipD = "Popup hint 4",
                TooltipE = "Popup hint 5",
                TooltipF = "Popup hint 6",
                TooltipG = "Popup hint 7",
                TooltipH = "Popup hint 8",
                TooltipI = "Popup hint 9"
            },
        };

        public static int GetLanguageIndex(string language)
        {
            switch (language)
            {
                case "russian":
                    return 0;
                case "english":
                    return 1;
                default:
                    return -1;
            }
        }
    }
}
