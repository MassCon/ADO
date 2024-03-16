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
    /// Interaction logic for DepartmentCrudWindow.xaml
    /// </summary>
    public partial class DepartmentCrudWindow : Window
    {
        public Department? Department { get; set; }
        public DepartmentCrudWindow()
        {
            InitializeComponent();
            Department = null!;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Department is null)
            {
                Department = new Department();
                Delete_Btn.IsEnabled = false;
            }
            else
            {
                Name_TxtBx.Text = Department.Name;
                Delete_Btn.IsEnabled = true;
            }
            Id_TxtBx.Text = Department.Id.ToString();
        }
        private void Save_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Department is null)
            {
                return;
            }

            if (Name_TxtBx.Text.Equals(String.Empty))
            {
                MessageBox.Show("Введіть назву відділу");
                Name_TxtBx.Focus();
                return;
            }

            if (Name_TxtBx.Text == Department.Name)
            {
                DialogResult = false;
            }

            Department.Name = Name_TxtBx.Text;

            DialogResult = true;
        }
        private void Delete_Btn_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Впевнені, що хочете вилучити відділ?", "Вилучення", MessageBoxButton.OKCancel);
            if (dialogResult == MessageBoxResult.OK)
            {
                Department.DeleteDt = DateTime.Now;
            }
            else
            {
                return;
            }
            DialogResult = true;
        }
        private void Cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}