using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 数据库课程设计作业.dao;
using 数据库课程设计作业.domain;

namespace 数据库课程设计作业.service
{
    public class UserService
    {
        private UserDao dao = new UserDao();
        public Users login(Users loginUser)
        {
            var user = dao.findUser(loginUser);
            if(user != null)
            {
                return user;
            }
            return null;
        }

        public Users register(Users u)
        {
            return dao.addUser(u);
        }

        public Users modifyUser(Users user)
        {
            return dao.modifyUser(user);
        }

        public List<Users> getUsers()
        {
            return dao.getUsers();
        }

        public Users getUserById(int id)
        { 
            return dao.getUserById(id); 
        }
    } 
}
