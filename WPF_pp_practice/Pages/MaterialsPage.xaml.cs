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
using WPF_pp_practice.services;

namespace WPF_pp_practice.Pages
{
    /// <summary>
    /// Логика взаимодействия для MaterialsPage.xaml
    /// </summary>
    public partial class MaterialsPage : Page
    {
        public MaterialsPage()
        {
            InitializeComponent();
            ResetDataGridItems();
        }

        private void ResetDataGridItems()
        {
            var materials = Core.Context.Materials.ToList();
            MaterialsDataGrid.ItemsSource = materials;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var item = MaterialsDataGrid.SelectedItem as Materials;
            if(item == null)
            {
                MessageHelper.ShowInfo("Выберите материал!");
                return;
            }

            NavigationService.Navigate(new MaterialAddEditPage(item));

            ResetDataGridItems();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MaterialAddEditPage(null));

            ResetDataGridItems();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var item = MaterialsDataGrid.SelectedItem as Materials;
            if (item == null)
            {
                MessageHelper.ShowInfo("Выберите материал!");
                return;
            }

            Core.Context.Materials.Remove(item);
            Core.Context.SaveChanges();

            ResetDataGridItems();
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
