using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfBlackjack.Annotations;

namespace WpfBlackjack
{
	public class GameState : INotifyPropertyChanged
	{

		public static readonly int[] BetAmounts = { 50, 100, 250, 1000 };
		public static GameState Instance { get; set; }

		public GameState()
		{
			Instance = this;
		}
		#region BS
		private bool _gameEnabled;

		public bool GameEnabled
		{
			get { return _gameEnabled; }
			set
			{
				_gameEnabled = value;
			}
		}

		public enum Change
		{
			Bet,
			Hit,
			DoubleUp,
			Split,
			Stand,
			Bust,
			Win,
			Push,
			Loss
		}

		public delegate void StateChange(Change change);

		public event StateChange OnStateChange;

		protected virtual void OnOnStateChange(Change change)
		{
			OnStateChange?.Invoke(change);
		}

		public void InvokeChange(Change change)
		{
			OnOnStateChange(change);
		} 
		#endregion

		private int _playerBalance = 5000;

		public int PlayerBalance
		{
			get { return _playerBalance; }
			set
			{
				if (value == _playerBalance) return;
				_playerBalance = value;
				OnPropertyChanged();
			}
		}

		private int _currentBet;

		public int CurrentBet
		{
			get { return _currentBet; }
			set
			{
				if (value == _currentBet) return;
				_currentBet = value;
				OnPropertyChanged();
			}
		}

		private int _lastWin;

		public int LastWin
		{
			get { return _lastWin; }
			set
			{
				if (value == _lastWin) return;
				_lastWin = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
