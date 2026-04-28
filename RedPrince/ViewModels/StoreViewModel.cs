using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RedPrince.Models.Titles;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPrince.ViewModels
{
    public partial class StoreViewModel : ObservableObject
    {
        public string Title => TitleStore.Title;
        public string CurrentBalanceLabel => TitleStore.CurrentBalance;
        public string CoinsLabel => TitleStore.Coins;
        public string Redeem100Label => TitleStore.Redeem100;
        public string Redeem1kLabel => TitleStore.Redeem1k;
        public string Redeem5kLabel => TitleStore.Redeem5k;
        public string Redeem10kLabel => TitleStore.Redeem10k;

        [ObservableProperty]
        private long userBalance;

        [ObservableProperty]
        private bool is100Enabled;

        [ObservableProperty]
        private bool is1kEnabled;

        [ObservableProperty]
        private bool is5kEnabled;

        [ObservableProperty]
        private bool is10kEnabled;

        [ObservableProperty]
        private string button100Status = "Available";

        [ObservableProperty]
        private string button1kStatus = "Available";

        [ObservableProperty]
        private string button5kStatus = "Available";

        [ObservableProperty]
        private string button10kStatus = "Available";

        private DateTime? lastRedeem100;
        private DateTime? lastRedeem1k;
        private DateTime? lastRedeem5k;
        private DateTime? lastRedeem10k;

        private const string KEY_BALANCE = "user_balance";
        private const string KEY_LAST_100 = "last_redeem_100";
        private const string KEY_LAST_1K = "last_redeem_1k";
        private const string KEY_LAST_5K = "last_redeem_5k";
        private const string KEY_LAST_10K = "last_redeem_10k";

        private readonly RedPrince.Services.DatabaseService _databaseService;
        private string _currentUsername;

        private System.Timers.Timer cooldownTimer;

        public StoreViewModel(RedPrince.Services.DatabaseService databaseService)
        {
            _databaseService = databaseService;
            _currentUsername = Preferences.Get("CurrentUser", string.Empty);

            LoadUserData();
            UpdateCooldownStatus();

            // Timer to update cooldown status every second
            cooldownTimer = new System.Timers.Timer(1000);
            cooldownTimer.Elapsed += (s, e) => UpdateCooldownStatus();
            cooldownTimer.Start();
        }

        private void LoadUserData()
        {
            // Load balance from DB if user logged in, otherwise fallback to preferences
            if (!string.IsNullOrEmpty(_currentUsername) && _databaseService != null)
            {
                try
                {
                    var user = Task.Run(() => _databaseService.GetUserByUsernameAsync(_currentUsername)).Result;
                    if (user != null)
                    {
                        UserBalance = user.Money;
                    }
                    else
                    {
                        UserBalance = Preferences.Default.Get(KEY_BALANCE, 0L);
                    }
                }
                catch
                {
                    UserBalance = Preferences.Default.Get(KEY_BALANCE, 0L);
                }
            }
            else
            {
                UserBalance = Preferences.Default.Get(KEY_BALANCE, 0L);
            }

            string last100 = Preferences.Default.Get(GetPrefKey(KEY_LAST_100), string.Empty);
            string last1k = Preferences.Default.Get(GetPrefKey(KEY_LAST_1K), string.Empty);
            string last5k = Preferences.Default.Get(GetPrefKey(KEY_LAST_5K), string.Empty);
            string last10k = Preferences.Default.Get(GetPrefKey(KEY_LAST_10K), string.Empty);

            lastRedeem100 = string.IsNullOrEmpty(last100) ? null : DateTime.Parse(last100);
            lastRedeem1k = string.IsNullOrEmpty(last1k) ? null : DateTime.Parse(last1k);
            lastRedeem5k = string.IsNullOrEmpty(last5k) ? null : DateTime.Parse(last5k);
            lastRedeem10k = string.IsNullOrEmpty(last10k) ? null : DateTime.Parse(last10k);
        }

        private void SaveUserData()
        {
            // Save cooldowns in preferences (per user)
            Preferences.Default.Set(GetPrefKey(KEY_LAST_100), lastRedeem100?.ToString() ?? string.Empty);
            Preferences.Default.Set(GetPrefKey(KEY_LAST_1K), lastRedeem1k?.ToString() ?? string.Empty);
            Preferences.Default.Set(GetPrefKey(KEY_LAST_5K), lastRedeem5k?.ToString() ?? string.Empty);
            Preferences.Default.Set(GetPrefKey(KEY_LAST_10K), lastRedeem10k?.ToString() ?? string.Empty);

            // Persist balance to DB if user available, otherwise save to preferences
            if (!string.IsNullOrEmpty(_currentUsername) && _databaseService != null)
            {
                try
                {
                    var user = Task.Run(() => _databaseService.GetUserByUsernameAsync(_currentUsername)).Result;
                    if (user != null)
                    {
                        user.Money = (int)UserBalance;
                        Task.Run(() => _databaseService.UpdateUserAsync(user)).Wait();
                        return;
                    }
                }
                catch
                {
                    // ignore and fallback to prefs
                }
            }

            Preferences.Default.Set(KEY_BALANCE, UserBalance);
        }

        private string GetPrefKey(string key)
        {
            if (string.IsNullOrEmpty(_currentUsername)) return key;
            return $"{_currentUsername}_{key}";
        }

        private void UpdateCooldownStatus()
        {
            UpdateButtonStatus(lastRedeem100, 5, button => Is100Enabled = button, status => Button100Status = status);
            UpdateButtonStatus(lastRedeem1k, 30, button => Is1kEnabled = button, status => Button1kStatus = status);
            UpdateButtonStatus(lastRedeem5k, 60, button => Is5kEnabled = button, status => Button5kStatus = status);
            UpdateButtonStatus(lastRedeem10k, 300, button => Is10kEnabled = button, status => Button10kStatus = status);
        }

        private void UpdateButtonStatus(DateTime? lastRedeemTime, int cooldownSeconds, Action<bool> setEnabled, Action<string> setStatus)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (lastRedeemTime == null)
                {
                    setEnabled(true);
                    setStatus("Available");
                }
                else
                {
                    TimeSpan timeSinceRedeem = DateTime.Now - lastRedeemTime.Value;
                    TimeSpan cooldownDuration = TimeSpan.FromSeconds(cooldownSeconds);

                    if (timeSinceRedeem < cooldownDuration)
                    {
                        setEnabled(false);
                        TimeSpan remaining = cooldownDuration - timeSinceRedeem;
                        setStatus(FormatTimeRemaining(remaining));
                    }
                    else
                    {
                        setEnabled(true);
                        setStatus("Available");
                    }
                }
            });
        }

        private string FormatTimeRemaining(TimeSpan timeSpan)
        {
            if (timeSpan.TotalHours >= 1)
                return $"{(int)timeSpan.TotalHours}h {timeSpan.Minutes}m";
            else if (timeSpan.TotalMinutes >= 1)
                return $"{(int)timeSpan.TotalMinutes}m {timeSpan.Seconds}s";
            else
                return $"{timeSpan.Seconds}s";
        }

        [RelayCommand]
        private async Task Redeem100Coins()
        {
            if (Is100Enabled)
            {
                UserBalance += 100;
                lastRedeem100 = DateTime.Now;
                SaveUserData(); // Save user data after redeeming coins
                UpdateCooldownStatus();
                await MainThread.InvokeOnMainThreadAsync(() =>
                    Application.Current?.MainPage?.DisplayAlert("Success", "100 coins redeemed!", "OK")
                );
            }
        }

        [RelayCommand]
        private async Task Redeem1kCoins()
        {
            if (Is1kEnabled && UserBalance >= 1000)
            {
                UserBalance += 1000;
                lastRedeem1k = DateTime.Now;
                SaveUserData();
                UpdateCooldownStatus();
                await MainThread.InvokeOnMainThreadAsync(() =>
                    Application.Current?.MainPage?.DisplayAlert("Success", "1,000 coins redeemed!", "OK")
                );
            }
        }

        [RelayCommand]
        private async Task Redeem5kCoins()
        {
            if (Is5kEnabled)
            {
                UserBalance += 5000;
                lastRedeem5k = DateTime.Now;
                SaveUserData();
                UpdateCooldownStatus();
                await MainThread.InvokeOnMainThreadAsync(() =>
                    Application.Current?.MainPage?.DisplayAlert("Success", "5,000 coins redeemed!", "OK")
                );
            }
        }

        [RelayCommand]
        private async Task Redeem10kCoins()
        {
            if (Is10kEnabled)
            {
                UserBalance += 10000;
                lastRedeem10k = DateTime.Now;
                SaveUserData();
                UpdateCooldownStatus();
                await MainThread.InvokeOnMainThreadAsync(() =>
                    Application.Current?.MainPage?.DisplayAlert("Success", "10,000 coins redeemed!", "OK")
                );
            }
        }
    }
}
