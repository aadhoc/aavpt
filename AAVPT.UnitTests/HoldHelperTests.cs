using Microsoft.VisualStudio.TestTools.UnitTesting;
using AAVPT.Library;
using AAVPT.Library.HandEvaluators.Helpers;

namespace AAVPT.UnitTests
{
    [TestClass]
    public class HoldHelperTests
    {
        [TestMethod]
        public void HoldHelper_DetermineHoldHand_WhenPossibleStraightAtFront_ShouldFindIt()
        {
            // Arrange
            var handStr = "7H 8H 9D TD AC";
            var expectedHoldHandStr = "7H 8H 9D TD";

            // Act
            var holdHand = HoldHelper.DetermineHoldHand(new Hand(handStr));

            // Assert
            var holdHandStr = holdHand.ToString(Card.SuitTexts);
            Assert.AreEqual(expectedHoldHandStr, holdHandStr);
        }

        [TestMethod]
        public void HoldHelper_DetermineHoldHand_WhenPossibleStraightAtBack_ShouldFindIt()
        {
            // Arrange
            var handStr = "5C 7H 8H 9D TD";
            var expectedHoldHandStr = "7H 8H 9D TD";

            // Act
            var holdHand = HoldHelper.DetermineHoldHand(new Hand(handStr));

            // Assert
            var holdHandStr = holdHand.ToString(Card.SuitTexts);
            Assert.AreEqual(expectedHoldHandStr, holdHandStr);
        }

        [TestMethod]
        public void HoldHelper_DetermineHoldHand_WhenPossibleFlushAtFront_ShouldFindIt()
        {
            // Arrange
            var handStr = "4D 8D JD KD AS";
            var expectedHoldHandStr = "4D 8D JD KD";

            // Act
            var holdHand = HoldHelper.DetermineHoldHand(new Hand(handStr));

            // Assert
            var holdHandStr = holdHand.ToString(Card.SuitTexts);
            Assert.AreEqual(expectedHoldHandStr, holdHandStr);
        }

        [TestMethod]
        public void HoldHelper_DetermineHoldHand_WhenPossibleFlushAtSplit_ShouldFindIt()
        {
            // Arrange
            var handStr = "2D 4H 8D JD KD";
            var expectedHoldHandStr = "2D 8D JD KD";

            // Act
            var holdHand = HoldHelper.DetermineHoldHand(new Hand(handStr));

            // Assert
            var holdHandStr = holdHand.ToString(Card.SuitTexts);
            Assert.AreEqual(expectedHoldHandStr, holdHandStr);
        }

        [TestMethod]
        public void HoldHelper_DetermineHoldHand_WhenPossibleFlushAtBack_ShouldFindIt()
        {
            // Arrange
            var handStr = "2C 4D 8D JD KD";
            var expectedHoldHandStr = "4D 8D JD KD";

            // Act
            var holdHand = HoldHelper.DetermineHoldHand(new Hand(handStr));

            // Assert
            var holdHandStr = holdHand.ToString(Card.SuitTexts);
            Assert.AreEqual(expectedHoldHandStr, holdHandStr);
        }

        [TestMethod]
        public void HoldHelper_DetermineHoldHand_WhenPossibleRoyalAtBack_ShouldFindIt()
        {
            // Arrange
            var handStr = "2S 7C TH QH AH";
            var expectedHoldHandStr = "TH QH AH";

            // Act
            var holdHand = HoldHelper.DetermineHoldHand(new Hand(handStr));

            // Assert
            var holdHandStr = holdHand.ToString(Card.SuitTexts);
            Assert.AreEqual(expectedHoldHandStr, holdHandStr);
        }
    }
}
