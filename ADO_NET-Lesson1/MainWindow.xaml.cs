using System.Windows;
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

        private void Ef_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new EfWindow().ShowDialog();
            this.Show();
        }
    }
}