using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfBlackjack
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private CardHandler _cardHandler;
		public MainWindow()
		{
			InitializeComponent();
			_cardHandler = new CardHandler();
			_cardHandler.Shuffle();
			Loaded += MainWindow_Loaded;
			Loaded += BettingLoaded;
			DataContext = _cardHandler.State;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			btnHit.Click += Button_Click;
			btnBet.Click += BtnBetClick;
			btnStand.Click += BtnStand_Click;
			btnDoubleUp.Click += BtnDoubleUpOnClick;

			_cardHandler.DealerHand.CollectionChanged += DealerHand_CollectionChanged;
			_cardHandler.PlayerHand.CollectionChanged += PlayerHand_CollectionChanged;
			_cardHandler.State.OnStateChange += State_OnStateChange;

			btnDoubleUp.IsEnabled = btnHit.IsEnabled = btnStand.IsEnabled = false;
		}

		private void BtnDoubleUpOnClick(object sender, RoutedEventArgs routedEventArgs)
		{
			_cardHandler.State.InvokeChange(GameState.Change.Hit);
			_cardHandler.State.InvokeChange(GameState.Change.Stand);
		}

		private void State_OnStateChange(GameState.Change change)
		{
			var psum = _cardHandler.PlayerHand.Sum();

			if (change == GameState.Change.Bet)
			{
				_cardHandler.Reset();
				_cardHandler.State.GameEnabled = true;

				var next = _cardHandler.Draw();
				var nxt = _cardHandler.Draw().AsHidden();
				_cardHandler.DealerHand.AddCard(next);
				_cardHandler.DealerHand.AddCard(nxt);
				lblDealerSum.Content = $"Dealer: {_cardHandler.DealerHand.Sum() - nxt.Value}?";

				_cardHandler.PlayerHand.AddCard(_cardHandler.Draw());
				_cardHandler.PlayerHand.AddCard(_cardHandler.Draw());
				btnDoubleUp.IsEnabled = btnHit.IsEnabled = btnStand.IsEnabled = true;
				btnBet.IsEnabled = false;
				lblStatus.Content = "OKAY";
			}
			else if (change == GameState.Change.Hit)
			{
				var card = _cardHandler.Draw();
				_cardHandler.PlayerHand.AddCard(card);
			}
			else if (change == GameState.Change.DoubleUp)
			{
			}
			else if (change == GameState.Change.Split)
			{
			}
			else if (change == GameState.Change.Stand)
			{
				_cardHandler.DealerHand.Show();

				if (psum > 21)
				{
					_cardHandler.State.InvokeChange(GameState.Change.Bust);
					return;
				}

				while (_cardHandler.DealerHand.Sum() < 17)
				{
					_cardHandler.DealerHand.AddCard(_cardHandler.Draw());
				}

				if (psum > _cardHandler.DealerHand.Sum() || _cardHandler.DealerHand.Sum() > 21)
				{
					_cardHandler.State.InvokeChange(GameState.Change.Win);
					lblStatus.Content = "WIN";
				}
				else if (psum == _cardHandler.DealerHand.Sum())
				{
					_cardHandler.State.InvokeChange(GameState.Change.Push);
					lblStatus.Content = "PUSH";
				}
				else
				{
					_cardHandler.State.InvokeChange(GameState.Change.Loss);
					lblStatus.Content = "LOSE";
				}
				btnDoubleUp.IsEnabled = btnHit.IsEnabled = btnStand.IsEnabled = false;
				btnBet.IsEnabled = true;
			}
			else if (change == GameState.Change.Bust)
			{
				_cardHandler.Bust();
				_cardHandler.DealerHand.Show();
				lblStatus.Content = "BUST";
				btnDoubleUp.IsEnabled = btnHit.IsEnabled = btnStand.IsEnabled = false;
				btnBet.IsEnabled = true;
			}
			SetText();
		}

		private void SetText()
		{
			var psum = _cardHandler.PlayerHand.Sum();
			if (psum > 21 && _cardHandler.State.GameEnabled)
			{
				lblStatus.Content = "BUST";
				_cardHandler.State.GameEnabled = false;
				_cardHandler.State.InvokeChange(GameState.Change.Bust);
			}

			lblDealerSum.Content = $"Dealer: {_cardHandler.DealerHand.Sum()}";
			lblPlayerSum.Content = $"Your Hand: {psum}";
		}
		private void PlayerHand_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
				wpPlayer.Children.Add(new CardControl(e.NewItems[0] as Card));
			else if (e.Action == NotifyCollectionChangedAction.Reset)
				wpPlayer.Children.Clear();
		}

		private void BtnStand_Click(object sender, RoutedEventArgs e)
		{
			_cardHandler.State.InvokeChange(GameState.Change.Stand);
		}

		private void BtnBetClick(object sender, RoutedEventArgs e)
		{
			if (_cardHandler.State.PlayerBalance <= 0 || _cardHandler.State.PlayerBalance - _cardHandler.State.CurrentBet <= 0)
				return;
			_cardHandler.DealerHand.Clear();
			_cardHandler.PlayerHand.Clear();
			_cardHandler.State.InvokeChange(GameState.Change.Bet);
		}

		private void DealerHand_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
				wpDealer.Children.Add(new CardControl(e.NewItems[0] as Card));
			else if (e.Action == NotifyCollectionChangedAction.Reset)
				wpDealer.Children.Clear();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			_cardHandler.State.InvokeChange(GameState.Change.Hit);
		}
	}
}
