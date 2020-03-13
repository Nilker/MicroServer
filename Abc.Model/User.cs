using System;
using System.Collections.Generic;
using System.Text;

namespace Abc.Model
{
    public class User
    {
        public int Id { get; set; }

        public string  Account { get; set; }

        public string Name { get; set; }

        public string  Role { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime LoginTime { get; set; }
    }
}
