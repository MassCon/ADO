using ADO_NET_Lesson1.EFCore;
using ADO_NET_Lesson1.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        public EfWindow()
        {
            InitializeComponent();
            DataContext = efContext;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MonitorBlock.Text = "Departments: " + efContext.Departments.Count().ToString();
            efContext.Departments.Load();
            Departments_LstVw.ItemsSource = efContext.Departments.Local.ToObservableCollection();
        }

        private void AddDepartment_Click(object sender, RoutedEventArgs e)
        {
            DepartmentCrudWindow dialog = new();
            if (dialog.ShowDialog() == true)
            {
                // dialog.Department - інша сутність, треба змінити під EF
                efContext.Departments.Add(
                    new EFCore.Department()
                    {
                        Name = dialog.Department.Name,
                        Id = dialog.Department.Id
                    });
                efContext.SaveChanges();
                MonitorBlock.Text += "\nDepartments: " + efContext.Departments.Count().ToString();
            }
        }
    }
}