namespace SimpleDownloaderClient.ViewModels.Commands
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using SimpleDownloaderClient.ViewModels.Helpers;

    public class DownloadCommand : BaseCommand
    {
        public DownloadCommand(ItemsViewModel viewModel)
            : base(viewModel)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace((string)parameter);
        }

        public override void Execute(object parameter)
        {
            this.DownloadItemAsync((string)parameter);
        }

        private async void DownloadItemAsync(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                var msg = $"'{url}' isn't a valid URL.";
                this.ViewModel.ShowError(msg, lastActionStatus: msg);
                return;
            }

            var name = uri.Segments.Last();
            bool success;

            try
            {
                success = await SimpleDownloaderHelper.DownloadItemAsync(this.ViewModel.Server, uri.AbsoluteUri);
            }
            catch (HttpRequestException e)
            {
                this.ViewModel.ShowError(e.Message, lastActionStatus: $"Cannot download '{name}'.");
                return;
            }

            if (!success)
            {
                var msg = $"Cannot download '{name}'.";
                this.ViewModel.ShowError(msg, lastActionStatus: msg);
                return;
            }

            this.ViewModel.Url = string.Empty;
            this.ViewModel.LoadItemsAsync();
            this.ViewModel.LastActionStatus = $"Download of '{name}' was started.";
        }
    }
}
