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
    [TestMethod]
    [TestCategory("Evolution")]
    public void TestTargetNumber()
    {
      var p = new Population(new TargetNumberChromosome(), 6, 12);
      var ga = new GeneticAlgorithm(p, new EliteSelection(), new TargetNumberCrossover(), new ElitistReinsertion());

      for (var j = 0; j < p.Size; j++) {
        ((TargetNumberChromosome) p[j]).Evaluate();
      }

      for (var i = 0; i < 500; i++) {
        ga.BeginGeneration();
        for (var j = 0; j < p.Size; j++) {
          ((TargetNumberChromosome) p[j]).Evaluate();
        }
        ga.EndGeneration();
      }

      var result = (TargetNumberChromosome) ga.FittestChromosome;
      Assert.IsTrue(Math.Abs(result.Value - 42.0) < 0.01);
    }

    [TestMethod]
    [TestCategory("Evolution")]
    public void TestBooleanArrayWithOnePoint()
    {
      var p = new Population(new BooleanChromosome(), 10, 20);
      var ga = new GeneticAlgorithm(p, new EliteSelection(), new BooleanChromosomeCrossover(), new ElitistReinsertion());
      var t = new bool[BooleanChromosome.Length];
      for (var i = 0; i < t.Length; i++) {
        t[i] = i % 2 == 0;
      }

      for (var j = 0; j < p.Size; j++) {
        ((BooleanChromosome) p[j]).Evaluate(t);
      }

      for (var i = 0; i < 1000; i++) {
        ga.BeginGeneration();
        for (var j = 0; j < p.Size; j++) {
          ((BooleanChromosome) p[j]).Evaluate(t);
        }
        ga.EndGeneration();
      }

      var result = (BooleanChromosome) ga.FittestChromosome;
      CollectionAssert.AreEqual(t, result.Value);
    }
  }
}