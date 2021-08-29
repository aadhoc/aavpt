using System.Collections.Generic;
using System.IO;

namespace AAVPT.Library
{

    public class EvaluationManager
	{
		public static bool Debug;
		private Dictionary<string, IHandEvaluator> _handEvaluators;

		public EvaluationManager()
		{
			_handEvaluators = new Dictionary<string, IHandEvaluator>();
		}

		public void AddHandEvaluator(string name, IHandEvaluator handEvaluator)
		{
			_handEvaluators[name] = handEvaluator;
		}

		public IHandEvaluator GetHandEvaluator(string name)
		{
			return _handEvaluators[name];
		}

		public void InitializeHandEvaluators()
        {
			foreach (var keyValuePair in _handEvaluators)
            {
				var handEvaluator = keyValuePair.Value;
				var filePath = GetFilePath(keyValuePair.Key, handEvaluator);
				if (File.Exists(filePath))
                {
					handEvaluator.LoadFromFile(filePath);
                }
				else
                {
					handEvaluator.GenerateMap();
                }
            }
        }

		public void PersistHandEvaluators()
		{
			foreach (var keyValuePair in _handEvaluators)
			{
				var handEvaluator = keyValuePair.Value;
				if (!handEvaluator.LoadedFromFile)
                {
					var filePath = GetFilePath(keyValuePair.Key, handEvaluator);
					handEvaluator.SaveToFile(filePath);
				}
			}
		}

		private string GetFilePath(string name, IHandEvaluator handEvaluator)
        {
			var criteriaAsFilePath = handEvaluator.GetCriteriaAsFilePath();

			if (!string.IsNullOrEmpty(criteriaAsFilePath))
			{
				return $"{name}-{criteriaAsFilePath}.cache";
			}
			return $"{name}.cache";
        }
	}
}
