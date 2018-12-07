using Organizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Interfaces
{
    public interface IMessagesService
    {
        List<MessageConversationRequest> GetConversationsById(int userId);
        List<MessageGetRequest> GetConversationByIds(int senderId, int receiverId);
        int Create(MessageCreateRequest message);
        void UpdateRead(int id);
        MutualFistBump IsFistBumped(int user1, int user2);
        int GetGradYearByUserId(int id);
        List<MutualFistBumpUser> GetAllFistBumped(int id);
    }
}
