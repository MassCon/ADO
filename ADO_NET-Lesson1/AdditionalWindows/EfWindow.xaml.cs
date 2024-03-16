using ADO_NET_Lesson1.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for EfWindow.xaml
    /// </summary>
    public partial class EfWindow : Window
    {
        public EFContext efContext { get; set; } = new();
        private ICollectionView depListView;
        public EfWindow()
        {
            InitializeComponent();
            DataContext = efContext;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //MonitorBlock.Text = "Departments: " + efContext.Departments.Count().ToString();
            efContext.Departments.Load();
            Departments_LstVw.ItemsSource = efContext.Departments.Local.ToObservableCollection();
            depListView = CollectionViewSource.GetDefaultView(Departments_LstVw.ItemsSource);
            depListView.Filter =
                obj => (obj as Department)?.DeleteDt == null;

            UpdateMonitor();
        }
        private void UpdateMonitor()
        {
            MonitorBlock.Text = "Departments: " + efContext.Departments.Count().ToString();
            MonitorBlock.Text += "\nProducts: " + efContext.Products.Count().ToString();
            MonitorBlock.Text += "\nManagers: " + efContext.Managers.Count().ToString();
            MonitorBlock.Text += "\nSales: " + efContext.Sales.Count().ToString();

        }

        private void AddDepartment_Click(object sender, RoutedEventArgs e)
        {
            DepartmentCrudWindow dialog = new();
            if (dialog.ShowDialog() == true)
            {
                // dialog.Department - інша сутність, треба змінити під EF
                efContext.Departments.Add(
                    new Department()
                    {
                        Name = dialog.Department.Name,
                        Id = dialog.Department.Id
                    });
                efContext.SaveChanges();
                MonitorBlock.Text += "\nDepartments: " + efContext.Departments.Count().ToString();
            }
        }
        private bool DepartmentsDeletedFilter(object item)
        {
            if (item is Department department)
            {
                return department.DeleteDt == null;
            }
            return false;
        }

        private void ShowAllDeps_ChckBx_Checked(object sender, RoutedEventArgs e)
        {
            depListView.Filter = null;
            ((GridView)Departments_LstVw.View).Columns[2].Width = Double.NaN;
        }

        private void ShowAllDeps_ChckBx_Unchecked(object sender, RoutedEventArgs e)
        {
            depListView.Filter = DepartmentsDeletedFilter;
            ((GridView)Departments_LstVw.View).Columns[2].Width = 0;
        }

        private void Departments_LstVw_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView item)
            {
                if (item.SelectedItem is EFCore.Department department)
                {
                    EF_DepartmentCrud_Window dialog = new EF_DepartmentCrud_Window();
                    dialog.Department = department;
                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog.Department is not null)
                        {
                            efContext.Departments.Where(d => d.Id == dialog.Department.Id).First().Name = dialog.Department.Name;
                            efContext.Departments.Where(d => d.Id == dialog.Department.Id).First().DeleteDt = dialog.Department.DeleteDt;
                            efContext.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}