using Abc.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abc.Model;

namespace Abc.Service
{
    public class UserService : IUserService
    {
        #region Init
        private List<User> _userList=new List<User>()
        {
            new User()
            {
                Id = 1,
                Account = "111",
                Email = "111@qq.com",
                Name="第1名",
                Password = "111111",
                LoginTime = DateTime.Now,
                Role = "Admin"
            },
            new User()
            {
                Id = 2,
                Account = "222",
                Email = "222@qq.com",
                Name="第2名",
                Password = "222222",
                LoginTime = DateTime.Now,
                Role = "Admin"
            },  new User()
            {
                Id = 3,
                Account = "333",
                Email = "333@qq.com",
                Name="第3名",
                Password = "333333",
                LoginTime = DateTime.Now,
                Role = "Admin"
            },

        };
        #endregion


        public User FindUser(int id)
        {
            return _userList.Find(s => s.Id == id);
        }

        public IEnumerable<User> UserAll()
        {
            return _userList;
        }

        public bool Authenticate(string userName, string password, out User loginUser)
        {
            var model = _userList.Where(s => s.Account == userName && s.Password == password).FirstOrDefault();
            loginUser = model;
            return model!=null;
        }
    }
}
