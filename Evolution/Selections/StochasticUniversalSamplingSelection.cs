using System.Collections.Generic;

namespace Brain.Evolution.Selections
{
  public class StochasticUniversalSamplingSelection : ISelection
  {
    public List<Chromosome> Select(List<Chromosome> chromosomes, int count)
    {
      var fitnessSum = SelectionUtil.FitnessSum(chromosomes);
      var p = new double[chromosomes.Count];
      var s = 0.0;

      for (var i = 0; i < chromosomes.Count; i++) {
        s += chromosomes[i].Fitness / fitnessSum;
        p[i] = s;
      }

      var stepSize = 1.0 / count;
      var pointer = Utility.RandomDouble();
      var selected = new List<Chromosome>();

      for (var i = 0; i < count; i++) {
        var r = pointer;
        pointer += stepSize;

        if (pointer > 1.0) {
          pointer -= 1.0;
        }

        for (var j = 0; j < p.Length; j++) {
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