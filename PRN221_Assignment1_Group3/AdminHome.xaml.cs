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
    /// Interaction logic for AdminHome.xaml
    /// </summary>
    public partial class AdminHome : Window
    {
        private int UserId { get; set; }
        private readonly MyStoreContext _dbContext;
        public AdminHome(int userId)
        {
            InitializeComponent();

            _dbContext = new MyStoreContext();
            UserId = userId;

            var user = _dbContext.Staffs.FirstOrDefault(u => u.StaffId == UserId);

            if (user != null)
            {
                UsernameTextBlock.Text = $"Hello, {user.Name}";
            }
        }
        private void UpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            // Open a new window to update profile
            UpdateProfileWindow updateProfileWindow = new UpdateProfileWindow(UserId);
            updateProfileWindow.Show();
            this.Hide();
        }
    }
}
