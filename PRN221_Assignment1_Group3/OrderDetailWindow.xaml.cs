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
    /// Interaction logic for OrderDetailWindow.xaml
    /// </summary>
    public partial class OrderDetailWindow : Window
    {
        private readonly MyStoreContext _context;
        public OrderDetailWindow()
        {
            InitializeComponent();
            _context = new MyStoreContext();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            var orderDetails = _context.OrderDetails.ToList();
            lvOrderDetails.ItemsSource = orderDetails.Select(od => new { od.OrderDetailId, od.OrderId, od.ProductId, od.Quantity, od.UnitPrice }).ToList();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int orderDetailId = int.Parse(txtOrderDetailId.Text);
                int orderId = int.Parse(txtOrderId.Text);
                int productId = int.Parse(txtProductId.Text);
                int quantity = int.Parse(txtQuantity.Text);
                int unitPrice = int.Parse(txtUnitPrice.Text);

                // Kiểm tra nếu OrderDetailId đã tồn tại
                var orderDetailExists = _context.OrderDetails.Any(od => od.OrderDetailId == orderDetailId);
                if (orderDetailExists)
                {
                    MessageBox.Show("OrderDetailId đã tồn tại.");
                    return;
                }

                // Kiểm tra tính hợp lệ của OrderId và ProductId
                var orderExists = _context.Orders.Any(o => o.OrderId == orderId);
                var productExists = _context.Products.Any(p => p.ProductId == productId);

                if (!orderExists)
                {
                    MessageBox.Show("OrderId không tồn tại.");
                    return;
                }

                if (!productExists)
                {
                    MessageBox.Show("ProductId không tồn tại.");
                    return;
                }

                var newOrderDetail = new OrderDetail
                {

                    OrderId = orderId,
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                };

                _context.OrderDetails.Add(newOrderDetail);
                _context.SaveChanges();
                btnLoad_Click(sender, e);
                MessageBox.Show($"{newOrderDetail.OrderDetailId} inserted successfully", "Insert OrderDetail");
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input. Please ensure all fields are filled correctly.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrderDetails.SelectedItem != null)
            {
                try
                {
                    int orderId = int.Parse(txtOrderId.Text);
                    int productId = int.Parse(txtProductId.Text);
                    int quantity = int.Parse(txtQuantity.Text);
                    int unitPrice = int.Parse(txtUnitPrice.Text);

                    // Kiểm tra tính hợp lệ của OrderId và ProductId
                    var orderExists = _context.Orders.Any(o => o.OrderId == orderId);
                    var productExists = _context.Products.Any(p => p.ProductId == productId);

                    if (!orderExists)
                    {
                        MessageBox.Show("OrderId không tồn tại.");
                        return;
                    }

                    if (!productExists)
                    {
                        MessageBox.Show("ProductId không tồn tại.");
                        return;
                    }

                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        var selectedOrderDetailId = ((dynamic)lvOrderDetails.SelectedItem).OrderDetailId;
                        var selectedOrderDetail = _context.OrderDetails.Find(selectedOrderDetailId);

                        if (selectedOrderDetail != null)
                        {
                            selectedOrderDetail.OrderId = orderId;
                            selectedOrderDetail.ProductId = productId;
                            selectedOrderDetail.Quantity = quantity;
                            selectedOrderDetail.UnitPrice = unitPrice;

                            _context.SaveChanges();
                            transaction.Commit();

                            btnLoad_Click(sender, e);
                            MessageBox.Show("Order detail updated successfully!");
                        }
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Invalid input. Please ensure all fields are filled correctly.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No order detail selected to update.");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete?", "Application Deleted", MessageBoxButton.YesNo);
            if (lvOrderDetails.SelectedItem != null && result == MessageBoxResult.Yes)
            {
                var selectedOrderDetailId = ((dynamic)lvOrderDetails.SelectedItem).OrderDetailId;
                var orderDetail = _context.OrderDetails.Find(selectedOrderDetailId);

                if (orderDetail != null)
                {
                    _context.OrderDetails.Remove(orderDetail);
                    _context.SaveChanges();
                    btnLoad_Click(sender, e);
                    MessageBox.Show($"Order detail {orderDetail.OrderDetailId} deleted successfully", "Removed OrderDetail");
                }
            }
            else
            {
                MessageBox.Show("No order detail selected to delete.");
            }
        }
    }
}
