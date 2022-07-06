using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 数据库课程设计作业.dao;
using 数据库课程设计作业.domain;

namespace 数据库课程设计作业.service
{
    public class TrainService
    {
        private TrainDao dao = new TrainDao();

        public List<Liechegaikuang> getAllTrains()
        {
            return dao.getAllTrains();
        }

        public List<Liechegaikuang> findTrainByName(string name)
        {
            return dao.findTrainByName(name);
        }

        public List<Liechegaikuang> findTrainByStation(string station)
        {
            return dao.findTrainByStation(station);
        }

        public List<Liechegaikuang> findTrainByLocomotive(string locomotive)
        {
            return dao.findTrainByLocomotive(locomotive);
        }

        public List<Liecheshikebiao> queryForSchedule(string name)
        {
            return dao.queryForSchedule(name);
        }


        public int addTrain(String tname, String tcaptain, String tstart, String tend)
        {
            return dao.addTrain(tname, tcaptain, tstart, tend);
        }

        public int addAct(String lname, String tname, String ldeiver, DateTime adate)
        {
            return dao.addAct(lname, tname, ldeiver, adate);
        }

        public List<Act> GetActs()
        {
            return dao.GetActs();
        }

        public int deleteAct(String lname, String tname, DateTime adate)
        {
            return dao.deleteAct(lname, tname, adate);
        }

        public List<Liechegaikuang> findTarinsByStartAndEndWithFilters(string startPlace, string endPlace, List<string> filters)
        {
            //查从startplace出发的车
            var l1 = dao.findTrainsByStartPlace(startPlace);
            //查从endlace到达的车
            var l2 = dao.findTrainsByEndPlace(endPlace);

            if (l1 != null && l1.Count() > 0 && l2 != null && l2.Count() > 0) 
            {
                //取交集
                var l3 = l1.Intersect<Liechegaikuang>(l2, new LiecheComparator());
                List<Liechegaikuang> resList = new List<Liechegaikuang>();

                //这里。。。其实当时把这两个方法放到triandao里面更合适，罢了罢了，懒得改了
                TicketDao ticketDao = new TicketDao();
                foreach (var lieche in l3)
                {
                    var cfsj = ticketDao.findWhenToGo(lieche.tname, startPlace).Value;
                    var ddsj = ticketDao.findWhenToArrive(lieche.tname, endPlace).Value;

                    if(cfsj < ddsj)
                    {
                        //根据条件过滤
                        bool isEligible = false;
                        foreach(var filter in filters)
                        {
                            if (filter.Equals("慢车"))//没办法只能特事特办了。。。
                            {
                                char x = lieche.tname[0];
                                if (Char.IsDigit(x)) 
                                {
                                    isEligible = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (lieche.tname.StartsWith(filter))
                                {
                                    isEligible = true;
                                    break;
                                }
                            }
                        }
                        if(isEligible)
                        {
                            resList.Add(lieche);
                        }
                    }
                }
                return resList;
            }
            return null;
        }


        //内部类，用作比较器
        //我不确定直接重写这些实体类会发生什么，所以就用这种方法了
        private class LiecheComparator : IEqualityComparer<Liechegaikuang>
        {
            public bool Equals(Liechegaikuang x, Liechegaikuang y)
            {
                return x.tname.Equals(y.tname);
            }

            public int GetHashCode(Liechegaikuang obj)
            {
                return obj.tname.GetHashCode();
            }
        }
    }
}
