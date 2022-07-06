using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 数据库课程设计作业.domain;

namespace 数据库课程设计作业.dao
{
    public class TrainDao
    {
        private Locomotive_systemEntities conn=new Locomotive_systemEntities();
        
        public List<Liechegaikuang> getAllTrains()
        {
            string sql = "select * from Liechegaikuang";
            var list = conn.Database.SqlQuery<Liechegaikuang>(sql);
            return list.ToList();
        }

        public List<Act> GetActs()
        {
            String sql = "select * from act order by adate desc";
            var list = conn.Database.SqlQuery<Act>(sql);
            return list.ToList();
        }

        public int deleteAct(String lname,String tname, DateTime adate)
        {
            string sql = "delete from act where lname={0} and tname={1} and adate={2}";
            return conn.Database.ExecuteSqlCommand(sql,lname,tname,adate);
        }

        public int addTrain(String tname,String tcaptain,String tstart,String tend)
        {
            String sql = "insert into train values({0},{1},{2},{3})";
            return conn.Database.ExecuteSqlCommand(sql, tname, tcaptain, tstart, tend);
        }

        public int addAct(String lname,String tname,String ldeiver,DateTime adate)
        {
            String sql = "insert into act values({0},{1},{2},{3})";
            return conn.Database.ExecuteSqlCommand(sql,adate,lname,tname,ldeiver);
        }

        public List<Liechegaikuang> findTrainByName(string name)
        {
            string sql = "select * from Liechegaikuang where tname={0}";
            var list = conn.Database.SqlQuery<Liechegaikuang>(sql, name);
            return list.ToList();
        }

        public List<Liechegaikuang> findTrainByStation(string station)
        {
            string sql = 
                "select * from Liechegaikuang " +
                "where Liechegaikuang.tname in " +
                "(select tname from  Yunzhuan " +
                "where yunzhuan.snname={0})";
            var list = conn.Database.SqlQuery<Liechegaikuang>(sql, station);
            return list.ToList();
        }

        public List<Liechegaikuang> findTrainByLocomotive(string locomotive)
        {
            string sql = "select * from Liechegaikuang where lname={0}";
            var list = conn.Database.SqlQuery<Liechegaikuang>(sql, locomotive);
            return list.ToList();
        }

        public List<Liecheshikebiao> queryForSchedule(string name)
        {
            string sql = "select * from Liecheshikebiao where tname={0}";
            var list=conn.Database.SqlQuery<Liecheshikebiao>(sql, name);
            return list.ToList();
        }


        public List<Liechegaikuang> findTrainsByStartPlace(string startPlace)
        {
            string sql = "select * from liechegaikuang where tname in (select tname from yunzhuan where snname={0} and yzleave is not null)";
            var ol = conn.Database.SqlQuery<Liechegaikuang>(sql, startPlace);
            if (ol == null)
            {
                return null; //没车
            }
            return ol.ToList();
        }

        public List<Liechegaikuang> findTrainsByEndPlace(string endPlace)
        {
            string sql = "select * from liechegaikuang where tname in (select tname from yunzhuan where snname={0} and yzarrive is not null)";
            var ol = conn.Database.SqlQuery<Liechegaikuang>(sql, endPlace);
            if (ol == null)
            {
                return null; //没车
            }
            return ol.ToList();
        }
    }
}
