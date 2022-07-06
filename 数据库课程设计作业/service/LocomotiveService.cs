using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 数据库课程设计作业.dao;
using 数据库课程设计作业.domain;

namespace 数据库课程设计作业.service
{
    public class LocomotiveService
    {
        public const int NO_RESTRICTIONS = 0;
        public const int BY_SERIES = 1;
        public const int BY_MANU = 2;
        public const int BY_NAME = 3;

        private LocomotiveDao dao = new LocomotiveDao();

        public List<Locomotive> findLocosByConditions(int method, params string[] cond)
        {
            List<Locomotive> list = null;
            switch(method)
            {
                case NO_RESTRICTIONS:
                {
                    list = dao.findAllLocos();
                    break;
                }
                case BY_SERIES:
                {
                    break;
                }
                case BY_MANU:
                {
                    break;
                }
                case BY_NAME:
                {
                    break;
                }

            }
            return list;
        }

        public Locomotive addAloco(Locomotive locomotive)
        {
            return dao.addALoco(locomotive);
        }
    }
}
