using Organizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Interfaces
{
    public interface IUsersService
    {
        bool LogIn(UserLogInRequest model);
        List<User> GetAll();
        int Create(UserCreateRequest model);
        void Update(UserUpdateRequest model);
        void Delete(int id);
        User GetById(int id);
        string GetProfileById(int id, int viewersUserId);
        void LogOut();
    }
}
