namespace Brain.Evolution
{
	public interface IChromosome
	{
		double Fitness { get; set; }
		IChromosome CreateNew();
		IChromosome Clone();
		void Mutate();
	}
}