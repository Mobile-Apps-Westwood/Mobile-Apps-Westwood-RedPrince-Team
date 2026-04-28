using CommunityToolkit.Mvvm.ComponentModel;
using RedPrince.Models.Titles;
using RedPrince.Models;
using RedPrince.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RedPrince.ViewModels
{
    public partial class LeaderboardViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        public string Title => TitleLeaderboard.Title;

        public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();

        public LeaderboardViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            var users = await _databaseService.GetAllUsersAsync();
            Users.Clear();
            foreach (var u in users)
                Users.Add(u);
        }
    }
}
