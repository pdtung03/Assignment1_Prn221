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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        private int UserId { get; set; }
        private int UserRole { get; set; } // 1 for Admin, 2 for Staff

        public Home(int userId, int userRole, string username)
        {
            InitializeComponent();
            UserId = userId;
            UserRole = userRole;
            UpdateUsername(username); // Initialize username
            ConfigureUserInterface();
        }

        private void UpdateUsername(string username)
        {
            UsernameTextBlock.Text = $"Hello, {username}";
        }

        private void ConfigureUserInterface()
        {
            if (UserRole == 1) // Admin
            {
                AdminPanel.Visibility = Visibility.Visible;
                StaffPanel.Visibility = Visibility.Collapsed;
            }
            else // Staff
            {
                AdminPanel.Visibility = Visibility.Collapsed;
                StaffPanel.Visibility = Visibility.Visible;
            }
        }

        private void UpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            UpdateProfileWindow updateProfileWindow = new UpdateProfileWindow(UserId);
            updateProfileWindow.ShowDialog();
            using (var dbContext = new MyStoreContext())
            {
                var user = dbContext.Staffs.FirstOrDefault(u => u.StaffId == UserId);
                if (user != null)
                {
                    UpdateUsername(user.Name);
                }
            }
        }

        private void ViewOrders_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow(UserId);
            orderWindow.Show();
        }
        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow(UserId);
            changePasswordWindow.ShowDialog();
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            var MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }

        private void ManageProducts_Click(object sender, RoutedEventArgs e)
        {
            // Implement product management functionality
            MessageBox.Show("Product management functionality is not implemented yet.");
        }

        private void ManageStaff_Click(object sender, RoutedEventArgs e)
        {
            // Implement staff management functionality
            MessageBox.Show("Staff management functionality is not implemented yet.");
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            // Implement reports functionality
            MessageBox.Show("Reports functionality is not implemented yet.");
        }
    }
}
