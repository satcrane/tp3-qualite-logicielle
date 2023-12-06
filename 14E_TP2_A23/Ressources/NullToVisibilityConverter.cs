using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace _14E_TP2_A23.Ressources
{
    /// <summary>
    /// Convertisseur de valeur nulle en visibilité. Utilisé pour afficher ou non un élément en fonction de la valeur de la propriété
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
