using System;
using Brain.Math;
using Brain.Neuro;
using Brain.Neuro.ParameterGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
  [TestClass]
  public class NetworkTest
  {
    [TestMethod]
    [TestCategory("Neuro")]
    public void TestForward()
    {
      var parameterGenerator = new ActionParameterGenerator(() => 0.1, () => 1.0);
      var neuronFactor = new NeuronFactory(parameterGenerator);
      var synapseFactory = new SynapseFactory(parameterGenerator);
      var n = NetworkFactory.CreateMultilayerPerceptron(new[] {2, 2, 1}, ActivationFunction.Sigmoid,
        ActivationFunction.Identity, null, neuronFactor, synapseFactory);
      var x = new Vector(3, 5);

      Assert.IsTrue(Math.Abs(n.Compute(x)[0] - 2.0993931) < 0.001);
    }

    [TestMethod]
    [TestCategory("Neuro")]
    public void TestBack()
    {
      var parameterGenerator = new ActionParameterGenerator(() => 0.1, () => 1.0);
      var neuronFactor = new NeuronFactory(parameterGenerator);
      var synapseFactory = new SynapseFactory(parameterGenerator);
      var network = NetworkFactory.CreateMultilayerPerceptron(new[] {2, 2, 1, 1}, ActivationFunction.Sigmoid,
        ActivationFunction.Identity, null, neuronFactor, synapseFactory);
      var networkTrainer = new NetworkTrainer(network);
      var x = new Vector(3, 5);

      network.Compute(x);
      networkTrainer.Back(42.0);

      var n = network.OutputLayer[0].Inputs[0].Source;

      Console.WriteLine(n.InputDerivative);
      Console.WriteLine(n.OutputDerivative);
      Console.WriteLine(n.InputDerivativeCount);

      Assert.IsTrue(Math.Abs(-3.987764 - n.InputDerivative) < 0.01);
      Assert.IsTrue(Math.Abs(-41.009155 - n.OutputDerivative) < 0.01);
      Assert.AreEqual(1, n.InputDerivativeCount);
    }

    [TestMethod]
    [TestCategory("Neuro")]
    public void TestTrainXor()
    {
      var pg = new PositiveUniformParameterGenerator();
      var neuronFactor = new NeuronFactory(pg);
      var synapseFactory = new SynapseFactory(pg);
      var n = NetworkFactory.CreateMultilayerPerceptron(new[] {2, 2, 1}, ActivationFunction.Sigmoid,
        ActivationFunction.Identity, null, neuronFactor, synapseFactory);
      var trainer = new NetworkTrainer(n, 0.9, 0.1);

      var examples = new Matrix(new double[,] {{0, 0}, {0, 1}, {1, 0}, {1, 1}});
      var labels = new Vector(0, 1, 1, 0);

      trainer.Train(examples, labels, 1000);

      for (var i = 0; i < labels.Length; i++) {
        var x = examples.GetRow(i);
        var y = labels[i];
        Console.WriteLine("Actual: {0}, Result: {1}", y, n.Compute(x));
        Assert.IsTrue(Math.Abs(y - n.Compute(x)[0]) < 0.01);
      }
    }
  }
}