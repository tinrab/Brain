using System.Collections.Generic;

namespace Brain.Evolution
{
	internal class ChromosomeFitnessComparer : IComparer<IChromosome>
	{
		public int Compare(IChromosome x, IChromosome y)
		{
			return x.Fitness.CompareTo(y.Fitness);
		}
	}
}