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
	/// Interaction logic for ChipControl.xaml
	/// </summary>
	public partial class ChipControl : UserControl
	{
		public ChipControl()
		{
			InitializeComponent();
		}

		private int val;
		public string Value { get; set; }

		public ChipControl(int val) : this()
		{
			this.val = val;
			Value = val > 0 ? $"+{val}" : $"-{val}";
			label.DataContext = this;
			
		}

		public void SetValue(int v)
		{
			this.val = val;
			Value = val > 0 ? $"+{val}" : $"-{val}";
		}
	}
}
