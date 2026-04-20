using System;
using System.Collections.Generic;
using System.Text;

namespace RedPrince.ViewModels.GameBlackJackViewModels
{
    public class Deck
    {
        private readonly List<Card> _cards = new();
        private readonly Random _rng = new();

        public Deck()
        {
            Reset();
        }

        public void Reset()
        {
            _cards.Clear();
            foreach (Suit suit in Enum.GetValues<Suit>())
                foreach (Rank rank in Enum.GetValues<Rank>())
                    _cards.Add(new Card(suit, rank));
            Shuffle();
        }

        public void Shuffle()
        {
            for (int i = _cards.Count - 1; i > 0; i--)
            {
                int j = _rng.Next(i + 1);
                (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
            }
        }

        public Card Deal(bool faceDown = false)
        {
            if (_cards.Count == 0) Reset();
            var card = _cards[^1];
            card.IsFaceDown = faceDown;
            _cards.RemoveAt(_cards.Count - 1);
            return card;
        }

        public int CardsRemaining => _cards.Count;
    }
}
