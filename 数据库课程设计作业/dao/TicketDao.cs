using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using 数据库课程设计作业.domain;

namespace 数据库课程设计作业.dao
{
    public class TicketDao
    {
        private Locomotive_systemEntities conn = new Locomotive_systemEntities();
        
        public List<Availableseats> findSeatsOnATrain(string name)
        {
            //修复了日期功能后的可用车票查询代码
            //先查所有的座位，然后交给服务层筛选可用的
            string sql = "select * from Availableseats where tname={0}";
            var list = conn.Database.SqlQuery<Availableseats>(sql, name);
            return list.ToList();
        }

        public Seat findSeatByLocation(string tname,int cnum,int snum)
        {
            string sql = "select * from seat where cno={0} and cid=(select cid from car where cnum={1} and tname={2})";
            var x = conn.Database.SqlQuery<Seat>(sql, snum, cnum, tname).ToList();
            return x.Count > 0 ? x[0] : null;
        }

        public Orders getOrder(DateTime odate, string zffs, int uid, int zje)
        {
            string sql = "insert into Orders(odate,zffs,uid,zje) values({0},{1},{2},{3})";
            var lines = conn.Database.ExecuteSqlCommand(sql, odate, zffs, uid, zje);
            if (lines > 0)
            {
                string sql2 = "select * from Orders where odate={0} and zffs={1} and uid={2} and zje={3}";
                //垂死病中惊坐起，小丑竟是我自己。。。
                var list = conn.Database.SqlQuery<Orders>(sql2, odate, zffs, uid, zje);
                return list.ToList()[0];
            }
            else
            {
                MessageBox.Show("ERROR!");
            }
            return null;
        }

        public Orderdetail findDetailById(int odid)
        {
            string sql = "select* from orderdetail where odid={0}";
            var list = conn.Database.SqlQuery<Orderdetail>(sql, odid).ToList();
            return list[0];
        }

        public int addTickets(List<Orderdetail> list, int oid)
        {
            foreach(var order in list)
            {
                order.oid = oid;//设置订单ID
            }
            int old = conn.Orderdetail.Count();
            conn.Orderdetail.AddRange(list);//没用SQL，直接ADO.NET
            conn.SaveChanges();
            return conn.Orderdetail.Count() - old;
        }

        //public int setStatusOfSeats(int seid, bool isused)
        //{
        //    string status = isused ? "是" : "否";
        //    string sql = "update Seat set isused={0} where seid={1}";
        //    return conn.Database.ExecuteSqlCommand(sql, status, seid);
        //}

        public Chepiao findChepiaoById(int id)
        {
            string sql = "select * from chepiao where odid={0}";
            var list=conn.Database.SqlQuery<Chepiao>(sql, id).ToList();
            return list[0];
        }

        public List<Chepiao> findChepiaosByDate(DateTime start, DateTime end)
        {
            String sql = "select * from chepiao where takedate >= {0} and takedate <= {1}";
            var list = conn.Database.SqlQuery<Chepiao>(sql, start, end).ToList();
            return list;
        }

        public List<Chepiao> findChepiaos()
        {
            String sql = "select * from chepiao";
            var list = conn.Database.SqlQuery<Chepiao>(sql).ToList();
            return list;
        }

        public int getTicketCount(int id)
        {
            String sql = "select count(*) from chepiao where uid={0}";
            return conn.Database.SqlQuery<Int32>(sql,id).ToList()[0];
        }

        public int getTicketCountBySStation(String station, DateTime start, DateTime end)
        {
            String sql = "select count(*) from chepiao where takestart={0} and takedate >= {1} and takedate <={2}";
            return conn.Database.SqlQuery<Int32>(sql,station,start,end).ToList()[0];
        }

        public int getTicketCountByEStation(String station, DateTime start, DateTime end)
        {
            String sql = "select count(*) from chepiao where takeend={0} and takedate>={1} and takedate <={2}";
            return conn.Database.SqlQuery<Int32>(sql, station, start, end).ToList()[0];
        }

        public int getTicketCountByCC(String tname, DateTime start, DateTime end)
        {
            String sql = "select count(*) from chepiao where tname={0} and takedate>={1} and takedate <={2}";
            return conn.Database.SqlQuery<Int32>(sql, tname, start, end).ToList()[0];
        }

        public int getOrderCount(int id)
        {
            String sql = "select count(*) from orders where uid={0}";
            return conn.Database.SqlQuery<Int32>(sql, id).ToList()[0];
        }

        public List<Chepiao> findChepiaosByUserID(int id)
        {
            string sql = "select * from chepiao where uid={0}";
            var list = conn.Database.SqlQuery<Chepiao>(sql, id).ToList();
            return list;
        }

        public List<Orderdetail> findDetailsByDateAndSeat(DateTime takedate, int seid)
        {
            string sql = "select * from orderdetail where takedate={0} and seid={1}";
            var list=conn.Database.SqlQuery<Orderdetail>(sql, takedate, seid).ToList();
            return list;
        }

        public Nullable<TimeSpan> findWhenToGo(string tname, string station)
        {
            string sql = "select yzleave from yunzhuan where tname={0} and snname={1}";
            //下面将修复空指针异常
            //三哥牛（sha）叉（que），直到今天还保留了结构这种东西，以至于对结构体的空指针处理要多费工夫，mmp的。。。
            var origin = conn.Database.SqlQuery<Nullable<TimeSpan> >(sql, tname, station);
            if (origin != null && origin.Count() > 0) 
            {
                var list = origin.ToList();
                return list[0];
            }
            return null;
        }

        public Nullable<TimeSpan> findWhenToArrive(string tname, string station)
        {
            //同样的理
            string sql = "select yzarrive from yunzhuan where tname={0} and snname={1}";
            var origin = conn.Database.SqlQuery<Nullable<TimeSpan> >(sql, tname, station);
            if (origin != null && origin.Count() > 0)
            {
                var list = origin.ToList();
                return list[0];
            }
            return null;
        }

        public int removeTicket(int odid)
        { 
            string sql = "delete from orderdetail where odid={0}";
            return conn.Database.ExecuteSqlCommand(sql, odid);
        }
    }
}