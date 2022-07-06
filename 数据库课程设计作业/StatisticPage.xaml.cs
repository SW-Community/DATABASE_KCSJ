using System;
using System.Collections.Generic;
using System.IO;
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
    /// StatisticPage.xaml 的交互逻辑
    /// </summary>
    public partial class StatisticPage : Page
    {
        private TicketService ticketService = new TicketService();
        private UserService userService = new UserService();

        public StatisticPage()
        {
            InitializeComponent();
        }

        private void btnQuerySegment_Click(object sender, RoutedEventArgs e)
        {
            DateTime start = dpStart.SelectedDate.Value;
            DateTime end = dpEnd.SelectedDate.Value; 
            var list=ticketService.findChepiaosByDate(start, end);
            dgBoughtTickets.ItemsSource = list;

            txbSHolder.Text = start.ToString();
            txbEHolder.Text = end.ToString();

            txbCnt.Text = list.Count.ToString();
        }

        private void btnQueryAll_Click(object sender, RoutedEventArgs e)
        {
            var list = ticketService.findChepiaos();
            dgBoughtTickets.ItemsSource = list;
            txbCnt.Text = list.Count.ToString();

        }

        private void btnQu_Click(object sender, RoutedEventArgs e)
        {
            int id=Int32.Parse(txbUid.Text);
            Users user = userService.getUserById(id);

            if(user==null)
            {
                MessageBox.Show("查无此人！", "错误", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            txbInfoUid.Text = "" + user.uid;
            txbInfoName.Text = user.uname;
            txbInfoEmail.Text = user.uemail;
            if (user.uphoto != null)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();//数据头
                img.StreamSource = new MemoryStream(user.uphoto);
                img.EndInit();//数据尾
                imgInfoPhoto.Source = img;
            }

            int dds = ticketService.getOrderCount(user.uid);
            int gps = ticketService.getTicketCount(user.uid);

            int ccs = gps;
            int gxs = App.SINGLE_PRICE * gps;

            txbCC.Text = ccs.ToString();
            txbDD.Text= dds.ToString();
            txbGP.Text= gps.ToString();
            txbGX.Text= gxs.ToString();
        }

        private void btnQZ_Click(object sender, RoutedEventArgs e)
        {
            String station=txbZhandian.Text;

            if(dpStart.SelectedDate==null || dpEnd.SelectedDate==null)
            {
                MessageBox.Show("未选择日期", "系统提示");
            }

            DateTime start = dpStart.SelectedDate.Value;
            DateTime end = dpEnd.SelectedDate.Value;

            int jzl=ticketService.getTicketCountBySStation(station,start,end);
            int czl=ticketService.getTicketCountByEStation(station,start,end);

            int ll = jzl + czl;

            txbRkll.Text = jzl.ToString();
            txbCkll.Text = czl.ToString();

            txbZkll.Text = ll.ToString();
            
        }

        private void btnQC_Click(object sender, RoutedEventArgs e)
        {
            String tname=txbCheci.Text;
            if (dpStart1.SelectedDate == null || dpEnd1.SelectedDate == null)
            {
                MessageBox.Show("未选择日期", "系统提示");
            }

            DateTime start = dpStart1.SelectedDate.Value;
            DateTime end = dpEnd1.SelectedDate.Value;

            int cnt = ticketService.getTicketCountByCC(tname, start, end);
            txbCCll.Text=cnt.ToString();
        }
    }
}
