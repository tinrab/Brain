using System.Collections.Generic;

namespace Brain.Evolution
{
	public interface ISelection
	{
		List<IChromosome> Select(List<IChromosome> chromosomes, int count);
	}
}