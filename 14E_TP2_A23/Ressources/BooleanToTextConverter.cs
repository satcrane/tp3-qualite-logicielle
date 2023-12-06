using System;
using System.Globalization;
using System.Windows.Data;

namespace _14E_TP2_A23.Ressources
{
    /// <summary>
    /// Convertisseur de booléen en texte.
    /// Utilisé pour la coloration des voies dans 'ManageClimbingWallsPage.xaml'.
    /// </summary>
    public class BooleanToTextConverter : IValueConverter
    {
        /// <summary>
        /// Convertir un booléen en texte. Utilisé pour la coloration des voies dans 'ManageClimbingWallsPage.xaml'
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isAssigned = (bool)value;
            string param = parameter as string;

            if (isAssigned)
            {
                return param == "Current Wall" ? "Assigné au mur actuel" : "Utilisé";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
