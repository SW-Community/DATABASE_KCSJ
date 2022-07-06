using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using 数据库课程设计作业.domain;
using 数据库课程设计作业.service;

namespace 数据库课程设计作业
{
    /// <summary>
    /// TicketPage.xaml 的交互逻辑
    /// </summary>
    public partial class TicketPage : Page
    {
        private TicketService service = new TicketService();
        private List<Orderdetail> tickets = new List<Orderdetail>();
        private int price = 0;

        public TicketPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txbUid.Text = (App.Current as App).User.uid.ToString();
            txbDate.Text = DateTime.Now.ToString();
            txbSumPrice.Text=price.ToString();
        }

        private void btnQueryForSeats_Click(object sender, RoutedEventArgs e)
        {
            //车次，日期，乘车区间
            string tname = txbName.Text;
          
            string takeStart = txbTakeStart.Text;
            string takeEnd = txbTakeEnd.Text;

            DateTime date;
            if(dpDate.SelectedDate.HasValue)
            {
                date = dpDate.SelectedDate.Value;
            }
            else
            {
                //不选默认是查今天的
                date=DateTime.Now;
            }

            var list = service.queryForSeats(tname, date, takeStart, takeEnd);
            dgSeats.ItemsSource=list;
        }

        private void btnAddTicket_Click(object sender, RoutedEventArgs e)
        {
            var taker = txbTaker.Text;
            var tname = txbTname.Text;
            var cnum = Int32.Parse(txbCnum.Text);
            var snum = Int32.Parse(txbSeatnum.Text);
            var takeStart=txbStart.Text;
            var takeEnd=txbEnd.Text;
            var takedate = dpTakedate.SelectedDate.Value;
            var ticket = service.generateTicket(taker, tname, cnum, snum, takeStart, takeEnd, takedate);
            if (ticket != null)
            {
                if(tickets.Contains(ticket))
                {
                    MessageBox.Show("不要重复购买同一张车票！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                tickets.Add(ticket);
                price += App.SINGLE_PRICE;
                txbSumPrice.Text = price.ToString();
                lbTickets.Items.Add(tname+"次\t"+cnum+"车"+snum+"座\t乘车人："+ticket.takername+"\t出发站 "+ticket.takestart+"\t到达站 "+ticket.takeend+"\t乘车日期 "+takedate);
            }

        }

        private void btnRemoveTicket_Click(object sender, RoutedEventArgs e)
        {
            int index=lbTickets.SelectedIndex;
            if (MessageBox.Show("确定删除所选车票？", "不要怪我们没有警告过你", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) 
            {
                tickets.RemoveAt(index);
                lbTickets.Items.RemoveAt(index);
                price -= App.SINGLE_PRICE;
                txbSumPrice.Text = price.ToString();
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //最核心的一个模块终于要来了。。。
            string zffs = "";
            switch(lbTickets.SelectedIndex)
            {
                case 0: zffs = "微信";break;
                case 1: zffs = "支付宝";break;
                case 2: zffs = "刷卡";break;
                case 4: zffs = "现金";break;
                default: zffs = "PayPal";break;
            }
            int uid = (App.Current as App).User.uid;
            int zje = price;

            int cnt = service.buyTickets(tickets, DateTime.Now, zffs, uid, zje);
            if (cnt > 0) 
            {
                MessageBox.Show("恭喜你，成功购买了"+cnt+"张车票！\n你知道吗，你的每一次有价值的购票，都是帮助我们改进体验，塑造未来！\n" +
                    "请于开车前一小时到候车室坐和放宽，等待列车进站。留意广播和大屏幕，我们将通过这两种方式通知您上车。", "你正在成功！");
            }
            else
            {
                MessageBox.Show("出错了", "我们都有不顺利的时候");
            }
            lbTickets.Items.Clear();
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            int uid = (App.Current as App).User.uid;
            dgBoughtTickets.ItemsSource = service.findChepiaosByUserID(uid);
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            var item = (Chepiao)dgBoughtTickets.SelectedItem;
            if (item != null) 
            {
                var saveDialog = new SaveFileDialog
                {
                    Title = "选择车票保存位置",
                    InitialDirectory = Environment.CurrentDirectory,
                    Filter = "文本文件(*.txt)|*.txt",
                    FileName = "铁路旅客车票_timestamp=" + (DateTime.Now.ToString()).Replace(":", "_").Replace("/", "_") + "_uid=" + (App.Current as App).User.uid + "__8wekyb3d8bbwe",
                };
                if (saveDialog.ShowDialog() == true)
                {
                    service.printChepiao(item, saveDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("未选择车票，请点击表格栏选择您要打印的车票", "这真是令人尴尬", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            int uid = (App.Current as App).User.uid;
            dgBoughtTickets.ItemsSource = service.findChepiaosByUserID(uid);
        }

        private void btnTuipiao_Click(object sender, RoutedEventArgs e)
        {
            var item = (Chepiao)dgBoughtTickets.SelectedItem;
            if(item != null)
            {
                int x = service.tuiPiaoByOdid(item);
                if (x > 0) 
                {
                    MessageBox.Show("退票已完成！", "你正在成功！", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Something Happened", "Something Happened", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("未选择车票，请点击表格栏选择您要处理的车票", "这真是令人尴尬", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
