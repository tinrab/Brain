using System;
using Brain.Evolution;
using Brain.Evolution.Reinsertions;
using Brain.Evolution.Selections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Evolution
{
	[TestClass]
	public class EvolutionTest
	{
		[TestMethod, TestCategory("Evolution")]
		public void TestTargetNumber()
		{
			var p = new Population(new TargetNumberChromosome(), 10, 50);
			var ga = new GeneticAlgorithm(p, new EliteSelection(), new TargetNumberCrossover(), new ElitistReinsertion());

			for (var i = 0; i < 100; i++) {
				for (var j = 0; j < p.Size; j++) {
					((TargetNumberChromosome) p[j]).Evaluate();
				}

				ga.Evolve();
			}

			var result = (TargetNumberChromosome) ga.BestChromosome;
			Assert.IsTrue(Math.Abs(result.Value - 42.0) < 0.01);
		}
	}
}