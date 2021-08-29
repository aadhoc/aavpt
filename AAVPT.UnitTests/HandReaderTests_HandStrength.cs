using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using AAVPT.Library;

namespace AAVPT.UnitTests
{
    [TestClass]
    public class HandReaderTests_HandStrength
    {
        [TestMethod]
        public void HandReader_GetHandStrength_WhenRoyal_ShouldFindRoyal()
        {
            // Arrange
            var handStr = "TH JH QH KH AH";
            var expectedKickersStr = "AH KH QH JH TH";
            var expectedHandRanking = HandRanking.RoyalFlush;
            var expectedPossibles = new List<Possible>();

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenFiveHeartSequential_ShouldFindStraightFlush()
        {
            // Arrange
            var handStr = "3H 4H 5H 6H 7H";
            var expectedKickersStr = "7H 6H 5H 4H 3H";
            var expectedHandRanking = HandRanking.StraightFlush;
            var expectedPossibles = new List<Possible>();

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenFourJacks_ShouldFindFourOfAKind()
        {
            // Arrange
            var handStr = "3H JS JH JD JC";
            var expectedKickersStr = "JC 3H";
            var expectedHandRanking = HandRanking.FourOfAKind;
            var expectedPossibles = new List<Possible>();

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenSuitedJQKA_ShouldFindHighCardAnd4PossibleRoyal()
        {
            // Arrange
            var handStr = "3H JS QS KS AS";
            var expectedKickersStr = "AS KS QS JS 3H";
            var expectedHandRanking = HandRanking.HighCard;
            var expectedPossibles = new List<Possible>()
            {
                new Possible()
                {
                    HandPossible = HandPossible.Royal,
                    Cards = new Hand("JS QS KS AS").Cards
                }
            };

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenThree4AndTwo9_ShouldFindFullHouse()
        {
            // Arrange
            var handStr = "4H 4D 4S 9H 9C";
            var expectedKickersStr = "9C 4S";
            var expectedHandRanking = HandRanking.FullHouse;
            var expectedPossibles = new List<Possible>();

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenSuited359TJ_ShouldFindFlushAndPossible3StraightFlush()
        {
            // Arrange
            var handStr = "3H 5H 9H TH JH";
            var expectedKickersStr = "JH TH 9H 5H 3H";
            var expectedHandRanking = HandRanking.Flush;
            var expectedPossibles = new List<Possible>()
            {
                new Possible()
                {
                    HandPossible = HandPossible.StraightFlush,
                    Cards = new Hand("9H TH JH").Cards
                }
            };

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenThree4_ShouldFindThreeOfAKind()
        {
            // Arrange
            var handStr = "3S 4S 4H 4C 7C";
            var expectedKickersStr = "4C 7C 3S";
            var expectedHandRanking = HandRanking.ThreeOfAKind;
            var expectedPossibles = new List<Possible>();

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_When3Through7_ShouldFindStraight()
        {
            // Arrange
            var handStr = "3S 4H 5D 6S 7C";
            var expectedKickersStr = "7C 6S 5D 4H 3S";
            var expectedHandRanking = HandRanking.Straight;
            var expectedPossibles = new List<Possible>();

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenTwo3AndTwo8_ShouldFindTwoPair()
        {
            // Arrange
            var handStr = "3S 3H 4D 8S 8C";
            var expectedKickersStr = "8C 3H 4D";
            var expectedHandRanking = HandRanking.TwoPair;
            var expectedPossibles = new List<Possible>();

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenTwoJacks_ShouldFindHighPair()
        {
            // Arrange
            var handStr = "3H 4H 8D JD JC";
            var expectedKickersStr = "JC 8D 4H 3H";
            var expectedHandRanking = HandRanking.HighPair;
            var expectedPossibles = new List<Possible>();

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenSuitedJQK_ShouldFindHighCardAnd3PossibleRoyal()
        {
            // Arrange
            var handStr = "3H JS QS KS AC";
            var expectedKickersStr = "AC KS QS JS 3H";
            var expectedHandRanking = HandRanking.HighCard;
            var expectedPossibles = new List<Possible>()
            {
                new Possible()
                {
                    HandPossible = HandPossible.Royal,
                    Cards = new Hand("JS QS KS").Cards
                }
            };

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenSuited3579_ShouldFindHighCardAnd3PossibleFlush()
        {
            // Arrange
            var handStr = "3H 5H 7H 9H AC";
            var expectedKickersStr = "AC 9H 7H 5H 3H";
            var expectedHandRanking = HandRanking.HighCard;
            var expectedPossibles = new List<Possible>()
            {
                new Possible()
                {
                    HandPossible = HandPossible.Flush,
                    Cards = new Hand("3H 5H 7H 9H").Cards
                }
            };

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenTwoTens_ShouldFindLowPair()
        {
            // Arrange
            var handStr = "3H 8H TD TC AS";
            var expectedKickersStr = "TC AS 8H 3H";
            var expectedHandRanking = HandRanking.LowPair;
            var expectedPossibles = new List<Possible>();

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenNonSuited5678_ShouldFindHighCardAndPossibleStraight()
        {
            // Arrange
            var handStr = "2H 5H 6D 7C 8S";
            var expectedKickersStr = "8S 7C 6D 5H 2H";
            var expectedHandRanking = HandRanking.HighCard;
            var expectedPossibles = new List<Possible>()
            {
                new Possible()
                {
                    HandPossible = HandPossible.Straight,
                    Cards = new Hand("5H 6D 7C 8S").Cards
                }
            };

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenSuited567_ShouldFindHighCardAndPossibleStraightFlush()
        {
            // Arrange
            var handStr = "2H 5H 6H 7H 8S";
            var expectedKickersStr = "8S 7H 6H 5H 2H";
            var expectedHandRanking = HandRanking.HighCard;
            var expectedPossibles = new List<Possible>()
            {
                new Possible()
                {
                    HandPossible = HandPossible.StraightFlush,
                    Cards = new Hand("5H 6H 7H").Cards
                }
            };

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenNonSuitedJQA_ShouldFindHighCardAndPossible3Highs()
        {
            // Arrange
            var handStr = "2H 7C JD QH AS";
            var expectedKickersStr = "AS QH JD 7C 2H";
            var expectedHandRanking = HandRanking.HighCard;
            var expectedPossibles = new List<Possible>()
            {
                new Possible()
                {
                    HandPossible = HandPossible.Highs,
                    Cards = new Hand("JD QH AS").Cards
                }
            };

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenNonSuitedQA_ShouldFindHighCardAndPossible2Highs()
        {
            // Arrange
            var handStr = "2H 7C 8D QH AS";
            var expectedKickersStr = "AS QH 8D 7C 2H";
            var expectedHandRanking = HandRanking.HighCard;
            var expectedPossibles = new List<Possible>()
            {
                new Possible()
                {
                    HandPossible = HandPossible.Highs,
                    Cards = new Hand("QH AS").Cards
                }
            };

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenNonSuitedA_ShouldFindHighCardAndPossible1Highs()
        {
            // Arrange
            var handStr = "2H 7C 8D TH AS";
            var expectedKickersStr = "AS TH 8D 7C 2H";
            var expectedHandRanking = HandRanking.HighCard;
            var expectedPossibles = new List<Possible>()
            {
                new Possible()
                {
                    HandPossible = HandPossible.Highs,
                    Cards = new Hand("AS").Cards
                }
            };

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        [TestMethod]
        public void HandReader_GetHandStrength_WhenNonSuited2457T_ShouldFindHighCardAndNoPossibles()
        {
            // Arrange
            var handStr = "2H 4C 5D 7H TS";
            var expectedKickersStr = "TS 7H 5D 4C 2H";
            var expectedHandRanking = HandRanking.HighCard;
            var expectedPossibles = new List<Possible>();

            // Act
            var actualHandStrength = HandReader.GetHandStrength(new Hand(handStr));

            // Assert
            AssertHandStrength(actualHandStrength, expectedHandRanking, expectedKickersStr, expectedPossibles);
        }

        private void AssertHandStrength(HandStrength actualHandStrength, HandRanking expectedHandRanking, string expectedKickersStr, List<Possible> expectedPossibles)
        {
            Assert.AreEqual(expectedHandRanking, actualHandStrength.HandRanking);
            CollectionAssert.AreEquivalent(new Hand(expectedKickersStr).Cards.Select(card => (int)card.Rank).ToArray(), actualHandStrength.Kickers);
            CollectionAssert.AreEquivalent(expectedPossibles, actualHandStrength.Possibles);
        }
    }
}
