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
    /// Логика взаимодействия для MaterialAddEditPage.xaml
    /// </summary>
    public partial class MaterialAddEditPage : Page
    {
        Materials _material = new Materials();
        public MaterialAddEditPage(Materials material)
        {
            InitializeComponent();

            SetComboBoxes();

            if (material is Materials)
            {
                _material = material;
                NameTextBox.Text = material.Name;
                MinQBoxTextBox.Text = material.MinQBox.ToString();
                MinVozmTextBox.Text = material.MinVozm.ToString();
                QScladTextBox.Text = material.QSclad.ToString();
                PriceTextBox.Text = material.Price.ToString();
                MaterialTypesComboBox.SelectedValue = material.MaterialTypes;
                EdiniciComboBox.SelectedValue = material.Edinici;
            }
        }

        private void SetComboBoxes()
        {
            MaterialTypesComboBox.ItemsSource = Core.Context.MaterialTypes.ToList();
            EdiniciComboBox.ItemsSource = Core.Context.Edinici.ToList();
        }

        private StringBuilder GetValidate()
        {
            var errMsg = new StringBuilder();

            if (string.IsNullOrEmpty(NameTextBox.Text)) errMsg.AppendLine("Неверное наименование!");
            if (!Int32.TryParse(MinQBoxTextBox.Text, out int resMinQBox)  || resMinQBox < 0) errMsg.AppendLine("Неверное количесто в упаковке!");
            if (!Int32.TryParse(MinVozmTextBox.Text, out int resMinVozm) || resMinVozm < 0) errMsg.AppendLine("Неверное минимально возможное кол-во!");
            if (!Int32.TryParse(QScladTextBox.Text, out int resQSclad) || resQSclad < 0) errMsg.AppendLine("Неверное количество на складен!");
            if (!Int32.TryParse(PriceTextBox.Text, out int resPrice) || resPrice < 0) errMsg.AppendLine("Неверная цена!");
            if (EdiniciComboBox.SelectedIndex < 0) errMsg.AppendLine("Укажите единицу измерения!"); 
            if (MaterialTypesComboBox.SelectedIndex < 0) errMsg.AppendLine("Укажите тип материала!"); 

            return errMsg;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var errMsg = GetValidate();
                if (errMsg.Length > 0)
                {
                    MessageHelper.ShowInfo(errMsg.ToString());
                    return;
                }

                _material.Name = NameTextBox.Text;
                _material.Price = Int32.Parse(PriceTextBox.Text);
                _material.MinQBox = Int32.Parse(MinQBoxTextBox.Text);
                _material.QSclad = Int32.Parse(QScladTextBox.Text);
                _material.MinVozm = Int32.Parse(MinVozmTextBox.Text);
                _material.MaterialTypes = MaterialTypesComboBox.SelectedItem as MaterialTypes;
                _material.Edinici = EdiniciComboBox.SelectedItem as Edinici;

                if(_material.ID == 0)
                {
                    Core.Context.Materials.Add(_material);
                }
                Core.Context.SaveChanges();

                MessageHelper.ShowInfo("Данные успешно сохранены!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Не удалось сохранить\n\n"+ex.Message);            
            
            }

        }
    }
}
