using System;
using System.Collections.Generic;
using Brain.Math;

namespace Brain.NeuralNetwork
{
	public class Network
	{
		public Neuron[] InputNeurons { get; set; }
		public Neuron OutputNeuron { get; set; }

		public void Train(Matrix examples, Vector labels, double learningRate, double regularizationRate, int maxIterations,
			IErrorFunction errorFunction)
		{
			while (maxIterations-- >= 0) {
				for (var i = 0; i < examples.Rows; i++) {
					Compute(examples.GetRow(i));
					Back(labels[i], errorFunction);
					Update(learningRate, regularizationRate);
				}
			}
		}

		public Vector Compute(Matrix inputs)
		{
			var predictions = new Vector(inputs.Rows);

			for (var i = 0; i < inputs.Rows; i++) {
				predictions[i] = Compute(inputs.GetRow(i));
			}

			return predictions;
		}

		public double Compute(Vector input)
		{
			if (input.Length != InputNeurons.Length) {
				throw new Exception("Invalid input vector length");
			}

			for (var i = 0; i < InputNeurons.Length; i++) {
				var n = InputNeurons[i];
				n.Output = input[i];
			}

			var s = new Stack<Neuron>();
			// add neurons from layer 1 to the stack
			for (var i = 0; i < InputNeurons.Length; i++) {
				var inputNeuron = InputNeurons[i];
				for (var j = 0; j < inputNeuron.Outputs.Count; j++) {
					s.Push(inputNeuron.Outputs[j].Destination);
				}
			}

			while (s.Count != 0) {
				var layer = s.ToArray();
				s.Clear();

				for (var i = 0; i < layer.Length; i++) {
					var n = layer[i];
					n.Update();

					for (var j = 0; j < n.Outputs.Count; j++) {
						s.Push(n.Outputs[j].Destination);
					}
				}
			}

			return OutputNeuron.Output;
		}

		public void Back(double actual, IErrorFunction errorFunction)
		{
			OutputNeuron.OutputDerivative = errorFunction.Derivative(OutputNeuron.Output, actual);

			// BFS starting at the end
			var s = new Stack<Neuron>();
			s.Push(OutputNeuron);

			while (s.Count != 0) {
				var layer = s.ToArray();
				s.Clear();

				// compute error derivative
				for (var i = 0; i < layer.Length; i++) {
					var n = layer[i];
					n.InputDerivative = n.OutputDerivative * n.Activation.Derivative(n.Input);
					n.InputDerivativeSum += n.InputDerivative;
					n.InputDerivativeCount++;
				}

				// error derivative with respect to input weights
				for (var i = 0; i < layer.Length; i++) {
					var n = layer[i];
					for (var j = 0; j < n.Inputs.Count; j++) {
						var inputLink = n.Inputs[j];
						inputLink.ErrorDerivative = n.InputDerivative * inputLink.Source.Output;
						inputLink.ErrorDerivativeSum += inputLink.ErrorDerivative;
						inputLink.DerivativeCount++;
					}
				}

				for (var i = 0; i < layer.Length; i++) {
					var n = layer[i];
					for (var j = 0; j < n.Inputs.Count; j++) {
						s.Push(n.Inputs[j].Source);
					}
				}

				if (s.Count != 0) {
					var prevLayer = s.ToArray();

					for (var i = 0; i < prevLayer.Length; i++) {
						var n = prevLayer[i];
						if (n.Inputs.Count == 0) {
							goto OUT;
						}
					}

					for (var i = 0; i < prevLayer.Length; i++) {
						var n = prevLayer[i];
						n.OutputDerivative = 0.0;
						for (var j = 0; j < n.Outputs.Count; j++) {
							var outSynapse = n.Outputs[j];
							n.OutputDerivative += outSynapse.Weight * outSynapse.Destination.InputDerivative;
						}
					}
				}

				OUT:
				;
			}
		}

		public void Update(double learningRate, double regularizationRate)
		{
			var s = new Stack<Neuron>();
			for (var i = 0; i < InputNeurons.Length; i++) {
				var inputNeuron = InputNeurons[i];
				for (var j = 0; j < inputNeuron.Outputs.Count; j++) {
					s.Push(inputNeuron.Outputs[j].Destination);
				}
			}

			while (s.Count != 0) {
				var layer = s.ToArray();
				s.Clear();

				for (var i = 0; i < layer.Length; i++) {
					var n = layer[i];

					// update bias
					if (n.InputDerivativeCount > 0) {
						n.Bias -= learningRate * n.InputDerivativeSum / n.InputDerivativeCount;
						n.InputDerivativeSum = 0.0;
						n.InputDerivativeCount = 0;
					}

					// update input weights
					for (var j = 0; j < n.Inputs.Count; j++) {
						var inSynapse = n.Inputs[j];
						if (inSynapse.DerivativeCount > 0) {
							var rd = inSynapse.Regularization == null ? 0.0 : inSynapse.Regularization.Derivative(inSynapse.Weight);
							inSynapse.Weight -= learningRate / inSynapse.DerivativeCount *
														(inSynapse.ErrorDerivativeSum + regularizationRate * rd);
							inSynapse.ErrorDerivativeSum = 0.0;
							inSynapse.DerivativeCount = 0;
						}
					}

					for (var j = 0; j < n.Outputs.Count; j++) {
						s.Push(n.Outputs[j].Destination);
					}
				}
			}
		}
	}
}