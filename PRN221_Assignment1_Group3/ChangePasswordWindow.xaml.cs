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
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private int UserId { get; set; }

        public ChangePasswordWindow(int userId)
        {
            InitializeComponent();
            UserId = userId;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string currentPassword = CurrentPasswordBox.Password;
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("New password and confirm password do not match.");
                return;
            }

            using (var dbContext = new MyStoreContext())
            {
                var user = dbContext.Staffs.FirstOrDefault(u => u.StaffId == UserId);
                if (user != null)
                {
                    if (user.Password == currentPassword) // Assuming password is stored in plain text, which is not recommended
                    {
                        user.Password = newPassword;
                        dbContext.SaveChanges();
                        MessageBox.Show("Password changed successfully.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Current password is incorrect.");
                    }
                }
            }
        }
    }
}