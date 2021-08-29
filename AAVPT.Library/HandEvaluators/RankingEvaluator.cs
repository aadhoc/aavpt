using System;

namespace AAVPT.Library.HandEvaluators
{
    public class RankingEvaluator : BaseEvaluator
    {
        public override bool Debug { get; }

        public RankingEvaluator(bool debug) : base()
        {
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
                Map[bitmap] = (ulong)strength.HandRanking;
            }
        }
    }
}
