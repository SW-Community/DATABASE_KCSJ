using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using 数据库课程设计作业.dao;
using 数据库课程设计作业.domain;

namespace 数据库课程设计作业.service
{
    public class CarService
    {
        public const int NO_RESTRICTIONS = 0;
        public const int BY_TNAME = 1;
        public const int BY_CARTYPE = 2;

        private CarDao dao = new CarDao();

        public List<Car> findCarsByCustomConditions(int method,params string[] cond)
        {
            List<Car> cars = null;
            
            switch(method)
            {
                case NO_RESTRICTIONS:
                {
                    cars = dao.findAllCars();
                    break;
                }
                
                case BY_TNAME:
                {
                    break;
                }

                case BY_CARTYPE:
                {
                    break;
                }
            }
            return cars;
        }

        public int addCar(Car car)
        {
            int cid = dao.addCar(car).cid;
            int capacity = car.ccapacity.Value;//MD被三哥的脑回路坑惨了
            for (int i = 0; i < capacity; i++)
            {
                //批量整活
                dao.addASeat(cid, i + 1);
            }
            return cid;
        }

        public void bianzuACar(int cid, string tname, string cnum)
        {
            dao.bianzuACar(cid, tname, cnum);
        }
    }
}
