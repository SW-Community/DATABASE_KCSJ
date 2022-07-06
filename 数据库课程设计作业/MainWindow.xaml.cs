using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace 数据库课程设计作业
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
 
    public partial class MainWindow : Window
    {
        //public Users user { get; set; }
        //放到全局变量里面是最合理的

        private Button last = null;//为了一些辅助视觉效果

        public MainWindow()
        {
            InitializeComponent();
        }

        //初始化
        private void Window_Activated(object sender, EventArgs e)
        {
            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            string myip = IpEntry.AddressList[0].ToString();
            lbIp.Content = myip;

            lbOs.Content = Environment.OSVersion.ToString();
            var u = (App.Current as App).User;
            txbUserInfo.Text = "您好，" + u.uname + "\n" + "用户ID：" + u.uid;
        }

        //主面板四大功能
        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            var button = e.Source as Button;
            button.Background = new SolidColorBrush(Color.FromRgb(114, 195, 248));
            if(last != null)
            {
                last.Background = new SolidColorBrush(Color.FromRgb(243, 243, 243));
            }
            frmMain.Source = new Uri("QueryPage.xaml", UriKind.RelativeOrAbsolute);
            last = button;
        }

        private void btnSchedule_Click(object sender, RoutedEventArgs e)
        {
            var button = e.Source as Button;
            button.Background = new SolidColorBrush(Color.FromRgb(114, 195, 248));
            if (last != null)
            {
                last.Background = new SolidColorBrush(Color.FromRgb(243, 243, 243));
            }
            frmMain.Source = new Uri("SchedulePage.xaml", UriKind.RelativeOrAbsolute);
            last = button;
        }

        private void btnTicket_Click(object sender, RoutedEventArgs e)
        {
            var button = e.Source as Button;
            button.Background = new SolidColorBrush(Color.FromRgb(114, 195, 248));
            if (last != null)
            {
                last.Background = new SolidColorBrush(Color.FromRgb(243, 243, 243));
            }
            frmMain.Source = new Uri("TicketPage.xaml", UriKind.RelativeOrAbsolute);
            last = button;
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            var button = e.Source as Button;
            button.Background = new SolidColorBrush(Color.FromRgb(114, 195, 248));
            if (last != null)
            {
                last.Background = new SolidColorBrush(Color.FromRgb(243, 243, 243));
            }
            frmMain.Source = new Uri("InfoPage.xaml", UriKind.RelativeOrAbsolute);
            last = button;
        }

        //退出登录
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).User = null;
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
