using ADO_NET_Lesson1.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
    /// Interaction logic for ProductCrudWindow.xaml
    /// </summary>
    public partial class ProductCrudWindow : Window
    {
        public Product? Product { get; set; }
        private SqlConnection _connection;
        public ProductCrudWindow(SqlConnection connection)
        {
            InitializeComponent();
            Product = null!;
            _connection = connection;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Product is null)
            {
                Product = new Product();
                Delete_Btn.IsEnabled = false;
            }
            else
            {
                Name_TxtBx.Text = Product.Name;
                Price_TxtBx.Text = Product.Price.ToString();
                Delete_Btn.IsEnabled = true;
            }
            Id_TxtBx.Text = Product.Id.ToString();
        }
        private void Save_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Product is null) { return; }

            if (Name_TxtBx.Text.Equals(String.Empty))
            {
                MessageBox.Show("Введіть назву продукту");
                Name_TxtBx.Focus();
                return;
            }

            if (Name_TxtBx.Text == Product.Name && Convert.ToDouble(Price_TxtBx.Text) == Product.Price)
            {
                DialogResult = false;
            }

            Product.Name = Name_TxtBx.Text;
            Product.Price = Convert.ToDouble(Price_TxtBx.Text);

            String sql = "UPDATE Products SET Name=(@name), Price=(@price) WHERE Id=(@id)";
            using SqlCommand cmd = new(sql, _connection);
            cmd.Parameters.AddWithValue("@name", Product.Name);
            cmd.Parameters.AddWithValue("@price", Product.Price);
            cmd.Parameters.AddWithValue("@id", Product.Id);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Зміни збережено!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            DialogResult = true;
        }
        private void Delete_Btn_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Впевнені, що хочете вилучити продукт?", "Вилучення", MessageBoxButton.OKCancel);
            if (dialogResult == MessageBoxResult.OK)
            {
                Product.DeleteDt = DateTime.Now;
                String sql = "UPDATE Products SET DeleteDt=(@DeleteDt) WHERE Id=(@id)";
                using SqlCommand cmd = new(sql, _connection);
                cmd.Parameters.AddWithValue("@DeleteDt", Product.DeleteDt);
                cmd.Parameters.AddWithValue("@id", Product.Id);

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Вилучення успішне!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            DialogResult = true;
        }
        private void Cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}