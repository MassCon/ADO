using ADO_NET_Lesson1.DAL;
using ADO_NET_Lesson1.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
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
    /// Interaction logic for DalWindow.xaml
    /// </summary>
    public partial class DalWindow : Window
    {
        private readonly DataContext dataContext;
        public ObservableCollection<Entities.Department> DepartmentsList { get; set; }
        public DalWindow()
        {
            InitializeComponent();
            dataContext = new DataContext();
            DepartmentsList = new(dataContext.Departments.GetAll());
            DataContext = this;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
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
                        dataContext.Departments.Update(dialog.Department);
                    }
                }
            }
        }
        private void AddDepartment_Btn_Click(object sender, RoutedEventArgs e)
        {
            DepartmentCrudWindow dialog = new();
            if (dialog.ShowDialog() == true)
            {
                dataContext.Departments.Add(dialog.Department);
            }
        }
    }
}