using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using 数据库课程设计作业.domain;

namespace 数据库课程设计作业.dao
{
    public class UserDao
    {
        private Locomotive_systemEntities conn= new Locomotive_systemEntities();
        public Users findUser(Users loginUser)
        {
            String sql = "select * from users where uemail= {0} and upassword = {1}";
            var list=conn.Database.SqlQuery<Users>(sql, loginUser.uemail, loginUser.upassword);
            if (list != null && list.Count() > 0) 
            {
                var x=list.ToList<Users>();
                return x[0];
            } 
            return null;
        }

        public List<Users> getUsers()
        {
            String sql = "select * from users";
            var list = conn.Database.SqlQuery<Users>(sql).ToList();
            return list;
        }

        public Users getUserById(int id)
        {
            String sql = "select * from users where uid ={0}";
            var list = conn.Database.SqlQuery<Users>(sql, id).ToList();
            if(list!=null && list.Count() > 0)
            {
                return list[0];
            }
            return null;
        }

        public Users addUser(Users newUser)
        {
            var res=conn.Users.Add(newUser);
            conn.SaveChanges();//提交事务
            return res;
        }

        public bool removeUser(int uid)
        {
            String sql = "delete from users where uid={0}";
            var ls=conn.Database.ExecuteSqlCommand(sql,uid);
            return ls == 0 ? false : true;
        }

        public Users modifyUser(Users user)
        {
            int uid = user.uid;
            var olduser=conn.Users.Find(uid);
            if (olduser != null) 
            {
                olduser.uid = user.uid;
                olduser.upassword = user.upassword;
                olduser.uemail = user.uemail;
                olduser.uphoto = user.uphoto;
                conn.SaveChanges();
                return olduser;
            }
            else
            {
                return null;
            }
            
        }
    }
}
