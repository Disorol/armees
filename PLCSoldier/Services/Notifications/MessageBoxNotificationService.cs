using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSoldier.Enums;

namespace PLCSoldier.Services
{
    public class MessageBoxNotificationService : INotificationService
    {
        public void Notify(string message, ENotificationType notificationType)
        {
            string title = GetTitle(notificationType);

            IMsBox<ButtonResult> messageBox = MessageBoxManager.GetMessageBoxStandard(title, message, MessageBox.Avalonia.Enums.ButtonEnum.Ok);
            messageBox.ShowAsync();
        }

        public async Task NotifyAsync(string message, ENotificationType notificationType)
        {
            string title = GetTitle(notificationType);

            IMsBox<ButtonResult> messageBox = MessageBoxManager.GetMessageBoxStandard(title, message, MessageBox.Avalonia.Enums.ButtonEnum.Ok);
            await messageBox.ShowAsync();
        }

        private string GetTitle(ENotificationType notificationType)
        {
            switch (notificationType)
            {
                case ENotificationType.Info:
                    return string.Empty;
                case ENotificationType.Success:
                    return "Успешно";
                case ENotificationType.Error:
                    return "Ошибка";
                case ENotificationType.Warning:
                    return "Внимание";
                case ENotificationType.Critical:
                    return "Фатальная ошибка";
                default:
                    return string.Empty;
            }
        }
    }
}
