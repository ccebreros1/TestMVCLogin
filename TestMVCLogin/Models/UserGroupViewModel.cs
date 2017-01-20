using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVCLogin.Models
{
    public class GroupedUserViewModel
    {
        public List<UserGroupViewModel> Users { get; set; }
    }
    public class UserGroupViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}