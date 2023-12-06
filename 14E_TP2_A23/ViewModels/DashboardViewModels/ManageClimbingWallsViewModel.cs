using _14E_TP2_A23.Models;
using _14E_TP2_A23.Services.ClimbingWalls;
using _14E_TP2_A23.Services.Navigation;
using _14E_TP2_A23.Views.DashboardSubPages.Climbing;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace _14E_TP2_A23.ViewModels.DashboardViewModels
{
    /// <summary>
    /// View model de ManageClimbingWallsPage.xaml
    /// </summary>
    public partial class ManageClimbingWallsViewModel : ObservableValidator
    {
        #region Propriétés
        /// <summary>
        /// Service de navigation injecté par le service provider
        /// </summary>
        private readonly IAppNavigationService _appNavigtionService;

        /// <summary>
        /// Service de gestion des murs d'escalade injecté par le service provider
        /// </summary>
        private readonly IClimbingManagementService _climbingManagementService;

        /// <summary>
        /// Liste des murs d'escalade
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<ClimbingWall>? _climbingWalls;

        /// <summary>
        /// Liste des voies d'escalade
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<ClimbingRoute>? _climbingRoutes;

        /// <summary>
        /// Mur sélectionné dans le list view
        /// </summary>
        [ObservableProperty]
        private ClimbingWall? _selectedClimbingWall;

        /// <summary>
        /// Voie sélectionnée dans le list view des voies
        /// </summary>
        [ObservableProperty]
        private ClimbingRoute? _selectedClimbingRoute;

        #endregion

        #region Constructeur
        public ManageClimbingWallsViewModel(IAppNavigationService appNavigtionService,
            IClimbingManagementService climbingManagementService)
        {
            _appNavigtionService = appNavigtionService;
            _climbingManagementService = climbingManagementService;
        }
        #endregion

        #region Méthodes
        /// <summary>
        /// Récupérer tous les murs d'escalade de la base de données
        /// </summary>
        public async Task<ObservableCollection<ClimbingWall>?> GetAllClimbingWalls()
        {
            try
            {
                return await _climbingManagementService.GetAllClimbingWalls();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des murs d'escalades : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Récupérer tous les murs d'escalade de la base de données
        /// </summary>
        public async Task<ObservableCollection<ClimbingRoute>?> GetAllClimbingRoutes()
        {
            try
            {
                return await _climbingManagementService.GetAllClimbingRoutes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des murs d'escalades : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        #endregion

        #region Commandes
        /// <summary>
        /// Afficher fenêtre de création d'un mur d'escalade
        /// </summary>
        [RelayCommand]
        public void ShowCreateClimbingWallWindow()
        {
            // afficher la fenêtre de création d'un mur d'escalade
            AddClimbingRouteWindow addClimbingRouteWindow = new AddClimbingRouteWindow();
            var result = addClimbingRouteWindow.ShowDialog();

            if (!result.HasValue || !result.Value) return;

            LoadData();
        }

        /// <summary>
        /// Commande déassigner une voie d'escalade à un mur
        /// </summary>
        /// <param name="climbingRoute">Voie d'escalade à déassigner</param>
        [RelayCommand]
        public async Task UnassignClimbingRoute(ClimbingRoute climbingRoute)
        {
            try
            {
                var result = await _climbingManagementService.UnassignClimbingRoute(climbingRoute);

                if (result)
                {
                    // Recharger les routes d'escalade
                    ClimbingRoutes = await _climbingManagementService.GetAllClimbingRoutes();
                    MessageBox.Show("Voie d'escalade déassignée avec succès", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Erreur lors de la déassignation de la voie d'escalade", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la déassignation de la voie d'escalade : {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Commande assigner une voie d'escalade à un mur
        /// </summary>
        [RelayCommand]
        public async Task AssignClimbingRouteToClimbingWall()
        {
            try
            {
                if (SelectedClimbingRoute == null)
                {
                    MessageBox.Show("Veuillez sélectionner une voie d'escalade", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (SelectedClimbingWall == null)
                {
                    MessageBox.Show("Veuillez sélectionner un mur d'escalade", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var result = await _climbingManagementService.AssignClimbingRouteToClimbingWall(SelectedClimbingRoute, SelectedClimbingWall);

                if (result)
                {
                    LoadData();
                    MessageBox.Show("Voie d'escalade assignée avec succès", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'assignation de la voie d'escalade", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'assignation de la voie d'escalade : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Afficher fenêtre pour ajouter une évaluation de difficulté à une voie d'escalade
        /// </summary>
        [RelayCommand]
        public void ShowAddRateRouteDifficultyWindow()
        {
            if (SelectedClimbingRoute == null)
            {
                MessageBox.Show("Veuillez sélectionner une voie d'escalade", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Afficher la fenêtre pour ajouter une évaluation de difficulté à une voie d'escalade
            AddClimbingRouteDifficultyRatingWindow window = new AddClimbingRouteDifficultyRatingWindow();
            window._addClimbingRouteDifficultyRatingWindowViewModel.SelectedClimbingRoute = SelectedClimbingRoute;

            var result = window.ShowDialog();
            if (!result.HasValue || !result.Value) return;
            LoadData();
        }

        /// <summary>
        /// Commande retourner à la page précédente
        /// </summary>
        [RelayCommand]
        public void GoBack()
        {
            _appNavigtionService?.GoBack();
        }
        #endregion

        #region Méthodes privées
        /// <summary>
        /// Charger les données
        /// </summary>
        private async void LoadData()
        {
            try
            {
                var loadWallsTask = _climbingManagementService.GetAllClimbingWalls();
                var loadRoutesTask = _climbingManagementService.GetAllClimbingRoutes();

                await Task.WhenAll(loadWallsTask, loadRoutesTask);

                // Recharcher murs et voies d'escalade
                ClimbingWalls = await loadWallsTask;
                ClimbingRoutes = await loadRoutesTask;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
        #region Validation
        #endregion
    }
}
