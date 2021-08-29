using System;
using System.Collections.Generic;

namespace AAVPT.Library
{
	public class HandStrength : IComparable<HandStrength>
	{
		public HandRanking HandRanking { get; set; }
		public List<int> Kickers { get; set; }

		public List<Possible> Possibles { get; set; }

		public int CompareTo(HandStrength other)
		{
			if (HandRanking > other.HandRanking) return 1;
			else if (HandRanking < other.HandRanking) return -1;

			for (var i = 0; i < Kickers.Count; i++)
			{
				if (Kickers[i] > other.Kickers[i]) return 1;
				if (Kickers[i] < other.Kickers[i]) return -1;
			}

			return 0;
		}
	}
}
