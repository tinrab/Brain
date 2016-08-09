using System.Collections.Generic;

namespace Brain.Evolution.Reinsertion
{
	public class PureReinsertion : IReinsertion
	{
		public List<IChromosome> Select(Population population, List<IChromosome> parents, List<IChromosome> offspring)
		{
			return offspring;
		}
	}
}