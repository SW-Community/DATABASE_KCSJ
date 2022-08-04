using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using 数据库课程设计作业.dao;
using 数据库课程设计作业.domain;

namespace 数据库课程设计作业.service
{
    public class TicketService
    {
        private TicketDao dao = new TicketDao();

        public List<Availableseats> queryForSeats(string tname, DateTime takedate, string takeStart, string takeEnd)
        {
            if (takeStart.Equals(takeEnd))//emmm这是个什么情况？？？
            {
                MessageBox.Show("别捣乱，小心被查水表！！！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            Nullable<TimeSpan> startTime = dao.findWhenToGo(tname, takeStart);
            Nullable<TimeSpan> endTime = dao.findWhenToArrive(tname, takeEnd);

            if (startTime == null || endTime == null || startTime >= endTime)//选反了
            {
                MessageBox.Show("兄弟，你坐错方向了吧。。。\n友情提示：相反方向的车次号码往往相差不大，请尝试将车次号码+1或-1试试~~~biu~~~o(〃＾▽＾〃)o", "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            var originalList = dao.findSeatsOnATrain(tname);
            List<Availableseats> nwlist = new List<Availableseats>();//一会返回给表示层的列表
            foreach (var seat in originalList) 
            {
                int seid = seat.seid;
                bool canUse = true;

                //今天这个座位的所有已购车票
                var list = dao.findDetailsByDateAndSeat(takedate, seid);
                foreach (var item in list) 
                {
                    string aTakeStart = item.takestart;
                    string aTakeEnd = item.takeend;

                    Nullable<TimeSpan> aStartTime = dao.findWhenToGo(tname, aTakeStart);
                    Nullable<TimeSpan> aEndTime = dao.findWhenToArrive(tname, aTakeEnd);

                    if (aStartTime == null || aEndTime == null || (!(endTime <= aStartTime.Value || startTime >= aEndTime.Value)))
                    {
                        //有冲突
                        canUse = false;//这个座位已经被用了
                        break;
                    }
                }
                if (canUse)
                {
                    nwlist.Add(seat);//加入要返回展示的列表
                }
            }
            return nwlist;
        }

        public Orderdetail generateTicket(string taker, string tname, int cnum, int snum, string takeStart, string takeEnd, DateTime takedate)
        {
            Seat seat = dao.findSeatByLocation(tname, cnum, snum);
            if(seat == null)
            {
                MessageBox.Show("座位不存在，请检查车次，车厢号，座号", "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            //垂死病中惊坐起，憨批竟是我自己

            //else if(seat.isused.Equals("是"))
            //{
            //    MessageBox.Show("座位已被占用", "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return null;
            //}

            //下面是修复代码

            if (takeStart.Equals(takeEnd))//emmm这是个什么情况？？？
            {
                MessageBox.Show("这里欢迎车迷参观，run许不买票的情况下进站拍照", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }

            //欲购买车票的运行区间
            Nullable<TimeSpan> startTime = dao.findWhenToGo(tname, takeStart);
            Nullable<TimeSpan> endTime = dao.findWhenToArrive(tname, takeEnd);

            if (startTime == null || endTime == null || startTime >= endTime)//选反了
            {
                MessageBox.Show("兄弟，你坐错方向了吧。。。\n友情提示：相反方向的车次号码往往相差不大，请尝试将车次号码+1或-1试试~~~biu~~~o(〃＾▽＾〃)o", "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            int seid = seat.seid;
            var list = dao.findDetailsByDateAndSeat(takedate, seid);
            foreach (var item in list) 
            {
                //如果有重复车票，查询其运行区间
                string aTakeStart = item.takestart;
                string aTakeEnd = item.takeend;

                Nullable<TimeSpan> aStartTime = dao.findWhenToGo(tname, aTakeStart);
                Nullable<TimeSpan> aEndTime = dao.findWhenToArrive(tname, aTakeEnd);

                if (aStartTime == null || aEndTime == null || (!(endTime <= aStartTime.Value || startTime >= aEndTime.Value))) 
                {
                    //运行区间重合或交集
                    MessageBox.Show("不好意思呢，座位已经被占用了", "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

            }
            Orderdetail orderdetail = new Orderdetail();
            orderdetail.takername = taker;
            orderdetail.takestart = takeStart;
            orderdetail.takeend = takeEnd;
            orderdetail.takedate = takedate;
            orderdetail.seid = seat.seid;
            return orderdetail;
        }

        public int buyTickets(List<Orderdetail> list, DateTime odate, string zffs, int uid, int zje)
        {
            var order = dao.getOrder(odate, zffs, uid, zje);
            if (order != null)
            {
                //这一段也不需要了

                //foreach(var item in list)
                //{
                //    int seid = item.seid.Value;//Optional<T>?
                //    dao.setStatusOfSeats(seid, true);
                //}

                int oid = order.oid;
                int lines = dao.addTickets(list, oid);
                return lines;
            }
            MessageBox.Show("Something Happened，请稍后再试。", "我们都有不顺利的时候", MessageBoxButton.OK, MessageBoxImage.Error);
            return 0;
        }

        public void printChepiao(Chepiao chepiao, string path)
        {
            //模拟打印车票的功能（写入txt文档）
            //格式参考自互联网
            StringBuilder sb=new StringBuilder();

            int odid = chepiao.odid;
            string tname = chepiao.tname.Trim();
            string takestart = chepiao.takestart.Trim();
            string takeend = chepiao.takeend.Trim();
            int year = chepiao.takedate.Value.Year;
            int month = chepiao.takedate.Value.Month;
            int day = chepiao.takedate.Value.Day;
            string cnum = chepiao.cnum.Trim();
            int snum = chepiao.cno.Value;
            string takename = chepiao.takername.Trim();
            string zwlx;
            switch(chepiao.ctypea.Trim())
            {
                case "YZ": { zwlx = "硬座";break; }
                case "YW": { zwlx = "硬卧"; break; }
                case "RZ": { zwlx = "软座"; break; }
                case "RW": { zwlx = "软卧"; break; }
                default: { zwlx = "无座";break; }
            }
            if (chepiao.ctypeb.Trim().Equals("25K") || chepiao.ctypeb.Trim().Equals("25G") || chepiao.ctypeb.Trim().Equals("25T")) 
            {
                zwlx = "新空调" + zwlx;
            }
            TimeSpan timeSpan = dao.findWhenToGo(tname, takestart).Value;
            
            sb.AppendLine("*********************************************************\n");
            sb.AppendLine(odid + "                                            (售)");
            sb.AppendLine("            " + takestart + "----" + tname + "次 ---->" + takeend);
            sb.AppendLine(year + "年" + month + "月" + day + "日" + timeSpan + "开" + "                " + cnum + "车" + snum + "座");
            sb.AppendLine(zwlx + "\n￥" + App.SINGLE_PRICE + "元\n");
            sb.AppendLine("限乘当日当次车\n");
            sb.AppendLine(takename + "\n");
            sb.AppendLine("               | 买票请到12306，发货请到95306 |");
            sb.AppendLine("               |     中国铁路祝您旅途愉快     |");     //被抢愉快bushu
            sb.AppendLine("\n*********************************************************");

            try
            {
                File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public List<Chepiao> findChepiaosByUserID(int id)
        {
            return dao.findChepiaosByUserID(id);
        }

        public int tuiPiaoByOdid(Chepiao chepiao)
        {
            var detail = dao.findDetailById(chepiao.odid);
            if(detail != null)
            {
                //int seid = detail.seid.Value;
                //dao.setStatusOfSeats(seid, false);
                return dao.removeTicket(chepiao.odid);
            }
            return 0;
        }

        public List<Chepiao> findChepiaos()
        {
            return dao.findChepiaos();
        }

        public List<Chepiao> findChepiaosByDate(DateTime start, DateTime end)
        {
            return dao.findChepiaosByDate(start, end);
        }


        public int getTicketCountBySStation(String station, DateTime start, DateTime end)
        { 
            return dao.getTicketCountBySStation(station, start, end); 
        }

        public int getTicketCountByEStation(String station, DateTime start, DateTime end)
        { 
            return dao.getTicketCountByEStation(station, start, end);
        }

        public int getTicketCountByCC(String tname, DateTime start, DateTime end)
        { 
            return dao.getTicketCountByCC(tname, start, end);
        }

        public int getTicketCount(int id)
        { 
            return dao.getTicketCount(id);
        }

        public int getOrderCount(int id)
        { 
            return dao.getOrderCount(id);
        }
    }
}
