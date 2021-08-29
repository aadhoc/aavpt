using System;
using System.Collections.Generic;
using System.Linq;

namespace AAVPT.Library
{

    public class Hand
	{
		public List<Card> Cards { get; set; }

		public Hand()
		{
			Cards = new List<Card>();
		}

		public Hand(IEnumerable<Card> cards)
		{
			Cards = cards.ToList();
		}

		public Hand(string cardsStr)
        {
			var cards = new List<Card>();
			var cardStrs = cardsStr.Split(' ');
			foreach (var cardStr in cardStrs)
            {
				cards.Add(new Card(cardStr));
            }
			Cards = cards;
		}

		private static char[] Ranks = Card.Ranks;
		private static char[] Suits = Card.SuitTexts;

		public Hand(ulong bitmap)
		{
			Cards = new List<Card>();

			for (int r = 0; r < Ranks.Length; r++)
			{
				for (int s = 0; s < Suits.Length; s++)
				{
					var shift = r * 4 + s;
					if (((1ul << shift) & bitmap) != 0)
					{
						Cards.Add(new Card(Ranks[r].ToString() + Suits[s].ToString()));
					}
				}
			}
		}

		public ulong GetBitmap()
        {
			ulong bitmap = 0;
			foreach (var card in Cards)
            {
				bitmap |= card.GetBitmap();
            }

			return bitmap;
        }

		//public void PrintColoredCards(string end = "")
		//{
		//	for (int i = 0; i < Cards.Count; i++)
		//	{
		//		Card card = Cards.ElementAt(i);
		//		Console.ForegroundColor = Card.SuitColors[(int)card.Suit];
		//		Console.Write("{0}", card);
		//		if (i < Cards.Count - 1) Console.Write(" ");
		//	}
		//	Console.ResetColor();
		//	Console.Write(end);
		//}

		public override string ToString()
		{
			return string.Join(" ", Cards.Select(card => card.ToString()));
		}

		public string ToString(char[] suits)
		{
			return string.Join(" ", Cards.Select(card => card.ToString(suits)));
		}
	}
}
