using System;

namespace Brain
{
	public static class Util
	{
		private static readonly Random random = new Random();

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