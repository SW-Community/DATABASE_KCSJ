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
    /// QueryPage.xaml 的交互逻辑
    /// </summary>
    public partial class QueryPage : Page
    {
        private TrainService service = new TrainService();
        public QueryPage()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            var field=txbQuField.Text;
            var index=cbCondition.SelectedIndex;
            
            switch(index)
            {
                case 0:
                    {
                        dgCheci.ItemsSource =service.findTrainByName(field);
                        break;
                    }
                case 1:
                    {
                        dgCheci.ItemsSource = service.findTrainByStation(field);
                        break;
                    }
                case 2:
                    {
                        dgCheci.ItemsSource=service.findTrainByLocomotive(field);
                        break;
                    }
            }
        }

        private void btnSwap_Click(object sender, RoutedEventArgs e)
        {
            string smid = txbStartPlace.Text;
            txbStartPlace.Text = txbEndPlace.Text;
            txbEndPlace.Text = smid;
        }

        private void cbXingbao_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("兄弟，你认真的吗？", "彩蛋", MessageBoxButton.OK, MessageBoxImage.Question);
            (sender as CheckBox).IsChecked = false;
        }

        private void cbAll_Click(object sender, RoutedEventArgs e)
        {
            bool status = (sender as CheckBox).IsChecked.Value;
            cbZhida.IsChecked = status;
            cbTekuai.IsChecked = status;
            cbKuaisu.IsChecked = status;
            cbPusu.IsChecked = status;
            cbLinke.IsChecked = status;
            cbLvyou.IsChecked = status;
        }

        private void cbOthers_Click(object sender, RoutedEventArgs e)
        {
            cbAll.IsChecked = false;
        }

        private void btnQueryForTake_Click(object sender, RoutedEventArgs e)
        {
            List<string> filters = new List<string>();
            bool isCheck = false;

            if (cbZhida.IsChecked == true)  { isCheck = true; filters.Add("Z"); }
            if (cbTekuai.IsChecked == true) { isCheck = true; filters.Add("T"); }
            if (cbKuaisu.IsChecked == true) { isCheck = true; filters.Add("K"); }
            if (cbPusu.IsChecked == true) { isCheck = true; filters.Add("慢车"); }//没辙了，搞个特殊标记吧
            if (cbLinke.IsChecked == true) { isCheck = true; filters.Add("L"); }
            if (cbLvyou.IsChecked == true) { isCheck = true; filters.Add("Y"); }
            
            if(!isCheck)
            {
                MessageBox.Show("未选择任何速度等级！", "这真是令人尴尬", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            string startPlace = txbStartPlace.Text;
            string endPlace = txbEndPlace.Text;
            var list = service.findTarinsByStartAndEndWithFilters(startPlace,endPlace,filters);
            dgCheci.ItemsSource = list;
        }

        private void dgCheci_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index=dgCheci.SelectedIndex;
            if (index != -1)
            {
                var a = (dgCheci.SelectedItem as Liechegaikuang).tname;
                (App.Current as App).Tname = a;
                foreach(Window w in App.Current.Windows)
                {
                    if ("MainWD".Equals(w.Name))
                    {
                        (w as MainWindow).frmMain.Source = new Uri("SchedulePage.xaml", UriKind.RelativeOrAbsolute);
                        (w as MainWindow).btnQuery.Background = new SolidColorBrush(Color.FromRgb(243, 243, 243));
                        (w as MainWindow).btnSchedule.Background = new SolidColorBrush(Color.FromRgb(114, 195, 248));
                        break;
                    }
                }
            }
        }
    }
}
