using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organizer.Models;

namespace Organizer.Interfaces
{
    public interface INotificationsService
    {
        string GetNotificationByUserId(int userId);
        void Delete(int id, int userId);
        int Create(NotificationCreateRequest model);
        bool NotificationExists(string type, int userId, int senderId);
    }
}
