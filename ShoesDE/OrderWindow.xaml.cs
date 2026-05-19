using ShoesDE.Database;
using ShoesDE.Statics;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ShoesDE
{
    public partial class OrderWindow : Window
    {
        private ShoesDEEntities _db = new ShoesDEEntities();
        private List<Order> _orders;

        public OrderWindow()
        {
            InitializeComponent();

            LoadOrders();
            LoadUser();
        }

        private void LoadUser()
        {
            var user = CurrentSession.CurrentUser;

            if (user != null)
                FullUserName.Text = $"{user.Surname} {user.Name} {user.Patronymic}";
        }

        private void LoadOrders()
        {
            _orders = _db.Order.ToList();
            OrderList.ItemsSource = _orders;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new ProductWindow().Show();
            Close();
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSession.CurrentUser.RoleId != 1)
                return;

            new AddEditOrderWindow(null).Show();
            Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentSession.CurrentUser.RoleId != 1)
                return;

            int id = (int)(sender as System.Windows.Controls.Border).Tag;

            new AddEditOrderWindow(id).Show();
            Close();
        }
    }
}