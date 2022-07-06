using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 数据库课程设计作业.domain;

namespace 数据库课程设计作业.dao
{
    public class CarDao
    {
        private Locomotive_systemEntities conn = new Locomotive_systemEntities();

        public Car findCarByID(int cid)
        {
            return conn.Car.Find(cid);
        }

        public List<Car> findCarsByTname(string tname)
        {
            string sql = "select * from car where tname = {0}";
            List<Car> cars = conn.Database.SqlQuery<Car>(sql,tname).ToList();
            return cars;
        }

        public List<Car> findAllCars()
        {
            string sql = "select * from car";
            return conn.Database.SqlQuery<Car>(sql).ToList();
        }

        //批处理，添加车辆的同时悉数添加座位
        //返回添加的座位ID
        public int addASeat(int cid, int snum)
        {
            Seat seat = new Seat();
            seat.cid = cid;
            seat.cno = snum;
            int newSeatId = Convert.ToInt32(conn.Database.SqlQuery<Decimal>("select IDENT_CURRENT('seat')").ToList()[0]);
            conn.Seat.Add(seat);
            conn.SaveChanges();
            return newSeatId++;
        }

        public Car addCar(Car car)
        {
            string sql = "select IDENT_CURRENT('car')";
            int lastCid = Convert.ToInt32(conn.Database.SqlQuery<Decimal>(sql).ToList()[0]);
            var c = conn.Car.Add(car);
            conn.SaveChanges();
            return c;
        }

        public void bianzuACar(int cid, string tname, string cnum)
        {
            string sql1 = "update car set tname={1} where cid={0}";
            string sql2 = "update car set cnum={1} where cid={0}";
            conn.Database.ExecuteSqlCommand(sql1, cid, tname);
            conn.Database.ExecuteSqlCommand(sql2, cid, cnum);
        }
    }
}
