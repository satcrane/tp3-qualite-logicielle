using _14E_TP2_A23.Helpers;
using _14E_TP2_A23.Models;
using _14E_TP2_A23.ViewModels.DashboardViewModels;
using System.Linq;
using System.Windows.Controls;

namespace _14E_TP2_A23.Views.DashboardSubPages
{
    /// <summary>
    /// Logique d'interaction pour ManageClimbingWallsPage.xaml
    /// </summary>
    public partial class ManageClimbingWallsPage : Page
    {
        ManageClimbingWallsViewModel _manageClimbingWallsViewModel = ServiceHelper.GetService<ManageClimbingWallsViewModel>();
        public ManageClimbingWallsPage()
        {
            InitializeComponent();
            DataContext = _manageClimbingWallsViewModel;

            FillListViewClimbingWalls();
        }

        /// <summary>
        /// Remplir le list view des murs d'escalade
        /// </summary>
        private async void FillListViewClimbingWalls()
        {
            _manageClimbingWallsViewModel.ClimbingWalls = await _manageClimbingWallsViewModel.GetAllClimbingWalls();
        }

        /// <summary>
        /// Événement de sélection d'un mur d'escalade
        /// </summary>
        private void lvClimbingWalls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvClimbingWalls.SelectedItem is ClimbingWall selectedWall)
            {
                _manageClimbingWallsViewModel.SelectedClimbingRoute = null;
                UpdateListViewClimbingRoutes(selectedWall.Id);
            }
        }


        /// <summary>   
        /// Événement de sélection d'une voie d'escalade
        /// </summary>
        private void lvClimbingRoutes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvClimbingRoutes.SelectedItem is ClimbingRoute selectedRoute)
            {
                HandleAverageDifficultyRating(selectedRoute);
                _manageClimbingWallsViewModel.SelectedClimbingRoute = selectedRoute;
            }
        }

        ///<summary>
        /// Mettre à jour la Listview des vois d'escalade.
        /// Appelé après la sélection d'un mur d'escalade.
        /// </summary>
        /// <param name="wallId">Id du mur sélectionné</param>
        private async void UpdateListViewClimbingRoutes(string wallId)
        {
            var climbingRoutes = await _manageClimbingWallsViewModel.GetAllClimbingRoutes();
            var climbingRouteAssignedToCurrentWall = climbingRoutes?.Where(cr => cr.WallId == wallId).FirstOrDefault();

            if (climbingRoutes == null)
            {
                return;
            }

            foreach (var route in climbingRoutes)
            {
                // Assigner IsAssignedToCurrentAWall à true si la voie d'escalade est assignée au mur sélectionné
                if (route.WallId == wallId)
                {
                    route.IsAssignedToCurrentAWall = true;
                }

                // Assigner IsAssignedToAWall à true si la voie d'escalade est assignée à un mur
                route.IsAssignedToAWall = route.WallId != null;

                // Assigner WallNameRouteIsAssigned 
                var walls = _manageClimbingWallsViewModel.ClimbingWalls?.Any();
                if (walls != null)
                {
                    route.WallNameRouteIsAssigned = _manageClimbingWallsViewModel.ClimbingWalls?.Where(w => w.Id == route.WallId).FirstOrDefault()?.Location;

                }
            }


            lvClimbingRoutes.SelectedItem = climbingRouteAssignedToCurrentWall;
            lvClimbingRoutes.ScrollIntoView(climbingRouteAssignedToCurrentWall);
            _manageClimbingWallsViewModel.ClimbingRoutes = climbingRoutes;
        }

        /// <summary>
        /// Calculer la difficulté moyenne de la voie et colorer la difficulté moyenne de la voie si elle est trop différente de la difficulté du mur
        /// </summary>
        /// <param name="selectedRoute">La voie séléctionée</param>
        private void HandleAverageDifficultyRating(ClimbingRoute selectedRoute)
        {
            if (selectedRoute.DifficultyRatings.Any() == false) { return; }

            // Calculer la difficulté moyenne de la voie
            var averageDifficultyRating = selectedRoute.DifficultyRatings.Average();
            selectedRoute.AverageDifficultyRating = averageDifficultyRating;

            // Colorer la difficulté moyenne de la voie si elle est trop différente de la difficulté du mur
            if (averageDifficultyRating - selectedRoute.Difficulty > 2 || selectedRoute.Difficulty - averageDifficultyRating > 2)
            {
                txtAverageDifficultyRating.Background = System.Windows.Media.Brushes.Red;
            }
        }

        /// <summary>
        /// Bouton ajouter une voie d'escalade
        /// </summary>
        private void btnAddRoute_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _manageClimbingWallsViewModel.ShowCreateClimbingWallWindowCommand.Execute(null);
        }

        /// <summary>
        /// Bouton désassigner une voie d'escalade
        /// </summary>
        private void btnUnasignRoute_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedRouteToUnassign = lvClimbingRoutes.SelectedItem as ClimbingRoute;
            if (selectedRouteToUnassign == null) { return; }

            var unassignConfirmation = System.Windows.MessageBox.Show($"Voulez-vous vraiment désassigner la voie d'escalade {selectedRouteToUnassign.Name} du mur ?", "Confirmation", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
            if (unassignConfirmation == System.Windows.MessageBoxResult.No) { return; }

            _manageClimbingWallsViewModel.UnassignClimbingRouteCommand.Execute(selectedRouteToUnassign);
        }

        /// <summary>
        /// Bouton assigner voie d'escalade sélectionnée à un mur d'escalade sélectionné
        /// </summary>
        private void btnAssignRouteToSelectedWall_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var assignConfirmation = System.Windows.MessageBox.Show($"Voulez-vous vraiment assigner la voie d'escalade {_manageClimbingWallsViewModel.SelectedClimbingRoute?.Name} au mur {_manageClimbingWallsViewModel.SelectedClimbingWall?.Location} ?", "Confirmation", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
            if (assignConfirmation == System.Windows.MessageBoxResult.No) { return; }

            _manageClimbingWallsViewModel.AssignClimbingRouteToClimbingWallCommand.Execute(null);
        }

        /// <summary>
        /// Bouton afficher fenêtre ajouter évaluation de difficulté d'une voie d'escalade
        /// </summary>
        private void btnRateRouteDifficulty_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _manageClimbingWallsViewModel.ShowAddRateRouteDifficultyWindowCommand.Execute(null);
        }

        /// <summary>
        /// Bouton retour en arrière
        /// </summary>
        private void btnBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _manageClimbingWallsViewModel.GoBackCommand.Execute(null);
        }

    }
}
