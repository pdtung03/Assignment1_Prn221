using PRN221_Assignment1_Group3.Models;
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

namespace PRN221_Assignment1_Group3
{
    /// <summary>
    /// Interaction logic for UpdateProfileWindow.xaml
    /// </summary>
    public partial class UpdateProfileWindow : Window
    {
        private readonly MyStoreContext _dbContext;
        private int UserId { get; set; }
        public UpdateProfileWindow(int userId)
        {
            InitializeComponent();
            _dbContext = new MyStoreContext();
            UserId = userId;

            // Load user information based on UserId
            var user = _dbContext.Staffs.FirstOrDefault(u => u.StaffId == userId);
            if (user != null)
            {
                txtName.Text = user.Name;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var user = _dbContext.Staffs.FirstOrDefault(u => u.StaffId == UserId);
            if (user != null)
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Please enter Name.");
                    return;
                }
                user.Name = txtName.Text;

                _dbContext.SaveChanges();
                MessageBox.Show("Profile updated successfully.");
                this.Close();
                Home home = new Home(user.StaffId, user.Role, user.Name);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var user = _dbContext.Staffs.FirstOrDefault(u => u.StaffId == UserId);
            Home home = new Home(user.StaffId, user.Role, user.Name);
            this.Close();
            home.Show();
        }
    }
}
