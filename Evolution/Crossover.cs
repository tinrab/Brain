using System.Collections.Generic;

namespace Brain.Evolution
{
	public static class Crossover
	{
		public static List<Chromosome> OnePoint(Chromosome leftParent, Chromosome rightParent, int point)
		{
			var child1 = leftParent.CreateNew();
			var child2 = rightParent.CreateNew();

			for (int i = 0; i < point; i++) {
				child1[i] = leftParent[i];
			}
			for (int i = point; i < rightParent.Length; i++) {
				child1[i] = rightParent[i];
			}

			for (int i = 0; i < point; i++) {
				child2[i] = rightParent[i];
			}
			for (int i = point; i < leftParent.Length; i++) {
				child2[i] = leftParent[i];
			}

			return new List<Chromosome>() {child1, child2 };
		}

		public static List<Chromosome> CutAndSplice(Chromosome leftParent, Chromosome rightParent)
		{
			var p1 = Util.RandomInt(1, leftParent.Length );
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

	}
}