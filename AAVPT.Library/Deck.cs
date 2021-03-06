using System;
using AAVPT.Library.Helpers;

namespace AAVPT.Library
{
	public class Deck
	{
		public static bool Debug;

		private ulong[] cards;
		private ulong removedCards;
		private int position;
		private Random random;

		// TODO: this metric doesn't account for removed cards
		public int CardsRemaining { get { return 52 - position; } }

		static Deck()
		{
			if (Debug) Console.WriteLine($"{nameof(Deck)}: Generating new map");
			var handBitmaps = CombinationsHelper.GenerateHandBitmaps((curCount, totalCount) =>
			{
				if (Debug) Console.Write($"{nameof(Deck)}: {curCount} / {totalCount}\r");
			});
		}

		public Deck(ulong removedCards = 0)
		{
			this.removedCards = removedCards;
			random = new Random();
			cards = new ulong[52];
			for (int i = 0; i < 52; i++) cards[i] = 1ul << i;
			position = 0;
		}

		public void Shuffle()
		{
			int n = cards.Length;
			while (n > 1)
			{
				n--;
				int k = random.Next(n + 1);
				ulong value = cards[k];
				cards[k] = cards[n];
				cards[n] = value;
			}
			position = 0;
		}

		public ulong Draw(int count)
		{
			ulong hand = 0;
			for (int i = 0; i < count; i++)
			{
				while ((cards[position] & removedCards) != 0) position++;
				hand |= cards[position];
				position++;
			}
			return hand;
		}
	}
}
