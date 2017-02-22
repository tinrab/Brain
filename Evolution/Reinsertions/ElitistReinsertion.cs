using System.Collections.Generic;

namespace Brain.Evolution.Reinsertions
{
  public class ElitistReinsertion : IReinsertion
  {
    public List<Chromosome> Select(Population population, List<Chromosome> parents, List<Chromosome> offspring)
    {
      if (offspring.Count < population.MinSize) {
        var n = population.MinSize - offspring.Count;
        parents.Sort();

        for (var i = 0; i < n; i++) {
          offspring.Add(parents[i]);
        }
      }

      return offspring;
    }
  }
}