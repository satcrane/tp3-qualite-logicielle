using _14E_TP2_A23.Models;
using _14E_TP2_A23.Services.Authentication;
using _14E_TP2_A23.Services.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows;

namespace _14E_TP2_A23.ViewModels
{
    /// <summary>
    /// View model de MainWindow.xaml
    /// </summary>
    public partial class MainViewModel : ObservableValidator
    {
        #region Propriétés
        private const int _UsernameMinLength = 1;
        private const int _UsernameMaxLength = 20;

        private const int _PasswordMinLength = 1;
        private const int _PasswordMaxLength = 20;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [MinLength(_UsernameMinLength, ErrorMessage = "Le nom d'utilisateur doit contenir au moins 1 caractères")]
        [MaxLength(_UsernameMaxLength, ErrorMessage = "Le nom d'utilisateur doit contenir au plus 20 caractères")]
        private string? _username;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [MinLength(_PasswordMinLength, ErrorMessage = "Le mot de passe doit contenir au moins 1 caractères")]
        [MaxLength(_PasswordMaxLength, ErrorMessage = "Le mot de passe doit contenir au plus 20 caractères")]
        private string? _password;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsLoggedIn))]
        private Employee? _employee;

        /// <summary>
        /// Affiche si l'utilisateur est connecté
        /// </summary>
        public bool? IsLoggedIn => Employee != null;

        /// <summary>
        /// Service d'authentification injecté par le service provider
        /// </summary>
        private readonly IAuthenticationService? _authenticationService;

        /// <summary>
        /// Service de navigation injecté par le service provider
        /// </summary>
        private readonly IAppNavigationService? _appNavigtionService;


        #endregion

        #region Constructeur
        public MainViewModel(IAuthenticationService authenticationService, IAppNavigationService appNavigtionService)
        {
            // authenticationService automatiquement injecté par le service provider dans App.xaml.cs
            _authenticationService = authenticationService;
            _appNavigtionService = appNavigtionService;
        }

        #endregion

        #region Commandes
        [RelayCommand]
        /// <summary>
        /// Commande de login
        /// </summary>
        public async Task Login()
        {

            if (!IsLoginFormValid())
            {
                MessageBox.Show("Formulaire invalide");
                return;
            }

            try
            {
                var isLoggedIn = await _authenticationService.Login(Username, Password);

                if (isLoggedIn)
                {
                    _appNavigtionService.NavigateTo("DashboardPage");
                }

                return;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la connexion : {ex.Message}");
                return;
            }

        }

        [RelayCommand]
        /// <summary>
        /// Commande creation compte
        /// </summary>
        public async Task Signup()
        {
            if (!IsLoginFormValid())
            {
                MessageBox.Show("Formulaire invalide");
                return;
            }

            try
            {
                var employee = new Employee()
                {
                    Username = Username,
                    Password = Password
                };

                var isSignedUp = await _authenticationService.Signup(employee);

                if (isSignedUp)
                {
                    MessageBox.Show("Compte créé avec succès");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la création du compte");
                }
                return;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création du compte : {ex.Message}");
                return;
            }
        }

        #endregion

        #region Méthodes de validations
        /// <summary>
        /// Valider si username et password sont remplis
        /// </summary>
        private bool IsLoginFormValid()
        {

            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                return false;
            }

            // Méthode de validation de ObservableValidator
            ValidateAllProperties();

            // Validation ObservableValidator
            if (HasErrors)
            {
                return false;
            }

            if (Username.Length < _UsernameMinLength || Username.Length > _UsernameMaxLength)
            {
                return false;
            }

            if (Password.Length < _PasswordMinLength || Password.Length > _PasswordMaxLength)
            {
                return false;
            }


            return true;
        }

        #endregion

    }
}
