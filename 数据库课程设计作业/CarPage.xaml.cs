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
    /// CarPage.xaml 的交互逻辑
    /// </summary>
    public partial class CarPage : Page
    {

        private CarService service = new CarService();

        public CarPage()
        {
            InitializeComponent();
        }

        private void cbCartype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(txbCcapacity!=null)//莫名其妙。。。
            {
                txbCcapacity.Text = (string)(cbCartype.SelectedItem as ComboBoxItem).Tag;
            }
            //
        }

        private void btnQueryCar_Click(object sender, RoutedEventArgs e)
        {
            var list = service.findCarsByCustomConditions(CarService.NO_RESTRICTIONS);
            dgCars.ItemsSource = list;
            
        }

        private void btnAddCar_Click(object sender, RoutedEventArgs e)
        {
            Car car = new Car();
            string ctype = cbCartype.Text;
            car.ccapacity = Int16.Parse(txbCcapacity.Text);
            car.cmanufacturer = cbCmanu.Text;

            car.ctypea = ctype.Substring(0, 2);
            car.ctypeb = ctype.Substring(2, 2);

            car.cjv = txbCjv.Text;
            car.cduan = txbCduan.Text;
            car.tname = txbCTname.Text;
            car.cnum = txbCnum.Text;
            int x =service.addCar(car);
            MessageBox.Show(x + "号车，祝贺你找到自己的家！");
        }


        private void btnRemoveCar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("禁止删除报废任何车辆！","不允许的操作",MessageBoxButton.OK,MessageBoxImage.Warning);
        }

        private void btnModfiyBz_Click(object sender, RoutedEventArgs e)
        {
            if(dgCars.SelectedIndex == -1)
            {
                MessageBox.Show("请选择一辆车后在进行操作");
                return;
            }
            Car car = (Car)dgCars.SelectedItem;
            service.bianzuACar(car.cid,txbCTnameC.Text,txbCnumC.Text);
            MessageBox.Show("done");
        }
    }
}
