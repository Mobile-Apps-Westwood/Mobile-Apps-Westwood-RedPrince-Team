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
        public string Redeem10kLabel => TitleStore.Redeem10k;
        public string Redeem25kLabel => TitleStore.Redeem25k;
        public string Redeem50kLabel => TitleStore.Redeem50k;
        public string Redeem100kLabel => TitleStore.Redeem100k;

        [ObservableProperty]
        private long userBalance;

        [ObservableProperty]
        private bool is10kEnabled;

        [ObservableProperty]
        private bool is25kEnabled;

        [ObservableProperty]
        private bool is50kEnabled;

        [ObservableProperty]
        private bool is100kEnabled;

        [ObservableProperty]
        private string button10kStatus = "Available";

        [ObservableProperty]
        private string button25kStatus = "Available";

        [ObservableProperty]
        private string button50kStatus = "Available";

        [ObservableProperty]
        private string button100kStatus = "Available";

        private DateTime? lastRedeem10k;
        private DateTime? lastRedeem25k;
        private DateTime? lastRedeem50k;
        private DateTime? lastRedeem100k;

        private const string KEY_BALANCE = "user_balance";
        private const string KEY_LAST_10K = "last_redeem_10k";
        private const string KEY_LAST_25K = "last_redeem_25k";
        private const string KEY_LAST_50K = "last_redeem_50k";
        private const string KEY_LAST_100K = "last_redeem_100k";

        private System.Timers.Timer cooldownTimer;

        public StoreViewModel()
        {
            LoadUserData();
            UpdateCooldownStatus();

            // Timer to update cooldown status every second
            cooldownTimer = new System.Timers.Timer(1000);
            cooldownTimer.Elapsed += (s, e) => UpdateCooldownStatus();
            cooldownTimer.Start();
        }

        private void LoadUserData()
        {
            UserBalance = Preferences.Default.Get(KEY_BALANCE, 0L);

            string last10k = Preferences.Default.Get(KEY_LAST_10K, string.Empty);
            string last25k = Preferences.Default.Get(KEY_LAST_25K, string.Empty);
            string last50k = Preferences.Default.Get(KEY_LAST_50K, string.Empty);
            string last100k = Preferences.Default.Get(KEY_LAST_100K, string.Empty);

            lastRedeem10k = string.IsNullOrEmpty(last10k) ? null : DateTime.Parse(last10k);
            lastRedeem25k = string.IsNullOrEmpty(last25k) ? null : DateTime.Parse(last25k);
            lastRedeem50k = string.IsNullOrEmpty(last50k) ? null : DateTime.Parse(last50k);
            lastRedeem100k = string.IsNullOrEmpty(last100k) ? null : DateTime.Parse(last100k);
        }

        private void SaveUserData()
        {
            Preferences.Default.Set(KEY_BALANCE, UserBalance);
            Preferences.Default.Set(KEY_LAST_10K, lastRedeem10k?.ToString() ?? string.Empty);
            Preferences.Default.Set(KEY_LAST_25K, lastRedeem25k?.ToString() ?? string.Empty);
            Preferences.Default.Set(KEY_LAST_50K, lastRedeem50k?.ToString() ?? string.Empty);
            Preferences.Default.Set(KEY_LAST_100K, lastRedeem100k?.ToString() ?? string.Empty);
        }

        private void UpdateCooldownStatus()
        {
            UpdateButtonStatus(lastRedeem10k, 5, button => Is10kEnabled = button, status => Button10kStatus = status);
            UpdateButtonStatus(lastRedeem25k, 30, button => Is25kEnabled = button, status => Button25kStatus = status);
            UpdateButtonStatus(lastRedeem50k, 60, button => Is50kEnabled = button, status => Button50kStatus = status);
            UpdateButtonStatus(lastRedeem100k, 300, button => Is100kEnabled = button, status => Button100kStatus = status);
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

        [RelayCommand]
        private async Task Redeem25kCoins()
        {
            if (Is25kEnabled)
            {
                UserBalance += 25000;
                lastRedeem25k = DateTime.Now;
                SaveUserData();
                UpdateCooldownStatus();
                await MainThread.InvokeOnMainThreadAsync(() =>
                    Application.Current?.MainPage?.DisplayAlert("Success", "25,000 coins redeemed!", "OK")
                );
            }
        }

        [RelayCommand]
        private async Task Redeem50kCoins()
        {
            if (Is50kEnabled)
            {
                UserBalance += 50000;
                lastRedeem50k = DateTime.Now;
                SaveUserData();
                UpdateCooldownStatus();
                await MainThread.InvokeOnMainThreadAsync(() =>
                    Application.Current?.MainPage?.DisplayAlert("Success", "50,000 coins redeemed!", "OK")
                );
            }
        }

        [RelayCommand]
        private async Task Redeem100kCoins()
        {
            if (Is100kEnabled)
            {
                UserBalance += 100000;
                lastRedeem100k = DateTime.Now;
                SaveUserData();
                UpdateCooldownStatus();
                await MainThread.InvokeOnMainThreadAsync(() =>
                    Application.Current?.MainPage?.DisplayAlert("Success", "100,000 coins redeemed!", "OK")
                );
            }
        }
    }
}
