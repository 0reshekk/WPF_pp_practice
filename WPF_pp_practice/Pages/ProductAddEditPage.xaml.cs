using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для ProductAddEditPage.xaml
    /// </summary>
    public partial class ProductAddEditPage : Page
    {
        string _photoName = null;
        Products _product = new Products();
        public ProductAddEditPage(Products product)
        {
            InitializeComponent();
            SetComboBoxes();
            if (product != null)
            {
                NameTextBox.Text = product.Name;
                ArticleTextBox.Text = product.Article.ToString();
                MinPriceTextBox.Text = product.MinPrice.ToString();
                ProductTypesComboBox.SelectedItem = product.ProductTypes; 
                QPeopleTextBox.Text = product.QPeople.ToString();
                CehTextBox.Text = product.Ceh.ToString();

                var filePath = System.IO.Path.Combine(Environment.CurrentDirectory, "images", product.Photo);

                if (File.Exists(filePath))
                {
                    PhotoImage.Source = new BitmapImage(new Uri(filePath));
                }
                else
                {
                    PhotoImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "images", "picture.jpg")));
                }

                _product = product;
            }
        }

        private void SetComboBoxes()
        {
            var productTypes = Core.Context.ProductTypes.ToList();
            ProductTypesComboBox.ItemsSource = productTypes;
        }

        // Валидация
        private StringBuilder GetValidate()
        {
            var errMsg = new StringBuilder();
            if (string.IsNullOrEmpty(NameTextBox.Text)) errMsg.AppendLine("Неверное наименование");
            if (!Int32.TryParse(ArticleTextBox.Text, out int resArticle) || resArticle < 0) errMsg.AppendLine("Неверный артикул");
            if (!Int32.TryParse(MinPriceTextBox.Text, out int resMinPrice) || resMinPrice < 0) errMsg.AppendLine("Неверная минимальная цена");
            if (ProductTypesComboBox.SelectedItem == null) errMsg.AppendLine("укажите тип продукта");
            if (!Int32.TryParse(QPeopleTextBox.Text, out int resQPeople) || resQPeople <= 0) errMsg.AppendLine("Неверное количество людей");
            if (!Int32.TryParse(CehTextBox.Text, out int resCeh) || resCeh < 0) errMsg.AppendLine("Неверный номер цеха");
            return errMsg;
        }

        // сохранение результата
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder errMsg = GetValidate();

                if (errMsg.Length > 0)
                {
                    MessageHelper.ShowError(errMsg.ToString());
                    return;
                }

                var name = NameTextBox.Text;
                int article = Int32.Parse(ArticleTextBox.Text);
                var minPrice = Int32.Parse(MinPriceTextBox.Text);
                var productType = ProductTypesComboBox.SelectedItem as ProductTypes;
                var qPeople = Int32.Parse(QPeopleTextBox.Text);
                var ceh = Int32.Parse(CehTextBox.Text);

                _product.Name = name;
                _product.Article = article;
                _product.MinPrice = minPrice;
                _product.ProductTypes = productType;
                _product.QPeople = qPeople;
                _product.Ceh = ceh;
                _product.Photo = _photoName;

                if(_product.ID == 0)
                {
                    Core.Context.Products.Add(_product);
                }
                Core.Context.SaveChanges();

                MessageHelper.ShowInfo("Данные успешно сохранены");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Не удалось сохранить данные\n\n" +  ex.Message);
            }
        }

        private void SelectPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.CheckFileExists = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    var fileName = openFileDialog.FileName;
                    var PhotoName = Guid.NewGuid().ToString() + ".jpg";
                    var newFileName = System.IO.Path.Combine(Environment.CurrentDirectory,"images", PhotoName);

                    System.IO.File.Copy(fileName, newFileName);

                    PhotoImage.Source = new BitmapImage(new Uri(newFileName));

                    _photoName = PhotoName;
                    MessageHelper.ShowInfo("Фото успешно обновлено!");  
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Не удалось сохранить фото\n\n" + ex.Message);
            }
        }
    }
}
