using System.Collections.Generic;

namespace Brain.Evolution
{
	public interface IReinsertion
	{
		List<IChromosome> Select(Population population, List<IChromosome> parents, List<IChromosome> offspring);
	}
}