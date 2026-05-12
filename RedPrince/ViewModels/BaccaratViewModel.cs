using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RedPrince.Models;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Storage;

namespace RedPrince.ViewModels
{
    public class BaccaratViewModel : ObservableObject
    {
        public enum BetType { None, Player, Banker, Tie }

        private readonly Deck _deck = new();
        private readonly Hand _playerHand = new();
        private readonly Hand _bankerHand = new();

        private GameState _gameState = GameState.Idle;
        private int _playerBalance = 0;
        private readonly RedPrince.Services.DatabaseService _databaseService;
        private string _currentUsername;
        private int _currentBet = 0;
        private string _statusMessage = "Place a bet & choose who to back!";
        private string _resultMessage = "";
        private bool _resultVisible = false;
        private bool _resultIsWin = false;
        private int _bankerScore;
        private int _playerScore;
        private int _wins = 0;
        private int _losses = 0;
        private int _ties = 0;

        public BaccaratViewModel(RedPrince.Services.DatabaseService databaseService)
        {
            _databaseService = databaseService;
            _currentUsername = Preferences.Get("CurrentUser", string.Empty);
            if (!string.IsNullOrEmpty(_currentUsername))
            {
                var user = Task.Run(() => _databaseService.GetUserByUsernameAsync(_currentUsername)).Result;
                if (user != null)
                {
                    _playerBalance = user.Money;
                }
            }
            PlayerCards = new ObservableCollection<CardViewModel>();
            BankerCards = new ObservableCollection<CardViewModel>();

            DealPlayerCommand = new RelayCommand(() => DoDeal(BetType.Player), () => CanDeal);
            DealBankerCommand = new RelayCommand(() => DoDeal(BetType.Banker), () => CanDeal);
            DealTieCommand = new RelayCommand(() => DoDeal(BetType.Tie), () => CanDeal);
            NewGameCommand = new RelayCommand(DoNewRound, () => _gameState == GameState.RoundOver || _gameState == GameState.Idle);
            AddChipCommand = new RelayCommand<string>(p => { if (int.TryParse(p, out int val)) DoAddChip(val); }, _ => CanPlaceBet);
            ClearBetCommand = new RelayCommand(DoClearBet, () => CanClearBet);
        }

        public ObservableCollection<CardViewModel> PlayerCards { get; }
        public ObservableCollection<CardViewModel> BankerCards { get; }

        public ICommand DealPlayerCommand { get; }
        public ICommand DealBankerCommand { get; }
        public ICommand DealTieCommand { get; }
        public ICommand NewGameCommand { get; }
        public ICommand AddChipCommand { get; }
        public ICommand ClearBetCommand { get; }

        private void RefreshCommands()
        {
            (DealPlayerCommand as IRelayCommand)?.NotifyCanExecuteChanged();
            (DealBankerCommand as IRelayCommand)?.NotifyCanExecuteChanged();
            (DealTieCommand as IRelayCommand)?.NotifyCanExecuteChanged();
            (NewGameCommand as IRelayCommand)?.NotifyCanExecuteChanged();
            (AddChipCommand as IRelayCommand)?.NotifyCanExecuteChanged();
            (ClearBetCommand as IRelayCommand)?.NotifyCanExecuteChanged();
        }

        public GameState GameState
        {
            get => _gameState;
            private set
            {
                if (SetProperty(ref _gameState, value))
                {
                    OnPropertyChanged(nameof(CanDeal));
                    OnPropertyChanged(nameof(CanPlaceBet));
                    OnPropertyChanged(nameof(CanClearBet));
                    OnPropertyChanged(nameof(IsBettingPhase));
                    RefreshCommands();
                }
            }
        }

        public int PlayerBalance { get => _playerBalance; private set => SetProperty(ref _playerBalance, value); }
        public int CurrentBet
        {
            get => _currentBet;
            private set
            {
                if (SetProperty(ref _currentBet, value))
                {
                    OnPropertyChanged(nameof(CanDeal));
                    RefreshCommands();
                }
            }
        }

        public string StatusMessage { get => _statusMessage; private set => SetProperty(ref _statusMessage, value); }
        public string ResultMessage { get => _resultMessage; private set => SetProperty(ref _resultMessage, value); }
        public bool ResultVisible { get => _resultVisible; private set => SetProperty(ref _resultVisible, value); }
        public bool ResultIsWin { get => _resultIsWin; private set => SetProperty(ref _resultIsWin, value); }
        public int BankerScore { get => _bankerScore; private set => SetProperty(ref _bankerScore, value); }
        public int PlayerScore { get => _playerScore; private set => SetProperty(ref _playerScore, value); }
        public int Wins { get => _wins; private set => SetProperty(ref _wins, value); }
        public int Losses { get => _losses; private set => SetProperty(ref _losses, value); }
        public int Ties { get => _ties; private set => SetProperty(ref _ties, value); }

        public bool IsBettingPhase => _gameState == GameState.Idle || _gameState == GameState.Betting;
        public bool CanDeal => IsBettingPhase && _currentBet > 0;
        public bool CanPlaceBet => IsBettingPhase && _playerBalance > 0;
        public bool CanClearBet => IsBettingPhase && _currentBet > 0;

        private void DoAddChip(int amount)
        {
            if (!CanPlaceBet) return;
            int add = Math.Min(amount, _playerBalance);
            if (add <= 0) return;
            CurrentBet += add;
            PlayerBalance -= add;
            SaveBalanceToDb();
            GameState = GameState.Betting;
            StatusMessage = $"Bet: ${CurrentBet}  |  Choose who will win!";
        }

        private void DoClearBet()
        {
            PlayerBalance += CurrentBet;
            CurrentBet = 0;
            SaveBalanceToDb();
            GameState = GameState.Idle;
            StatusMessage = "Place a bet & choose who to back!";
        }

        private async void DoDeal(BetType bet)
        {
            if (!CanDeal) return;
            GameState = GameState.DealerTurn;

            _playerHand.Clear();
            _bankerHand.Clear();
            PlayerCards.Clear();
            BankerCards.Clear();
            ResultVisible = false;
            ResultMessage = "";

            _deck.Reset();
            StatusMessage = "Dealing cards...";

            await Task.Delay(500);
            AddCardToPlayer(_deck.Deal());
            await Task.Delay(500);
            AddCardToBanker(_deck.Deal());
            await Task.Delay(500);
            AddCardToPlayer(_deck.Deal());
            await Task.Delay(500);
            AddCardToBanker(_deck.Deal());

            UpdateScores();

            // Simplified Baccarat rules
            if (PlayerScore >= 8 || BankerScore >= 8)
            {
                DetermineWinner(bet);
                return;
            }

            if (PlayerScore <= 5)
            {
                await Task.Delay(1000);
                AddCardToPlayer(_deck.Deal());
                UpdateScores();
            }

            if (BankerScore <= 5)
            {
                await Task.Delay(1000);
                AddCardToBanker(_deck.Deal());
                UpdateScores();
            }

            await Task.Delay(500);
            DetermineWinner(bet);
        }

        private void DoNewRound()
        {
            if (_playerBalance <= 0)
            {
                PlayerBalance = 0;
                StatusMessage = "Balance is 0!";
                SaveBalanceToDb();
            }

            _playerHand.Clear();
            _bankerHand.Clear();
            PlayerCards.Clear();
            BankerCards.Clear();
            CurrentBet = 0;
            ResultVisible = false;
            ResultMessage = "";
            BankerScore = 0;
            PlayerScore = 0;
            GameState = GameState.Idle;
            StatusMessage = "Place a bet to start!";
        }

        private void DetermineWinner(BetType bet)
        {
            BetType actualWinner;
            if (PlayerScore > BankerScore) actualWinner = BetType.Player;
            else if (BankerScore > PlayerScore) actualWinner = BetType.Banker;
            else actualWinner = BetType.Tie;

            EndRound(actualWinner, bet);
        }

        private void EndRound(BetType actualWinner, BetType bet)
        {
            string msg;
            int payout = 0;

            if (actualWinner == bet)
            {
                if (bet == BetType.Tie) payout = _currentBet * 9;
                else payout = _currentBet * 2;
                PlayerBalance += payout;
                Wins++;
                msg = $"You won! {actualWinner} scored highest.";
                ResultIsWin = true;
            }
            else if (actualWinner == BetType.Tie && bet != BetType.Tie)
            {
                PlayerBalance += _currentBet; // Push
                Ties++;
                msg = "It's a Tie! Bet returned.";
                ResultIsWin = true;
            }
            else
            {
                Losses++;
                msg = $"You lost. {actualWinner} won.";
                ResultIsWin = false;
            }

            CurrentBet = 0;
            ResultMessage = msg;
            ResultVisible = true;
            StatusMessage = "Round over. Play again?";
            GameState = GameState.RoundOver;
            SaveBalanceToDb();
        }

        private void SaveBalanceToDb()
        {
            if (string.IsNullOrEmpty(_currentUsername) || _databaseService == null) return;
            try
            {
                var user = Task.Run(() => _databaseService.GetUserByUsernameAsync(_currentUsername)).Result;
                if (user != null)
                {
                    user.Money = PlayerBalance;
                    Task.Run(() => _databaseService.UpdateUserAsync(user)).Wait();
                }
            }
            catch { }
        }

        private void AddCardToPlayer(Card card)
        {
            _playerHand.Add(card);
            PlayerCards.Add(new CardViewModel(card));
        }

        private void AddCardToBanker(Card card)
        {
            _bankerHand.Add(card);
            BankerCards.Add(new CardViewModel(card));
        }

        private int BaccaratValue(Card c)
        {
            if (c.Rank == Rank.Ace) return 1;
            if (c.Rank >= Rank.Ten) return 0;
            return (int)c.Rank;
        }

        private void UpdateScores()
        {
            PlayerScore = _playerHand.Cards.Sum(BaccaratValue) % 10;
            BankerScore = _bankerHand.Cards.Sum(BaccaratValue) % 10;
        }
    }
}