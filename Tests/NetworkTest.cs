using System;
using Brain.Math;
using Brain.NeuralNetwork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class NetworkTest
	{
		[TestMethod]
		public void TestBuildNetwork()
		{
			var neuronFactor = new NeuronFactory();
			var synapseFactory = new SynapseFactory();
			var n = NetworkFactory.CreateMultilayerPerceptron(2, new[] {2}, ActivationFunction.Sigmoid, null, neuronFactor,
				synapseFactory);

			Util.PrintNetwork(n);
		}

		[TestMethod]
		public void TestForward()
		{
			var parameterGenerator = new TestParameterGenerator();
			var neuronFactor = new NeuronFactory(parameterGenerator);
			var synapseFactory = new SynapseFactory(parameterGenerator);
			var n = NetworkFactory.CreateMultilayerPerceptron(2, new[] {2}, ActivationFunction.Sigmoid, null, neuronFactor,
				synapseFactory);
			var x = new Vector(3, 5);

			Assert.IsTrue(Math.Abs(n.Compute(x) - 2.0993931) < 0.001);
		}

		[TestMethod]
		public void TestBack()
		{
			var parameterGenerator = new TestParameterGenerator();
			var neuronFactor = new NeuronFactory(parameterGenerator);
			var synapseFactory = new SynapseFactory(parameterGenerator);
			var network = NetworkFactory.CreateMultilayerPerceptron(2, new[] {2, 1}, ActivationFunction.Sigmoid, null,
				neuronFactor,
				synapseFactory);
			var x = new Vector(3, 5);

			network.Compute(x);
			network.Back(42.0, ErrorFunction.Square);

			var n = network.OutputNeuron.Inputs[0].Source;

			Console.WriteLine(n.InputDerivative);
			Console.WriteLine(n.OutputDerivative);
			Console.WriteLine(n.InputDerivativeCount);

			Assert.IsTrue(Math.Abs(-3.987764 - n.InputDerivative) < 0.01);
			Assert.IsTrue(Math.Abs(-41.009155 - n.OutputDerivative) < 0.01);
			Assert.AreEqual(1, n.InputDerivativeCount);
		}

		[TestMethod]
		public void TestTrainXor()
		{
			var neuronFactor = new NeuronFactory();
			var synapseFactory = new SynapseFactory();
			var n = NetworkFactory.CreateMultilayerPerceptron(2, new[] {2}, ActivationFunction.Sigmoid, null, neuronFactor,
				synapseFactory);

			var examples = new Matrix(new double[,] {
				{0, 0},
				{0, 1},
				{1, 0},
				{1, 1}
			});
			var labels = new Vector(0, 1, 1, 0);

			n.Train(examples, labels, 0.7, 0.1, 1000, ErrorFunction.Square);

			for (var i = 0; i < labels.Length; i++) {
				var x = examples.GetRow(i);
				var y = labels[i];
				Console.WriteLine("Actual: {0}, Result: {1}", y, n.Compute(x));
				Assert.IsTrue(Math.Abs(y - n.Compute(x)) < 0.01);
			}
		}

		[TestMethod]
		public void TestIris()
		{
			var examples = Util.LoadIrisDataSet();
			var labels = examples.GetColumn(4);
			examples.RemoveColumn(4);
			var neuronFactor = new NeuronFactory();
			var synapseFactory = new SynapseFactory();
			var n = NetworkFactory.CreateMultilayerPerceptron(4, new[] {4}, ActivationFunction.Sigmoid, null, neuronFactor,
				synapseFactory);

			n.Train(examples, labels, 0.1, 0.1, 1000, ErrorFunction.Square);

			var predictions = n.Compute(examples);

			Console.WriteLine(RegressionError.Mae(labels, predictions));
		}
	}
}