using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBlackjack
{
	public class CardHandler
	{
		public GameState State { get; set; } = new GameState();
		public Hand DealerHand { get; set; } = new Hand();
		public Hand PlayerHand { get; set; } = new Hand();
		public CardHandler()
		{
			Reset();
		}

		public void Reset()
		{
			Deck.Clear();
			_location = 0;
			foreach (var kvp in new Dictionary<string, int>
			{
				["2"] = 2,
				["3"] = 3,
				["4"] = 4,
				["5"] = 5,
				["6"] = 6,
				["7"] = 7,
				["8"] = 8,
				["9"] = 9,
				["10"] = 10,
				["J"] = 10,
				["Q"] = 10,
				["K"] = 10,
				["A"] = 1
			})
			{
				Deck.Add(new Card(Suite.Clubs, kvp.Key, kvp.Value));
				Deck.Add(new Card(Suite.Diamonds, kvp.Key, kvp.Value));
				Deck.Add(new Card(Suite.Hearts, kvp.Key, kvp.Value));
				Deck.Add(new Card(Suite.Spades, kvp.Key, kvp.Value));
			}
			Shuffle();
			PlayerHand.Clear();
			DealerHand.Clear();
		}

		private void SwapCard(int a, int b)
		{
			var tmp = Deck[a];
			Deck[a] = Deck[b];
			Deck[b] = tmp;
		}
		public void Shuffle(int n = 1024)
		{
			var len = Deck.Count - 1;
			foreach (var i in Enumerable.Range(0, n))
			{
				SwapCard(0, Rander.RandomInt(0, len));
			}
		}

		public Card Draw()
		{
			if(_location > Deck.Count - 1)
				Reset();
			return Deck[_location++];
		}

		private int _location;
		public List<Card> Deck { get; set; } = new List<Card>();

		public void Bust()
		{
			State.GameEnabled = false;
		}
	}
}
