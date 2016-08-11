using System.Collections.Generic;

namespace Brain.Evolution.Selections
{
	public class EliteSelection : ISelection
	{
		public List<IChromosome> Select(List<IChromosome> chromosomes, int count)
		{
			chromosomes.Sort(new ChromosomeFitnessComparer());

			return chromosomes.GetRange(0, count);
		}
	}
}