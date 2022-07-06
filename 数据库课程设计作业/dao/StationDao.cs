using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 数据库课程设计作业.domain;

namespace 数据库课程设计作业.dao
{
    public class StationDao
    {
        private Locomotive_systemEntities conn = new Locomotive_systemEntities();


        public List<Station> GetStations()
        {
            string sql = "select * from station";
            var list = conn.Database.SqlQuery<Station>(sql);
            return list.ToList();
        }

        public int deleteStation(String name)
        {
            String sql = "delete from station where snname={0}";
            return conn.Database.ExecuteSqlCommand(sql, name);
        }

        public int addStation(String name,String level)
        {
            String sql = "insert into station values({0},{1})";
            return conn.Database.ExecuteSqlCommand(sql, name, level);
        }
    }
}
