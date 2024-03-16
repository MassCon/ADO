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
    public partial class OrmWindow : Window
    {
        public ObservableCollection<Entities.Department> departments { get; set; }
        public ObservableCollection<Entities.Product> products { get; set; }
        public ObservableCollection<Entities.Manager> managers { get; set; }
        private ProductCrudWindow _dialogProduct;
        public SqlConnection _connection;
        public OrmWindow()
        {
            InitializeComponent();
            departments = new();
            products = new();
            managers = new ObservableCollection<Entities.Manager>();
            DataContext = this;
            _connection = new(App.ConnectionString);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region Load Departments
            try
            {
                _connection.Open();

                using SqlCommand cmd = new() { Connection = _connection };
                cmd.CommandText = "SELECT D.* FROM Departments D";
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    departments.Add(new Entities.Department(reader));

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

                cmd.CommandText = "SELECT P.* FROM Products P WHERE P.DeleteDt IS NULL";
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    products.Add(new Entities.Product(reader));

                reader.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Window will be closed", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
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

        #region Редагування записів
        private void DepartmentsView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView item)
            {
                if (item.SelectedItem is Entities.Department department)
                {
                    DepartmentCrudWindow dialog = new DepartmentCrudWindow(_connection);
                    dialog.Department = department;
                    dialog.ShowDialog();
                }
            }
        }
        private void SecondView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _dialogProduct = new ProductCrudWindow();
            Product? product = ((ListView)sender).SelectedItem as Entities.Product;
            _dialogProduct.Product = product;
            if (_dialogProduct.ShowDialog() == true)
            {
                if (_dialogProduct.Product is null)
                {
                    MessageBox.Show("Deleted");
                }
                else
                {
                    MessageBox.Show(product.ToString());
                }
            }
        }
        private void ManagersView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView item)
            {
                if (item.SelectedItem is Entities.Manager manager)
                {
                    ManagerCrudWindow dialog = new ManagerCrudWindow() { Owner = this };
                    dialog.Manager = manager;
                    if (dialog.ShowDialog() == true)
                    {
                        MessageBox.Show(dialog.Manager.ToString());
                    }
                }
            }
        }
        #endregion

        #region Додавання записів
        private void AddDepartment_Btn_Click(object sender, RoutedEventArgs e)
        {
            DepartmentCrudWindow dialog = new DepartmentCrudWindow(_connection);
            if (dialog.ShowDialog() == true)
            {
                if (dialog.Department is not null)
                {
                    String sql = "INSERT INTO Departments(Id,Name) VALUES (@id,@name)";
                    using SqlCommand cmd = new(sql, _connection);
                    cmd.Parameters.AddWithValue("@id", dialog.Department.Id);
                    cmd.Parameters.AddWithValue("@name", dialog.Department.Name);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Відділ успішно додано!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void AddProduct_Btn_Click(object sender, RoutedEventArgs e)
        {
            ProductCrudWindow dialog = new ProductCrudWindow() { Product = null };
            if (dialog.ShowDialog() == true)
            {
                if (dialog.Product is not null)
                {
                    string sql = "INSERT INTO Products(Id, Name, Price) VALUES( @id, @name, @price ) ";
                    using SqlCommand cmd = new SqlCommand(sql, _connection);
                    cmd.Parameters.AddWithValue("@id", dialog.Product.Id);
                    cmd.Parameters.AddWithValue("@name", dialog.Product.Name);
                    cmd.Parameters.AddWithValue("@price", dialog.Product.Price);

                    #region Not recomended
                    //string sql = $"INSERT INTO Products(Id, Name, Price) " +
                    //    $"VALUES('{dialog.Product.Id}', N'{dialog.Product.Name}', {dialog.Product.Price})";
                    //using SqlCommand cmd = new SqlCommand(sql, _connection);
                    //try
                    //{
                    //    cmd.ExecuteNonQuery();
                    //    MessageBox.Show("Insert OK");
                    //}
                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show(ex.Message);
                    //}
                    #endregion
                }
            }
        }
        #endregion
    }
}