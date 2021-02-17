namespace SimpleDownloaderClient.ViewModels.Commands
{
    using System;
    using System.Net.Http;
    using System.Windows.Input;
    using SimpleDownloaderClient.Models;
    using SimpleDownloaderClient.ViewModels.Helpers;

    public class DeleteCommand : ICommand
    {
        private readonly ItemsViewModel viewModel;

        public DeleteCommand(ItemsViewModel viewModel)
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
            return true;
        }

        public void Execute(object parameter)
        {
            this.DeleteItemAsync((Item)parameter);
        }

        private async void DeleteItemAsync(Item item)
        {
            bool success;

            try
            {
                success = await SimpleDownloaderHelper.DeleteItemAsync(this.viewModel.Server, item.Actions);
            }
            catch (HttpRequestException e)
            {
                this.viewModel.ShowError(e.Message, lastActionStatus: $"Cannot delete '{item.Name}'.");
                return;
            }

            if (!success)
            {
                this.viewModel.LastActionStatus = $"Cannot delete '{item.Name}'.";
                return;
            }

            this.viewModel.LoadItemsAsync();
            this.viewModel.LastActionStatus = $"File '{item.Name}' was deleted.";
        }
    }
}
