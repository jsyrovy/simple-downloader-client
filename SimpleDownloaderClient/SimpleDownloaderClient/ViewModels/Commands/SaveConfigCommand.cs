namespace SimpleDownloaderClient.ViewModels.Commands
{
    using System.Collections.Generic;
    using SimpleDownloaderClient.ViewModels.Helpers;

    public class SaveConfigCommand : BaseCommand
    {
        public SaveConfigCommand(ItemsViewModel viewModel)
            : base(viewModel)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(this.ViewModel.Server) && int.TryParse(this.ViewModel.Reload, out _);
        }

        public override void Execute(object parameter)
        {
            ConfigHelper.SaveValues(new Dictionary<string, string>()
            {
                [ConfigHelper.Server] = this.ViewModel.Server,
                [ConfigHelper.Reload] = this.ViewModel.Reload,
            });

            this.ViewModel.UpdateTimerInterval();
            this.ViewModel.LoadItemsAsync();
        }
    }
}
