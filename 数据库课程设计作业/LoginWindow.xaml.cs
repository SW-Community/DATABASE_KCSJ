using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using 数据库课程设计作业.domain;
using 数据库课程设计作业.service;

namespace 数据库课程设计作业
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        private String filePath = "";
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var email=txbEmail.Text;
            var password=pwbPassword.Password;
            Users loginUser = new Users();
            loginUser.uemail = email;
            loginUser.upassword = password;
            UserService service = new UserService();
            var user = service.login(loginUser);
            if(user != null)
            {
                MainWindow window= new MainWindow();
                var app = Application.Current as App;
                app.User = user;
                //window.user = user;
                window.Show();
                this.Close();
            }
            else
            {
                txbMsg.Text = "登陆失败！请检查用户名或密码。";
                txbEmail.Text = "";
                pwbPassword.Password = "";
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if(!txbPwd.Text.Equals(txbConfirm.Text))
            {
                MessageBox.Show("两次密码不一致，请检查输入！","系统提示",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            String name=txbName.Text;
            String email=txbEmail2.Text;
            if(name.Equals("") || email.Equals(""))
            {
                MessageBox.Show("用户名和邮箱均不能为空！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            String password=txbPwd.Text;
            byte[] b;
            if (!filePath.Equals(""))
            {
               b = File.ReadAllBytes(filePath);
            }
            else
            {
                b = null;
            }
            Users user=new Users();
            user.uname = name;
            user.upassword = password;
            user.uemail = email;
            user.uphoto = b;
            UserService service = new UserService();
            Users res;
            if (MessageBox.Show("此程序适合有一定数码常识的铁路爱好者，要成为我们的用户，你需要：\n" +
                "· 年满18周岁并有一定的民事行为能力。\n" +
                "· 对火车购票流程有基本了解。\n" +
                "· 有一定铁路常识，或萌新但态度端正诚恳，虚心学习，三观正确，不吐槽，不过度玩梗，不做违规违纪的事（例如在不run许拍照的地方拍照），安全意识强。\n" +
                "· 注册即代表同意用户协议及隐私策略。\n" +
                "PS：我们很认真对待专业方面的内容，如果你认为西红柿是一种蔬菜，那么此程序可能不适合你\n" +
                "要继续注册吗？", "不要说我们没有警告过你", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
            if ((res = service.register(user)) != null) 
            {
                MessageBox.Show(name + "，欢迎你成为威海地铁乘客！\n您的ID是" + res.uid + "\n威海地铁竭诚为您服务！", "有朋自远方来，不亦乐乎", MessageBoxButton.OK, MessageBoxImage.Information);
                MessageBox.Show("休对故人思故国，且将新火试新茶。\n敬请开始吧！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                tabLogin.IsSelected = true;
            }
            else
            {
                MessageBox.Show("目前，我们可能无法完成注册。", "这真是让人尴尬", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = Assembly.GetExecutingAssembly().Location,
            };
            if(dialog.ShowDialog()==true)
            {
                var uri = new Uri(dialog.FileName, UriKind.RelativeOrAbsolute);
                imgPhoto.Source = new BitmapImage(uri);
                filePath = dialog.FileName;
            }
        }

        private void btnRemovePhoto_Click(object sender, RoutedEventArgs e)
        {
            imgPhoto.Source = null;
            filePath = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("警告，即将以超级用户身份登录系统。\n此模式具有最高权限，仅适用于铁路工作人员维护系统使用！\n闲杂人员未经许可不得擅自闯入，违者将追究责任，造成恶劣影响的，依法移交公安机关处理！\n要继续吗？", "威海铁路局提醒您", MessageBoxButton.YesNo, MessageBoxImage.Exclamation)== MessageBoxResult.Yes)
            {
                if(MessageBox.Show("按钮一按，线路中断，造成事故，移交法办！\n您确定您有权限进入超级用户模式？", "威海铁路局提醒您", MessageBoxButton.YesNo, MessageBoxImage.Exclamation)==MessageBoxResult.Yes)
                {
                    if(MessageBox.Show("按钮重千斤，按下毁一生；上有老，下有小，按了这个不得了！\n如果你不确定自己在做什么，请立即停止！", "威海铁路局提醒您", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    {
                        SuLogin window = new SuLogin();
                        window.ShowDialog();

                    }
                }
            }
             


        }
    }
}
