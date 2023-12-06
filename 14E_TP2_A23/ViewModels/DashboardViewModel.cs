using _14E_TP2_A23.Models;
using _14E_TP2_A23.Services.Authentication;
using _14E_TP2_A23.Services.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _14E_TP2_A23.ViewModels
{
    /// <summary>
    /// View model de DashboardPage.xaml
    /// </summary>
    public partial class DashboardViewModel : ObservableRecipient
    {
        #region Propriétés
        /// <summary>
        /// Service de navigation injecté par le service provider
        /// </summary>
        private readonly IAppNavigationService _appNavigtionService;

        /// <summary>
        /// Service d'authentification injecté par le service provider
        /// </summary>
        private IAuthenticationService _authenticationService;

        #endregion

        #region Constructeur
        public DashboardViewModel(IAppNavigationService appNavigtionService, IAuthenticationService authenticationService)
        {
            _appNavigtionService = appNavigtionService;
            _authenticationService = authenticationService;
        }

        #endregion

        #region Méthodes
        public Employee? GetCurrentLoggedInUser()
        {
            return _authenticationService.GetCurrentLoggedInUser();
        }
        #endregion

        #region Commandes
        [RelayCommand]
        /// <summary>
        /// Comande afficher la page d'ajout de client
        /// </summary>
        public void ShowAddCustomerPage()
        {
            _appNavigtionService?.NavigateTo("AddCustomerPage");
        }

        [RelayCommand]
        /// <summary>
        /// Commande afficher la page de modification des clients
        /// </summary>
        public void ShowUpdateCustomerPage()
        {
            _appNavigtionService?.NavigateTo("UpdateCustomerPage");
        }

        /// <summary>
        /// Commande afficher la page de gestion des murs d'escalade
        /// </summary>
        [RelayCommand]
        public void ShowManageClimbingWallsPage()
        {
            _appNavigtionService?.NavigateTo("ManageClimbingWallsPage");
        }

        [RelayCommand]
        /// <summary>
        /// Commande afficher la page de modification des employés
        /// </summary>
        public void ShowUpdateEmployeesPage()
        {
            _appNavigtionService?.NavigateTo("UpdateEmployeePage");
        }

        [RelayCommand]
        /// <summary>
        /// Commande déconnexion
        /// </summary>
        public void Logout()
        {
            _authenticationService.Logout();
            _appNavigtionService.ShowMainWindowContent();
        }
        #endregion
    }
}
