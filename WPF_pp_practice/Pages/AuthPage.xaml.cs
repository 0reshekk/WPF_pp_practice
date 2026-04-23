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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }
        private void AuthAsGuestButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductsPage(null));
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginTextBox.Text;
            var password = PasswordPasswordBox.Password;

            if (string.IsNullOrEmpty(login))
            {
                MessageHelper.ShowInfo("Пустой логин!");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageHelper.ShowInfo("Пустой пароль!");
                return;
            }

            var user = Core.Context.Users.AsNoTracking().FirstOrDefault(u => u.Login == login);
            if (user == null)
            {
                MessageHelper.ShowInfo("Пользователь не найден!");
                return;
            }
            if (user.Password != password)
            {
                MessageHelper.ShowInfo("Пароль не совпадает!");
                return;
            }
            
            MainWindow mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow.ChangeUsername(user.Name);

            NavigationService.Navigate(new ProductsPage(user));
        }
    }
}
