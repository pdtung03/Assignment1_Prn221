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
using PRN221_Assignment1_Group3.Models;
namespace PRN221_Assignment1_Group3
{
    /// <summary>
    /// Interaction logic for viewStaff.xaml
    /// </summary>
    public partial class viewStaff : Window
    {
        MyStoreContext context;
        private List<Staff> _staffs;

        public viewStaff()
        {
            context = new MyStoreContext();
            InitializeComponent();
            LoadData();
            LoadRoles();
        }
        private void LoadData()
        {
            _staffs = context.Staffs.ToList();
            StaffDataGrid.ItemsSource = _staffs;
        }

        private void LoadRoles()
        {
            var roles = _staffs.Select(s => s.Role).Distinct().ToList();
            roles.Insert(0, 0);
            RoleFilterComboBox.ItemsSource = roles;
            RoleFilterComboBox.SelectedIndex = 0;
        }

        private void RoleFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void NameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filteredStaffs = _staffs.AsQueryable();

            if (RoleFilterComboBox.SelectedIndex > 0)
            {
                var selectedRole = RoleFilterComboBox.SelectedItem.ToString();
                filteredStaffs = filteredStaffs.Where(s => s.Role == int.Parse(selectedRole));
            }

            if (!string.IsNullOrEmpty(NameSearchTextBox.Text))
            {
                filteredStaffs = filteredStaffs.Where(s => s.Name.Contains(NameSearchTextBox.Text, StringComparison.OrdinalIgnoreCase));
            }

            StaffDataGrid.ItemsSource = filteredStaffs.ToList();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            updateStaff updateForm = new updateStaff(null);
            updateForm.Show();
            LoadData();
        }

        private void StaffDataGrid_SelectedCellsChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StaffDataGrid.SelectedItem != null)
            {
                var selectedCell = StaffDataGrid.SelectedCells[0];
                int id = int.Parse(selectedCell.ToString());
                updateStaff update = new updateStaff(id);
                update.Show();
            }
        }

        private void StaffDataGrid_SelectedCellsChanged_1(object sender, SelectedCellsChangedEventArgs e)
        {

        }
    }
}
