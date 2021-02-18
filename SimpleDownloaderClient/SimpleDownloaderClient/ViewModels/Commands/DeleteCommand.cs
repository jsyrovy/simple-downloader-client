namespace SimpleDownloaderClient.ViewModels.Commands
{
    using System.Net.Http;
    using SimpleDownloaderClient.Models;
    using SimpleDownloaderClient.ViewModels.Helpers;

    public class DeleteCommand : BaseCommand
    {
        public DeleteCommand(ItemsViewModel viewModel)
            : base(viewModel)
        {
        }

        public override void Execute(object parameter)
        {
            this.DeleteItemAsync((Item)parameter);
        }

        private async void DeleteItemAsync(Item item)
        {
            bool success;

            try
            {
                success = await SimpleDownloaderHelper.DeleteItemAsync(this.ViewModel.Server, item.Actions);
            }
            catch (HttpRequestException e)
            {
                this.ViewModel.ShowError(e.Message, lastActionStatus: $"Cannot delete '{item.Name}'.");
                return;
            }

            if (!success)
            {
                this.ViewModel.LastActionStatus = $"Cannot delete '{item.Name}'.";
                return;
            }

            this.ViewModel.LoadItemsAsync();
            this.ViewModel.LastActionStatus = $"File '{item.Name}' was deleted.";
        }
    }
}
