using _14E_TP2_A23.Models;
using _14E_TP2_A23.Services.ClimbingWalls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows;

namespace _14E_TP2_A23.ViewModels.ClimbingViewModels
{
    /// <summary>
    /// View model de AddClimbingRouteDifficultyRatingWindow.xaml
    /// </summary>
    public partial class AddClimbingRouteDifficultyRatingViewModel : ObservableValidator
    {
        #region Propriétés
        private const int _difficultyMinValue = 0;
        private const int _difficultyMaxValue = 10;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "La difficulté est requise")]
        [Range(_difficultyMinValue, _difficultyMaxValue, ErrorMessage = "La difficulté doit être entre 0 et 10")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "La difficulté doit être un nombre")]
        private double _difficulty;

        /// <summary>
        /// Voie d'escalade sélectionnée
        /// </summary>
        [ObservableProperty]
        private ClimbingRoute _selectedClimbingRoute;

        /// <summary>
        /// Service de gestions de voies d'escalades
        /// </summary>
        private readonly IClimbingManagementService _climbingManagementService;
        #endregion

        #region Constructeur
        public AddClimbingRouteDifficultyRatingViewModel(IClimbingManagementService climbingManagementService)
        {
            _climbingManagementService = climbingManagementService;
        }
        #endregion

        #region Commandes
        /// <summary>
        /// Ajouter une note de difficulté à une voie d'escalade
        /// </summary>
        [RelayCommand]
        public async Task AddClimbingRouteDifficultyRating()
        {
            try
            {
                if (!IsFormValid())
                {
                    MessageBox.Show("Le formulaire n'est pas valide", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var result = await _climbingManagementService.AddClimbingRouteDifficultyRating(SelectedClimbingRoute, Difficulty);

                if (result)
                {
                    MessageBox.Show("La note de difficulté a été ajoutée avec succès", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Une erreur est survenue lors de l'ajout de la note de difficulté", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue lors de l'ajout de la note de difficulté : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Méthodes
        /// <summary>
        /// Valider le formulaire
        /// </summary>
        private bool IsFormValid()
        {
            ValidateAllProperties();

            if (HasErrors)
            {
                return false;
            }

            if (Difficulty < _difficultyMinValue || Difficulty > _difficultyMaxValue)
            {
                return false;
            }

            if (SelectedClimbingRoute == null)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
