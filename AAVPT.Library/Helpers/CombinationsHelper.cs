using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AAVPT.Library.Helpers
{
    internal class CombinationsHelper
    {
		internal static List<ulong> GenerateHandBitmaps(Action<int, BigInteger> progressAction)
		{
			var sourceSet = Enumerable.Range(0, 52).ToList();
			var combinations = new Combinatorics.Collections.Combinations<int>(sourceSet, 5);

			var handBitmaps = new List<ulong>();
			int count = 0;
			foreach (List<int> values in combinations)
			{
				if (progressAction != null && count++ % 1000 == 0)
				{
					progressAction(count, combinations.Count);
				}
				handBitmaps.Add(values.Aggregate(0ul, (acc, el) => acc | 1ul << el));
			}

			return handBitmaps;
		}
	}
}
