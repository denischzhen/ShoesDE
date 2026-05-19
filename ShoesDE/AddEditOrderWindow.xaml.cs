using ShoesDE.Database;
using ShoesDE.Statics;
using System;
using System.Linq;
using System.Windows;

namespace ShoesDE
{
    public partial class AddEditOrderWindow : Window
    {
        private ShoesDEEntities _db = new ShoesDEEntities();
        private Order _order;
        private bool _isEdit;

        public AddEditOrderWindow(int? id)
        {
            InitializeComponent();

            StatusBox.ItemsSource = _db.OrderStatus.ToList();
            PickupBox.ItemsSource = _db.PickUpPoint.ToList();

            if (id == null)
            {
                _isEdit = false;
                DeleteButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                _isEdit = true;
                _order = _db.Order.Find(id);
                LoadData();
            }
        }

        private void LoadData()
        {
            ReceiptCodeBox.Text = _order.ReceiptCode;
            CreationDateBox.SelectedDate = _order.CreationDate;
            DeliveryDateBox.SelectedDate = _order.DeliveryDate;

            StatusBox.SelectedItem = _order.OrderStatus;
            PickupBox.SelectedItem = _order.PickUpPoint;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!_isEdit)
            {
                _order = new Order
                {
                    ReceiptCode = ReceiptCodeBox.Text,
                    CreationDate = CreationDateBox.SelectedDate ?? DateTime.Now,
                    DeliveryDate = DeliveryDateBox.SelectedDate ?? DateTime.Now,
                    StatusId = (StatusBox.SelectedItem as OrderStatus).Id,
                    PickUpPointId = (PickupBox.SelectedItem as PickUpPoint).Id,
                    UserId = CurrentSession.CurrentUser.Id
                };

                _db.Order.Add(_order);
            }
            else
            {
                _order.ReceiptCode = ReceiptCodeBox.Text;
                _order.CreationDate = CreationDateBox.SelectedDate ?? DateTime.Now;
                _order.DeliveryDate = DeliveryDateBox.SelectedDate ?? DateTime.Now;
                _order.StatusId = (StatusBox.SelectedItem as OrderStatus).Id;
                _order.PickUpPointId = (PickupBox.SelectedItem as PickUpPoint).Id;
            }

            _db.SaveChanges();

            new OrderWindow().Show();
            Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (_order == null) return;

            _db.Order.Remove(_order);
            _db.SaveChanges();

            new OrderWindow().Show();
            Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new OrderWindow().Show();
            Close();
        }
    }
}