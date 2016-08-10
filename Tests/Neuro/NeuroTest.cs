using System;
using Brain.Math;
using Brain.Neuro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class NetworkTest
	{
		[TestMethod, TestCategory("Neuro")]
		public void TestForward()
		{
			var parameterGenerator = new ActionParameterGenerator(() => 0.1, () => 1.0);
			var neuronFactor = new NeuronFactory(parameterGenerator);
			var synapseFactory = new SynapseFactory(parameterGenerator);
			var n = NetworkFactory.CreateMultilayerPerceptron(new[] {2, 2, 1}, ActivationFunction.Sigmoid,
				ActivationFunction.Linear, null, neuronFactor,
				synapseFactory);
			var x = new Vector(3, 5);

			Assert.IsTrue(Math.Abs(n.Compute(x)[0] - 2.0993931) < 0.001);
		}

		[TestMethod, TestCategory("Neuro")]
		public void TestBack()
		{
			var parameterGenerator = new ActionParameterGenerator(() => 0.1, () => 1.0);
			var neuronFactor = new NeuronFactory(parameterGenerator);
			var synapseFactory = new SynapseFactory(parameterGenerator);
			var network = NetworkFactory.CreateMultilayerPerceptron(new[] {2, 2, 1, 1}, ActivationFunction.Sigmoid,
				ActivationFunction.Linear, null,
				neuronFactor,
				synapseFactory);
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

		[TestMethod, TestCategory("Neuro")]
		public void TestTrainXor()
		{
			var neuronFactor = new NeuronFactory();
			var synapseFactory = new SynapseFactory();
			var n = NetworkFactory.CreateMultilayerPerceptron(new[] {2, 2, 1}, ActivationFunction.Sigmoid,
				ActivationFunction.Linear, null, neuronFactor,
				synapseFactory);
			var trainer = new NetworkTrainer(n, 0.7, 0.1);

			var examples = new Matrix(new double[,] {
				{0, 0},
				{0, 1},
				{1, 0},
				{1, 1}
			});
			var labels = new Vector(0, 1, 1, 0);

			trainer.Train(examples, labels, 2000);

			for (var i = 0; i < labels.Length; i++) {
				var x = examples.GetRow(i);
				var y = labels[i];
				Console.WriteLine("Actual: {0}, Result: {1}", y, n.Compute(x));
				Assert.IsTrue(Math.Abs(y - n.Compute(x)[0]) < 0.01);
			}
		}

		[TestMethod, TestCategory("Neuro")]
		public void TestIris()
		{
			var examples = Util.LoadIrisDataSet();
			examples.ShuffleRows();
			var labels = examples.GetColumn(4);
			examples.RemoveColumn(4);

			for (var i = 0; i < examples.Rows; i++) {
				examples[i].Standardize();
			}

			// expand labels
			var labelMatrix = new Matrix(labels.Length, 3); // 3 classes
			for (var i = 0; i < labels.Length; i++) {
				var c = MathUtil.RoundToInt(labels[i]);
				labelMatrix[i][c] = 1.0;
			}

			var trainSize = MathUtil.RoundToInt(examples.Rows * 0.8);
			var testSize = examples.Rows - trainSize;
			var trainData = examples.Take(trainSize);
			var trainLabels = labelMatrix.Take(trainSize);
			var testData = examples.Skip(trainSize);
			var testLabels = new int[testSize];
			for (var i = 0; i < testSize; i++) {
				testLabels[i] = MathUtil.RoundToInt(labels[trainSize + i]);
			}

			var pg = new PositiveUniformParameterGenerator();
			var neuronFactor = new NeuronFactory(pg);
			var synapseFactory = new SynapseFactory(pg);

			var n = NetworkFactory.CreateMultilayerPerceptron(new[] {4, 4, 3}, ActivationFunction.Sigmoid,
				ActivationFunction.Sigmoid, null, neuronFactor,
				synapseFactory);
			var trainer = new NetworkTrainer(n, 0.2, 0.01);

			trainer.Train(trainData, trainLabels, 500);

			var predictions = n.Classify(testData);
			var ca = StatisticsUtil.ClassificationAccuracy(testLabels, predictions);

			Assert.IsTrue(ca >= 0.9);
		}
	}
}