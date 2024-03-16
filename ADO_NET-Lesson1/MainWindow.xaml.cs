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
            this.Hide();
            new StatusWindow().ShowDialog();
            this.Show();
        }

        private void ORM_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new OrmWindow().ShowDialog();
            this.Show();
        }

        private void DAL_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new DalWindow().ShowDialog();
            this.Show();
        }
    }
}