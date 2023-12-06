using _14E_TP2_A23.Helpers;
using _14E_TP2_A23.ViewModels.DashboardViewModels;
using System.Windows.Controls;

namespace _14E_TP2_A23.Views.DashboardSubPages
{
    /// <summary>
    /// Logique d'interaction pour AddClientPage.xaml
    /// </summary>
    public partial class AddCustomerPage : Page
    {
        AddCustomerPageViewModel _addClientPageViewModel = ServiceHelper.GetService<AddCustomerPageViewModel>();
        public AddCustomerPage()
        {
            InitializeComponent();
            this.DataContext = _addClientPageViewModel;
        }

        /// <summary>
        /// Clic sur bouton retour
        /// </summary>
        private void btnBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _addClientPageViewModel.GoBackCommand.Execute(null);
        }

        /// <summary>
        /// Clic sur bouton ajouter un utilisateur
        /// </summary>
        private void btnAddCustomer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _addClientPageViewModel.AddCustomerCommand.Execute(null);
        }
    }
}
