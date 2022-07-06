using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;

namespace 数据库课程设计作业
{
    /// <summary>
    /// SuLogin.xaml 的交互逻辑
    /// </summary>
    public partial class SuLogin : Window
    {
        private bool isLegal=false;
        public SuLogin()
        {
            InitializeComponent();
        }

        private void btnSu_Click(object sender, RoutedEventArgs e)
        {
            string truePwd = ConfigurationManager.AppSettings["suPassword"];
            if (pwdSu.Password.Equals(truePwd)) 
            {
                SuWindow suWindow = new SuWindow();
                suWindow.Show();
                isLegal = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("密码错误", "系统提示");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            isLegal = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!isLegal)
            {
                e.Cancel = true;
            }
            
        }
    }
}
