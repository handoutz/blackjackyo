using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfBlackjack.Annotations;

namespace WpfBlackjack
{
	public class Card : INotifyPropertyChanged
	{
		public Suite Suite { get; set; }
		public string Title { get; set; }
		private bool _shown = true;

		public bool Shown
		{
			get { return _shown; }
			set
			{
				if (value == _shown) return;
				_shown = value;
				OnPropertyChanged();
			}
		}

		private int _value;

		public int Value
		{
			get { return _value; }
			set
			{
				if (value == _value) return;
				_value = value;
				OnPropertyChanged();
			}
		}

		public Card(Suite suite, string text, int value, bool shown=true)
		{
			Suite = suite;
			Title = text;
			Value = value;
			_shown = shown;
		}

		public Card AsHidden()
		{
			Shown = false;
			return this;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}