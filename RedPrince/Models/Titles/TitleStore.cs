using System;
using System.Collections.Generic;
using System.Text;

namespace RedPrince.Models.Titles
{
    public static class TitleStore
    {
        public static string Title { get; } = "Coin Store";
        public static string CurrentBalance { get; } = "Current Balance:";
        public static string Coins { get; } = "coins";
        public static string Redeem100 { get; } = "Redeem 100";
        public static string Redeem1k { get; } = "Redeem 1k";
        public static string Redeem5k { get; } = "Redeem 5k";
        public static string Redeem10k { get; } = "Redeem 10k";
        public static string Available { get; } = "Available";
        public static string CooldownRemaining { get; } = "Cooldown: {0}";
        public static string RedemptionSuccess { get; } = "Redeemed successfully!";
        public static string OnCooldown { get; } = "This button is on cooldown. Please wait {0}";
    }
}
