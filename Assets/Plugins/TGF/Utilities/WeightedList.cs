using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

namespace TGF.Utilities
{
	[Serializable]
	public class WeightedList<T>
	{

		private int _randomSeed;
		
		[SerializeField, TableList]
		private List<WeightedElement<T>> m_Elements;
		
		public T GetRandom()
		{
			int total = m_Elements.Sum(x => x.Weight);
			IOrderedEnumerable<WeightedElement<T>> sorted = m_Elements.OrderBy(x => x.Weight);
			
			System.Random _PseudoRandom = new System.Random(_randomSeed);
			int random = _PseudoRandom.Next(0, total);

			int accumulatedWeight = 0;
			foreach (WeightedElement<T> weightedElement in sorted)
			{
				accumulatedWeight += weightedElement.Weight;
				if (random < accumulatedWeight) return weightedElement.Element;
			}

			return default;
		}

		public void SetSeed(int seed)
		{
			_randomSeed = seed;
		}
		
		public void SetSeed(string seed)
		{
			_randomSeed = seed.GetHashCode();
		}
	}

	[Serializable]
	public class WeightedElement<T>
	{
		public T Element;
		public int Weight;
	}
}