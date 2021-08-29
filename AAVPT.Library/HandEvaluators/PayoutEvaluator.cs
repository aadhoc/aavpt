using System;
using AAVPT.Library.Helpers;
using AAVPT.Library.Enums;

namespace AAVPT.Library.HandEvaluators
{
    public class PayoutEvaluator : BaseEvaluator
    {
        public override bool Debug { get; }

        private readonly int _bet;
        private readonly bool _maxPay;

        //public bool LoadedFromFile { get; set; } = false;

        public PayoutEvaluator(int bet, bool maxPay, bool debug) : base()
        {
            _bet = bet;
            _maxPay = maxPay;
            Debug = debug;
        }

        public override void GenerateMap()
        {
            if (Debug) Console.WriteLine($"{TypeName}: Generating new map");

            var handBitmaps = GenerateHandBitmaps();
            int count;

            Console.WriteLine($"{TypeName}: Creating lookup table");
            count = 0;
            foreach (ulong bitmap in handBitmaps)
            {
                if (Debug && count++ % 1000 == 0) Console.Write($"{TypeName}: {count} / {handBitmaps.Count}\r");
                var hand = new Hand(bitmap);
                HandStrength strength = HandReader.GetHandStrength(hand);
                var multiplier = HandStrengthPayoutMultiplier(strength);
                var payout = _bet * multiplier;
                Map[bitmap] = (ulong)payout;
            }
        }

        private int HandStrengthPayoutMultiplier(HandStrength handStrength)
        {
            switch (handStrength.HandRanking)
            {
                case HandRanking.RoyalFlush:
                    return _maxPay ? 800 : 250;
                case HandRanking.StraightFlush:
                    return 50;
                case HandRanking.FourOfAKind:
                    return 25;
                case HandRanking.FullHouse:
                    return 9;
                case HandRanking.Flush:
                    return 6;
                case HandRanking.Straight:
                    return 4;
                case HandRanking.ThreeOfAKind:
                    return 3;
                case HandRanking.TwoPair:
                    return 2;
                case HandRanking.HighPair:
                    return 1;
                default:
                    return 0;
            }
        }

        public override string GetCriteriaAsFilePath()
        {
            return $"bet{_bet}-{(_maxPay ? "maxPay" : null)}";
        }
    }
}
