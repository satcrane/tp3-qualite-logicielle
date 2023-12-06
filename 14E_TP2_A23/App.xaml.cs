using _14E_TP2_A23.Data;
using _14E_TP2_A23.Services;
using _14E_TP2_A23.Services.Authentication;
using _14E_TP2_A23.Services.ClimbingWalls;
using _14E_TP2_A23.Services.CustomerManagement;
using _14E_TP2_A23.Services.EmployeesManagement;
using _14E_TP2_A23.Services.Navigation;
using _14E_TP2_A23.ViewModels;
using _14E_TP2_A23.ViewModels.ClimbingViewModels;
using _14E_TP2_A23.ViewModels.DashboardViewModels;
using _14E_TP2_A23.Views;
using _14E_TP2_A23.Views.DashboardSubPages;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Windows;
using System.Windows.Controls;

namespace _14E_TP2_A23
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Référence à l'application courante
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Services de l'application
        /// </summary>
        public IServiceProvider Services { get; private set; }

        public App()
        {
            Services = ConfigureServices();
        }

        /// <summary>
        /// Initialise les services de l'application (injection de dépendances)
        /// Transiant = nouvelle instance à chaque appel
        /// Singleton = une seule instance pour toute l'application
        /// </summary>
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IMongoClient>(provider =>
            {
                var connectionUri = "mongodb://localhost:27017";

                var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionUri));
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                return new MongoClient(settings);
            });

            // Services de l'application (injection de dépendances)
            services.AddSingleton<IDALService, DAL>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IAppNavigationService, AppNavigationService>();
            services.AddSingleton<ICustomerManagementService, CustomerManagementService>();
            services.AddSingleton<IEmployeeManagementService, EmployeeManagementService>();
            services.AddSingleton<IClimbingManagementService, ClimbingManagementService>();

            // Services automatiquement injectés dans le constructeur des view models
            services.AddTransient<MainViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<AddCustomerPageViewModel>();
            services.AddTransient<UpdateCustomerPageViewModel>();
            services.AddTransient<UpdateEmployeeViewModel>();
            services.AddTransient<ManageClimbingWallsViewModel>();
            services.AddTransient<AddClimbingRouteViewModel>();
            services.AddTransient<AddClimbingRouteDifficultyRatingViewModel>();

            return services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow = new MainWindow();
            MainWindow.Show();

            // Setup service de navigation
            var navigationService = Services.GetRequiredService<IAppNavigationService>() as AppNavigationService;
            var mainFrame = MainWindow.FindName("MainFrame") as Frame;

            // Initialize the navigation service with the frame
            if (navigationService != null && mainFrame != null)
            {
                navigationService.Initialize(mainFrame);

                // Enregister les pages et fenêtres (ne pas enregister MainWindow pour eviter duplication)
                navigationService.RegisterPage("DashboardPage", typeof(DashboardPage));
                navigationService.RegisterPage("AddCustomerPage", typeof(AddCustomerPage));
                navigationService.RegisterPage("UpdateCustomerPage", typeof(UpdateCustomerPage));
                navigationService.RegisterPage("UpdateEmployeePage", typeof(UpdateEmployeesPage));
                navigationService.RegisterPage("ManageClimbingWallsPage", typeof(ManageClimbingWallsPage));
            }
        }

    }
}
