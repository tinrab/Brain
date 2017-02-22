using System.Collections.Generic;

namespace Brain.Evolution
{
  public interface ISelection
  {
    List<Chromosome> Select(List<Chromosome> chromosomes, int count);
  }
}