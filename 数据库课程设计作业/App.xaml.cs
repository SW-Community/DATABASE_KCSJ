using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using 数据库课程设计作业.domain;

namespace 数据库课程设计作业
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static int SINGLE_PRICE = 2;//一张票票价
        public Users User { get; set; }
        public string Tname { get; set; }
    }
}
