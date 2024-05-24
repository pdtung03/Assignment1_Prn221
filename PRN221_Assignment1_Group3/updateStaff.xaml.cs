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
    /// Interaction logic for updateStaff.xaml
    /// </summary>
    public partial class updateStaff : Window
    {
        private int staffId;
        MyStoreContext context = new MyStoreContext();
        public updateStaff(int? id)
        {
            InitializeComponent();
            id = id == null ? 0 : id;
            staffId = (int)id;
            RoleComboBox.ItemsSource = context.Staffs.Select(x => x.Role).Distinct().ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var id = StaffIdTextBox.Text;
            var name = NameTextBox.Text;
            var pass = PasswordBox.Password;
            var role = RoleComboBox.SelectedValue.ToString();



            try
            {
                Staff s = new Staff()
                {
                    Name = name,
                    Password = pass,
                    Role = int.Parse(role)
                };
                context.Staffs.Add(s);
                context.SaveChanges();
                MessageBox.Show("Add succssfuly!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
