using ADO_NET_Lesson1.Entities;
using System;
using System.Collections.Generic;
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
        public ProductCrudWindow()
        {
            InitializeComponent();
            Product = null;
        }

        private void Save_Btn_Click(object sender, RoutedEventArgs e)
        {
            Product.Name = Name_TxtBx.Text;
            Product.Price = Convert.ToDouble(Price_TxtBx.Text);
            DialogResult = true;
        }

        private void Delete_Btn_Click(object sender, RoutedEventArgs e)
        {
            Product = null!;
            DialogResult = true;
        }

        private void Cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Product is null)
            {
                Delete_Btn.IsEnabled = false;
                Product = new Product { Id = Guid.NewGuid() };
            }
            else
            {
                Name_TxtBx.Text = Product.Name.ToString();
                Price_TxtBx.Text = Product.Price.ToString();
                Delete_Btn.IsEnabled = true;
            }
            Id_TxtBx.Text = Product.Id.ToString();
        }
    }
}