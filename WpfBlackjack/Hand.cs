using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfBlackjack.Annotations;

namespace WpfBlackjack
{
	public class Hand : INotifyCollectionChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<Card> Cards { get; set; } = new ObservableCollection<Card>();

		public int Sum()
		{
			var cds = Cards.Where(cd => cd.Shown).ToArray();
			var aces = cds.Where(c => c.Title == "A");
			var sum = cds.Where(c => c.Title != "A")
				.Sum(cd => cd.Value);
			var n = aces.Count();
			for (var i = 0; i < n; i++)
			{
				sum += 11;
			}

			while (sum > 21 && n > 0)
			{
				sum -= 10;
				n--;
			}
			return sum;
		}

		public void Show()
		{
			foreach (var cd in Cards)
				cd.Shown = true;
		}
		public void AddCard(Card card)
		{
			Cards.Add(card);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, card));
		}

		public void Clear()
		{
			Cards.Clear();
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, null));
		}

		#region Implementation of INotifyCollectionChanged

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		#endregion

		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			CollectionChanged?.Invoke(this, e);
		}
	}
}
