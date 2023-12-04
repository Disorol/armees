using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Services
{
    public class NotificationValidationErrorHandler : IValidationErrorHandler
    {
        private readonly INotificationService _notificationService;

        public NotificationValidationErrorHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void Handle(string message)
        {
            _notificationService.Notify(message, Enums.ENotificationType.Error);
        }

        public async Task HandleAsync(string message)
        {
            await _notificationService.NotifyAsync(message, Enums.ENotificationType.Error);
        }
    }
}
