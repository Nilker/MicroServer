using System;
using System.Collections.Generic;
using System.Text;
using Abc.Model;

namespace Abc.Interface
{
    public interface IUserService
    {
        User FindUser(int id);
        IEnumerable<User> UserAll();
        bool Authenticate(string userName, string password, out User loginUser);
    }
}
