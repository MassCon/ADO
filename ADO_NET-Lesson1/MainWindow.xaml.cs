using System.Windows;

using System.Data.SqlClient;
using System.Windows.Media;
using System;
using System.Reflection.Metadata;
using System.IO;
using ADO_NET_Lesson1.AdditionalWindows;

namespace ADO_NET_Lesson1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Status_Btn_Click(object sender, RoutedEventArgs e)
        {
            Window window = new StatusWindow();
            window.Show();
        }

        private void ORM_Btn_Click(object sender, RoutedEventArgs e)
        {
            Window window = new OrmWindow();
            window.Show();
        }
    }
}