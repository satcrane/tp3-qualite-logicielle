using _14E_TP2_A23.Models;
using _14E_TP2_A23.Services.ClimbingWalls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows;

namespace _14E_TP2_A23.ViewModels.ClimbingViewModels
{
    /// <summary>
    /// View model de AddClimbingRouteWindow.xaml
    /// </summary>
    public partial class AddClimbingRouteViewModel : ObservableValidator
    {
        #region Propriétés
        private const int _nameMinLength = 1;
        private const int _nameMaxLength = 20;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Le nom de la voie est requis")]
        [MinLength(_nameMinLength, ErrorMessage = "Le nom de la voie doit contenir au moins 1 caractères")]
        [MaxLength(_nameMaxLength, ErrorMessage = "Le nom de la voie doit contenir au plus 20 caractères")]
        private string _name;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "La difficulté est requise")]
        [Range(0, 10, ErrorMessage = "La difficulté doit être entre 0 et 10")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "La difficulté doit être un nombre")]
        private double _difficulty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "La couleur des prises est requise")]
        private string _holdsColor;

        /// <summary>
        /// Service de gestion des voies d'escalade injecté par le service provider
        /// </summary>
        private readonly IClimbingManagementService _climbingManagementService;

        #endregion

        #region Constructeur
        public AddClimbingRouteViewModel(IClimbingManagementService climbingManagementService)
        {
            _climbingManagementService = climbingManagementService;
        }
        #endregion

        #region Commandes
        [RelayCommand]

        /// <summary>
        /// Ajouter une voie d'escalade
        /// </summary>
        public async Task AddClimbingRoute()
        {
            if (!IsFormValid())
            {
                MessageBox.Show("Le formulaire n'est pas valide");
                return;
            }

            try
            {
                var newClibingRoute = new ClimbingRoute
                {
                    Name = Name,
                    WallId = null,
                    Difficulty = Difficulty,
                    HoldsColor = HoldsColor,
                    DifficultyRatings = new List<double>()
                };

                var isAdded = await _climbingManagementService.AddClimbingRoute(newClibingRoute);

                if (isAdded)
                {
                    MessageBox.Show("La voie a été ajoutée",
                        "Succés",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    // Error message
                    MessageBox.Show("La voie n'a pas été ajoutée",
                        "Erreur",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout d'une nouvelle voie : {ex.Message}", "" +
                    "Erreur",
                    MessageBoxButton.OK,
                   MessageBoxImage.Error);
            }

        }
        #endregion

        #region Méthodes
        /// <summary>
        /// Valide le formulaire
        /// </summary>
        private bool IsFormValid()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }

            ValidateAllProperties();

            if (HasErrors)
            {
                return false;
            }

            if (Difficulty < 0 || Difficulty > 10)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(HoldsColor))
            {
                return false;
            }

            return true;

        }
        #endregion
    }
}
