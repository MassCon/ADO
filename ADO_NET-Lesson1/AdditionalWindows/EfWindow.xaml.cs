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
        private static readonly Random random = new Random();
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
            UpdateDailyStatistic();
        }
        private void UpdateMonitor()
        {
            MonitorBlock.Text = "Departments: " + efContext.Departments.Count().ToString();
            MonitorBlock.Text += "\nProducts: " + efContext.Products.Count().ToString();
            MonitorBlock.Text += "\nManagers: " + efContext.Managers.Count().ToString();
            MonitorBlock.Text += "\nSales: " + efContext.Sales.Count().ToString();

        }
        private void UpdateDailyStatistic()
        {
            // Checks
            var todaySales = efContext.Sales.Where(s => s.SaleDt.Date == DateTime.Today);
            SalesChecks_Lbl.Content = todaySales.Count().ToString();

            // General quantity
            var todayQuantity = efContext.Sales.Where(s => s.SaleDt.Date == DateTime.Today).Sum(s => s.Quantity);
            SalesPcs_Lbl.Content = todayQuantity;

            // Time of first sale
            var startMoment = efContext.Sales.Where(s => s.SaleDt.Date == DateTime.Today).Min(s => s.SaleDt);
            StartMoment_Lbl.Content = startMoment.ToString("HH:mm:ss");

            // Time of last sale
            var finishMoment = efContext.Sales.Where(s => s.SaleDt.Date == DateTime.Today).Max(s => s.SaleDt);
            FinishMoment_Lbl.Content = finishMoment.ToString("HH:mm:ss");

            // Best check by quantity
            var bestCheckByQuantity = efContext.Sales.Where(s => s.SaleDt.Date == DateTime.Today).Max(s => s.Quantity);
            BestPcs_Lbl.Content = bestCheckByQuantity;

            // Average quantity by check
            var averageByQuantity = efContext.Sales.Where(s => s.SaleDt.Date == DateTime.Today).Average(s => s.Quantity);
            AvgPcs_Lbl.Content = averageByQuantity.ToString("0.00");

            // Deleted count
            var deletedCount = efContext.Sales.Where(s => s.DeleteDt.HasValue).Where(s => s.DeleteDt.Value.Date == DateTime.Today).Count();
            DeletedCount_Lbl.Content = deletedCount;

            var query3 = efContext.Products
                //.Where(s => s.SaleDt.Date == DateTime.Today)
                .GroupJoin(
                  efContext.Sales.Where(s => s.SaleDt.Date == DateTime.Today),
                  p => p.Id,
                  s => s.Product_Id,
                  (p, sales) => new
                  {
                      Name = p.Name,
                      Cnt = sales.Count()
                  }
                ).OrderByDescending(g => g.Cnt);

            BestProduct_Lbl.Content = query3.First().Name + " -- " + query3.First().Cnt;

            foreach (var item in query3)
            {
                Log_TxtBlck.Text += $"{item.Name} -- {item.Cnt}\n";
            }

            var queryMan = efContext.Managers
                .GroupJoin(
                  efContext.Sales.Where(s => s.SaleDt.Date == DateTime.Today),
                  m => m.Id,
                  s => s.Manager_Id,
                  (m, sales) => new
                  {
                      Manager = m,
                      Cnt = sales.Count()
                  }
                ).OrderByDescending(g => g.Cnt)
                .First();

            BestManager_Lbl.Content = queryMan.Manager.Surname + " " + queryMan.Manager.Name + "---" + queryMan.Cnt;

            var queryTop3 = efContext.Managers
                .GroupJoin(
                  efContext.Sales.Where(s => s.SaleDt.Date == DateTime.Today),
                  m => m.Id,
                  s => s.Manager_Id,
                  (m, sales) => new
                  {
                      Manager = m,
                      Cnt = sales.Sum(s => s.Quantity)
                  }
                ).OrderByDescending(g => g.Cnt).Take(3);

            int i = 1;

            foreach (var item in queryTop3)
            {
                Top3ManagersByItems_Lbl.Content +=
                    $"{i} - {item.Manager.Surname} {item.Manager.Name[0]}. -- {item.Cnt}\n";
            }

            var queryBestManByMoney = efContext.Managers
                .GroupJoin(
                  efContext.Sales.Where(s => s.SaleDt.Date == DateTime.Today),
                  m => m.Id,
                  s => s.Manager_Id,
                  (m, sales) => new
                  {
                      Manager = m,
                      Cnt = sales
                            .Join(efContext.Products,
                            s => s.Product_Id,
                            p => p.Id,
                            (sale, product) => sale.Quantity * product.Price).Sum()
                  }
                ).OrderByDescending(g => g.Cnt).First();

            BestManagerByMoney_Lbl.Content = $"{queryBestManByMoney.Manager.Surname} {queryBestManByMoney.Manager.Name[0]}. -- {queryBestManByMoney.Cnt} hrn";
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

        private void GenerateSales_Btn_Click(object sender, RoutedEventArgs e)
        {
            // Manager manager = efContext.Managers.ToList()[random.Next(efContext.Managers.Count())];
            // Manager manager = efContext.Managers.ElementAt(random.Next(efContext.Managers.Count()));

            double maxPrice = efContext.Products.Max(p => p.Price);
            int manCnt = efContext.Managers.Count();
            int prodCnt = efContext.Products.Count();

            for (int i = 0; i < 100; i++)
            {
                int indexM = random.Next(manCnt);
                Manager manager = efContext.Managers.Skip(indexM).First();

                int indexP = random.Next(prodCnt);
                Product product = efContext.Products.Skip(indexP).First();

                DateTime moment = DateTime.Today.AddSeconds(random.Next(0, 86400));

                int max = Convert.ToInt32(20 * (1 - product.Price / maxPrice) + 2);
                int quantity = random.Next(1, max);

                efContext.Sales.Add(new Sale()
                {
                    Id = Guid.NewGuid(),
                    Manager_Id = manager.Id,
                    Product_Id = product.Id,
                    Quantity = quantity,
                    SaleDt = moment
                });
            }
            //MessageBox.Show(manager.Surname + " " + manager.Name + "\n" + product.Name + "\n" + moment.ToString() + "\n" + product.Price + " -- " + quantity.ToString());

            efContext.SaveChanges();
            UpdateMonitor();
            UpdateDailyStatistic();
        }
    }
}