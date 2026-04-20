using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RedPrince.ViewModels.GameBlackJackViewModels
{
        public class CardViewModel : ObservableObject
        {
            private readonly Card _card;

            public CardViewModel(Card card)
            {
                _card = card;
            }

            public bool IsFaceDown
            {
                get => _card.IsFaceDown;
                set
                {
                    if (_card.IsFaceDown != value)
                    {
                        _card.IsFaceDown = value;
                        OnPropertyChanged();
                        OnPropertyChanged(nameof(RankDisplay));
                        OnPropertyChanged(nameof(SuitSymbol));
                        OnPropertyChanged(nameof(IsRed));
                    }
                }
            }

            public string RankDisplay => _card.IsFaceDown ? "" : _card.RankDisplay;
            public string SuitSymbol => _card.IsFaceDown ? "" : _card.SuitSymbol;
            public bool IsRed => !_card.IsFaceDown && _card.IsRed;

            public void Reveal()
            {
                IsFaceDown = false;
            }
        }
    }
