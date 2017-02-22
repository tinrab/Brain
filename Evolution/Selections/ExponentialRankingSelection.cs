using System;
using System.Collections.Generic;

namespace Brain.Evolution.Selections
{
  public class ExponentialRankingSelection : ISelection
  {
    private double _exponentBase;

    public double ExponentBase
    {
      get { return _exponentBase; }
      set
      {
        if (value >= 1 || value <= 0) {
          throw new Exception("ExponentBase must be in (0, 1)");
        }

        _exponentBase = value;
      }
    }

    public ExponentialRankingSelection(double exponentBase = 0.5)
    {
      ExponentBase = exponentBase;
    }

    public List<Chromosome> Select(List<Chromosome> chromosomes, int count)
    {
      chromosomes.Sort();
      var p = new double[chromosomes.Count];
      var s = 0.0;

      for (int i = 0, n = chromosomes.Count; i < chromosomes.Count; i++, n--) {
        s += (_exponentBase - 1.0) / (System.Math.Pow(_exponentBase, chromosomes.Count) - 1.0) *
             System.Math.Pow(_exponentBase, chromosomes.Count - n);
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