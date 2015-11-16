using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfBlackjack
{
	public partial class MainWindow : Window
	{
		private Button[] _plusButtons;
		private Button[] _minusButtons;
		private List<ChipControl> _plusChips = new List<ChipControl>();
		private List<ChipControl> _minusChips = new List<ChipControl>();
		private GameState _inst => GameState.Instance;
		private void BettingLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			_plusButtons = new[] { btnBetA, btnBetB, btnBetC, btnBetD };
			_minusButtons = new[] { btnMBetA, btnMBetB, btnMBetC, btnMBetD };
			foreach (var i in Enumerable.Range(0, GameState.BetAmounts.Length))
			{
				_plusButtons[i].Content = $"+{GameState.BetAmounts[i]}";
				_minusButtons[i].Content = $"{GameState.BetAmounts[i]}";
				_plusButtons[i].Click += pbClick;
				_minusButtons[i].Click += mbClick;
			}
			lblBalance.DataContext = lblBet.DataContext = lblBet_Copy.DataContext = GameState.Instance;
			GameState.Instance.OnStateChange += BettingStateChange;
		}

		private void BettingStateChange(GameState.Change change)
		{
			switch (change)
			{
				case GameState.Change.Loss:
				case GameState.Change.Bust:
					_inst.PlayerBalance -= _inst.CurrentBet;
					_inst.LastWin = 0;
					break;
				case GameState.Change.Win:
					_inst.PlayerBalance += _inst.CurrentBet * 2;
					_inst.LastWin = _inst.CurrentBet * 2;
					break;
				case GameState.Change.DoubleUp:
					_inst.CurrentBet *= 2;
					break;
				case GameState.Change.Push:
					_inst.LastWin = _inst.CurrentBet;
					_inst.PlayerBalance += _inst.CurrentBet;
					break;
			}
		}

		//plus button click
		private void pbClick(object sender, RoutedEventArgs e)
		{
			var btn = sender as Button;
			if (btn == null) return;
			var val = int.Parse(btn.Content.ToString());
			if (!OffsetBet(val)) return;
			var chip = new ChipControl(val);
			_plusChips.Add(chip);
			wpChipArea.Children.Add(chip);
		}
		//minus button click
		private void mbClick(object sender, RoutedEventArgs e)
		{
			var btn = sender as Button;
			if (btn == null) return;
			var val = int.Parse(btn.Content.ToString());
			if (!OffsetBet(val)) return;
			var chip = new ChipControl(val);
			_minusChips.Add(chip);
			wpChipArea.Children.Add(chip);
		}

		private bool OffsetBet(int amt)
		{
			var val = GameState.Instance.CurrentBet;
			var adjusted = val + amt;
			if (adjusted < 0)
				return false;
			if (adjusted > GameState.Instance.PlayerBalance)
				return false;
			GameState.Instance.CurrentBet = adjusted;
			return true;
		}
	}
}
