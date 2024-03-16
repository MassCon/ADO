using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ADO_NET_Lesson1.Service;
using System;

namespace ADO_NET_Lesson1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string ConnectionString = @"Data Source=ANDREW-LAPTOP;Initial Catalog=ADO-201;Integrated Security=True;";
        internal static readonly Logger Logger = new("log.txt");
    }
}
