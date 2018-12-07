using Organizer.Interfaces;
using Organizer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Organizer.Services
{
    public class MessagesService : IMessagesService
    {
        readonly IDataProvider dataProvider;

        public MessagesService(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        public List<MessageConversationRequest> GetConversationsById(int userId)
        {
            List<MessageConversationRequest> results = new List<MessageConversationRequest>();

            dataProvider.ExecuteCmd(
                "Messages_GetConversationsById",
                inputParamMapper: param =>
                {
                    param.AddWithValue("@UserId", userId);
                },
                singleRecordMapper: (reader, resultSetNumber) =>
                {
                    MessageConversationRequest conversation = new MessageConversationRequest();
                    conversation.OtherUserId = (int)reader["OtherUserId"];
                    conversation.OtherUserName = (string)reader["OtherUserName"];
                    conversation.UnreadMsgCount = (int)reader["UnreadMsgCount"];
                    conversation.LastMsgDate = (DateTime)reader["LastMsgDate"];

                    results.Add(conversation);
                });

            return results;
        }

        public List<MessageGetRequest> GetConversationByIds(int senderId, int receiverId)
        {
            List<MessageGetRequest> results = new List<MessageGetRequest>();

            dataProvider.ExecuteCmd(
                "Messages_GetConversationByIds",
                inputParamMapper: param =>
                {
                    param.AddWithValue("@SenderId", senderId);
                    param.AddWithValue("@ReceiverId", receiverId);
                },
                singleRecordMapper: (reader, resultSetNumber) =>
                {
                    MessageGetRequest message = new MessageGetRequest();
                    message.Id = (int)reader["Id"];
                    message.Message = (string)reader["Message"];
                    message.SenderId = (int)reader["SenderId"];
                    message.SenderName = (string)reader["SenderName"];
                    message.ReceiverId = (int)reader["ReceiverId"];
                    message.ReceiverName = (string)reader["ReceiverName"];
                    message.DateRead = reader.GetSafeDateTimeNullable("DateRead");
                    message.DateCreated = (DateTime)reader["DateCreated"];

                    results.Add(message);
                });

            return results;
        }

        public int Create(MessageCreateRequest message)
        {
            int id = new int();

            dataProvider.ExecuteNonQuery(
                "Messages_Create",
                inputParamMapper: param =>
                {
                    param.AddWithValue("@Message", message.Message);
                    param.AddWithValue("@SenderId", message.SenderId);
                    param.AddWithValue("@ReceiverId", message.ReceiverId);

                    param.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                },
                returnParameters: param =>
                {
                    id = (int)param["@Id"].Value;
                });

            return id;
        }

        public void UpdateRead(int id)
        {
            dataProvider.ExecuteNonQuery(
                "Messages_UpdateRead",
                inputParamMapper: param =>
                {
                    param.AddWithValue("@Id", id);
                }
                );
        }

        public MutualFistBump IsFistBumped(int user1, int user2)
        {
            MutualFistBump fistBumped = new MutualFistBump();

            dataProvider.ExecuteCmd(
                "UserConnections_IsFistBumped",
                inputParamMapper: param =>
                {
                    param.AddWithValue("@User1", user1);
                    param.AddWithValue("@User2", user2);
                },
                singleRecordMapper: (reader, resultSetNumber) =>
                {
                    fistBumped.User1BumpedUser2 = (bool)reader["User1BumpedUser2"];
                    fistBumped.User2BumpedUser1 = (bool)reader["User2BumpedUser1"];
                });

            return fistBumped;
        }

        public int GetGradYearByUserId(int id)
        {
            int gradYear = new int();

            dataProvider.ExecuteCmd(
                "Athletes_GetGradYearByUserId",
                inputParamMapper: param =>
                {
                    param.AddWithValue("@Id", id);
                },
                singleRecordMapper: (reader, resultSetNumber) =>
                {
                    gradYear = (int)reader["GraduationYear"];
                });

            return gradYear;
        }

        public List<MutualFistBumpUser> GetAllFistBumped(int id)
        {
            List<MutualFistBumpUser> fistBumpedUsers = new List<MutualFistBumpUser>();

            dataProvider.ExecuteCmd(
                "UserConnections_GetAllFistBumped",
                inputParamMapper: param =>
                {
                    param.AddWithValue("@Id", id);
                },
                singleRecordMapper: (reader, resultSetNumber) =>
                {
                    MutualFistBumpUser fistBumpedUser = new MutualFistBumpUser();
                    fistBumpedUser.OtherUserId = (int)reader["ReceiverId"];
                    fistBumpedUser.OtherUserName = (string)reader["FullName"];

                    fistBumpedUsers.Add(fistBumpedUser);
                });

            return fistBumpedUsers;
        }
    }
}