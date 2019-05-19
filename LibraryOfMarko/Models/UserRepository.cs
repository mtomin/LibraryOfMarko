using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibraryOfMarko.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _config;
        private readonly string connectionString;

        public UserRepository(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetConnectionString("LibraryDB").Replace("%DataDirectory%", Environment.CurrentDirectory);
        }
        public void AddUser(User newUser)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                newUser.ID = db.Query<int>("AddUser @FirstName, @LastName, @Address, @Email", newUser).Single();
            }
        }

        public void EditUser(User editedUser)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("EditUser", editedUser, commandType: CommandType.StoredProcedure);
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> userList = new List<User>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                userList = db.Query<User>("AllUsers").ToList();
            }
            return userList;
        }

        public User GetUser(int UserId)
        {
            User user = new User();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                user = db.Query<User>("GetUser", new { @Id = UserId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
            return user;
        }

        public List<UserData> LoadUserData(int userID)
        {
            List<UserData> userData = new List<UserData>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                userData = db.Query<UserData>("LoadUserData", new { @UserId = userID }, commandType: CommandType.StoredProcedure).ToList();
            }
            return userData;
        }

        public void RemoveUser(int userID)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("RemoveUser", new { Id = userID }, commandType: CommandType.StoredProcedure);
            }
        }
        public List<User> SearchUser(string query)
        {
            List<User> users = new List<User>();
            if (query != null)
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    users = db.Query<User>("SearchUsers", new { query = query.ToLower() }, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            return users;
        }
    }
}
