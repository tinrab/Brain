using System.Collections.Generic;

namespace Brain.Evolution.Selections
{
  public class LinearRankingSelection : ISelection
  {
    public List<Chromosome> Select(List<Chromosome> chromosomes, int count)
    {
      chromosomes.Sort();
      var ranges = chromosomes.Count * (chromosomes.Count + 1) / 2.0;
      var p = new double[chromosomes.Count];
      var s = 0.0;

      for (int i = 0, n = chromosomes.Count; i < chromosomes.Count; i++,n--) {
        s += n / ranges;
        p[i] = s;
      }

      var selected = new List<Chromosome>();

      for (var i = 0; i < count; i++) {
        var r = Utility.RandomDouble();

        for (var j = 0; j < chromosomes.Count; j++) {
          if (r <= p[j]) {
            selected.Add(chromosomes[j].Clone());
            break;
          }
        }
      }

      return selected;
    }
  }
}