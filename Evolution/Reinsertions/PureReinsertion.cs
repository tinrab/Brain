using System.Collections.Generic;

namespace Brain.Evolution.Reinsertions
{
	public class PureReinsertion : IReinsertion
	{
		public List<IChromosome> Select(Population population, List<IChromosome> parents, List<IChromosome> offspring)
		{
			return offspring;
		}
	}
}