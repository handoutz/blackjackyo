using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Security;

namespace WpfBlackjack
{
	public static class Rander
	{
		public static double RandomDouble(double min = 0, double max = 0)
		{
			var randr = new SecureRandom();
			var tmp = randr.NextDouble();
			return (tmp * max) + min;
		}

		public static int RandomInt(int min = 0, int max = int.MaxValue)
		{
			var randr = new SecureRandom();
			return randr.Next(min, max);
		}
	}
}
