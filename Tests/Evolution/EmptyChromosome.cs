using Brain.Evolution;

namespace Tests.Evolution
{
  internal class EmptyChromosome : Chromosome
  {
    public int Index { get; set; }
    public EmptyChromosome() {}

    public EmptyChromosome(int index, double fitness)
    {
      Index = index;
      Fitness = fitness;
    }

    public override Chromosome CreateNew()
    {
      return new EmptyChromosome();
    }

    public override Chromosome Clone()
    {
      return new EmptyChromosome(Index, Fitness);
    }

    public override void Mutate() {}
  }
}