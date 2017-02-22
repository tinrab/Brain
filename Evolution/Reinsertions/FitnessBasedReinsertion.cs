using System.Collections.Generic;

namespace Brain.Evolution.Reinsertions
{
  public class FitnessBasedReinsertion : IReinsertion
  {
    public List<Chromosome> Select(Population population, List<Chromosome> parents, List<Chromosome> offspring)
    {
      if (offspring.Count > population.MaxSize) {
        var selected = new List<Chromosome>();
        offspring.Sort();

        for (var i = 0; i < population.MaxSize; i++) {
          selected.Add(offspring[i]);
        }

        return selected;
      }

      return offspring;
    }
  }
}