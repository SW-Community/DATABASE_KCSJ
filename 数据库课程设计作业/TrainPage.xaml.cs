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
    /// TrainPage.xaml 的交互逻辑
    /// </summary>
    public partial class TrainPage : Page
    {
        private TrainService trainService = new TrainService();

        public TrainPage()
        {
            InitializeComponent();
        }

        private void btnQueryTrain_Click(object sender, RoutedEventArgs e)
        {
            List<Liechegaikuang> liechegaikuangs = trainService.getAllTrains();
            dgTrain.ItemsSource = liechegaikuangs;
        }

        private void btnAddTrain_Click(object sender, RoutedEventArgs e)
        {
            String tname = txbCheciHao.Text;
            String tcaptain=txbLieCheZhang.Text;
            String tstart = txbQdz.Text;
            String tend= txbZdz.Text;
            int res = trainService.addTrain(tname, tcaptain, tstart, tend);
            if (res > 0) 
            {
                MessageBox.Show("成功！", "来自窗10商业经验", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("您和您的软件需要重新启动，这可能需要20分钟", "错误");
            }
        }

        private void btnAddAct_Click(object sender, RoutedEventArgs e)
        {
            String lname=txbChehao.Text;
            String tname=txbTname.Text;
            String ldriver = txbSiji.Text;
            DateTime adate = dpRiQi.SelectedDate.Value;
            trainService.addAct(lname, tname, ldriver, adate);
            MessageBox.Show("OK!");
        }

        private void btnQyeryAct_Click(object sender, RoutedEventArgs e)
        {
            List<Act> acts = trainService.GetActs();
            dgAct.ItemsSource= acts;
        }

        private void btnDeleteAct_Click(object sender, RoutedEventArgs e)
        {
            Act act= (Act)dgAct.SelectedItem;
            trainService.deleteAct(act.lname, act.tname, act.adate);
            MessageBox.Show("删除成功！");
        }
    }
}
