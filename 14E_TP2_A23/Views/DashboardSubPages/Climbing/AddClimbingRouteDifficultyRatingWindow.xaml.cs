using _14E_TP2_A23.Helpers;
using _14E_TP2_A23.Models;
using _14E_TP2_A23.ViewModels.ClimbingViewModels;
using System.Windows;

namespace _14E_TP2_A23.Views.DashboardSubPages.Climbing
{
    /// <summary>
    /// Logique d'interaction pour AddClimbingRouteDifficultyRatingWindow.xaml
    /// </summary>
    public partial class AddClimbingRouteDifficultyRatingWindow : Window
    {
        public AddClimbingRouteDifficultyRatingViewModel _addClimbingRouteDifficultyRatingWindowViewModel = ServiceHelper.GetService<AddClimbingRouteDifficultyRatingViewModel>();

        public AddClimbingRouteDifficultyRatingWindow()
        {
            InitializeComponent();
            this.DataContext = _addClimbingRouteDifficultyRatingWindowViewModel;
        }

        /// <summary>
        /// Bouton ajouter
        /// </summary>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            _addClimbingRouteDifficultyRatingWindowViewModel.AddClimbingRouteDifficultyRatingCommand.Execute(null);
        }

        /// <summary>
        /// Bouton annuler
        /// </summary>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
