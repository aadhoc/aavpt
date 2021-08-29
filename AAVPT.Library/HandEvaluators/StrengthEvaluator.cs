using System;
using System.Collections.Generic;

namespace AAVPT.Library.HandEvaluators
{
    public class StrengthEvaluator : BaseEvaluator
    {
        public override bool Debug { get; }

        public StrengthEvaluator(bool debug) : base()
        {
            Debug = debug;
        }

        public override void GenerateMap()
        {
            if (Debug) Console.WriteLine($"{TypeName}: Generating new map");

            var handBitmaps = GenerateHandBitmaps();
            int count;

            // Calculate hand strength of each hand
            Console.WriteLine($"{TypeName}: Calculating hand strength");
            var handStrengths = new Dictionary<ulong, HandStrength>();
            count = 0;
            foreach (ulong bitmap in handBitmaps)
            {
                if (Debug && count++ % 1000 == 0) Console.Write($"{TypeName}: {count} / {handBitmaps.Count}\r");
                var hand = new Hand(bitmap);
                handStrengths.Add(bitmap, HandReader.GetHandStrength(hand));
            }

            // Generate a list of all unique hand strengths
            Console.WriteLine($"{TypeName}: Generating equivalence classes");
            var uniqueHandStrengths = new List<HandStrength>();
            count = 0;
            foreach (KeyValuePair<ulong, HandStrength> strength in handStrengths)
            {
                if (Debug && count++ % 1000 == 0) Console.Write($"{TypeName}: {count} / {handStrengths.Count}\r");
                uniqueHandStrengths.BinaryInsert(strength.Value);
            }
            Console.WriteLine($"{TypeName}: {uniqueHandStrengths.Count} unique hand strengths");

            // Create a map of hand bitmaps to hand strength indices
            Console.WriteLine($"{TypeName}: Creating lookup table");
            count = 0;
            foreach (ulong bitmap in handBitmaps)
            {
                if (Debug && count++ % 1000 == 0) Console.Write($"{TypeName}: {count} / {handBitmaps.Count}\r");
                var hand = new Hand(bitmap);
                HandStrength strength = HandReader.GetHandStrength(hand);
                var equivalence = Utilities.BinarySearch(uniqueHandStrengths, strength);
                if (equivalence == null) throw new Exception($"{TypeName}: {hand} hand not found");
                else
                {
                    Map[bitmap] = (ulong)equivalence;
                }
            }
        }
    }
}
