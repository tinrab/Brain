using System.Collections.Generic;

namespace Brain.Evolution.Reinsertions
{
  public class PureReinsertion : IReinsertion
  {
    public List<Chromosome> Select(Population population, List<Chromosome> parents, List<Chromosome> offspring)
    {
      return offspring;
    }
  }
}