using System.Windows;

using System.Data.SqlClient;
using System.Windows.Media;

namespace ADO_NET_Lesson1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // об'єкт підключення: головний елемент ADO
        private SqlConnection _connection;
        public MainWindow()
        {
            InitializeComponent();
            // Створення об'єкту не відкриває підключення
            _connection = new SqlConnection();
            // рядок підключення - головний параметр підключення
            _connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\ADO-201-DB\ADO-201.mdf"";Integrated Security=True";

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _connection.Open();
                StatusConnection.Content = "Connected";
                StatusConnection.Foreground = Brushes.Green;
            }
            catch (SqlException ex)
            {
                StatusConnection.Content = "Disconnected";
                StatusConnection.Foreground = Brushes.Red;
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            if (_connection?.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        #region Запити без повернення результатів
        private void InstallDepartments_Click(object sender, RoutedEventArgs e)
        {
            // Команда - інструмент для виконання SQL запитів
            SqlCommand cmd = new SqlCommand();
            // Головні параметри команди 
            cmd.Connection = _connection;       // підключення (відкрите)
            cmd.CommandText =                   // Запит
                @"CREATE TABLE Departments (
	                Id		UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	                Name    NVARCHAR(50) NOT NULL
                )";
            try
            {
                cmd.ExecuteNonQuery(); // NonQuery - без повернення результату
                MessageBox.Show("Create OK");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            cmd.Dispose(); // команда - некерований ресурс, вимагає утилізації
        }
        #endregion

        private void FillDepartments_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText =
                @"INSERT INTO Departments 
	                    ( Id, Name )
                VALUES 
	                ( 'D3C376E4-BCE3-4D85-ABA4-E3CF49612C94',  N'IT отдел'		 	 ), 
	                ( '131EF84B-F06E-494B-848F-BB4BC0604266',  N'Бухгалтерия'		 ), 
	                ( '8DCC3969-1D93-47A9-8B79-A30C738DB9B4',  N'Служба безопасности'), 
	                ( 'D2469412-0E4B-46F7-80EC-8C522364D099',  N'Отдел кадров'		 ),
	                ( '1EF7268C-43A8-488C-B761-90982B31DF4E',  N'Канцелярия'		 ), 
	                ( '415B36D9-2D82-4A92-A313-48312F8E18C6',  N'Отдел продаж'		 ), 
	                ( '624B3BB5-0F2C-42B6-A416-099AAB799546',  N'Юридическая служба' )";
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Fill OK");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            cmd.Dispose();
        }
    }
}