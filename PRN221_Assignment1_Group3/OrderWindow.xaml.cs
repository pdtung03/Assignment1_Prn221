using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private readonly MyStoreContext _context;
        public OrderWindow()
        {
            InitializeComponent();
            _context = new MyStoreContext();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            var orders = _context.Orders.Include(o => o.Staff).ToList();
            lvOrders.ItemsSource = orders.Select(o => new { o.OrderId, o.StaffId, o.OrderDate }).ToList();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (
        int.TryParse(txtStaffId.Text, out int staffId) &&
        DateTime.TryParse(txtOrderDate.Text, out DateTime orderDate))
            {

                var existingStaff = _context.Staffs.Find(staffId);
                if (existingStaff == null)
                {
                    MessageBox.Show("StaffId does not exist. Please enter a valid StaffId.");
                    return;
                }

                var newOrder = new Order
                {
                    //OrderId = orderId,
                    StaffId = staffId,
                    OrderDate = orderDate,
                };

                _context.Orders.Add(newOrder);
                _context.SaveChanges();
                btnLoad_Click(sender, e);
                MessageBox.Show($"{newOrder.OrderId} inserted successfully", "Insert Order");
            }
            else
            {
                MessageBox.Show("Invalid input. Please ensure all fields are filled correctly.");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrders.SelectedItem != null &&
                //int.TryParse(txtStaffId.Text, out int staffId) &&
                DateTime.TryParse(txtOrderDate.Text, out DateTime orderDate))
            {
                var result = MessageBox.Show("Are you sure you want to update?", "Application Update", MessageBoxButton.YesNo);
                var selectedOrderId = ((dynamic)lvOrders.SelectedItem).OrderId;
                var order = _context.Orders.Find(selectedOrderId);

                if (order != null && result == MessageBoxResult.Yes)
                {
                    //order.StaffId = staffId;
                    order.OrderDate = orderDate;

                    _context.SaveChanges();
                    btnLoad_Click(sender, e);
                    MessageBox.Show($"Order {order.OrderId} updated successfully", "Updated Order");
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete?", "Application Deleted", MessageBoxButton.YesNo);
            if (lvOrders.SelectedItem != null && result == MessageBoxResult.Yes)
            {
                var selectedOrderId = ((dynamic)lvOrders.SelectedItem).OrderId;
                var order = _context.Orders.Find(selectedOrderId);

                if (order != null)
                {
                    _context.Orders.Remove(order);
                    _context.SaveChanges();
                    btnLoad_Click(sender, e);
                    MessageBox.Show($"Order {order.OrderId} deleted successfully", "Removed Order");
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Lấy ngày đặt hàng từ DatePicker
            DateTime searchDate = dpSearchOrderDate.SelectedDate ?? DateTime.MinValue;

            // Nếu ngày không được chọn, hiển thị thông báo và thoát
            if (searchDate == DateTime.MinValue)
            {
                MessageBox.Show("Please select a valid date.");
                return;
            }

            // Tìm các đơn hàng có ngày đặt hàng trùng khớp với ngày tìm kiếm
            var orders = _context.Orders.Where(o => o.OrderDate.Date == searchDate.Date).ToList();

            // Hiển thị kết quả trên ListView
            lvOrders.ItemsSource = orders;
        }

        private void btnOrderDetail_Click(object sender, RoutedEventArgs e)
        {
            OrderDetailWindow orderDetailWindow = new OrderDetailWindow();

            // Hiển thị OrderDetailWindow
            orderDetailWindow.Show();
        }
    }
}
