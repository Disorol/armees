using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSoldier.Enums;

namespace PLCSoldier.Services
{
    public interface INotificationService
    {
        void Notify(string message, ENotificationType notificationType);
        Task NotifyAsync(string message, ENotificationType notificationType);
    }
}
