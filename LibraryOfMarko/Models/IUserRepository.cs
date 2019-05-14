using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryOfMarko.Models
{
    public interface IUserRepository
    {
        User GetUser(int id);
        List<User> GetAllUsers();
        void AddUser(User newUser);
        void RemoveUser(int userID);
        void EditUser(User editedUser);
        List<User> SearchUser(string query);
        List<UserData> LoadUserData(int userID);
    }
}
