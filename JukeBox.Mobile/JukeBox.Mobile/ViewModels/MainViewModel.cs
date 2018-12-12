using JukeBox.Mobile.Common;
using JukeBox.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JukeBox.Mobile.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private bool IsLoaded;

        public MainViewModel()
        {
            MediaFiles = new ObservableCollection<MediaFile>();
            CreateCommands();
        }

        #region Command

        public AutoRelayCommand PlayCommand { get; set; }
        public AutoRelayCommand StopCommand { get; set; }
        public AutoRelayCommand PauseCommand { get; set; }

        #endregion

        #region Properties
        private bool isPlayCommandEnabled;
        public bool IsPlayCommandEnabled
        {
            get => isPlayCommandEnabled;
            set => Set(ref isPlayCommandEnabled, value, true);
        }

        private bool isStopCommandEnabled;
        public bool IsStopCommandEnabled
        {
            get => isStopCommandEnabled;
            set => Set(ref isStopCommandEnabled, value, true);
        }

        private bool isPauseCommandEnabled;
        public bool IsPauseCommandEnabled
        {
            get => isPauseCommandEnabled;
            set => Set(ref isPauseCommandEnabled, value, true);
        }

        private ObservableCollection<MediaFile> mediaFiles;
        public ObservableCollection<MediaFile> MediaFiles
        {
            get { return mediaFiles; }
            set { Set(ref mediaFiles, value, broadcast: true); }
        }

        private MediaFile selectedMediaFile;
        public MediaFile SelectedMediaFile
        {
            get { return selectedMediaFile; }
            set { Set(ref selectedMediaFile, value, broadcast: true); }
        }
        #endregion

        private void CreateCommands()
        {
            PlayCommand = new AutoRelayCommand(async () =>
            {
                await JukeBoxService.PlayAsync(SelectedMediaFile.FileUri);
            }, 
            () => 
            {
                IsPlayCommandEnabled = !IsBusy && SelectedMediaFile != null;

                return IsPlayCommandEnabled;
            }).
                DependsOn(() => IsBusy).
                DependsOn(() => SelectedMediaFile).
                DependsOn(()=>IsPlayCommandEnabled);

            StopCommand = new AutoRelayCommand(async () =>
            {
                await JukeBoxService.StopAsync();
            }, 
            () => 
            {
                IsStopCommandEnabled = !IsBusy && SelectedMediaFile != null;

                return IsStopCommandEnabled; 
            }).
                DependsOn(() => IsBusy).
                DependsOn(() => SelectedMediaFile).
                DependsOn(() => IsStopCommandEnabled);

            PauseCommand = new AutoRelayCommand(async () =>
            {
                await JukeBoxService.PauseAsync();
            }, 
            () =>
            {
                IsPauseCommandEnabled = !IsBusy && SelectedMediaFile != null;

                return IsPauseCommandEnabled;
            }).
                DependsOn(() => IsBusy).
                DependsOn(() => SelectedMediaFile).
                DependsOn(() => IsPauseCommandEnabled);
        }

        public override async void Activate(object parameter)
        {
            if (!IsLoaded)
            {
                IsBusy = true;
                try
                {
                    await LoadAllFilesAsync();
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private async Task LoadAllFilesAsync()
        {
            if (!IsConnected)
            {
                await ShowMessage("No connection available");
                return;
            }

            try
            {
                MediaFiles.Clear();
                var files = await AzureService.GetAllFilesAsync();

                foreach (var file in files)
                {
                    MediaFiles.Add(file);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessageAsync(ex.Message);
            }
        }
    }
}
