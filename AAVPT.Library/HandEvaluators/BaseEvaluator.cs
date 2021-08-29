using System;
using System.Collections.Generic;
using System.IO;
using AAVPT.Library.Helpers;

namespace AAVPT.Library.HandEvaluators
{
    public abstract class BaseEvaluator : IHandEvaluator
    {
        public static double LoadFactor = 1.25;

        protected string TypeName;

        public abstract bool Debug { get; }

        protected HashMap Map;

        public bool LoadedFromFile { get; set; } = false;

        public BaseEvaluator()
        {
            TypeName = GetType().Name;

            int minHashMapSize = 2598960;
            Map = new HashMap((uint)(minHashMapSize * LoadFactor));
        }

        public ulong GetMappedValue(ulong bitmap)
        {
            return Map[bitmap];
        }

        public void SaveToFile(string filePath)
        {
            if (Debug) Console.WriteLine($"{GetType().Name} Saving table to {filePath}");

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                var bytes = HashMap.Serialize(Map);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        public void LoadFromFile(string filePath)
        {
            if (Debug) Console.WriteLine($"{GetType().Name}: Loading table from {filePath}");

            using (FileStream inputStream = new FileStream(filePath, FileMode.Open))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                inputStream.CopyTo(memoryStream);
                Map = HashMap.Deserialize(memoryStream.ToArray());
            }

            LoadedFromFile = true;
        }

        public virtual string GetCriteriaAsFilePath()
        {
            return null;
        }

        protected List<ulong> GenerateHandBitmaps()
        {
            if (Debug) Console.WriteLine($"{GetType().Name}: Generating hand bitmaps");

            var handBitmaps = CombinationsHelper.GenerateHandBitmaps((curCount, totalCount) =>
            {
                if (Debug) Console.Write($"{GetType().Name}: {curCount} / {totalCount}\r");
            });

            return handBitmaps;
        }

        public abstract void GenerateMap();
    }
}
