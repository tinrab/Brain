using System.Collections.Generic;

namespace Brain.Evolution
{
	public interface ICrossover
	{
		int RequiredParents { get; }
		List<IChromosome> Cross(List<IChromosome> parents);
	}
}