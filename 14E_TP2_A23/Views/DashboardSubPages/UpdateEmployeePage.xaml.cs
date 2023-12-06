using _14E_TP2_A23.Helpers;
using _14E_TP2_A23.Models;
using _14E_TP2_A23.ViewModels.DashboardViewModels;
using System.Windows.Controls;

namespace _14E_TP2_A23.Views.DashboardSubPages
{
    /// <summary>
    /// Logique d'interaction pour UpdateEmployeesPage.xaml
    /// </summary>
    public partial class UpdateEmployeesPage : Page
    {
        UpdateEmployeeViewModel _updateEmployeeViewModel = ServiceHelper.GetService<UpdateEmployeeViewModel>();
        public UpdateEmployeesPage()
        {
            InitializeComponent();
            FillDataGrid();
        }

        /// <summary>
        /// Remplir la grille de données
        /// </summary>
        private async void FillDataGrid()
        {
            var employees = await _updateEmployeeViewModel.GetAllEmployees();
            dgEmployees.ItemsSource = employees;
        }

        /// <summary>
        /// Commande retour en arrière
        /// </summary>
        private void btnBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _updateEmployeeViewModel.GoBackCommand.Execute(null);
        }

        /// <summary>
        /// Evenement de fin d'édition d'une cellule du data grid
        /// </summary>
        private void dgEmployees_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (!e.Cancel && e.EditAction == DataGridEditAction.Commit)
            {
                var employee = e.Row.Item as Employee;
                if (employee != null)
                {
                    _updateEmployeeViewModel.UpdateEmployeCommand.Execute(employee);
                }
            }
        }
    }
}
