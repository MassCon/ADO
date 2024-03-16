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
        public ObservableCollection<Department> departments { get; set; }
        public ObservableCollection<Product> products { get; set; }
        public ObservableCollection<Manager> managers { get; set; }
        public ObservableCollection<Sale> sales { get; set; }
        public SqlConnection _connection;
        public OrmWindow()
        {
            InitializeComponent();
            departments = new();
            products = new();
            managers = new ObservableCollection<Manager>();
            sales = new ObservableCollection<Sale>();
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

            #region Sales
            try
            {
                using SqlCommand cmd = new() { Connection = _connection };

                cmd.CommandText = "SELECT S.* FROM Sales S";
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    sales.Add(new Sale(reader));

                reader.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Window will be closed", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
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
                    DepartmentCrudWindow dialog = new DepartmentCrudWindow();
                    dialog.Department = department;
                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog.Department is not null)
                        {
                            String sql = "UPDATE Departments SET Name=(@name), DeleteDt=(@DeleteDt) WHERE Id=(@id)";
                            using SqlCommand cmd = new(sql, _connection);
                            cmd.Parameters.AddWithValue("@name", dialog.Department.Name);
                            cmd.Parameters.AddWithValue("@DeleteDt", dialog.Department.DeleteDt is null ? DBNull.Value : dialog.Department.DeleteDt);
                            cmd.Parameters.AddWithValue("@id", dialog.Department.Id);

                            try
                            {
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Зміни збережено!");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
            }
        }
        private void ProductsView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView item)
            {
                if (item.SelectedItem is Entities.Product product)
                {
                    ProductCrudWindow dialog = new ProductCrudWindow();
                    dialog.Product = product;
                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog.Product is not null)
                        {
                            String sql = "UPDATE Products SET Name=(@name), Price=(@price), DeleteDt=(@deleteDt) WHERE Id=(@id)";
                            using SqlCommand cmd = new(sql, _connection);
                            cmd.Parameters.AddWithValue("@name", dialog.Product.Name);
                            cmd.Parameters.AddWithValue("@price", dialog.Product.Price);
                            cmd.Parameters.AddWithValue("@deleteDt", dialog.Product.DeleteDt is null ? DBNull.Value : dialog.Product.DeleteDt);
                            cmd.Parameters.AddWithValue("@id", dialog.Product.Id);

                            try
                            {
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Зміни збережено!");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
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
                        if (dialog.Manager is not null)
                        {
                            String sql = "UPDATE Managers SET Name=(@name), Surname=(@surname), Secname=(@secname), Id_main_dep=(@main_dep), Id_sec_dep=(@sec_dep), Id_chief=(@chief), DeleteDt=(@delete) WHERE Id=(@id)";
                            using SqlCommand cmd = new(sql, _connection);
                            cmd.Parameters.AddWithValue("@name", dialog.Manager.Name);
                            cmd.Parameters.AddWithValue("@surname", dialog.Manager.Surname);
                            cmd.Parameters.AddWithValue("@secname", dialog.Manager.Secname is null ? DBNull.Value : dialog.Manager.Secname);
                            cmd.Parameters.AddWithValue("@main_dep", dialog.Manager.IdMainDep);
                            cmd.Parameters.AddWithValue("@sec_dep", dialog.Manager.IdSecDep is null ? DBNull.Value : dialog.Manager.IdSecDep);
                            cmd.Parameters.AddWithValue("@chief", dialog.Manager.IdChief is null ? DBNull.Value : dialog.Manager.IdChief);
                            cmd.Parameters.AddWithValue("@delete", dialog.Manager.DeleteDt is null ? DBNull.Value : dialog.Manager.DeleteDt);
                            cmd.Parameters.AddWithValue("@id", dialog.Manager.Id);

                            try
                            {
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Зміни збережено!");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
            }
        }
        private void SalesView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView item)
            {
                if (item.SelectedItem is Sale sale)
                {
                    SaleCrudWindow dialog = new SaleCrudWindow() { Owner = this };
                    dialog.Sale = sale;
                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog.Sale is not null)
                        {
                            String sql = "UPDATE Sales SET SaleDt=(@sale_dt), Product_Id=(@product), Quantity=(@quantity), Manager_Id=(@manager), DeleteDt=(@delete) WHERE Id=(@id)";
                            using SqlCommand cmd = new(sql, _connection);
                            cmd.Parameters.AddWithValue("@sale_dt", dialog.Sale.SaleDt);
                            cmd.Parameters.AddWithValue("@product", dialog.Sale.Product_Id);
                            cmd.Parameters.AddWithValue("@quantity", dialog.Sale.Quantity);
                            cmd.Parameters.AddWithValue("@manager", dialog.Sale.Manager_Id);
                            cmd.Parameters.AddWithValue("@delete", dialog.Sale.DeleteDt is null ? DBNull.Value : dialog.Sale.DeleteDt);
                            cmd.Parameters.AddWithValue("@id", dialog.Sale.Id);

                            try
                            {
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Зміни збережено!");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Додавання записів
        private void AddDepartment_Btn_Click(object sender, RoutedEventArgs e)
        {
            DepartmentCrudWindow dialog = new DepartmentCrudWindow();
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
            ProductCrudWindow dialog = new ProductCrudWindow();
            if (dialog.ShowDialog() == true)
            {
                if (dialog.Product is not null)
                {
                    string sql = "INSERT INTO Products(Id, Name, Price) VALUES(@id, @name, @price)";
                    using SqlCommand cmd = new SqlCommand(sql, _connection);
                    cmd.Parameters.AddWithValue("@id", dialog.Product.Id);
                    cmd.Parameters.AddWithValue("@name", dialog.Product.Name);
                    cmd.Parameters.AddWithValue("@price", dialog.Product.Price);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Продукт успішно додано!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

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
        private void AddManager_Btn_Click(object sender, RoutedEventArgs e)
        {
            ManagerCrudWindow dialog = new ManagerCrudWindow() { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                if (dialog.Manager is not null)
                {
                    string sql = "INSERT INTO Managers(Id, Name, Surname, Secname, Id_main_dep, Id_sec_dep, Id_chief) VALUES(@id, @name, @surname, @secname, @main_dep, @sec_dep, @chief)";
                    using SqlCommand cmd = new SqlCommand(sql, _connection);
                    cmd.Parameters.AddWithValue("@name", dialog.Manager.Name);
                    cmd.Parameters.AddWithValue("@surname", dialog.Manager.Surname);
                    cmd.Parameters.AddWithValue("@secname", dialog.Manager.Secname is null ? DBNull.Value : dialog.Manager.Secname);
                    cmd.Parameters.AddWithValue("@main_dep", dialog.Manager.IdMainDep);
                    cmd.Parameters.AddWithValue("@sec_dep", dialog.Manager.IdSecDep is null ? DBNull.Value : dialog.Manager.IdSecDep);
                    cmd.Parameters.AddWithValue("@chief", dialog.Manager.IdChief is null ? DBNull.Value : dialog.Manager.IdChief);
                    cmd.Parameters.AddWithValue("@id", dialog.Manager.Id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Співробітника успішно додано!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void AddSale_Btn_Click(object sender, RoutedEventArgs e)
        {
            SaleCrudWindow dialog = new SaleCrudWindow() { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                if (dialog.Sale is not null)
                {
                    string sql = "INSERT INTO Sales(Id, SaleDt, Product_Id, Quantity, Manager_Id) VALUES(@id, @saleDt, @productId, @quantity, @managerId)";
                    using SqlCommand cmd = new SqlCommand(sql, _connection);
                    cmd.Parameters.AddWithValue("@saleDt", dialog.Sale.SaleDt);
                    cmd.Parameters.AddWithValue("@productId", dialog.Sale.Product_Id);
                    cmd.Parameters.AddWithValue("@quantity", dialog.Sale.Quantity);
                    cmd.Parameters.AddWithValue("@managerId", dialog.Sale.Manager_Id);
                    cmd.Parameters.AddWithValue("@id", dialog.Sale.Id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Продаж успішно додано!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        #endregion
    }
}