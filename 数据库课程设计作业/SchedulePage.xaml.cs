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
using 数据库课程设计作业.service;

namespace 数据库课程设计作业
{
    /// <summary>
    /// SchedulePage.xaml 的交互逻辑
    /// </summary>
    public partial class SchedulePage : Page
    {
        private TrainService service = new TrainService();
        public SchedulePage()
        {
            InitializeComponent();
        }

        private void btnQuerySchedule_Click(object sender, RoutedEventArgs e)
        {
            var name = txbTname.Text;
            var list=service.queryForSchedule(name);
            dgSchedule.ItemsSource = list;
        }

        private void txbTname_Loaded(object sender, RoutedEventArgs e)
        {
            var a = (App.Current as App).Tname;
            if (a != null && a != "")
            {
                txbTname.Text = a;
                btnQuerySchedule_Click(sender, e);
            }
        }
    }
}
