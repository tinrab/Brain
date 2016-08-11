using System.Collections.Generic;

namespace Brain.Evolution
{
	public static class CrossoverUtil
	{
		public static List<object[]> CutAndSpliceCrossover(object[] leftGenes, object[] rightGenes)
		{
			var p1 = Util.RandomInt(1, leftGenes.Length) + 1;
			var p2 = Util.RandomInt(1, rightGenes.Length) + 1;

			var offspring1 = CreateCutAndSpliceOffspring(leftGenes, rightGenes, p1, p2);
			var offspring2 = CreateCutAndSpliceOffspring(rightGenes, leftGenes, p2, p1);

			return new List<object[]> {offspring1, offspring2};
		}

		private static object[] CreateCutAndSpliceOffspring(object[] leftGenes, object[] rightGenes, int leftPoint, int rightPoint)
		{
			var offspring = new object[leftPoint + (rightGenes.Length - rightPoint)];

			for (var i = 0; i < leftPoint; i++) {
				offspring[i] = leftGenes[i];
			}

			for (var i = leftPoint; i < offspring.Length; i++) {
				offspring[i] = rightGenes[rightPoint + i - leftPoint];
			}

			return offspring;
		}
	}
}