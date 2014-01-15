using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SoundBoard.Resources;
using SoundBoard.ViewModels;
using Coding4Fun.Toolkit.Controls;
using System.IO;
using System.IO.IsolatedStorage;
namespace SoundBoard
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Establecer el contexto de datos del control ListBox control en los datos de ejemplo
            DataContext = App.ViewModel;

            // Código de ejemplo para traducir ApplicationBar
            BuildLocalizedApplicationBar();
        }

        // Cargar datos para los elementos ViewModel
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            // verifying our sender is actually a LongListSelector
            if (selector == null)
            {
                return;
            }
            SoundData data = selector.SelectedItem as SoundData;
            // verifying our selected item is actually a SoundData
            if (data == null)
            {
                return;
            }

            if (File.Exists(data.FilePath))
            {
                AudioPlayer.Source = new Uri(data.FilePath, UriKind.RelativeOrAbsolute);
            }
            else
            {
                using (var storageFolder = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var stream = new IsolatedStorageFileStream(data.FilePath, FileMode.Open, storageFolder))
                    {
                        AudioPlayer.SetSource(stream);
                    }
                }
            }

            
            selector.SelectedItem = null;


        }

        // Código de ejemplo para compilar una ApplicationBar traducida
        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton recordAudioAppBar = new ApplicationBarIconButton();
            recordAudioAppBar.IconUri = new Uri("/Assets/AppBar/microphone.png", UriKind.Relative);
            recordAudioAppBar.Text = AppResources.AppBarRecord;

            recordAudioAppBar.Click += recordAudioAppBar_Click;

            ApplicationBarMenuItem aboutAppBar = new ApplicationBarMenuItem();
            aboutAppBar.Text = AppResources.AppBarAbout;

            aboutAppBar.Click += aboutAppBar_Click;

            ApplicationBar.Buttons.Add(recordAudioAppBar);
            ApplicationBar.MenuItems.Add(aboutAppBar);

        }

        void aboutAppBar_Click(object sender, EventArgs e)
        {
            AboutPrompt aboutMe = new AboutPrompt();
            aboutMe.Show("Jose Arias", "@jearias", "jearias@mobbeel.com", "http://www.mobbeel.com");
        }

        void recordAudioAppBar_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/RecordAudio.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}