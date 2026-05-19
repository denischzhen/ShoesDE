using ShoesDE.Database;
using ShoesDE.Helpers;
using ShoesDE.Statics;
using ShoesDE.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShoesDE
{
    public partial class ProductWindow : Window
    {
        private ShoesDEEntities _db = new ShoesDEEntities();

        private MessageHelper _mh = new MessageHelper();

        private string[] _sortingTypes =
        {
            "По умолчанию",
            "По убыванию",
            "По возрастанию"
        };

        private List<string> _filteringTypes =
            new List<string>()
            {
                "Все поставщики"
            };

        public ProductWindow()
        {
            InitializeComponent();

            LoadData();

            LoadUI();

            ApplyFilters();
        }

        private void LoadUI()
        {
            User user = CurrentSession.CurrentUser;

            if (user == null || user.RoleId == 3)
            {
                AdminPanel.Visibility = Visibility.Collapsed;

                CreateButton.Visibility = Visibility.Collapsed;
            }
            else if (user.RoleId == 2)
            {
                CreateButton.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadData()
        {
            SortingComboBox.ItemsSource = _sortingTypes;

            SortingComboBox.SelectedIndex = 0;

            var providers = _db.Provider.ToList();

            foreach (var provider in providers)
            {
                _filteringTypes.Add(provider.Name);
            }

            FilteringComboBox.ItemsSource = _filteringTypes;

            FilteringComboBox.SelectedIndex = 0;

            User user = CurrentSession.CurrentUser;

            if (user != null)
            {
                FullUserName.Text =
                    $"{user.Surname} {user.Name} {user.Patronymic}";
            }
        }

        private void ApplyFilters()
        {
            var products = _db.Product.ToList();

            string searchText = "";

            if (SearchingTextBox.Text != null)
            {
                searchText = SearchingTextBox.Text.ToLower();
            }

            products = products.Where(p =>
                p.Name.ToLower().Contains(searchText) ||
                p.Description.ToLower().Contains(searchText) ||
                p.Category.Name.ToLower().Contains(searchText) ||
                p.Provider.Name.ToLower().Contains(searchText) ||
                p.Producer.Name.ToLower().Contains(searchText) ||
                p.Unit.Name.ToLower().Contains(searchText)
            ).ToList();

            string provider = "Все поставщики";

            if (FilteringComboBox.SelectedItem != null)
            {
                provider = FilteringComboBox.SelectedItem.ToString();
            }

            if (provider != "Все поставщики")
            {
                products = products
                    .Where(p => p.Provider.Name == provider)
                    .ToList();
            }

            int sortType = SortingComboBox.SelectedIndex;

            if (sortType == 1)
            {
                products = products
                    .OrderByDescending(p => p.AmountInStock)
                    .ToList();
            }
            else if (sortType == 2)
            {
                products = products
                    .OrderBy(p => p.AmountInStock)
                    .ToList();
            }

            ProductList.ItemsSource = products
                .Select(p => new ProductViewModel(p))
                .ToList();
        }

        private void SearchingTextBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SortingComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void FilteringComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void LogOutButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentSession.CurrentUser = null;

            new MainWindow().Show();

            Close();
        }

        private void CreateButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            new AddEditProductWindow(null).Show();

            Close();
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            new OrderWindow().Show();
            Close();
        }

        private void Border_MouseDown(
            object sender,
            MouseButtonEventArgs e)
        {
            User user = CurrentSession.CurrentUser;

            if (user == null || user.RoleId != 1)
                return;

            int id = (int)(sender as Border).Tag;

            new AddEditProductWindow(id).Show();

            Close();
        }
    }
}