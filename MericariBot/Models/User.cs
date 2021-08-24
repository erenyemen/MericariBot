using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MericariBot.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IPAddress { get; set; }
        public string MACAddress { get; set; }
        public bool IsActive { get; set; }
        public UserRole Role { get; set; }
        public bool IsFirstLogin { get; set; }
    }

    public enum UserRole
    {
        User,
        Admin
    }

    public class UserLoginResult
    {
        public bool IsError { get; set; }

        public bool IsWarning { get; set; }

        public string ErrorMessage { get; set; }

        public User LoginedUser { get; set; }
    }
}
