using System.Windows;

using System.Data.SqlClient;
using System.Windows.Media;
using System;
using System.Reflection.Metadata;
using System.IO;

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
            _connection.ConnectionString = @"Data Source=ANDREW-LAPTOP;Initial Catalog=ADO-201;Integrated Security=True;";
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

            ShowMonitor();
            ShowAll();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            if (_connection?.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        #region Monitors
        private void ShowMonitor()
        {
            ShowMonitorDepartments();
            ShowMonitorProducts();
            ShowMonitorManagers();
        }
        private void ShowMonitorDepartments()
        {
            using SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Departments", _connection);
            try
            {
                var res = cmd.ExecuteScalar(); // повертає "лівий верхній" результат запиту, тип - object
                int count = Convert.ToInt32(res);
                StatusDepartments.Content = count.ToString();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusDepartments.Content = "--";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cast Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                StatusDepartments.Content = "--";
            }
        }
        private void ShowMonitorProducts()
        {
            using SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Products", _connection);
            try
            {
                var res = cmd.ExecuteScalar(); // повертає "лівий верхній" результат запиту, тип - object
                int count = Convert.ToInt32(res);
                StatusProducts.Content = count.ToString();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusProducts.Content = "--";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cast Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                StatusProducts.Content = "--";
            }
        }
        private void ShowMonitorManagers()
        {
            using SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Managers", _connection);
            try
            {
                var res = cmd.ExecuteScalar(); // повертає "лівий верхній" результат запиту, тип - object
                int count = Convert.ToInt32(res);
                StatusManagers.Content = count.ToString();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusManagers.Content = "--";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cast Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                StatusManagers.Content = "--";
            }
        }
        #endregion

        #region Installs
        private void InstallDepartments_Click(object sender, RoutedEventArgs e)
        {
            // Команда - інструмент для виконання SQL запитів
            using SqlCommand cmd = new SqlCommand();
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
                MessageBox.Show(ex.Message, "Creation error");
            }
        }
        private void InstallProducts_Click(object sender, RoutedEventArgs e)
        {
            // Команда - інструмент для виконання SQL запитів
            using SqlCommand cmd = new SqlCommand();
            // Головні параметри команди 
            cmd.Connection = _connection;       // підключення (відкрите)
            cmd.CommandText =                   // Запит
                @"CREATE TABLE Products (
	                Id			UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	                Name		NVARCHAR(50) NOT NULL,
	                Price		FLOAT  NOT NULL
                )";
            try
            {
                cmd.ExecuteNonQuery(); // NonQuery - без повернення результату
                MessageBox.Show("Create OK");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Creation error");
            }
        }
        private void InstallManagers_Click(object sender, RoutedEventArgs e)
        {
            // Команда - інструмент для виконання SQL запитів
            using SqlCommand cmd = new SqlCommand();
            // Головні параметри команди 
            cmd.Connection = _connection;       // підключення (відкрите)
            cmd.CommandText =                   // Запит
                @"CREATE TABLE Managers (
	                Id			UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	                Surname		NVARCHAR(50) NOT NULL,
	                Name		NVARCHAR(50) NOT NULL,
	                Secname		NVARCHAR(50) NOT NULL,
	                Id_main_dep UNIQUEIDENTIFIER NOT NULL REFERENCES Departments( Id ),
	                Id_sec_dep	UNIQUEIDENTIFIER REFERENCES Departments( Id ),
	                Id_chief	UNIQUEIDENTIFIER
                )";
            try
            {
                cmd.ExecuteNonQuery(); // NonQuery - без повернення результату
                MessageBox.Show("Create OK");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Creation error");
            }
        }
        #endregion

        #region Fills
        private void FillDepartments_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand cmd = new SqlCommand();
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
                MessageBox.Show(ex.Message, "Fill error");
            }
            ShowMonitorDepartments();
        }
        private void FillProducts_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText =
                @"INSERT INTO Products
	                ( Id, Name,	Price	)
                VALUES
                    ( 'DA1E17BB-A90D-4C79-B801-5462FB070F57', N'Гвоздь 100мм',			10.50	),
                    ( 'A8E6BE17-5447-4804-AB61-F31ABF5A76D3', N'Шуруп 4х35',			4.25	),
                    ( '21B0F444-2E4F-47D8-80C1-E69BF1C34CA8', N'Гайка М4',				6.50	),
                    ( '2DCA5E44-B06D-4613-BB6A-D3BC91430BFE', N'Гровер М4',			    5.99	),
                    ( '64A4DF8A-0733-4BE9-AABA-C01B4EC3612A', N'Болт 4х60',			    9.98	),
                    ( 'B6D20749-B495-4B1A-BA1C-80B88E78B7CD', N'Гвоздь 80мм',			19.98	),
                    ( '7B08197B-C55F-4389-891F-BF12A575DFFB', N'Отвертка PZ2',			35.50	),
                    ( '870DA1A9-44F4-4018-B7FC-727A2058FAF0', N'Шуруповерт',			799		),
                    ( '8FF90E21-DCDB-4D55-A557-7C6D57DBB029', N'Молоток',				216.50	),
                    ( 'F7F1E576-AF8D-4749-869E-4A794FE69D42', N'Набор ""Новосел""',		52.40	),
                    ( 'BB29F63D-1261-41F2-89E8-88F44D5EC409', N'Сверло 6х80',			39.98	),
                    ( 'D17A4442-0A71-4673-B450-36929048ADEF', N'Шуруп 5х45',			5.98	),
                    ( '69B125D7-99CC-42D6-A6FA-46687F333749', N'Винт ""потай"" 3х16',		3.98	),
                    ( '94BC671A-A6B6-417A-BC9F-8AE4871A58EC', N'Дюбель 6х60',			5.50	),
                    ( 'EFC6578A-00B7-4766-A7E3-79CDBA8C294B', N'Органайзер для шурупов',199		),
                    ( '9654271B-AB52-4225-A30C-D75054B1733F', N'Лазерный дальномер',	1950	),
                    ( 'F2585221-1ACA-4EFE-A5E8-C2F4534D1F92', N'Дрель электрическая',	990		),
                    ( '4A550D3B-D1F2-40EF-AE4E-963612C6713A', N'Сварочный аппарат',		2099	),
                    ( '17DB11D1-F50E-4CF4-9C54-CF1BD45802EA', N'Электроды 3мм',			49.98	),
                    ( '7264D33A-16B9-4E22-B3F1-63D6DAE60078', N'Паяльник 40 Вт',		199.98	)";
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Fill OK");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            ShowMonitorProducts();
        }
        private void FillManagers_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand cmd = new SqlCommand() { Connection = _connection };
            try
            {
                cmd.CommandText = File.ReadAllText("/SQL/Fill_managers.sql");
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "SQL error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "IO error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            ShowMonitorManagers();
        }
        #endregion

        #region Drops
        private void DropDepartments_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand cmd = new SqlCommand("DROP TABLE Departments", _connection);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Departments deleted");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message + "Delete managers first");
            }
        }

        private void DropProducts_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand cmd = new SqlCommand("DROP TABLE Products", _connection);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Products deleted");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DropManagers_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand cmd = new SqlCommand("DROP TABLE Managers", _connection);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Managers deleted");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Shows Data
        private void ShowAll()
        {
            ShowDepartments();
            ShowProducts();
            ShowManagers();
        }
        private void ShowDepartments()
        {
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Departments", _connection);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                /* reader - інструмент для передачі даних від БД до програми
                 * Принцип - передача по одному рядку (табличного результату) 
                 * команда reader.Read() передає рядок. Дані залишаються в reader
                 * для доступу до даних використовуються 
                 * - геттери на кшталт reader.GerGuid(0)
                 * індексатори типу reader[1]
                 * індекси (0, 1) - це порядкові індекси полів у запиті
                 * наступний рядок - повторний виклик reader.Read()
                 */
                string str = "";
                while (reader.Read())
                {
                    string id = reader.GetGuid(0).ToString();
                    id = id[0] + id[1] + id[2] + id[3] + "..." + id[id.Length - 2] + id[id.Length - 1];
                    str += id + " " + reader.GetString(1) + "\n";
                }
                ViewDepartments.Text = str;
                reader.Close(); // незакритий рідер блокує інші команди
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void ShowProducts()
        {
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Products", _connection);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                /* reader - інструмент для передачі даних від БД до програми
                 * Принцип - передача по одному рядку (табличного результату) 
                 * команда reader.Read() передає рядок. Дані залишаються в reader
                 * для доступу до даних використовуються 
                 * - геттери на кшталт reader.GerGuid(0)
                 * індексатори типу reader[1]
                 * індекси (0, 1) - це порядкові індекси полів у запиті
                 * наступний рядок - повторний виклик reader.Read()
                 */
                string str = "";
                while (reader.Read())
                {
                    string id = reader.GetGuid(0).ToString();
                    id = id[0] + id[1] + id[2] + id[3] + "..." + id[id.Length - 2] + id[id.Length - 1];
                    str += id + " " + reader.GetString(1) + " " + reader.GetDouble(2) + "\n";
                }
                ViewProducts.Text = str;
                reader.Close(); // незакритий рідер блокує інші команди
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cast error");
            }

        }
        private void ShowManagers()
        {
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Managers", _connection);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                /* reader - інструмент для передачі даних від БД до програми
                 * Принцип - передача по одному рядку (табличного результату) 
                 * команда reader.Read() передає рядок. Дані залишаються в reader
                 * для доступу до даних використовуються 
                 * - геттери на кшталт reader.GerGuid(0)
                 * індексатори типу reader[1]
                 * індекси (0, 1) - це порядкові індекси полів у запиті
                 * наступний рядок - повторний виклик reader.Read()
                 */
                string str = "";
                while (reader.Read())
                {
                    string id = reader.GetGuid(0).ToString();
                    id = id[0] + id[1] + id[2] + id[3] + "..." + id[id.Length - 2] + id[id.Length - 1];

                    string? id_main_dep = reader.GetGuid(4).ToString();
                    if (id_main_dep != null)
                        id_main_dep = id_main_dep[0] + id_main_dep[1] + id_main_dep[2] + id_main_dep[3] + "..." + id_main_dep[id_main_dep.Length - 2] + id_main_dep[id_main_dep.Length - 1];

                    string? id_sec_dep = reader.GetSqlGuid(5).ToString();
                    if (id_sec_dep != null)
                        id_sec_dep = id_sec_dep[0] + id_sec_dep[1] + id_sec_dep[2] + id_sec_dep[3] + "..." + id_sec_dep[id_sec_dep.Length - 2] + id_sec_dep[id_sec_dep.Length - 1];

                    string? id_chief = reader.GetSqlGuid(6).ToString();
                    if (id_chief != null)
                        id_chief = id_chief[0] + id_chief[1] + id_chief[2] + id_chief[3] + "..." + id_chief[id_chief.Length - 2] + id_chief[id_chief.Length - 1];

                    //str += id + " " + reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3) + " " + id_main_dep + " " + id_sec_dep + " " + id_chief + "\n";

                    str += id + " " + reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3) + " " + id_main_dep + " " + id_sec_dep + " " + id_chief + "\n";
                }
                ViewManagers.Text = str;
                reader.Close(); // незакритий рідер блокує інші команди
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Managers cast error");
            }

        }
        #endregion
    }
}