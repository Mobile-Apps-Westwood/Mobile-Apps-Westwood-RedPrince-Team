using System;
using System.Collections.Generic;
using System.Text;

namespace RedPrince.ViewModels.GameBlackJackViewModels
{
    public enum GameState
    {
        Idle,        // Waiting to place bet / start
        Betting,     // Player choosing bet amount
        PlayerTurn,  // Player hitting/standing
        DealerTurn,  // Dealer playing
        RoundOver    // Showing result
    }

    public enum RoundResult
    {
        None,
        PlayerWins,
        PlayerBlackjack,
        DealerWins,
        DealerBlackjack,
        Push
    }

    public class GameResult
    {
        public RoundResult Result { get; set; }
        public int Payout { get; set; }

        public string Message => Result switch
        {
            RoundResult.PlayerBlackjack => "🃏 BLACKJACK! You win!",
            RoundResult.PlayerWins => "🎉 You Win!",
            RoundResult.DealerBlackjack => "Dealer Blackjack. You lose.",
            RoundResult.DealerWins => "Dealer Wins. Better luck next time.",
            RoundResult.Push => "Push — It's a tie!",
            _ => ""
        };

        public bool IsWin => Result is RoundResult.PlayerWins or RoundResult.PlayerBlackjack;
        public bool IsPush => Result == RoundResult.Push;
    }
}
