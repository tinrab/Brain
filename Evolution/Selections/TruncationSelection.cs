using System.Collections.Generic;

namespace Brain.Evolution.Selections
{
  public class TruncationSelection : ISelection
  {
    public List<Chromosome> Select(List<Chromosome> chromosomes, int count)
    {
      chromosomes.Sort();

      var selected = new List<Chromosome>();
      var size = count;

      do {
        var len = System.Math.Min(chromosomes.Count, size);

        for (var i = 0; i < len; i++) {
          selected.Add(chromosomes[i]);
        }

        size -= len;
      } while (size > 0);

      return selected;
    }
  }
}