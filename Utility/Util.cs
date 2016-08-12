using System;
using System.Collections.Generic;

namespace Brain
{
	public static class Util
	{
		private static readonly Random random = new Random();

		public static int RandomInt(int maxValue)
		{
			return random.Next(maxValue);
		}

		public static int RandomInt(int minValue, int maxValue)
		{
			return random.Next(minValue, maxValue);
		}

		public static int[] RandomUniqueInts(int count, int minValue, int maxValue)
		{
			var a = new int[count];
			var list = new List<int>();

			for (int i = minValue; i < maxValue; i++) {
				list.Add(i);
			}

			for (int i = 0; i < count; i++) {
				var idx = RandomInt(list.Count);
				a[i] = list[idx];
				list.RemoveAt(idx);
			}

			return a;
		}

		public static double RandomDouble()
		{
			return random.NextDouble();
		}

		public static bool RandomBoolean()
		{
			return random.Next(2) == 0;
		}

		public static T[] RemoveElement<T>(T[] array, int index)
		{
			var a = new T[array.Length - 1];
			var j = 0;

			for (var i = 0; i < array.Length; i++) {
				if (i != index) {
					a[j] = array[i];
					j++;
				}
			}

			return a;
		}
	}
}