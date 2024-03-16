using ADO_NET_Lesson1.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ManagerCrudWindow.xaml
    /// </summary>
    public partial class ManagerCrudWindow : Window
    {
        public Manager? Manager { get; set; }
        private ObservableCollection<Entities.Department> OwnerDepartments;
        public ManagerCrudWindow()
        {
            InitializeComponent();
            Manager = null!;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Owner is OrmWindow owner)
            {
                DataContext = Owner;
                OwnerDepartments = owner.departments;
            }
            else
            {
                MessageBox.Show("Owner is not OrmWindow");
                Close();
            }

            if (Manager is null)
            {
                Manager = new Entities.Manager();
                Delete_Btn.IsEnabled = false;
            }
            else
            {
                Surname_TxtBx.Text = Manager.Surname;
                Secname_TxtBx.Text = Manager.Secname;
                Name_TxtBx.Text = Manager.Name;
                MainDep_CmbBx.SelectedItem =
                    OwnerDepartments
                    .Where(d => d.Id == this.Manager.IdMainDep)
                    .First();
                SecDep_CmbBx.SelectedItem =
                   OwnerDepartments
                   .Where(d => d.Id == this.Manager.IdSecDep)
                   .FirstOrDefault();
                Chief_CmbBx.SelectedItem = (Owner as OrmWindow)?
                    .managers.FirstOrDefault(m => m.Id == this.Manager.IdChief);
                Delete_Btn.IsEnabled = true;
            }

            Id_TxtBx.Text = Manager.Id.ToString();
        }
        private void Save_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Manager is null)
                return;

            if (Name_TxtBx.Text.Equals(String.Empty))
            {
                MessageBox.Show("Необхідно ввести ім'я");
                Name_TxtBx.Focus();
                return;
            }

            if (Surname_TxtBx.Text.Equals(String.Empty))
            {
                MessageBox.Show("Необхідно ввести прізвище");
                Surname_TxtBx.Focus();
                return;
            }

            if (MainDep_CmbBx.SelectedItem is null)
            {
                MessageBox.Show("Необхідно вибрати робочий відділ");
                MainDep_CmbBx.Focus();
                return;
            }

            Manager.Surname = Surname_TxtBx.Text;
            Manager.Name = Name_TxtBx.Text;
            Manager.Secname = Secname_TxtBx.Text;

            if (MainDep_CmbBx.SelectedItem is Entities.Department mainDep)
                Manager.IdMainDep = mainDep.Id;
            else
                MessageBox.Show("MainDepComboBox.SelectedItem CAST Error");

            if (SecDep_CmbBx.SelectedItem is null)
                Manager.IdSecDep = null;
            else if (SecDep_CmbBx.SelectedItem is Entities.Department secDep)
                Manager.IdSecDep = secDep.Id;
            else
                MessageBox.Show("SecDep_CmbBx.SelectedItem CAST Error");

            if (Chief_CmbBx.SelectedItem is null)
                Manager.IdChief = null;
            else if (Chief_CmbBx.SelectedItem is Entities.Manager chief)
                Manager.IdChief = chief.Id;
            else
                MessageBox.Show("Chief_CmbBx.SelectedItem CAST Error");

            DialogResult = true;
        }
        private void Delete_Btn_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Впевнені, що хочете вилучити співробітника?", "Вилучення", MessageBoxButton.OKCancel);
            if (dialogResult == MessageBoxResult.OK)
            {
                Manager.DeleteDt = DateTime.Now;
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

        private void RemoveSecDep_Btn_Click(object sender, RoutedEventArgs e)
        {
            SecDep_CmbBx.SelectedItem = null;
        }
        private void RemoveChief_Btn_Click(object sender, RoutedEventArgs e)
        {
            Chief_CmbBx.SelectedItem = null;
        }
    }
}