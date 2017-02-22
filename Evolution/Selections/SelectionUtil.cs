using System.Collections.Generic;

namespace Brain.Evolution.Selections
{
  internal static class SelectionUtil
  {
    public static double FitnessSum(List<Chromosome> chromosomes)
    {
      var sum = 0.0;

      for (var i = 0; i < chromosomes.Count; i++) {
        sum += chromosomes[i].Fitness;
      }

      return sum;
    }
  }
}