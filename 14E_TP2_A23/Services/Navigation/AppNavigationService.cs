using _14E_TP2_A23.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace _14E_TP2_A23.Services.Navigation
{
    /// <summary>
    /// Service de navigation entre les pages
    /// </summary>
    public class AppNavigationService : IAppNavigationService
    {
        #region Propriétés

        /// <summary>
        /// Contient les pages de l'application
        /// </summary>
        private readonly Dictionary<string, Type> _pagesByKey;

        /// <summary>
        /// Contient les pages de l'fenêtres de l'application
        /// </summary>
        private readonly Dictionary<string, Func<Window>> _windowsByKey;

        /// <summary>
        /// Main frame de l'application (pour les pages)
        /// </summary>
        private Frame _frame;
        #endregion

        #region Constructeur
        public AppNavigationService()
        {
            _pagesByKey = new Dictionary<string, Type>();
            _windowsByKey = new Dictionary<string, Func<Window>>();
        }

        #endregion

        #region Méthodes

        /// <summary>
        /// Initialise le service de navigation avec le contrôle de cadre principal.
        /// </summary>
        /// <param name="frame">Le contrôle de cadre utilisé pour la navigation.</param>
        public void Initialize(Frame frame)
        {
            _frame = frame;
        }

        /// <summary>
        /// Enregistre un type de page avec une clé unique à des fins de navigation.
        /// </summary>
        /// <param name="key">La clé associée au type de page.</param>
        /// <param name="pageType">Le type de la page à enregistrer.</param>
        public void RegisterPage(string key, Type pageType)
        {
            _pagesByKey[key] = pageType;
        }

        /// <summary>
        /// Enregistre un type de fenêtre avec une clé unique à des fins de navigation.
        /// </summary>
        /// <param name="key">La clé associée a la fenêtre.</param>
        /// <param name="pageType">Le type de la fenêtre à enregistrer.</param>
        public void RegisterWindow(string key, Func<Window> windowFactory)
        {
            _windowsByKey[key] = windowFactory;
        }


        /// <summary>
        /// Retourne à la page précédente
        /// </summary>
        public void GoBack()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }

        /// <summary>
        /// Navigue vers la page spécifiée
        /// </summary>
        /// <param name="pageKey">Le nom de la page vers ou naviguer</param>
        /// <exception cref="ArgumentException">Si la page n'existe pas</exception>
        public void NavigateTo(string pageKey)
        {
            if (_pagesByKey.ContainsKey(pageKey))
            {
                // Afficher les frames dans la page principale (MainView.xaml)
                ShowFrames();

                // Cacher le contenu de MainView.xaml pour éviter de le voir pendant le chargement de la page
                HideMainWindowContent();

                _frame.Navigate(Activator.CreateInstance(_pagesByKey[pageKey]));
            }
            else
            {
                throw new ArgumentException($"Aucune page avec la clé {pageKey}.", nameof(pageKey));
            }
        }

        /// <summary>
        /// Navigate vers la fenêtre spécifiée
        /// </summary>
        /// <param name="windowKey">Nom de la fenêtre</param>
        /// <exception cref="Exception">Si la fenêtre n'existe pas</exception>
        public void OpenWindow(string windowKey)
        {
            if (_windowsByKey.TryGetValue(windowKey, out var windowFactory))
            {
                Window window = windowFactory();
                window.Show();
            }
            else
            {
                throw new Exception($"Aucune fênetre au nom de ${windowKey}");
            }
        }

        /// <summary>
        /// Affiche le contenu de MainView.xaml.
        /// Appelé quand on se déconnecte, afin d'afficher la page de login.
        /// 
        /// MainContent = fênetre principale de l'application (dans MainWindows.xaml).
        /// </summary>
        public void ShowMainWindowContent()
        {
            var mainContent = _frame.FindName("MainContent") as FrameworkElement;
            if (mainContent != null)
            {
                mainContent.Visibility = Visibility.Visible;
            }

            HideFrames();
        }

        /// <summary>
        /// Cache le contenu de MainView.xaml.
        /// Appelé lors de la navigation vers une autre page, afin de ne pas voir le contenu de MainView.xaml pendant le chargement de la page.
        /// 
        /// MainContent = fênetre principale de l'application (dans MainWindows.xaml).
        /// </summary>
        /// 
        public void HideMainWindowContent()
        {
            var mainContent = _frame.FindName("MainContent") as FrameworkElement;
            if (mainContent != null)
            {
                mainContent.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Affiche les frames dans la page principale (MainView.xaml).
        /// 
        /// MainFrame = frame principale de l'application (dans MainView.xaml).
        /// </summary>
        private void ShowFrames()
        {
            var mainFrame = _frame.FindName("MainFrame") as FrameworkElement;
            if (mainFrame != null)
            {
                mainFrame.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Cache les frames dans la page principale (MainView.xaml).
        /// 
        /// MainFrame = frame principale de l'application (dans MainView.xaml).
        /// </summary>
        private void HideFrames()
        {
            var mainFrame = _frame.FindName("MainFrame") as FrameworkElement;
            if (mainFrame != null)
            {
                mainFrame.Visibility = Visibility.Collapsed;
            }
        }

        #endregion
    }
}
