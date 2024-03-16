using ADO_NET_Lesson1.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace ADO_NET_Lesson1.AdditionalWindows
{
    /// <summary>
    /// Interaction logic for OmrWindow.xaml
    /// </summary>
    public partial class OmrWindow : Window
    {
        public ObservableCollection<Entities.Department> departments { get; set; }
        public ObservableCollection<Entities.Product> products { get; set; }
        public ObservableCollection<Entities.Manager> managers { get; set; }
        private DepartmentCrudWindow _dialogDepartment;
        public SqlConnection _connection;
        public OmrWindow()
        {
            InitializeComponent();
            departments = new();
            products = new();
            managers = new ObservableCollection<Entities.Manager>();
            DataContext = this;
            _connection = new(App.ConnectionString);
            _dialogDepartment = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region Load Departments
            try
            {
                _connection.Open();
                using SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = "SELECT Id, Name FROM Departments D";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    departments.Add(new Entities.Department
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1)

                    });
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Window will be closed", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            #endregion

            #region Load Products
            try
            {
                using SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = "SELECT Id, Name FROM Products D";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Entities.Product
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1)

                    });
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Window will be closed", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            #endregion

            #region Load Managers
            try
            {
                using SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = "SELECT * FROM Managers";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    managers.Add(new Entities.Manager
                    {
                        Id = reader.GetGuid(0),
                        Surname = reader.GetString(1),
                        Name = reader.GetString(2),
                        Secname = reader.GetString(3),
                        IdMainDep = reader.GetGuid(4),
                        IdSecDep = reader.GetValue(5) == DBNull.Value ? null : reader.GetGuid(5),
                        IdChief = reader.IsDBNull(6) ? null : reader.GetGuid(6)
                    });
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Window will be closed", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            #endregion

        }

        private void FirstView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _dialogDepartment = new DepartmentCrudWindow();
            Department? department = ((ListView)sender).SelectedItem as Entities.Department;
            _dialogDepartment.Department = department;
            if (_dialogDepartment.ShowDialog() == true)
            {
                if (_dialogDepartment.Department is null)
                {
                    MessageBox.Show("Deleted");
                }
                else
                {
                    MessageBox.Show(department.ToString());
                }
            }
        }

        private void SecondView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ThirdView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}