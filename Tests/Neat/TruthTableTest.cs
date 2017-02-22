using System;
using System.Collections.Generic;
using Brain.Neat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Neat
{
  [TestClass]
  public class TruthTableTest
  {
    [TestMethod]
    [TestCategory("Neat")]
    public void TestTruthTable()
    {
      var neat = new Brain.Neat.Neat();
      var ne = new NeuroEvolution(neat);

      var bodies = new List<IBody>();

      // population size is 10
      for (int i = 0; i < 10; i++) {
        bodies.Add(new Body());
      }

      var champ = ne.Begin(bodies);
      for (var i = 0; i < 100; i++) {
        Console.WriteLine("Champ fitness: " + champ.CalculateRawFitness());
        champ = ne.Epoch();
      }
    }

    private class Body : IBody
    {
      private int _row;
      private int _correctResults;
      private double _error;

      public Body()
      {
        // number of bits
        InputCount = 6;
        // larger output is "true", smaller is "false"
        OutputCount = 2;
        MaxFitness = 1 << InputCount;
      }

      public void Reset()
      {
        _row = 0;
        _correctResults = 0;
        _error = 0.0;
      }

      public void Activate(double[] outputs)
      {
        var predicted = outputs[0] > outputs[1] ? 0 : 1;
        var actual = Expression();

        if (predicted == actual) {
          _correctResults++;
        }

        _row++;

        if (_correctResults == (1 << InputCount)) {
          _error = 0.0;
          return;
        }

        for (int i = 0; i < OutputCount; i++) {
          var target = actual == 1 ? 1.0 : -1.0;
          _error += Math.Abs(outputs[i] - target);
        }
      }

      public bool HasFinished()
      {
        return _row > (1 << InputCount);
      }

      public double[] GetInputs()
      {
        var inputs = new double[InputCount];

        for (int i = 0; i < InputCount; i++) {
          inputs[i] = (_row & (1 << i)) == 1 ? 1.0 : -1.0;
        }

        return inputs;
      }

      public int InputCount { get; }
      public int OutputCount { get; }
      public double Fitness { get { return MaxFitness - _error; } }
      public double MaxFitness { get; }

      private int Expression()
      {
        var b = new bool[InputCount];
        for (int i = 0; i < InputCount; i++) {
          b[i] = (_row & (1 << i)) == 1;
        }

        // boolean expression
        var r = b[0] ^ b[1] || (b[2] || b[3]) && b[4] ^ b[5];

        return r ? 1 : 0;
      }
    }
  }
}