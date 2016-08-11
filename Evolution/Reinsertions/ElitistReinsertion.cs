using System.Collections.Generic;

namespace Brain.Evolution.Reinsertions
{
	public class ElitistReinsertion : IReinsertion
	{
		public List<IChromosome> Select(Population population, List<IChromosome> parents, List<IChromosome> offspring)
		{
			if (offspring.Count < population.MinSize) {
				var n = population.MinSize - offspring.Count;
				parents.Sort(new ChromosomeFitnessComparer());

				for (var i = 0; i < n; i++) {
					offspring.Add(parents[i]);
				}
			}

			return offspring;
		}
	}
}