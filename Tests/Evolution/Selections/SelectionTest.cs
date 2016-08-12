using System;
using System.Collections.Generic;
using Brain.Evolution;
using Brain.Evolution.Selections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Evolution.Selections
{
	[TestClass]
	public class SelectionTest
	{
		[TestMethod, TestCategory("Evolution/Selections")]
		public void TestElite()
		{
			var s = new EliteSelection();
			var chromosomes = CreateEmptyChromosomes(10);

			var selected = s.Select(chromosomes, 5);
			Assert.AreEqual(5, selected.Count);

			for (var i = 0; i < selected.Count; i++) {
				Assert.AreEqual(9 - i, ((EmptyChromosome) selected[i]).Index);
			}
		}

		[TestMethod, TestCategory("Evolution/Selections")]
		public void TestTruncationSelection()
		{
			var s = new TruncationSelection();
			var chromosomes = CreateEmptyChromosomes(10);

			var selected = s.Select(chromosomes, 5);
			Assert.AreEqual(5, selected.Count);

			for (var i = 0; i < selected.Count; i++) {
				Assert.AreEqual(9 - i, ((EmptyChromosome)selected[i]).Index);
			}
		}

		private List<Chromosome> CreateEmptyChromosomes(int n)
		{
			var chromosomes = new List<Chromosome>();
			for (var i = 0; i < n; i++) {
				chromosomes.Add(new EmptyChromosome(i, i));
			}
			return Util.Shuffle(chromosomes);
		}
	}
}