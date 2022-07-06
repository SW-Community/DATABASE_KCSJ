using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 数据库课程设计作业.domain;

namespace 数据库课程设计作业.dao
{
    public class RevolveDao
    {
        private Locomotive_systemEntities conn = new Locomotive_systemEntities();

        public List<Yunzhuan> GetYunzhuans()
        {
            string sql = "select * from yunzhuan";
            var list = conn.Database.SqlQuery<Yunzhuan>(sql);
            return list.ToList();
        }

        public List<Yunzhuan> GetYunzhuansByTname(String tname)
        {
            string sql = "select * from yunzhuan where tname={0}";
            var list = conn.Database.SqlQuery<Yunzhuan>(sql,tname);
            return list.ToList();
        }

        public int addYunZhuan(String tname,String snname,Nullable<TimeSpan> yzarrive, Nullable<TimeSpan> yzleave)
        {
            String sql = "insert into yunzhuan values({0},{1},{2},{3})";
            return conn.Database.ExecuteSqlCommand(sql, tname, snname, yzarrive, yzleave);  
        }  
        
        public int deleteYunzhuan(String tname, String snname, Nullable<TimeSpan> yzarrive, Nullable<TimeSpan> yzleave)
        {

            String sql = "delete from yunzhuan where tname={0} and snname={1} and yzarrive={2} and yzleave={3}";
            return conn.Database.ExecuteSqlCommand(sql, tname, snname, yzarrive, yzleave);
        }
    }
}
