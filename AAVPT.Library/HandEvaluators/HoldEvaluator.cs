using System;
using AAVPT.Library.HandEvaluators.Helpers;

namespace AAVPT.Library.HandEvaluators
{
    public class HoldEvaluator : BaseEvaluator
    {
        public override bool Debug { get; }

        public HoldEvaluator(bool debug)
        {
            Debug = debug;
        }

        public override void GenerateMap()
        {
            if (Debug) Console.WriteLine($"{nameof(HoldEvaluator)}: Generating new map");

            var handBitmaps = GenerateHandBitmaps();

            Console.WriteLine($"{nameof(HoldEvaluator)}: Creating lookup table");
            int count = 0;
            foreach (ulong bitmap in handBitmaps)
            {
                if (Debug && count++ % 1000 == 0) Console.Write($"{nameof(HoldEvaluator)}: {count} / {handBitmaps.Count}\r");
                var hand = new Hand(bitmap);
                var holdHand = HoldHelper.DetermineHoldHand(hand);
                Map[bitmap] = holdHand.GetBitmap();
            }
        }
    }
}
