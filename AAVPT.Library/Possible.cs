using System;
using System.Collections.Generic;
using System.Linq;

namespace AAVPT.Library
{
    public class Possible : IComparable<Possible>
    {
        public HandPossible HandPossible { get; set; }
        public List<Card> Cards { get; set; }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (!(other is Possible otherPossible))
            {
                return false;
            }
            if (HandPossible != otherPossible.HandPossible)
            {
                return false;
            }
            if (Cards.Count != otherPossible.Cards.Count)
            {
                return false;
            }

            var cards = Cards.OrderBy(card => card.PrimeRank * 100 + card.PrimeSuit).ToList();
            var otherCards = otherPossible.Cards.OrderBy(card => card.PrimeRank * 100 + card.PrimeSuit).ToList();

            return cards.SequenceEqual(otherCards);
        }

        public override int GetHashCode()
        {
            return (int)HandPossible + Cards.Sum(card => card.GetHashCode());
        }

        public int CompareTo(Possible other)
		{
			if (HandPossible > other.HandPossible) return 1;
			else if (HandPossible < other.HandPossible) return -1;

			for (var i = 0; i < Cards.Count; i++)
			{
				if (Cards[i].Rank > other.Cards[i].Rank) return 1;
				if (Cards[i].Rank < other.Cards[i].Rank) return -1;
			}

			return 0;
		}
	}
}