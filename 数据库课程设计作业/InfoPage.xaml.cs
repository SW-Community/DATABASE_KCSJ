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
    /// InfoPage.xaml 的交互逻辑
    /// </summary>
    public partial class InfoPage : Page
    {
        private String filePath = "";
        private UserService service = new UserService();
        public InfoPage()
        {
            InitializeComponent();
        }

        private void btnInfoAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = Assembly.GetExecutingAssembly().Location,
            };
            if (dialog.ShowDialog() == true)
            {
                var uri = new Uri(dialog.FileName, UriKind.RelativeOrAbsolute);
                imgInfoPhoto.Source = new BitmapImage(uri);
                filePath = dialog.FileName;
            }
        }

        private void btnInfoRemovePhoto_Click(object sender, RoutedEventArgs e)
        {
            imgInfoPhoto.Source = null;
            filePath = "";
        }

        private void btnInfoSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!txbInfoPwd.Text.Equals(txbInfoConfirm.Text))
            {
                MessageBox.Show("两次密码不一致，请检查输入！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            String name = txbInfoName.Text;
            String email = txbInfoEmail.Text;
            if (name.Equals("") || email.Equals(""))
            {
                MessageBox.Show("用户名和邮箱均不能为空！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            String password = txbInfoPwd.Text;
            byte[] b;
            if (!filePath.Equals(""))
            {
                b = File.ReadAllBytes(filePath);
            }
            else
            {
                b = null;
            }

            Users user = new Users();
            user.uid = (App.Current as App).User.uid;
            user.uname = name;
            user.upassword = password;
            user.uemail = email;
            user.uphoto = b;
            var nuser=service.modifyUser(user);
            if(nuser != null)
            {
                MessageBox.Show("修改用户信息成功，继续将退出并重新登陆", "你正在成功！", MessageBoxButton.OK, MessageBoxImage.Information);
                (App.Current as App).User = null;
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                var w=(MainWindow)Window.GetWindow(this);
                w.Close();
            }
            else
            {
                MessageBox.Show("Something Happened", "Something Happened", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnInfoSubmit_Loaded(object sender, RoutedEventArgs e)
        {
            var user=(App.Current as App).User;
            txbInfoUid.Text = user.uid.ToString();
            txbInfoName.Text = user.uname;
            if(user.uphoto != null)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();//数据头
                img.StreamSource = new MemoryStream(user.uphoto);
                img.EndInit();//数据尾
                imgInfoPhoto.Source = img;
            }
        }   
    }
}
