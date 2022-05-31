using Laybuy.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laybuy.Models.User
{
    class UsersResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }    
        public string Phone { get; set; }
        public string Website { get; set; }
        public Company Company { get; set; }
    }
}
