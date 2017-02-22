using System;
using System.Collections.Generic;

namespace Brain.Evolution
{
  public static class Crossover
  {
    public static List<Chromosome> OnePoint(Chromosome left, Chromosome right, int point)
    {
      var first = left.CreateNew();
      var second = right.CreateNew();

      for (var i = 0; i < point; i++) {
        first[i] = left[i];
      }
      for (var i = point; i < right.Length; i++) {
        first[i] = right[i];
      }

      for (var i = 0; i < point; i++) {
        second[i] = right[i];
      }
      for (var i = point; i < left.Length; i++) {
        second[i] = left[i];
      }

      return new List<Chromosome> {first, second};
    }

    public static List<Chromosome> CutAndSplice(Chromosome left, Chromosome right)
    {
      var p1 = Utility.RandomInt(1, left.Length);
      var p2 = Utility.RandomInt(1, right.Length);

      return new List<Chromosome> {
        CreateCutAndSpliceOffspring(left, right, p1, p2),
        CreateCutAndSpliceOffspring(right, left, p2, p1)
      };
    }

    private static Chromosome CreateCutAndSpliceOffspring(Chromosome left, Chromosome right, int leftPoint,
      int rightPoint)
    {
      var offspring = left.CreateNew();
      offspring.Resize(leftPoint + (right.Length - rightPoint));

      for (var i = 0; i < leftPoint; i++) {
        offspring[i] = left[i];
      }

      for (var i = leftPoint; i < offspring.Length; i++) {
        offspring[i] = right[rightPoint + i - leftPoint];
      }

      return offspring;
    }

    /// <summary>
    ///   Cycle crossover (CX)
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static List<Chromosome> Cycle(Chromosome left, Chromosome right)
    {
      var cycles = new List<List<int>>();
      var first = left.CreateNew();
      var second = right.CreateNew();
      var leftGenes = left.Genes;
      var rightGenes = right.Genes;

      for (var i = 0; i < left.Length; i++) {
        if (!Utility.Flatten(cycles)
          .Contains(i)) {
          var cycle = cycles[i];
          CreateCycle(leftGenes, rightGenes, i, cycle);
          cycles.Add(cycle);
        }
      }

      for (var i = 0; i < cycles.Count; i++) {
        var cycle = cycles[i];

        if (i % 2 == 0) {
          for (var j = 0; j < cycle.Count; j++) {
            var geneCycleIndex = cycle[j];
            first[geneCycleIndex] = leftGenes[geneCycleIndex];
            second[geneCycleIndex] = rightGenes[geneCycleIndex];
          }
        } else {
          for (var j = 0; j < cycle.Count; j++) {
            var geneCycleIndex = cycle[j];
            second[geneCycleIndex] = leftGenes[geneCycleIndex];
            first[geneCycleIndex] = rightGenes[geneCycleIndex];
          }
        }
      }

      return new List<Chromosome> {first, second};
    }

    private static void CreateCycle(Gene[] leftGenes, Gene[] rightGenes, int geneIndex, List<int> cycle)
    {
      if (!cycle.Contains(geneIndex)) {
        var rightGene = rightGenes[geneIndex];
        var newGeneIndex = -1;

        cycle.Add(geneIndex);

        for (var i = 0; i < leftGenes.Length; i++) {
          var g = leftGenes[i];

          if (g.Value.Equals(rightGene.Value)) {
            newGeneIndex = i;
            break;
          }
        }

        if (geneIndex != newGeneIndex) {
          CreateCycle(leftGenes, rightGenes, newGeneIndex, cycle);
        }
      }
    }

    /// <summary>
    ///   Ordered crossover (OX1)
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static List<Chromosome> Ordered(Chromosome left, Chromosome right)
    {
      var idxs = Utility.RandomUniqueInts(2, 0, left.Length);
      Array.Sort(idxs);

      var begin = idxs[0];
      var end = idxs[1];

      return new List<Chromosome> {
        CreateOrderedOffspring(left, right, begin, end),
        CreateOrderedOffspring(right, left, begin, end)
      };
    }

    private static Chromosome CreateOrderedOffspring(Chromosome left, Chromosome right, int begin, int end)
    {
      var middleGenes = new List<Gene>();
      for (var i = begin; i <= end; i++) {
        middleGenes.Add(left[i]);
      }

      var remainingGenes = new List<Gene>();
      for (var i = 0; i < right.Length; i++) {
        if (!middleGenes.Contains(right[i])) {
          remainingGenes.Add(right[i]);
        }
      }

      var remainingGenesEnum = remainingGenes.GetEnumerator();
      var offspring = left.CreateNew();

      for (var i = 0; i < left.Length; i++) {
        var firstParentGene = left[i];

        if (i >= begin && i <= end) {
          offspring[i] = firstParentGene;
        } else {
          remainingGenesEnum.MoveNext();
          offspring[i] = remainingGenesEnum.Current;
        }
      }

      return offspring;
    }

    /// <summary>
    ///   Order-based crossover (OX2)
    /// </summary>
    /// <returns></returns>
    public static List<Chromosome> OrderBased(Chromosome leftParent, Chromosome rightParent)
    {
      var swapLength = Utility.RandomInt(1, leftParent.Length - 1);
      var swapIndexes = Utility.RandomUniqueInts(swapLength, 0, leftParent.Length);

      return new List<Chromosome> {
        CreateOrderBasedOffspring(leftParent, rightParent, swapIndexes),
        CreateOrderBasedOffspring(rightParent, leftParent, swapIndexes)
      };
    }

    private static Chromosome CreateOrderBasedOffspring(Chromosome left, Chromosome right, int[] swapIndexes)
    {
      var rightSwapGenes = new List<Gene>();

      for (var i = 0; i < right.Length; i++) {
        var g = right[i];

        if (Array.IndexOf(swapIndexes, i) != -1) {
          rightSwapGenes.Add(g);
        }
      }

      var leftGenes = left.Genes;
      var leftSwapGenes = new List<int>();

      for (var i = 0; i < leftGenes.Length; i++) {
        var g = leftGenes[i];

        for (var j = 0; j < rightSwapGenes.Count; j++) {
          var g2 = rightSwapGenes[j];

          if (g == g2) {
            leftSwapGenes.Add(i);
            break;
          }
        }
      }

      var offspring = left.CreateNew();
      var rightParentSwapGensIndex = 0;

      for (var i = 0; i < left.Length; i++) {
        if (leftSwapGenes.Contains(i)) {
          offspring[i] = rightSwapGenes[rightParentSwapGensIndex++];
        } else {
          offspring[i] = leftGenes[i];
        }
      }

      return offspring;
    }

    /// <summary>
    ///   Position based crossover (POS)
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static List<Chromosome> PositionBased(Chromosome left, Chromosome right)
    {
      var swapLength = Utility.RandomInt(1, left.Length - 1);
      var swapIndexes = Utility.RandomUniqueInts(swapLength, 0, left.Length);

      return new List<Chromosome> {
        CreatePositionBasedOffspring(left, right, swapIndexes),
        CreatePositionBasedOffspring(right, left, swapIndexes)
      };
    }

    private static Chromosome CreatePositionBasedOffspring(Chromosome left, Chromosome right, int[] swapIndexes)
    {
      var leftGenes = new List<Gene>(left.Genes);
      var offspring = left.CreateNew();

      for (var i = 0; i < left.Length; i++) {
        if (Array.IndexOf(swapIndexes, i) != -1) {
          var gene = right[i];

          leftGenes.Remove(gene);
          leftGenes.Insert(i, gene);
        }
      }

      for (var i = 0; i < leftGenes.Count; i++) {
        offspring[i] = leftGenes[i];
      }

      return offspring;
    }

    /// <summary>
    ///   Three parent crossover
    /// </summary>
    /// <param name="parent1"></param>
    /// <param name="parent2"></param>
    /// <param name="parent3"></param>
    /// <returns></returns>
    public static List<Chromosome> ThreeParent(Chromosome parent1, Chromosome parent2, Chromosome parent3)
    {
      var parent1Genes = parent1.Genes;
      var parent2Genes = parent2.Genes;
      var parent3Genes = parent3.Genes;
      var offspring = parent1.CreateNew();

      for (var i = 0; i < parent1.Length; i++) {
        var parent1Gene = parent1Genes[i];

        if (parent1Gene == parent2Genes[i]) {
          offspring[i] = parent1Gene;
        } else {
          offspring[i] = parent3Genes[i];
        }
      }

      return new List<Chromosome> {offspring};
    }

    public static List<Chromosome> TwoPoint(Chromosome left, Chromosome right, int point1, int point2)
    {
      var len = left.Length;
      var swapLen = len - 1;

      if (point2 >= swapLen) {
        throw new Exception("Too few genes");
      }

      return new List<Chromosome> {
        CreateTwoPointOffspring(left, right, point1, point2),
        CreateTwoPointOffspring(right, left, point1, point2)
      };
    }

    private static Chromosome CreateTwoPointOffspring(Chromosome left, Chromosome right, int point1, int point2)
    {
      var firstCutGenesCount = point1 + 1;
      var secondCutGenesCount = point2 + 1;
      var offspring = left.CreateNew();

      for (var i = 0; i < firstCutGenesCount; i++) {
        offspring[i] = left[i];
      }

      for (var i = firstCutGenesCount; i < secondCutGenesCount; i++) {
        offspring[i] = right[i];
      }

      for (var i = secondCutGenesCount; i < left.Length; i++) {
        offspring[i] = left[i];
      }

      return offspring;
    }

    /// <summary>
    ///   Uniform crossover (UX)
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="p">Probability</param>
    /// <returns></returns>
    public static List<Chromosome> Uniform(Chromosome left, Chromosome right, double p)
    {
      var offspring1 = left.CreateNew();
      var offspring2 = right.CreateNew();

      for (var i = 0; i < left.Length; i++) {
        if (Utility.RandomDouble() < p) {
          offspring1[i] = left[i];
          offspring2[i] = right[i];
        } else {
          offspring1[i] = right[i];
          offspring2[i] = left[i];
        }
      }

      return new List<Chromosome> {offspring1, offspring2};
    }

    /// <summary>
    ///   Partially mapped crossover (PMX)
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static List<Chromosome> PartiallyMappedCrossover(Chromosome left, Chromosome right)
    {
      var cutPointsIndexes = Utility.RandomUniqueInts(2, 0, left.Length);
      var p1 = cutPointsIndexes[0];
      var p2 = cutPointsIndexes[1];

      var leftGenes = left.Genes;
      var leftMappingSection = new Gene[p2 - p1 + 1];

      for (var i = 0; i < leftMappingSection.Length; i++) {
        leftMappingSection[i] = leftGenes[i + p1];
      }

      var rightGenes = right.Genes;
      var rightMappingSection = new Gene[p2 - p1 + 1];

      for (var i = 0; i < rightMappingSection.Length; i++) {
        rightMappingSection[i] = rightGenes[i + p1];
      }

      var offspring1 = left.CreateNew();
      var offspring2 = right.CreateNew();

      for (var i = 0; i < leftMappingSection.Length; i++) {
        offspring2[i + p1] = leftMappingSection[i];
      }

      for (var i = 0; i < rightMappingSection.Length; i++) {
        offspring1[i + p1] = rightMappingSection[i];
      }

      var length = left.Length;

      for (var i = 0; i < length; i++) {
        if (i >= p1 && i <= p2) {
          continue;
        }

        offspring1[i] = GetGeneNotInMappingSection(leftGenes[i], rightMappingSection, leftMappingSection);
        offspring2[i] = GetGeneNotInMappingSection(rightGenes[i], leftMappingSection, rightMappingSection);
      }

      return new List<Chromosome> {offspring1, offspring2};
    }

    private static Gene GetGeneNotInMappingSection(Gene candidateGene, Gene[] mappingSection, Gene[] otherMappingSection)
    {
      var idx = -1;

      for (var i = 0; i < mappingSection.Length; i++) {
        if (mappingSection[i].Equals(candidateGene)) {
          idx = i;
        }
      }

      if (idx != -1) {
        return GetGeneNotInMappingSection(otherMappingSection[idx], mappingSection, otherMappingSection);
      }

      return candidateGene;
    }
  }
}