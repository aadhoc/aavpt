using System;
using System.Collections.Generic;
using System.Linq;
using AAVPT.Library.Enums;

namespace AAVPT.Library
{
	public static class HandReader
	{
		public static bool Debug;

		private static Rank[] RanksOfRoyal = new Rank[]
		{
			Rank.Ten,
			Rank.Jack,
			Rank.Queen,
			Rank.King,
			Rank.Ace
		};

		public static HandStrength GetHandStrength(Hand hand)
		{
			var strength = new HandStrength
			{
				Kickers = new List<int>(),
				Possibles = new List<Possible>()
			};

			var cards = hand.Cards.OrderBy(card => card.PrimeRank * 100 + card.PrimeSuit).ToList();
			//var cardRanks = GetCardRanks(cards).ToArray();

			int rankProduct = cards.Select(card => card.PrimeRank).Aggregate((acc, r) => acc * r);
			int suitProduct = cards.Select(card => card.PrimeSuit).Aggregate((acc, r) => acc * r);

			bool straight =
				rankProduct == 8610         // 5-high straight
				|| rankProduct == 2310      // 6-high straight
				|| rankProduct == 15015     // 7-high straight
				|| rankProduct == 85085     // 8-high straight
				|| rankProduct == 323323    // 9-high straight
				|| rankProduct == 1062347   // T-high straight
				|| rankProduct == 2800733   // J-high straight
				|| rankProduct == 6678671   // Q-high straight
				|| rankProduct == 14535931  // K-high straight
				|| rankProduct == 31367009; // A-high straight

			bool royal = rankProduct == 31367009;

			bool flush =
				suitProduct == 147008443        // Spades
				|| suitProduct == 229345007     // Hearts
				|| suitProduct == 418195493     // Diamonds
				|| suitProduct == 714924299;    // Clubs

			var cardCounts = cards.GroupBy(card => (int)card.Rank).Select(group => group).ToList();

			var fourOfAKind = -1;
			var threeOfAKind = -1;
			var onePair = -1;
			var twoPair = -1;

			foreach (var group in cardCounts)
			{
				var rank = group.Key;
				var count = group.Count();
				if (count == 4) fourOfAKind = rank;
				else if (count == 3) threeOfAKind = rank;
				else if (count == 2)
				{
					twoPair = onePair;
					onePair = rank;
				}
			}

			if (royal && flush)
            {
				strength.HandRanking = HandRanking.RoyalFlush;
				strength.Kickers = cards.Select(card => (int)card.Rank).Reverse().ToList();
			}
			else if (straight && flush)
			{
				strength.HandRanking = HandRanking.StraightFlush;
				strength.Kickers = cards.Select(card => (int)card.Rank).Reverse().ToList();
			}
			else if (fourOfAKind >= 0)
			{
				strength.HandRanking = HandRanking.FourOfAKind;
				strength.Kickers.Add(fourOfAKind);
				strength.Kickers.AddRange(cards
					.Where(card => (int)card.Rank != fourOfAKind)
					.Select(card => (int)card.Rank));
			}
			else if (threeOfAKind >= 0 && onePair >= 0)
			{
				strength.HandRanking = HandRanking.FullHouse;
				strength.Kickers.Add(threeOfAKind);
				strength.Kickers.Add(onePair);
			}
			else if (flush)
			{
				strength.HandRanking = HandRanking.Flush;
				strength.Kickers.AddRange(cards
					.Select(card => (int)card.Rank)
					.Reverse());
			}
			else if (straight)
			{
				strength.HandRanking = HandRanking.Straight;
				strength.Kickers.AddRange(cards
					.Select(card => (int)card.Rank)
					.Reverse());
			}
			else if (threeOfAKind >= 0)
			{
				strength.HandRanking = HandRanking.ThreeOfAKind;
				strength.Kickers.Add(threeOfAKind);
				strength.Kickers.AddRange(cards
					.Where(card => (int)card.Rank != threeOfAKind)
					.Select(card => (int)card.Rank));
			}
			else if (twoPair >= 0)
			{
				strength.HandRanking = HandRanking.TwoPair;
				strength.Kickers.Add(Math.Max(twoPair, onePair));
				strength.Kickers.Add(Math.Min(twoPair, onePair));
				strength.Kickers.AddRange(cards
					.Where(card => (int)card.Rank != twoPair && (int)card.Rank != onePair)
					.Select(card => (int)card.Rank));
			}
			else if (onePair >= 0)
			{
				strength.HandRanking = onePair >= (int)Rank.Jack ? HandRanking.HighPair : HandRanking.LowPair;
                strength.Kickers.Add(onePair);
				strength.Kickers.AddRange(cards
					.Where(card => (int)card.Rank != onePair)
					.Select(card => (int)card.Rank));
			}
			else
			{
				strength.HandRanking = HandRanking.HighCard;
				strength.Kickers.AddRange(cards
					.Select(card => (int)card.Rank)
					.Reverse());
			}

			var cardsToRoyal = CardsToRoyal(cards);
			var cardsToStraightFlush = CardsToStraight(cards, true);
			var cardsToFlush = CardsToFlush(cards);
			var cardsToStraight = CardsToStraight(cards, false);
			var cardsHigh = CardsHigh(cards);

			//if (cardsToRoyal != null && cardsToRoyal.Count >= 3)
   //         {
   //             Console.WriteLine($"{hand} : cardsToRoyal : {new Hand(cardsToRoyal)}");
   //         }
			//if (cardsToFlush != null && cardsToFlush.Count >= 4)
			//{
			//	Console.WriteLine($"{hand} : cardsToFlush : {new Hand(cardsToFlush)}");
			//}
			//if (cardsToStraight != null && cardsToStraight.Count >= 4)
			//{
			//	Console.WriteLine($"{hand} : cardsToStraight : {new Hand(cardsToStraight)}");
			//}
			//if (cardsToStraightFlush != null && cardsToStraightFlush.Count >= 4)
			//{
			//	Console.WriteLine($"{hand} : cardsToStraightFlush : {new Hand(cardsToStraightFlush)}");
			//}
			//if (cardsHigh != null && cardsHigh.Count >= 4)
			//{
			//	Console.WriteLine($"{hand} : cardsHigh : {new Hand(cardsHigh)}");
			//}

			if ((strength.HandRanking < HandRanking.RoyalFlush) &&
				cardsToRoyal != null && (cardsToRoyal.Count == 3 || cardsToRoyal.Count == 4))
            {
				strength.Possibles.Add(new Possible
				{
					HandPossible = HandPossible.Royal,
					Cards = cardsToRoyal
				});
			}
			else if ((strength.HandRanking < HandRanking.StraightFlush) &&
				cardsToStraightFlush != null && (cardsToStraightFlush.Count == 3 || cardsToStraightFlush.Count == 4))
			{
				strength.Possibles.Add(new Possible
				{
					HandPossible = HandPossible.StraightFlush,
					Cards = cardsToStraightFlush
				});
			}
			else if ((strength.HandRanking < HandRanking.Flush) && 
				cardsToFlush != null && cardsToFlush.Count == 4)
			{
				strength.Possibles.Add(new Possible
				{
					HandPossible = HandPossible.Flush,
					Cards = cardsToFlush
				});
			}
			else if ((strength.HandRanking < HandRanking.Straight) &&
				cardsToStraight != null && cardsToStraight.Count == 4)
            {
				strength.Possibles.Add(new Possible
				{
					HandPossible = HandPossible.Straight,
					Cards = cardsToStraight
				});
			}
			else if ((strength.HandRanking < HandRanking.LowPair) &&
				cardsHigh != null && cardsHigh.Count > 0)
            {
				strength.Possibles.Add(new Possible
				{
					HandPossible = HandPossible.Highs,
					Cards = cardsHigh
				});
			}

			return strength;
		}

		public static List<Card> CardsToRoyal(List<Card> cardsRankLowToHigh)
		{
			return CardsTo(cardsRankLowToHigh, true, (card, previousCard) => card.Rank >= Rank.Ten && card.Rank <= Rank.Ace, startOverWhenUndesired: false);
		}

		public static List<Card> CardsToFlush(List<Card> cardsRankLowToHigh)
		{
			return CardsTo(cardsRankLowToHigh, true, (card, previousCard) => true, startOverWhenUndesired: false);
		}

		public static List<Card> CardsToStraight(List<Card> cardsRankLowToHigh, bool suited)
        {
			return CardsTo(cardsRankLowToHigh, suited, (card, previousCard) => {
				return ((previousCard == null) || (card.Rank == previousCard.Rank + 1));
			}, startOverWhenUndesired: true);
		}

		private static List<Card> CardsHigh(List<Card> cardsRankLowToHigh)
		{
			return CardsTo(cardsRankLowToHigh, false, (card, previousCard) => card.Rank >= Rank.Jack && card.Rank <= Rank.Ace, startOverWhenUndesired: false);
		}

		private static List<Card> CardsTo(List<Card> cardsRankLowToHigh, bool suited, Func<Card, Card, bool> desiredCardFunc, bool startOverWhenUndesired)
        {
			return suited ?
				CardsToSuited(cardsRankLowToHigh, desiredCardFunc, startOverWhenUndesired: startOverWhenUndesired) :
				CardsToUnSuited(cardsRankLowToHigh, desiredCardFunc, startOverWhenUndesired: startOverWhenUndesired);
        }

		private static List<Card> CardsToUnSuited(List<Card> cardsRankLowToHigh, Func<Card, Card, bool> desiredCardFunc, bool startOverWhenUndesired)
		{
			var holdCards = new List<Card>();
			var curHoldCards = new List<Card>();
			Card previousCard = null;

			void SnapshotHoldCards() {
				if (curHoldCards.Count >= holdCards.Count)
				{
					holdCards.Clear();
					holdCards.AddRange(curHoldCards);
					curHoldCards.Clear();
				}
				previousCard = null;
			}

			foreach (var card in cardsRankLowToHigh)
            {
				var desired = desiredCardFunc(card, previousCard);
				if (!desired)
                {
					if (startOverWhenUndesired)
					{
						SnapshotHoldCards();
						desired = desiredCardFunc(card, previousCard);
					}
				}

				if (desired)
				{
					curHoldCards.Add(card);
				}
				previousCard = card;
			}
			SnapshotHoldCards();

			return holdCards;
		}

		private static List<Card> CardsToSuited(List<Card> cardsRankLowToHigh, Func<Card, Card, bool> desiredCardFunc, bool startOverWhenUndesired)
		{
			List<Card> holdCards = null;

			foreach (var group in cardsRankLowToHigh
				.GroupBy(card => card.Suit))
            {
				var cards = CardsToUnSuited(group.ToList(), desiredCardFunc, startOverWhenUndesired: startOverWhenUndesired);
				if (cards.Count >= (holdCards == null ? 0 : holdCards.Count))
                {
					holdCards = cards;
                }
            }
			return holdCards;
		}
	}
}
