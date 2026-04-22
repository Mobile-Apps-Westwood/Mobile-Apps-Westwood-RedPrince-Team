using System;
using System.Collections.Generic;
using System.Text;

namespace RedPrince.ViewModels.GameBlackJackViewModels
{
    public enum Suit { Hearts, Diamonds, Clubs, Spades }
    public enum Rank { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }

    public class Card
    {
        public Suit Suit { get; }
        public Rank Rank { get; }
        public bool IsFaceDown { get; set; }

        public Card(Suit suit, Rank rank, bool isFaceDown = false)
        {
            Suit = suit;
            Rank = rank;
            IsFaceDown = isFaceDown;
        }

        public bool IsRed => Suit == Suit.Hearts || Suit == Suit.Diamonds;

        public string SuitSymbol => Suit switch
        {
            Suit.Hearts => "♥",
            Suit.Diamonds => "♦",
            Suit.Clubs => "♣",
            Suit.Spades => "♠",
            _ => ""
        };

        public string RankDisplay => Rank switch
        {
            Rank.Ace => "A",
            Rank.King => "K",
            Rank.Queen => "Q",
            Rank.Jack => "J",
            Rank.Ten => "10",
            _ => ((int)Rank).ToString()
        };

        public int Value => Rank switch
        {
            Rank.Jack or Rank.Queen or Rank.King => 10,
            Rank.Ace => 1, // Ace is ALWAYS 1 here
            _ => (int)Rank
        };      

        

        public override string ToString() => $"{RankDisplay}{SuitSymbol}";
    }
}
