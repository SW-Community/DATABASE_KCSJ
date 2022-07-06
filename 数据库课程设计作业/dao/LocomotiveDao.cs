using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 数据库课程设计作业.domain;

namespace 数据库课程设计作业.dao
{
    public class LocomotiveDao
    {
        private Locomotive_systemEntities conn = new Locomotive_systemEntities();
        public List<Locomotive> findLocosBySeries(string series)
        {
            string sql = "select * from locomotive whhere lname like {0}";
            var list = conn.Database.SqlQuery<Locomotive>(sql,series+"%");
            return list.ToList();
        }

        public List<Locomotive> findAllLocos()
        {
            string sql = "select * from locomotive";
            var list = conn.Database.SqlQuery<Locomotive>(sql);
            return list.ToList();
        }

        public Locomotive addALoco(Locomotive locomotive)
        {
            int oldlen = conn.Locomotive.Count();
            var x = conn.Locomotive.Add(locomotive);
            conn.SaveChanges();
            return x;
        }

        public void updateALoco(string lname, string ljv, string lduan, byte[] lphoto)
        {
            Locomotive l = conn.Locomotive.Find(lname);
            l.ljv = ljv;
            l.lduan = lduan;
            l.lphoto = lphoto;
            conn.SaveChanges();
            return;
        }
    }
}
