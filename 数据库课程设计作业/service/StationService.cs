using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 数据库课程设计作业.dao;
using 数据库课程设计作业.domain;

namespace 数据库课程设计作业.service
{
    public class StationService
    {
        private StationDao dao = new StationDao();
        private RevolveDao revolveDao = new RevolveDao();

        public List<Station> GetStations()
        {
            return dao.GetStations();
        }

        public int deleteStation(String name)
        {
            return dao.deleteStation(name);
        }

        public int addStation(String name, String level)
        {
            return dao.addStation(name, level);
        }

        public List<Yunzhuan> GetYunzhuans()
        {
            return revolveDao.GetYunzhuans();
        }


        public List<Yunzhuan> GetYunzhuansByTname(String tname)
        {
            return revolveDao.GetYunzhuansByTname(tname);
        }

        public int addYunZhuan(String tname, String snname, Nullable<TimeSpan> yzarrive, Nullable<TimeSpan> yzleave)
        {
            return revolveDao.addYunZhuan(tname, snname, yzarrive, yzleave);
        }


        public int deleteYunzhuan(String tname, String snname, Nullable<TimeSpan> yzarrive, Nullable<TimeSpan> yzleave)
        {
            return revolveDao.deleteYunzhuan(tname,snname, yzarrive, yzleave); 
        }
    }
}
