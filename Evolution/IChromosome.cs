using System;

namespace Brain.Evolution
{
	public interface IChromosome
	{
		double Fitness { get; }
		IChromosome CreateNew();
		IChromosome Clone();
		void Mutate();
	}
}