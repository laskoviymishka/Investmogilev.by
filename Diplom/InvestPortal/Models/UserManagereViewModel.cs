using System;
using System.Collections.Generic;

namespace InvestPortal.Models
{
    public class UserManagerViewModel
    {
        private IList<UserRoles> _roles;

        public string _id { get; set; }

        public string Username { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsLockedOut { get; set; }

        public bool IsApproved { get; set; }

        public string LoweredEmail { get; set; }

        public string Password { get; set; }

        public string PasswordQuestion { get; set; }

        public string PasswordAnswer { get; set; }

        public IList<UserRoles> Roles
        {
            get
            {
                if (_roles == null)
                {
                    return new List<UserRoles>(){ UserRoles.User};
                }

                return _roles;
            }
            set { _roles = value; }
        }
    }

    public enum UserRoles
    {
        Admin,
        User,
        Investor
    }
}