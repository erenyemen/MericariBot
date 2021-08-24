using Dapper;
using MericariBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MericariBot.Helper
{
    public class DataAccess
    {
        DapperRepository repo;

        public DataAccess()
        {
            repo = new DapperRepository();
        }

        public UserLoginResult LoginUser(User user)
        {
            UserLoginResult resultObject = new UserLoginResult() { IsError = false, IsWarning = false};

            var result = SelectQuery<User>($"SP_GetUser('{user.UserName}','{user.Password}')").FirstOrDefault();

            if (result is null)
            {
                resultObject.IsError = true;
                resultObject.ErrorMessage = "Username or Password is Incorrect";
                return resultObject;
            }

            if (!result.IsActive)
            {
                resultObject.IsError = true;
                resultObject.ErrorMessage = "User is not Active";
                return resultObject;
            }

            if (result.IsFirstLogin)
            {
                var res = UpdateUserToFirstLogin(result.UserId, user.IPAddress, user.MACAddress);

                if (res == -1)
                {
                    resultObject.IsError = true;
                    resultObject.ErrorMessage = "Failed to Update User Login Information";
                    return resultObject;
                }
            }
            else
            {
                if (user.MACAddress != result.MACAddress)
                {
                    resultObject.IsWarning = true;
                    resultObject.ErrorMessage = "Cannot be Accessed From an Unidentified Device !";
                    return resultObject;
                }
            }

            resultObject.LoginedUser = result;
            return resultObject;
        }

        public List<User> GetUsers()
        {
            var result = SelectQuery<User>("SELECT * FROM User").ToList();

            return result;
        }

        public User GetUserById(int userId)
        {
            var result = SelectQuery<User>($"SELECT * FROM User WHERE User.UserId = {userId}").FirstOrDefault();

            return result;
        }

        public int AddUser(User user)
        {
            var query = $"INSERT INTO User (UserName,Password,IPAddress,MACAddress,IsActive,Role,IsFirstLogin) VALUES ('{user.UserName}','{user.Password}','{user.IPAddress}','{user.MACAddress}',{user.IsActive},'{user.Role.ToString()}',1)";

            var result = ExecuteQuery(query);

            return result;
        }

        public int UpdateUser(User user)
        {
            var result = ExecuteQuery($"UPDATE User SET UserName='{user.UserName}',Password='{user.Password}', IPAddress='{user.IPAddress}', MACAddress='{user.MACAddress}', IsActive={user.IsActive}, Role='{user.Role.ToString()}' WHERE UserId = {user.UserId}");

            return result;
        }

        public int SaveUser(User user)
        {
            if (user.UserId == 0)
            {
                return AddUser(user);
            }
            else
            {
                return UpdateUser(user);
            }
        }

        public int UpdateUserToFirstLogin(int userId, string ipAddress, string macAddress)
        {
            var result = ExecuteQuery($"UPDATE User SET IsFirstLogin={0}, IPAddress='{ipAddress}', MACAddress='{macAddress}' WHERE UserId = {userId}");

            return result;
        }

        public int DeleteUser(int userId)
        {
            var result = ExecuteQuery($"DELETE FROM User WHERE User.UserId={userId}");

            return result;
        }

        private IEnumerable<T> SelectQuery<T>(string query)
        {
            try
            {
                return repo.Connection.Query<T>(query);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }
        }

        private int ExecuteQuery(string query)
        {
            try
            {
                return repo.Connection.Execute(query);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return -1;
            }
        }
    }
}
