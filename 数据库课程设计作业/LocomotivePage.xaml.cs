using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// LocomotivePage.xaml 的交互逻辑
    /// </summary>
    public partial class LocomotivePage : Page
    {
        private LocomotiveService service = new LocomotiveService();
        private byte[] imgData = null;

        public LocomotivePage()
        {
            InitializeComponent();
        }

        private void cbCondition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i =cbCondition.SelectedIndex;
            switch(i)
            {
                case 0:
                    {
                        if (dpSeries != null) { dpSeries.Visibility = Visibility.Collapsed; }
                        if (dpManu != null) { dpManu.Visibility = Visibility.Collapsed; }
                        if (dpNum != null) { dpNum.Visibility = Visibility.Collapsed; }
                        break;
                    }
                case 1:
                    {
                        dpSeries.Visibility = Visibility.Visible;
                        dpManu.Visibility = Visibility.Collapsed;
                        dpNum.Visibility = Visibility.Collapsed;
                        break;
                    }
                case 2:
                    {
                        dpSeries.Visibility = Visibility.Collapsed;
                        dpManu.Visibility = Visibility.Visible;
                        dpNum.Visibility = Visibility.Collapsed;
                        break;
                    }
                case 3:
                    {
                        dpSeries.Visibility = Visibility.Collapsed;
                        dpManu.Visibility = Visibility.Collapsed;
                        dpNum.Visibility = Visibility.Visible;
                        break;
                    }
            }
        }

        private void btnQueryLoco_Click(object sender, RoutedEventArgs e)
        {
            var list = service.findLocosByConditions(LocomotiveService.NO_RESTRICTIONS);
            dgLoco.ItemsSource = list;
        }

        private void btnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = Assembly.GetExecutingAssembly().Location,
            };
            if (dialog.ShowDialog() == true)
            {
                //打开图片
                var uri = new Uri(dialog.FileName, UriKind.RelativeOrAbsolute);
                //相对绝对都可以
                imgLphoto.Source = new BitmapImage(uri);
                imgData = File.ReadAllBytes(dialog.FileName);
            }

        }

        private void btnRemPhoto_Click(object sender, RoutedEventArgs e)
        {
            imgData = null;
            imgLphoto.Source = null;
        }

        private void btnAddLoco_Click(object sender, RoutedEventArgs e)
        {
            Locomotive loco = new Locomotive();
            loco.lname = txbLame.Text;
            loco.lmanufacturer = cbLmanufacturer.Text;
            loco.ljv = txbLjv.Text;
            loco.lduan = txbLduan.Text;
            loco.lbirth = dpLbirth.SelectedDate;
            loco.lphoto = imgData;
            var l = service.addAloco(loco);
            MessageBox.Show("欢迎" + l.lname + "，这里就是你的家了！", "恭喜恭喜", MessageBoxButton.OK);
        }

        private void btnRemoveLoco_Click(object sender, RoutedEventArgs e)
        {
            //故意Ban掉的，禁止删除数据
            //愿另一个世界没有事故和报废淘汰
            MessageBox.Show("机车那么可爱，你怎么忍心报废她呢？\n如果你这样做，她在另一个世界也会记恨你一辈子的！", "不允许的操作", MessageBoxButton.OK, MessageBoxImage.Stop);
        }
    }
}
