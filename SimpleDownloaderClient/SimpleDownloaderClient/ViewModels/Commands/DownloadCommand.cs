namespace SimpleDownloaderClient.ViewModels.Commands
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Windows.Input;
    using SimpleDownloaderClient.ViewModels.Helpers;

    public class DownloadCommand : ICommand
    {
        private readonly ItemsViewModel viewModel;

        public DownloadCommand(ItemsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace((string)parameter);
        }

        public void Execute(object parameter)
        {
            this.DownloadItemAsync((string)parameter);
        }

        private async void DownloadItemAsync(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                var msg = $"'{url}' isn't a valid URL.";
                this.viewModel.ShowError(msg, lastActionStatus: msg);
                return;
            }

            var name = uri.Segments.Last();
            bool success;

            try
            {
                success = await SimpleDownloaderHelper.DownloadItemAsync(this.viewModel.Server, uri.AbsoluteUri);
            }
            catch (HttpRequestException e)
            {
                this.viewModel.ShowError(e.Message, lastActionStatus: $"Cannot download '{name}'.");
                return;
            }

            if (!success)
            {
                var msg = $"Cannot download '{name}'.";
                this.viewModel.ShowError(msg, lastActionStatus: msg);
                return;
            }

            this.viewModel.Url = string.Empty;
            this.viewModel.LoadItemsAsync();
            this.viewModel.LastActionStatus = $"Download of '{name}' was started.";
        }
    }
}
