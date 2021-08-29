using System.Linq;

namespace AAVPT.Library.HandEvaluators.Helpers
{
    public static class HoldHelper
    {
        public static Hand DetermineHoldHand(Hand hand)
        {
            Hand holdHand = null;
            HandStrength handStrength = HandReader.GetHandStrength(hand);
            var handRanking = handStrength.HandRanking;
            var possibles = handStrength.Possibles;

            var possibleRoyal = possibles.FirstOrDefault(possible => possible.HandPossible == HandPossible.Royal);
            var possibleFlush = possibles.FirstOrDefault(possible => possible.HandPossible == HandPossible.Flush);
            var possibleStraight = possibles.FirstOrDefault(possible => possible.HandPossible == HandPossible.Straight);
            var possibleStraightFlush = possibles.FirstOrDefault(possible => possible.HandPossible == HandPossible.StraightFlush);
            var possibleHighs = possibles.FirstOrDefault(possible => possible.HandPossible == HandPossible.Highs);

            if (handRanking == HandRanking.RoyalFlush)
            {
                holdHand = hand;
            }
            else if (handRanking == HandRanking.StraightFlush)
            {
                holdHand = hand;
            }
            else if (handRanking == HandRanking.FourOfAKind)
            {
                holdHand = new Hand(hand.Cards.Where(card => (int)card.Rank == handStrength.Kickers[0]));
            }
            else if (possibleRoyal != null && possibleRoyal.Cards.Count == 4)
            {
                holdHand = new Hand(possibleRoyal.Cards);
            }
            else if (handRanking == HandRanking.FullHouse)
            {
                holdHand = hand;
            }
            else if (handRanking == HandRanking.Flush)
            {
                holdHand = hand;
            }
            else if (handRanking == HandRanking.ThreeOfAKind)
            {
                holdHand = new Hand(hand.Cards.Where(card => (int)card.Rank == handStrength.Kickers[0]));
            }
            else if (handRanking == HandRanking.Straight)
            {
                holdHand = hand;
            }
            else if (handRanking == HandRanking.TwoPair)
            {
                holdHand = new Hand(
                    hand.Cards.Where(card => (int)card.Rank == handStrength.Kickers[0])
                    .Union(
                        hand.Cards.Where(card => (int)card.Rank == handStrength.Kickers[1])
                    ));
            }
            else if (handRanking == HandRanking.HighPair)
            {
                holdHand = new Hand(hand.Cards.Where(card => (int)card.Rank == handStrength.Kickers[0]));
            }
            else if (possibleRoyal != null && possibleRoyal.Cards.Count == 3)
            {
                holdHand = new Hand(possibleRoyal.Cards);
            }
            else if (possibleFlush != null && possibleFlush.Cards.Count == 4)
            {
                holdHand = new Hand(possibleFlush.Cards);
            }
            else if (handRanking == HandRanking.LowPair)
            {
                holdHand = new Hand(hand.Cards.Where(card => (int)card.Rank == handStrength.Kickers[0]));
            }
            else if (possibleStraight != null && possibleStraight.Cards.Count == 4)
            {
                holdHand = new Hand(possibleStraight.Cards);
            }
            else if (possibleStraightFlush != null && possibleStraightFlush.Cards.Count == 3)
            {
                holdHand = new Hand(possibleStraightFlush.Cards);
            }
            else if (possibleHighs != null)
            {
                if (possibleHighs.Cards.Count >= 3)
                {
                    var suitedAndConnectedCards = HandReader.CardsToRoyal(possibleHighs.Cards);
                    if (suitedAndConnectedCards.Count >= 2)
                    {
                        holdHand = new Hand(new[] { suitedAndConnectedCards[0], suitedAndConnectedCards[1] });
                    }
                    else
                    {
                        var suitedCards = HandReader.CardsToFlush(possibleHighs.Cards);
                        if (suitedCards.Count >= 2)
                        {
                            holdHand = new Hand(new[] { suitedCards[0], suitedCards[1] });
                        }
                        else
                        {
                            var connectedCards = HandReader.CardsToStraight(possibleHighs.Cards, false);
                            if (connectedCards.Count >= 2)
                            {
                                holdHand = new Hand(new[] { connectedCards[0], connectedCards[1] });
                            }
                        }
                    }

                    if (holdHand == null)
                    {
                        holdHand = new Hand(new[] { possibleHighs.Cards[0], possibleHighs.Cards[1] });
                    }
                }
                else
                {
                    holdHand = new Hand(possibleHighs.Cards);
                }
            }
            else
            {
                holdHand = new Hand(0);
            }

            return holdHand;
        }
    }
}
