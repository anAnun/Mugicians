using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Interfaces
{
    public interface INotificationSender
    {
        void SendNotificationsToUser(int userId, string notifications);
    }
}
