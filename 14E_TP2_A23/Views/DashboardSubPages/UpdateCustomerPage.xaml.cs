using _14E_TP2_A23.Helpers;
using _14E_TP2_A23.Models;
using _14E_TP2_A23.ViewModels.DashboardViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace _14E_TP2_A23.Views.DashboardSubPages
{
    /// <summary>
    /// Logique d'interaction pour UpdateCustomerPage.xaml
    /// </summary>
    public partial class UpdateCustomerPage : Page
    {
        UpdateCustomerPageViewModel _updateCustomerPageViewModel = ServiceHelper.GetService<UpdateCustomerPageViewModel>();
        public UpdateCustomerPage()
        {
            InitializeComponent();
            DataContext = _updateCustomerPageViewModel;

            FillDataGrid();
        }

        /// <summary>
        /// Remplir le data grid des clients
        /// </summary>
        private async void FillDataGrid()
        {
            var customers = await _updateCustomerPageViewModel.GetAllCustomers();
            dgCustomers.ItemsSource = customers;
        }

        /// <summary>
        /// Clic bouton retour en arrère
        /// </summary>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _updateCustomerPageViewModel.GoBackCommand.Execute(null);
        }

        /// <summary>
        /// Evenement de fin d'édition d'une cellule du data grid
        /// </summary>
        private void dgCustomers_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (!e.Cancel && e.EditAction == DataGridEditAction.Commit)
            {
                var customer = e.Row.Item as Customer;
                if (customer != null)
                {
                    _updateCustomerPageViewModel.UpdateCustomerCommand.Execute(customer);
                }
            }
        }
    }
}
