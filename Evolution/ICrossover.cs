using System.Collections.Generic;

namespace Brain.Evolution
{
  public interface ICrossover
  {
    int RequiredParents { get; }
    List<Chromosome> Cross(List<Chromosome> parents);
  }
}