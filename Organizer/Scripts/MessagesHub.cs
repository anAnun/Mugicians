using Microsoft.AspNet.SignalR;
using Organizer.Data;
using Organizer.Interfaces;
using Organizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;

namespace Organizer.Scripts
{
    [Authorize]
    public class MessagesHub : Hub
    {
        readonly IMessagesService messagesService;
        readonly INotificationsService notificationsService;

        public MessagesHub(IMessagesService messagesService, INotificationsService notificationsService)
        {
            this.messagesService = messagesService;
            this.notificationsService = notificationsService;
        }

        public int GetUserId()
        {
            int userId = Context.User.Identity.GetId().Value;
            return userId;
        }

        private readonly static ConnectionMapping<int> _connections =
            new ConnectionMapping<int>();

        public void SendMessage(int receiverId, string senderName, string message)
        {
            MessageCreateRequest newMessage = new MessageCreateRequest();
            newMessage.SenderId = GetUserId();
            newMessage.ReceiverId = receiverId;
            newMessage.Message = message;

            int messageId = messagesService.Create(newMessage);

            var connections = _connections.GetConnections(receiverId);

            if (connections == null)
            {
                bool notificationExists = notificationsService.NotificationExists("Message", receiverId, newMessage.SenderId);

                if (!notificationExists)
                {
                    NotificationCreateRequest notification = new NotificationCreateRequest();
                    notification.UserId = receiverId;

                    var v = new { type = "message", fromUserId = newMessage.SenderId, fromName = senderName };
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string toJSON = serializer.Serialize(v);

                    notification.NotificationJson = toJSON;

                    int notificationId = notificationsService.Create(notification);
                }
            }
            else
            {
                foreach (var connectionId in connections)
                {
                    Clients.Client(connectionId).sendMessage(newMessage, messageId);
                }
            }
        }

        public override Task OnConnected()
        {
            int userId = GetUserId();

            _connections.Add(userId, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            int userId = GetUserId();

            _connections.Remove(userId, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            int userId = GetUserId();

            if (!_connections.GetConnections(userId).Contains(Context.ConnectionId))
            {
                _connections.Add(userId, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}