using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RedPrince.ViewModels.GameBlackJackViewModels
{
        public class GameViewModel : ObservableObject
        {
            // ── Model ─────────────────────────────────────────────────────────────
            private readonly Deck _deck = new();
            private readonly Hand _playerHand = new();
            private readonly Hand _dealerHand = new();

            // ── Backing fields ────────────────────────────────────────────────────
            private GameState _gameState = GameState.Idle;
            private int _playerBalance = 1000;
            private int _currentBet = 0;
            private int _selectedChip = 25;
            private string _statusMessage = "Place your bet to start!";
            private string _resultMessage = "";
            private bool _resultVisible = false;
            private bool _resultIsWin = false;
            private int _dealerScore;
            private int _playerScore;
            private int _wins = 0;
            private int _losses = 0;
            private int _pushes = 0;

            // ── Constructor ───────────────────────────────────────────────────────
            public GameViewModel()
            {
                PlayerCards = new ObservableCollection<CardViewModel>();
                DealerCards = new ObservableCollection<CardViewModel>();

                DealCommand = new RelayCommand(DoDeal, () => CanDeal);
                HitCommand = new RelayCommand(DoHit, () => CanHit);
                StandCommand = new RelayCommand(DoStand, () => CanStand);
                DoubleDownCommand = new RelayCommand(DoDoubleDown, () => CanDoubleDown);
                NewGameCommand = new RelayCommand(DoNewRound, () => _gameState == GameState.RoundOver || _gameState == GameState.Idle);
                AddChipCommand = new RelayCommand<int>(p => DoAddChip(p), _ => CanPlaceBet);
                ClearBetCommand = new RelayCommand(DoClearBet, () => CanClearBet);
            }

            // ── Observable Collections ────────────────────────────────────────────
            public ObservableCollection<CardViewModel> PlayerCards { get; }
            public ObservableCollection<CardViewModel> DealerCards { get; }

            // ── Commands ──────────────────────────────────────────────────────────
            public ICommand DealCommand { get; }
            public ICommand HitCommand { get; }
            public ICommand StandCommand { get; }
            public ICommand DoubleDownCommand { get; }
            public ICommand NewGameCommand { get; }
            public ICommand AddChipCommand { get; }
            public ICommand ClearBetCommand { get; }

            // Notify RelayCommands to refresh their CanExecute state
            private void RefreshCommands()
            {
                // RelayCommand implements IRelayCommand which exposes NotifyCanExecuteChanged
                (DealCommand as IRelayCommand)?.NotifyCanExecuteChanged();
                (HitCommand as IRelayCommand)?.NotifyCanExecuteChanged();
                (StandCommand as IRelayCommand)?.NotifyCanExecuteChanged();
                (DoubleDownCommand as IRelayCommand)?.NotifyCanExecuteChanged();
                (NewGameCommand as IRelayCommand)?.NotifyCanExecuteChanged();
                (AddChipCommand as IRelayCommand)?.NotifyCanExecuteChanged();
                (ClearBetCommand as IRelayCommand)?.NotifyCanExecuteChanged();
            }

            // ── Properties ────────────────────────────────────────────────────────
            public GameState GameState
            {
                get => _gameState;
                private set
                {
                    if (SetProperty(ref _gameState, value))
                    {
                        OnPropertyChanged(nameof(CanDeal));
                        OnPropertyChanged(nameof(CanHit));
                        OnPropertyChanged(nameof(CanStand));
                        OnPropertyChanged(nameof(CanDoubleDown));
                        OnPropertyChanged(nameof(CanPlaceBet));
                        OnPropertyChanged(nameof(CanClearBet));
                        OnPropertyChanged(nameof(IsBettingPhase));
                        OnPropertyChanged(nameof(IsPlayerTurn));
                        OnPropertyChanged(nameof(IsRoundOver));
                        // Ensure commands re-evaluate their CanExecute
                        RefreshCommands();
                    }
                }
            }

            public int PlayerBalance
            {
                get => _playerBalance;
                private set => SetProperty(ref _playerBalance, value);
            }

            public int CurrentBet
            {
                get => _currentBet;
                private set
                {
                    if (SetProperty(ref _currentBet, value))
                    {
                        OnPropertyChanged(nameof(CanDeal));
                        // Bet changed, update command state
                        RefreshCommands();
                    }
                }
            }

            public int SelectedChip
            {
                get => _selectedChip;
                set => SetProperty(ref _selectedChip, value);
            }

            public string StatusMessage
            {
                get => _statusMessage;
                private set => SetProperty(ref _statusMessage, value);
            }

            public string ResultMessage
            {
                get => _resultMessage;
                private set => SetProperty(ref _resultMessage, value);
            }

            public bool ResultVisible
            {
                get => _resultVisible;
                private set => SetProperty(ref _resultVisible, value);
            }

            public bool ResultIsWin
            {
                get => _resultIsWin;
                private set => SetProperty(ref _resultIsWin, value);
            }

            public int DealerScore
            {
                get => _dealerScore;
                private set => SetProperty(ref _dealerScore, value);
            }

            public int PlayerScore
            {
                get => _playerScore;
                private set => SetProperty(ref _playerScore, value);
            }

            public int Wins { get => _wins; private set => SetProperty(ref _wins, value); }
            public int Losses { get => _losses; private set => SetProperty(ref _losses, value); }
            public int Pushes { get => _pushes; private set => SetProperty(ref _pushes, value); }

            // ── Computed booleans ─────────────────────────────────────────────────
            public bool IsBettingPhase => _gameState == GameState.Idle || _gameState == GameState.Betting;
            public bool IsPlayerTurn => _gameState == GameState.PlayerTurn;
            public bool IsRoundOver => _gameState == GameState.RoundOver;

            public bool CanDeal => IsBettingPhase && _currentBet > 0;
            public bool CanHit => _gameState == GameState.PlayerTurn;
            public bool CanStand => _gameState == GameState.PlayerTurn;
            public bool CanDoubleDown => _gameState == GameState.PlayerTurn
                                          && _playerHand.Count == 2
                                          && _playerBalance >= _currentBet;
            public bool CanPlaceBet => IsBettingPhase && _playerBalance > 0;
            public bool CanClearBet => IsBettingPhase && _currentBet > 0;

            // ── Chip values ───────────────────────────────────────────────────────
            public int[] ChipValues => new[] { 5, 10, 25, 50, 100 };

            // ── Actions ───────────────────────────────────────────────────────────
            private void DoAddChip(int amount)
            {
                if (!CanPlaceBet) return;
                int add = Math.Min(amount, _playerBalance);
                if (add <= 0) return;
                CurrentBet += add;
                PlayerBalance -= add;
                GameState = GameState.Betting;
                StatusMessage = $"Bet: ${CurrentBet}  |  Click Deal to play!";
            }

            private void DoClearBet()
            {
                PlayerBalance += CurrentBet;
                CurrentBet = 0;
                GameState = GameState.Idle;
                StatusMessage = "Place your bet to start!";
            }

            private void DoDeal()
            {
                if (!CanDeal) return;

                // Reset table
                _playerHand.Clear();
                _dealerHand.Clear();
                PlayerCards.Clear();
                DealerCards.Clear();
                ResultVisible = false;
                ResultMessage = "";

                _deck.Reset();

                // Deal: player, dealer(face-down), player, dealer
                AddCardToPlayer(_deck.Deal());
                AddCardToDealer(_deck.Deal(faceDown: true));
                AddCardToPlayer(_deck.Deal());
                AddCardToDealer(_deck.Deal());

                UpdateScores();
                GameState = GameState.PlayerTurn;

                // Immediate blackjack check
                if (_playerHand.IsBlackjack)
                {
                    RevealDealerCard();
                    UpdateScores();
                    if (_dealerHand.IsBlackjack)
                        EndRound(RoundResult.Push);
                    else
                        EndRound(RoundResult.PlayerBlackjack);
                    return;
                }

                StatusMessage = "Your turn: Hit, Stand, or Double Down";
            }

            private void DoHit()
            {
                if (!CanHit) return;
                AddCardToPlayer(_deck.Deal());
                UpdateScores();

                if (_playerHand.IsBust)
                {
                    RevealDealerCard();
                    UpdateScores();
                    EndRound(RoundResult.DealerWins);
                    return;
                }

                if (_playerHand.Value == 21)
                    DoStand();

                StatusMessage = $"Your score: {_playerHand.Value}. Hit or Stand?";
            }

            private void DoStand()
            {
                if (!CanStand) return;
                RevealDealerCard();
                UpdateScores();
                GameState = GameState.DealerTurn;
                RunDealerAI();
            }

            private void DoDoubleDown()
            {
                if (!CanDoubleDown) return;
                PlayerBalance -= CurrentBet;
                CurrentBet *= 2;
                AddCardToPlayer(_deck.Deal());
                UpdateScores();

                if (_playerHand.IsBust)
                {
                    RevealDealerCard();
                    UpdateScores();
                    EndRound(RoundResult.DealerWins);
                    return;
                }

                DoStand();
            }

            private void DoNewRound()
            {
                if (_playerBalance <= 0)
                {
                    PlayerBalance = 1000;
                    StatusMessage = "Balance refilled to $1,000!";
                }

                _playerHand.Clear();
                _dealerHand.Clear();
                PlayerCards.Clear();
                DealerCards.Clear();
                CurrentBet = 0;
                ResultVisible = false;
                ResultMessage = "";
                DealerScore = 0;
                PlayerScore = 0;
                GameState = GameState.Idle;
                StatusMessage = "Place your bet to start!";
            }

            // ── Dealer AI ─────────────────────────────────────────────────────────
            private async void RunDealerAI()
            {
                StatusMessage = "Dealer's turn...";

                // Dealer hits on soft 17 or less
                while (_dealerHand.Value < 17 || (_dealerHand.IsSoft && _dealerHand.Value == 17))
                {
                    await Task.Delay(700);
                    AddCardToDealer(_deck.Deal());
                    UpdateScores();
                }

                await Task.Delay(400);
                DetermineWinner();
            }

            private void DetermineWinner()
            {
                int p = _playerHand.Value;
                int d = _dealerHand.Value;

                RoundResult result;
                if (_dealerHand.IsBust) result = RoundResult.PlayerWins;
                else if (p > d) result = RoundResult.PlayerWins;
                else if (d > p) result = RoundResult.DealerWins;
                else result = RoundResult.Push;

                EndRound(result);
            }

            private void EndRound(RoundResult result)
            {
                var gameResult = new GameResult { Result = result };

                switch (result)
                {
                    case RoundResult.PlayerBlackjack:
                        int bjPayout = (int)(_currentBet * 1.5);
                        gameResult.Payout = _currentBet + bjPayout;
                        PlayerBalance += gameResult.Payout;
                        Wins++;
                        break;
                    case RoundResult.PlayerWins:
                        gameResult.Payout = _currentBet * 2;
                        PlayerBalance += gameResult.Payout;
                        Wins++;
                        break;
                    case RoundResult.Push:
                        gameResult.Payout = _currentBet;
                        PlayerBalance += gameResult.Payout;
                        Pushes++;
                        break;
                    case RoundResult.DealerWins:
                    case RoundResult.DealerBlackjack:
                        // Bet already deducted
                        Losses++;
                        break;
                }

                CurrentBet = 0;
                ResultMessage = gameResult.Message;
                ResultIsWin = gameResult.IsWin || gameResult.IsPush;
                ResultVisible = true;
                StatusMessage = gameResult.Message;
                GameState = GameState.RoundOver;
            }

            // ── Helpers ───────────────────────────────────────────────────────────
            private void AddCardToPlayer(Card card)
            {
                _playerHand.Add(card);
                PlayerCards.Add(new CardViewModel(card));
            }

            private void AddCardToDealer(Card card)
            {
                _dealerHand.Add(card);
                DealerCards.Add(new CardViewModel(card));
            }

            private void RevealDealerCard()
            {
                var faceDown = DealerCards.FirstOrDefault(c => c.IsFaceDown);
                faceDown?.Reveal();
            }

            private void UpdateScores()
            {
                PlayerScore = _playerHand.Value;

                // Only count visible dealer cards for display
                int dealerVisible = 0;
                int aces = 0;
                foreach (var c in _dealerHand.Cards)
                {
                    if (c.IsFaceDown) continue;
                    if (c.Rank == Rank.Ace) aces++;
                    dealerVisible += c.Value;
                }
                while (dealerVisible > 21 && aces > 0) { dealerVisible -= 10; aces--; }
                DealerScore = dealerVisible;
            }
        }
    }
