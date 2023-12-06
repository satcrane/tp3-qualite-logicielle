using _14E_TP2_A23.Helpers;
using _14E_TP2_A23.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace _14E_TP2_A23.Views
{
    /// <summary>
    /// Logique d'interaction pour DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        DashboardViewModel _dashBoardViewModel = ServiceHelper.GetService<DashboardViewModel>();

        public DashboardPage()
        {
            InitializeComponent();
            this.DataContext = _dashBoardViewModel;
            ShowCurrentLoggedInUser();
        }

        private void ShowCurrentLoggedInUser()
        {
            var currentLoggedInUser = _dashBoardViewModel.GetCurrentLoggedInUser();
            var isAdmin = currentLoggedInUser?.IsAdmin == true ? "Administrateur" : "Employé";
            lbCurrentUser.Content = $"Connecté en tant que {currentLoggedInUser?.Username}";
            lbCurrentUserRole.Content = $"Rôle: {isAdmin}";
        }

        /// <summary>
        /// Commande pour afficher la page d'ajout d'un client
        /// </summary>
        private void btnAddCustomer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _dashBoardViewModel.ShowAddCustomerPageCommand.Execute(null);
        }

        /// <summary>
        /// Commande pour afficher la page de modification d'un client
        /// </summary>
        private void btnUpdateCustomers_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _dashBoardViewModel.ShowUpdateCustomerPageCommand.Execute(null);
        }

        /// <summary>
        /// Commande afficher page gestion des murs d'escalade
        /// </summary>
        private void btnManageClimbingWalls_Click(object sender, RoutedEventArgs e)
        {
            _dashBoardViewModel.ShowManageClimbingWallsPageCommand.Execute(null);
        }

        /// <summary>
        /// Commande afficher page modification employé
        /// </summary>
        private void btnUpdateEmployees_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var currentLoggedInUser = _dashBoardViewModel.GetCurrentLoggedInUser();
            var isAdmin = currentLoggedInUser?.IsAdmin == true ? "Administrateur" : "Employé";

            if (isAdmin != "Administrateur")
            {
                MessageBox.Show("Vous n'avez pas les droits pour accéder à cette page", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _dashBoardViewModel.ShowUpdateEmployeesPageCommand.Execute(null);
        }

        /// <summary>
        /// Commande déconnexion
        /// </summary>
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            _dashBoardViewModel.LogoutCommand.Execute(null);
        }

    }
}
