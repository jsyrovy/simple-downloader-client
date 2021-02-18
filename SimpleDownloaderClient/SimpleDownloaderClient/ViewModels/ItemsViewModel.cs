namespace SimpleDownloaderClient.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Net.Http;
    using System.Windows;
    using System.Windows.Threading;
    using SimpleDownloaderClient.Models;
    using SimpleDownloaderClient.ViewModels.Commands;
    using SimpleDownloaderClient.ViewModels.Helpers;

    public class ItemsViewModel : INotifyPropertyChanged
    {
        private readonly DispatcherTimer reloadTimer;

        private string server;
        private string reload;
        private int reloadSeconds;
        private string url;
        private string connectionStatus;
        private string lastActionStatus;

        public ItemsViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                return;
            }

            this.Items = new ObservableCollection<Item>();
            this.Server = ConfigHelper.GetValue(ConfigHelper.Server);
            this.Reload = ConfigHelper.GetValue(ConfigHelper.Reload);
            this.DownloadCommand = new DownloadCommand(this);
            this.DeleteCommand = new DeleteCommand(this);
            this.SaveConfigCommand = new SaveConfigCommand(this);

            this.reloadTimer = new DispatcherTimer();
            this.reloadTimer.Tick += (s, e) => this.LoadItemsAsync();
            this.UpdateTimerInterval();

            this.ConnectionStatus = $"Connecting to '{this.Server}'.";
            this.LoadItemsAsync();
            this.LastActionStatus = "Items were loaded.";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Item> Items { get; set; }

        public DownloadCommand DownloadCommand { get; set; }

        public DeleteCommand DeleteCommand { get; set; }

        public SaveConfigCommand SaveConfigCommand { get; set; }

        public string Server
        {
            get
            {
                return this.server;
            }

            set
            {
                this.server = value;
                this.OnPropertyChanged(nameof(this.Server));
            }
        }

        public string Reload
        {
            get
            {
                return this.reload;
            }

            set
            {
                this.reload = value;
                this.OnPropertyChanged(nameof(this.Reload));
                if (int.TryParse(value, out int seconds))
                    this.reloadSeconds = seconds;
                else
                    this.reloadSeconds = 15;
            }
        }

        public string Url
        {
            get
            {
                return this.url;
            }

            set
            {
                this.url = value;
                this.OnPropertyChanged(nameof(this.Url));
            }
        }

        public string ConnectionStatus
        {
            get
            {
                return this.connectionStatus;
            }

            set
            {
                this.connectionStatus = value;
                this.OnPropertyChanged(nameof(this.ConnectionStatus));
            }
        }

        public string LastActionStatus
        {
            get
            {
                return this.lastActionStatus;
            }

            set
            {
                this.lastActionStatus = value;
                this.OnPropertyChanged(nameof(this.LastActionStatus));
            }
        }

        public void UpdateTimerInterval()
        {
            this.reloadTimer.Interval = TimeSpan.FromSeconds(this.reloadSeconds);
        }

        public async void LoadItemsAsync()
        {
            try
            {
                this.reloadTimer.Stop();
                this.Items.Clear();

                foreach (var item in await SimpleDownloaderHelper.GetItemsAsync(this.Server))
                    this.Items.Add(item);

                this.ConnectionStatus = $"Connected to '{this.Server}'.";
                this.reloadTimer.Start();
            }
            catch (HttpRequestException e)
            {
                this.ShowError(e.Message, connectionStatus: $"Cannot connect to '{this.Server}'.");
            }
        }

        public void ShowError(string message, string connectionStatus = null, string lastActionStatus = null)
        {
            if (connectionStatus != null)
                this.ConnectionStatus = connectionStatus;

            if (lastActionStatus != null)
                this.LastActionStatus = lastActionStatus;

            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
