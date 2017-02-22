using System.Collections.Generic;
using Brain.Evolution;

namespace Tests.Evolution
{
  internal class BooleanChromosomeCrossover : ICrossover
  {
    public int RequiredParents
    {
      get { return 2; }
    }

    public List<Chromosome> Cross(List<Chromosome> parents)
    {
      var left = parents[0];
      var right = parents[1];

      return Crossover.OnePoint(left, right, 2);
    }
  }
}