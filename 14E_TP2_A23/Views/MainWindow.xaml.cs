using _14E_TP2_A23.Helpers;
using _14E_TP2_A23.ViewModels;
using System.Windows;

namespace _14E_TP2_A23
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MainViewModel _mainViewModel = ServiceHelper.GetService<MainViewModel>();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _mainViewModel;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (_mainViewModel == null)
            {
                return;
            }

            // Exécute la commande LoginCommand
            _mainViewModel.LoginCommand.Execute(null);
        }

        /// <summary>
        /// Commande pour créer un compte
        /// </summary>
        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            _mainViewModel.SignupCommand.Execute(null);
        }
    }
}
