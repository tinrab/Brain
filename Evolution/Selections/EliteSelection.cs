using System.Collections.Generic;

namespace Brain.Evolution.Selections
{
  public class EliteSelection : ISelection
  {
    public List<Chromosome> Select(List<Chromosome> chromosomes, int count)
    {
      chromosomes.Sort();

      return chromosomes.GetRange(0, count);
    }
  }
}