using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace TGF.Utilities
{
	public static class RandomNumber {
	
		private static System.Random _PseudoRandom;

		public static void SetSeed(string seed)
		{
			//Debug.Log("Seed: " + seed);
			//_PseudoRandom = new System.Random(seed.GetHashCode());
		}

		public static int GetRandomPositiveNegative() => Random.Range(0f, 1f) >= 0.5f ? 1 : -1;



		/// <summary>
		/// Returns boolean based on whether the passed value was 
		/// <br />above a random value from 0 to 1
		/// </summary>
		public static bool CheckProbability(float val)
		{
			return val >= GetProbability();
		}



		/// <summary>
		/// Returns a float from 0 to 1(exclusive)
		/// </summary>
		public static float GetProbability()
		{
			return GetNextRandomNumber(0, 100) / 100f;
		}



		public static int GetNextRandomNumber(int max)
		{
			return GetNextRandomNumber(0, max);
		}

		public static int GetNextRandomNumber(int min, int max)
		{
			return Random.Range(min, max);
			/*
			 if(_PseudoRandom == null) return min;

			int rnd = _PseudoRandom.Next(min, max);
			//Debug.Log("RandomNumber: " + rnd);
			return rnd;
			*/
		}
		
		
		public static float GetNextRandomNumber(float max)
		{
			return GetNextRandomNumber(0, max);
		}
		
		public static float GetNextRandomNumber(float min, float max)
		{
			return Random.Range(min, max);
		}

		public static float GetNextRandomNumber(Vector2 range)
		{
			return Random.Range(range.x, range.y);
		}


		public static T GetRandomElement<T>(this List<T> list) 
		{
			int r = GetNextRandomNumber(list.Count);
			//Debug.Log("Count: " + list.Count + "\tRand: " + r);
			return list[r];
		}
		
		public static T GetRandomElement<T>(this T[] array) 
		{
			int r = GetNextRandomNumber(array.Length);
			//Debug.Log("Count: " + list.Count + "\tRand: " + r);
			return array[r];
		}
	}

}
