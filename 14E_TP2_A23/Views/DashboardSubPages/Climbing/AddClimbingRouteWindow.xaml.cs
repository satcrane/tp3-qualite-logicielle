using _14E_TP2_A23.Helpers;
using _14E_TP2_A23.ViewModels.ClimbingViewModels;
using System.Collections.Generic;
using System.Windows;

namespace _14E_TP2_A23.Views.DashboardSubPages.Climbing
{
    /// <summary>
    /// Logique d'interaction pour AddClimbingRouteWindow.xaml
    /// </summary>
    public partial class AddClimbingRouteWindow : Window
    {
        #region Propriétés
        AddClimbingRouteViewModel _addClimbingRouteViewModel = ServiceHelper.GetService<AddClimbingRouteViewModel>();

        #endregion

        #region Contructeur
        public AddClimbingRouteWindow()
        {
            InitializeComponent();
            cbHoldsColor.ItemsSource = _GetColors();
            this.DataContext = _addClimbingRouteViewModel;
        }

        #endregion

        #region Méthodes
        /// <summary>
        /// Retourne une liste de couleurs
        /// </summary>
        private List<string> _GetColors()
        {
            return new List<string>
            {
                "Rouge",
                "Bleu",
                "Vert",
                "Jaune",
                "Noir",
                "Blanc",
                "Orange",
                "Rose",
                "Violet",
                "Marron",
                "Gris",
                "Beige",
                "Autre"
            };
        }

        /// <summary>
        /// Événement ajout d'une voie d'escalade
        /// </summary>
        private void btnAddClimbingRoute_Click(object sender, RoutedEventArgs e)
        {
            _addClimbingRouteViewModel.AddClimbingRouteCommand.Execute(null);
        }

        /// <summary>
        /// Événement annulation de l'ajout d'une voie d'escalade
        /// </summary>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        #endregion
    }
}
