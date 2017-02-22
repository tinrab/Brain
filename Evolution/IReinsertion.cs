using System.Collections.Generic;

namespace Brain.Evolution
{
  public interface IReinsertion
  {
    List<Chromosome> Select(Population population, List<Chromosome> parents, List<Chromosome> offspring);
  }
}