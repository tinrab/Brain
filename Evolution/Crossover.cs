using System;
using System.Collections.Generic;

namespace Brain.Evolution
{
	public static class Crossover
	{
		public static List<Chromosome> OnePoint(Chromosome leftParent, Chromosome rightParent, int point)
		{
			var child1 = leftParent.CreateNew();
			var child2 = rightParent.CreateNew();

			for (var i = 0; i < point; i++) {
				child1[i] = leftParent[i];
			}
			for (var i = point; i < rightParent.Length; i++) {
				child1[i] = rightParent[i];
			}

			for (var i = 0; i < point; i++) {
				child2[i] = rightParent[i];
			}
			for (var i = point; i < leftParent.Length; i++) {
				child2[i] = leftParent[i];
			}

			return new List<Chromosome> {child1, child2};
		}

		public static List<Chromosome> CutAndSplice(Chromosome leftParent, Chromosome rightParent)
		{
			var p1 = Util.RandomInt(1, leftParent.Length);
			var p2 = Util.RandomInt(1, rightParent.Length);

			var offspring1 = CreateCutAndSpliceOffspring(leftParent, rightParent, p1, p2);
			var offspring2 = CreateCutAndSpliceOffspring(rightParent, leftParent, p2, p1);

			return new List<Chromosome> {offspring1, offspring2};
		}

		private static Chromosome CreateCutAndSpliceOffspring(Chromosome leftParent, Chromosome rightParent, int leftPoint,
			int rightPoint)
		{
			var offspring = leftParent.CreateNew();
			offspring.Resize(leftPoint + (rightParent.Length - rightPoint));

			for (var i = 0; i < leftPoint; i++) {
				offspring[i] = leftParent[i];
			}

			for (var i = leftPoint; i < offspring.Length; i++) {
				offspring[i] = rightParent[rightPoint + i - leftPoint];
			}

			return offspring;
		}

		/// <summary>
		///    Cycle crossover (CX)
		/// </summary>
		/// <param name="leftParent"></param>
		/// <param name="rightParent"></param>
		/// <returns></returns>
		public static List<Chromosome> Cycle(Chromosome leftParent, Chromosome rightParent)
		{
			var cycles = new List<List<int>>();
			var offspring1 = leftParent.CreateNew();
			var offspring2 = rightParent.CreateNew();
			var leftGenes = leftParent.Genes;
			var rightGenes = rightParent.Genes;

			for (var i = 0; i < leftParent.Length; i++) {
				if (!Util.Flatten(cycles)
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
						offspring1[geneCycleIndex] = leftGenes[geneCycleIndex];
						offspring2[geneCycleIndex] = rightGenes[geneCycleIndex];
					}
				} else {
					for (var j = 0; j < cycle.Count; j++) {
						var geneCycleIndex = cycle[j];
						offspring2[geneCycleIndex] = leftGenes[geneCycleIndex];
						offspring1[geneCycleIndex] = rightGenes[geneCycleIndex];
					}
				}
			}

			return new List<Chromosome> {offspring1, offspring2};
		}

		private static void CreateCycle(Gene[] leftParentGenes, Gene[] rightParentGenes, int geneIndex, List<int> cycle)
		{
			if (!cycle.Contains(geneIndex)) {
				var rightGene = rightParentGenes[geneIndex];
				var newGeneIndex = -1;

				cycle.Add(geneIndex);

				for (var i = 0; i < leftParentGenes.Length; i++) {
					var g = leftParentGenes[i];

					if (g.Value.Equals(rightGene.Value)) {
						newGeneIndex = i;
						break;
					}
				}

				if (geneIndex != newGeneIndex) {
					CreateCycle(leftParentGenes, rightParentGenes, newGeneIndex, cycle);
				}
			}
		}

		/// <summary>
		///    Order-based crossover (OX2)
		/// </summary>
		/// <returns></returns>
		public static List<Chromosome> OrderBased(Chromosome leftParent, Chromosome rightParent)
		{
			var swapLength = Util.RandomInt(1, leftParent.Length - 1);
			var swapIndexes = Util.RandomUniqueInts(swapLength, 0, leftParent.Length);
			var first = CreateOrderBasedOffspring(leftParent, rightParent, swapIndexes);
			var second = CreateOrderBasedOffspring(rightParent, leftParent, swapIndexes);

			return new List<Chromosome> {first, second};
		}

		private static Chromosome CreateOrderBasedOffspring(Chromosome firstParent, Chromosome secondParent, int[] swapIndexes)
		{
			var secondParentSwapGenes = new List<Gene>();
			for (var i = 0; i < secondParent.Length; i++) {
				var g = secondParent[i];
				if (Array.IndexOf(swapIndexes, i) != -1) {
					secondParentSwapGenes.Add(g);
				}
			}

			var firstParentGenes = firstParent.Genes;
			var firstParentSwapGenes = new List<Gene>();
			for (var i = 0; i < firstParentGenes.Length; i++) {
				var g = firstParentGenes[i];
				for (var j = 0; j < secondParentSwapGenes.Count; j++) {
					var g2 = secondParentSwapGenes[j];
					if (g == g2) {
						firstParentSwapGenes.Add(g);
						break;
					}
				}
			}

			var child = firstParent.CreateNew();
			var secondParentSwapGensIndex = 0;

			/*
			for (var i = 0; i < firstParent.Length; i++) {
				if (firstParentSwapGenes.Any(f => f.Index == i)) {
					child[i] = secondParentSwapGenes[secondParentSwapGensIndex++];
				} else {
					child[i] = firstParentGenes[i];
				}
			}*/


			return child;
		}
	}
}