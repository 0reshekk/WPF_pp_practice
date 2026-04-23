using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_pp_practice.services
{
    static internal class MessageHelper
    {
        static public void ShowError(string text)
        {
            MessageBox.Show(text, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        static public void ShowInfo(string text)
        {
            MessageBox.Show(text, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
