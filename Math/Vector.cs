using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Brain.Math
{
	public class Vector : IEnumerable<double>
	{
		public Vector(int length)
		{
			Values = new double[length];
		}

		public Vector(params double[] values)
		{
			Values = values;
		}

		public Vector(Vector v)
		{
			Values = new double[v.Values.Length];
			for (var i = 0; i < Values.Length; i++) {
				Values[i] = v.Values[i];
			}
		}

		public double[] Values { get; set; }

		public int Length
		{
			get { return Values.Length; }
		}

		public double this[int index]
		{
			get { return Values[index]; }
			set { Values[index] = value; }
		}

		public IEnumerator<double> GetEnumerator()
		{
			for (var i = 0; i < Values.Length; i++) {
				yield return Values[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			for (var i = 0; i < Values.Length; i++) {
				yield return Values[i];
			}
		}

		public double[] ToArray()
		{
			var arr = new double[Values.Length];
			Array.Copy(Values, arr, arr.Length);
			return arr;
		}

		public double Variance()
		{
			var mean = Mean();
			var sum = 0.0;

			for (var i = 0; i < Length; i++) {
				sum += System.Math.Pow(Values[i] - mean, 2.0);
			}

			return sum / (Length - 1);
		}

		public void Normalize(double min = double.NaN, double max = double.NaN)
		{
			if (double.IsNaN(min)) {
				min = Min();
			}

			if (double.IsNaN(max)) {
				max = Max();
			}

			var a = max - min;

			for (var i = 0; i < Length; i++) {
				Values[i] = (Values[i] - min) / a;
			}
		}

		public void Standardize()
		{
			var sd = StandardDeviation();
			var m = Mean();

			for (var i = 0; i < Length; i++) {
				Values[i] = (Values[i] - m) / sd;
			}
		}

		public double StandardDeviation()
		{
			return System.Math.Sqrt(Variance());
		}

		public double Sum()
		{
			var sum = 0.0;

			for (var i = 0; i < Length; i++) {
				sum += Values[i];
			}

			return sum;
		}

		public double Min()
		{
			var min = double.PositiveInfinity;

			for (var i = 0; i < Length; i++) {
				var v = Values[i];

				if (v < min) {
					min = v;
				}
			}

			return min;
		}

		public double Max()
		{
			var max = double.NegativeInfinity;

			for (var i = 0; i < Length; i++) {
				var v = Values[i];

				if (v > max) {
					max = v;
				}
			}

			return max;
		}

		public double Mean()
		{
			var avg = 0.0;

			for (var i = 0; i < Length; i++) {
				avg += Values[i] / Length;
			}

			return avg;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("[");
			for (var i = 0; i < Length; i++) {
				sb.Append(Values[i]);

				if (i < Length - 1) {
					sb.Append(", ");
				}
			}
			sb.Append("]");
			return sb.ToString();
		}
	}
}