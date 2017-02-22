using System.Collections.Generic;

namespace Brain.Evolution.Reinsertions
{
  public class UniformReinsertion : IReinsertion
  {
    public List<Chromosome> Select(Population population, List<Chromosome> parents, List<Chromosome> offspring)
    {
      while (offspring.Count < population.MinSize) {
        offspring.Add(offspring[Utility.RandomInt(offspring.Count)]);
      }

      return offspring;
    }
  }
}