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
    /// StationPage.xaml 的交互逻辑
    /// </summary>
    public partial class StationPage : Page
    {
        private StationService stationService= new StationService();

        public StationPage()
        {
            InitializeComponent();
        }

        private void btnQueryStation_Click(object sender, RoutedEventArgs e)
        {
            List<Station> stations = stationService.GetStations();
            dgStation.ItemsSource = stations;
        }

        private void btnAddCz_Click(object sender, RoutedEventArgs e)
        {
            String snname = txbZhanming.Text;
            String snlevel = "";
            if (cbZhandeng!=null)
            {
                snlevel = cbZhandeng.Text;
            }
            stationService.addStation(snname, snlevel);
            MessageBox.Show("成功！");
        }

        private void btnDeleteCz_Click(object sender, RoutedEventArgs e)
        {
            Station station = dgStation.SelectedItem as Station;
            stationService.deleteStation(station.snname);
        }

        private void btnAddLx_Click(object sender, RoutedEventArgs e)
        {
            String snname= tabZhandian.Text;
            String checi=txbTname.Text;

            Nullable<TimeSpan> ddsj = null;
            Nullable<TimeSpan> lksj = null;

            if ((!"".Equals(txbDDxs.Text)) && (!"".Equals(txbDDfz.Text)))
            {
                int dds=Int32.Parse(txbDDxs.Text);
                int ddf = Int32.Parse(txbDDfz.Text);

                ddsj = new TimeSpan(dds, ddf, 0);
            }

            if((!"".Equals(txbLKxs.Text)) && (!"".Equals(txbLKfz.Text)))
            {
                int lks=Int32.Parse(txbLKxs.Text);
                int lkf=Int32.Parse(txbLKfz.Text);

                lksj = new TimeSpan(lks, lkf, 0);
            }

            stationService.addYunZhuan(checi, snname, ddsj, lksj);
            MessageBox.Show("OK!");
            
        }

        private void btnQueryName_Click(object sender, RoutedEventArgs e)
        {
            String tname=txbCheci.Text;
            List<Yunzhuan> yunzhuans = stationService.GetYunzhuansByTname(tname);
            dgYunzhuan.ItemsSource = yunzhuans;
        }

        private void btnQueryAll_Click(object sender, RoutedEventArgs e)
        {
            List<Yunzhuan> yunzhuans = stationService.GetYunzhuans();
            dgYunzhuan.ItemsSource = yunzhuans;
        }

        private void btnDeleteLx_Click(object sender, RoutedEventArgs e)
        {
            var yz = dgYunzhuan.SelectedItem as Yunzhuan;
            stationService.deleteYunzhuan(yz.tname, yz.snname, yz.yzarrive, yz.yzleave);
            MessageBox.Show("OK!");
        }
    }
}
