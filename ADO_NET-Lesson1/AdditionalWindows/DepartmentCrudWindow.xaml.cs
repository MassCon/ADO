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
    /// Interaction logic for DepartmentCrudWindow.xaml
    /// </summary>
    public partial class DepartmentCrudWindow : Window
    {
        public Department? Department { get; set; }
        public DepartmentCrudWindow()
        {
            InitializeComponent();
            Department = null;
        }

        private void Save_Btn_Click(object sender, RoutedEventArgs e)
        {
            Department.Name = Name_TxtBx.Text;
            DialogResult = true;
        }

        private void Delete_Btn_Click(object sender, RoutedEventArgs e)
        {
            Department = null!;
            DialogResult = true;
        }

        private void Cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Department is null)
            {
                Delete_Btn.IsEnabled = false;
            }
            else
            {
                Id_TxtBx.Text = Department.Id.ToString();
                Name_TxtBx.Text = Department.Name;
                Delete_Btn.IsEnabled = true;
            }
        }
    }
}
