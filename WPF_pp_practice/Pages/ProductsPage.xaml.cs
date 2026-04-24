using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
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
using WPF_pp_practice.services;

namespace WPF_pp_practice.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ProductsPage(Users user)
        {
            InitializeComponent();

            ProductsListView.ItemsSource = Core.Context.Products.ToList();

            SetComboBoxItems(); 
            SetInterfaceVisibility(user?.RoleID);
            Filter(null, null);
        }

        private void SetComboBoxItems()
        {
            var ProductTypes = Core.Context.ProductTypes.ToList().Prepend(new ProductTypes { ID = 0, Name = "Все типы"});
            List<string> SortTypes = new List<string> { "По умолчанию", "По возрастанию", "По убыванию" };

            TypeComboBox.ItemsSource = ProductTypes;
            SortComboBox.ItemsSource = SortTypes;
        }

        // Установка видимости элементов интерфейса
        private void SetInterfaceVisibility(int? roleID)
        {
            switch (roleID)
            {
                case null:
                    {
                        FiltersStackPanel.Visibility = Visibility.Collapsed;
                        MaterialsButton.Visibility = Visibility.Collapsed;
                        ManagmentStackPanel.Visibility = Visibility.Collapsed;

                        break;
                    }
                case 3:
                    {
                        MaterialsButton.Visibility = Visibility.Collapsed;
                        ManagmentStackPanel.Visibility = Visibility.Collapsed;
                        break;
                    }
                case 2:
                    {
                        ManagmentStackPanel.Visibility = Visibility.Collapsed;
                        break;
                    }
                case 1:
                    {
                        break;
                    }
            }
        }

        // Фильтр
        public void Filter(object sender, EventArgs e)
        {
            if (ProductsListView is null) return;

            var products = Core.Context.Products.ToList();

            // Поиск по поисковику
            var searchText = SearchTextBox.Text.Trim().ToLower();
            products = products.Where(p => p.Name.ToLower().Contains(searchText)).ToList();

            // Поиск по типу
            var type = TypeComboBox.SelectedItem as ProductTypes;

            if(type != null && type.ID != 0)   // 0 - все типы
            {
                products = products.Where(p => p.ProductTypes == type).ToList();
            }

            // Сортировка
            var sortIndex = SortComboBox.SelectedIndex;
            switch (sortIndex)
            {
                case 1:
                    products = products.OrderBy(p => p.MinPrice).ToList();
                    break;
                case 2:
                    products = products.OrderByDescending(p => p.MinPrice).ToList();
                    break;
            }


            ProductsListView.ItemsSource = products;
        }




        // 
        // Управление продуктами
        //
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var product = ProductsListView.SelectedItem as Products;
            try
            {
                Core.Context.Products.Remove(product);
                Core.Context.SaveChanges();
                MessageHelper.ShowInfo("Продукт успешно удален!");
            }
            catch (Exception ex)
            {
                MessageHelper.ShowInfo("Продукт не удалось удалить!\n\n" + ex.Message);
            }
            Filter(null, null);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var product = ProductsListView.SelectedItem as Products;
            NavigationService.Navigate(new ProductAddEditPage(product));
            Filter(null, null);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductAddEditPage(null));
            Filter(null, null);
        }

        private void MaterialsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MaterialsPage());
        }
    }
}
