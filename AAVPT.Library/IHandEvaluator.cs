namespace AAVPT.Library
{
    public interface IHandEvaluator
    {
        void GenerateMap();
        void SaveToFile(string filePath);
        void LoadFromFile(string filePath);

        bool LoadedFromFile { get; }

        ulong GetMappedValue(ulong bitmap);

        string GetCriteriaAsFilePath();
    }
}