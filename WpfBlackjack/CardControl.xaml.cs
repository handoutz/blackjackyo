using System;
using System.Collections.Generic;
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
	/// Interaction logic for CardControl.xaml
	/// </summary>
	public partial class CardControl : UserControl
	{
		public Card Card { get; set; }
		public CardControl()
		{
			InitializeComponent();
		}

		public CardControl(Card card) : this()
		{
			Card = card;
			Card.PropertyChanged += Card_PropertyChanged;
			SetText();
		}

		private void SetText()
		{
			var card = Card;
			lblName2.Content = lblSuite.Content = card.Shown ? card.Title : "---";
			if (Card.Shown)
				lblSuite.Content = Enum.GetName(typeof(Suite), Card.Suite);
		}

		private void Card_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			var card = Card;
			lblName2.Content = lblSuite.Content = card.Shown ? card.Title : "---";
			if (card.Shown)
				lblSuite.Content = Enum.GetName(typeof(Suite), card.Suite);
		}
	}
}
