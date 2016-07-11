using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using OBSQuiz.ViewModel;

namespace OBSQuiz.View
{
    /// <summary>
    /// Interaktionslogik für MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        #region Fields

        private const char SLASH = '/';
        private const string STYLEPATH = "../../View/Styles/";
        private const string THEME = "THEME";
        private const string DOTXAML = ".XAML";
        private const string SHOWANDHIDEANIMATION = "ShowAndHideAnimation";
        private const string STARTUPTHEME = "StartUpTheme";

        private readonly Dictionary<int, string> themeDictionary;
        private int currentThemeKey;

        #endregion

        #region Constructor

        public MainView()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel();

            this.TxtBlRightAnswer.Text = XamlResources.Strings.Common.Right;
            this.TxtBlWrongAnswer.Text = XamlResources.Strings.Common.Wrong;

            #region Theme

            List<string> themeList = Directory.GetFiles(STYLEPATH).Where(p => p.ToUpper().Contains(THEME)).ToList();
            string startUpTheme = ConfigurationManager.AppSettings.Get(STARTUPTHEME).ToUpper();

            this.currentThemeKey = 0;
            this.themeDictionary = new Dictionary<int, string>();

            for (int i = 0; i < themeList.Count(); i++)
            {
                this.themeDictionary.Add(i, themeList[i]);
                if (themeList[i].ToUpper().Contains(THEME + startUpTheme))
                {
                    this.currentThemeKey = i;
                }
            }

            this.changeTheme();

            #endregion
        }

        #endregion

        #region Private Methods

        #region changeTheme

        private void changeTheme()
        {
            Application.Current.Resources.MergedDictionaries[0].Source =
                new Uri(this.themeDictionary[this.currentThemeKey], UriKind.Relative);

            this.TxtBlCurrentTheme.Text =
                this.themeDictionary[this.currentThemeKey].Split(SLASH)
                    .Last()
                    .ToUpper()
                    .Replace(THEME, string.Empty)
                    .Replace(DOTXAML, string.Empty);

            ((Storyboard) FindResource(SHOWANDHIDEANIMATION)).Begin(this.TxtBlCurrentTheme);
        }

        #endregion

        #endregion

        #region TitleBar Event Handling

        #region OnChangeTheme

        private void OnChangeTheme(object sender, RoutedEventArgs e)
        {
            if (this.currentThemeKey < (this.themeDictionary.Count - 1))
            {
                this.currentThemeKey++;
            }
            else
            {
                this.currentThemeKey = 0;
            }

            this.changeTheme();
        }

        #endregion

        #region OnDragMoveWindow

        private void OnDragMoveWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        #endregion

        #region OnMinimizeWindow

        private void OnMinimizeWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        #endregion

        #region OnMaximizeWindow

        private void OnMaximizeWindow(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
        }

        #endregion

        #region OnCloseWindow

        private void OnCloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #endregion
    }
}
