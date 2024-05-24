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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media;
using Microsoft.Extensions.Configuration;
using static System.Collections.Specialized.BitVector32;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment1_Group3.Models;


namespace PRN221_Assignment1_Group3
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        private readonly MyStoreContext _dbContext;
        public MainWindow()
		{
            InitializeComponent();
            _dbContext = new MyStoreContext();
        }
        public void ResetFormLogin()
        {
            txtBoxUsername.Text = null;
            pwdBoxPassword.Password = null;
        }
        private void Btn_login(object sender, RoutedEventArgs e)
        {
            string username = txtBoxUsername.Text;
            string password = pwdBoxPassword.Password;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                // Tìm kiếm người dùng trong cơ sở dữ liệu
                var user = _dbContext.Staffs
                                    .FirstOrDefault(u => u.Name == username && u.Password == password);

                if (user != null)
                {
                    MessageBox.Show("Login successful."); 

                        Home home = new Home(user.StaffId, user.Role, user.Name);
                        home.Show();
                    

                    this.Close(); 
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }
            }
            else
            {
                MessageBox.Show("Please enter username and password.");
            }
        }

    }
}
