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

        public viewStaff()
        {
            context = new MyStoreContext();
            InitializeComponent();
            LoadData();
            LoadRoles();
        }
        private void LoadData()
        {
            var _staffs = context
             .Staffs
             .Select(s => new
             {
                 StaffId = s.StaffId,
                 Name = s.Name,
                 Password = "*******",
                 Role = s.Role == 1 ? "Admin" : "Staff"
             })
             .ToList();
            StaffDataGrid.ItemsSource = _staffs;
        }

        // Phương thức để tải các vai trò khác nhau để lọc
        private void LoadRoles()
        {
            var listRole = context.Staffs.Select(s => new
            {
                roleId = s.Role,
                name = s.Role == 1 ? "Admin" : "Staff"
            }).Distinct().ToList();

            listRole.Insert(0, new { roleId = 0, name = "All" });

            RoleFilterComboBox.ItemsSource = listRole;
            RoleFilterComboBox.DisplayMemberPath = "name";
            RoleFilterComboBox.SelectedValuePath = "roleId";
            RoleFilterComboBox.SelectedIndex = 0;

        }

        // Xử lý sự kiện khi thay đổi lựa chọn trong ComboBox lọc vai trò
        private void RoleFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        // Xử lý sự kiện khi thay đổi văn bản tìm kiếm tên
        private void NameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        // Phương thức để áp dụng các bộ lọc dựa trên vai trò và tên
        private void ApplyFilters()
        {
            var filteredStaffs = context
                        .Staffs
                        .Select(s => new
                        {
                            StaffId = s.StaffId,
                            Name = s.Name,
                            Password = "*******",
                            Role = s.Role == 1 ? "Admin" : "Staff"
                        })
                        .ToList()
                        .AsQueryable();

            if (RoleFilterComboBox.SelectedIndex > 0)
            {
                var selectedRole = int.Parse(RoleFilterComboBox.SelectedValue.ToString()) == 1 ? "Admin" : "Staff";
                filteredStaffs = filteredStaffs.Where(s => s.Role == selectedRole);
            }

            if (!string.IsNullOrEmpty(NameSearchTextBox.Text))
            {
                filteredStaffs = filteredStaffs.Where(s => s.Name.Contains(NameSearchTextBox.Text, StringComparison.OrdinalIgnoreCase));
            }

            StaffDataGrid.ItemsSource = filteredStaffs.ToList();
        }

        // Xử lý sự kiện khi nhấn nút Tạo mới
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            updateStaff updateForm = new updateStaff(null);
            this.Close();
            updateForm.Show();
            LoadData();
        }

        // Xử lý sự kiện khi nhấn chuột trái lên DataGrid
        private void StaffDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (StaffDataGrid.SelectedItem != null)
            {
                // Lấy hàng được chọn
                var selectedItem = StaffDataGrid.SelectedItem;

                // Lấy nội dung của ô đầu tiên trong hàng được chọn
                var firstCell = StaffDataGrid.SelectedCells.First();
                var firstCellContent = firstCell.Column.GetCellContent(selectedItem);

                if (firstCellContent is TextBlock textBlock)
                {
                    int id;
                    if (int.TryParse(textBlock.Text, out id))
                    {
                        updateStaff update = new updateStaff(id);
                        this.Close();
                        update.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Staff ID", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Unable to retrieve the cell content.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}
