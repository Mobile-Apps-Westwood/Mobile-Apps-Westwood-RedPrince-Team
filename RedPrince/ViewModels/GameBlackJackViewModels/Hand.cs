using System;
using System.Collections.Generic;
using System.Text;

namespace RedPrince.ViewModels.GameBlackJackViewModels
{
    public class Hand
    {
        private readonly List<Card> _cards = new();

        public IReadOnlyList<Card> Cards => _cards.AsReadOnly();

        public void Add(Card card) => _cards.Add(card);

        public void Clear() => _cards.Clear();

        public int Count => _cards.Count;

        /// <summary>
        /// Calculates the best hand value, treating Aces as 11 or 1 as needed.
        /// </summary>

        public int Value
        {
            get
            {
                int total = 0;
                int aceCount = 0;

                foreach (var card in Cards)
                {
                    total += card.Value;

                    if (card.Rank == Rank.Ace)
                        aceCount++;
                }

                // Upgrade Aces from 1 → 11 when possible
                while (aceCount > 0 && total + 10 <= 21)
                {
                    total += 10;
                    aceCount--;
                }

                return total;
            }
        }

        public bool IsBust => Value > 21;

        public bool IsBlackjack => _cards.Count == 2 && Value == 21;

        public bool IsSoft
        {
            get
            {
                int total = 0;
                int aces = 0;
                foreach (var card in _cards)
                {
                    if (card.IsFaceDown) continue;
                    if (card.Rank == Rank.Ace) aces++;
                    total += card.Value;
                }
                return aces > 0 && total <= 21;
            }
        }

        public bool CanSplit => _cards.Count == 2 && _cards[0].Rank == _cards[1].Rank;
    }
}
