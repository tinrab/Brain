using System.Collections.Generic;
using Brain.Math;

namespace Brain.NeuralNetwork
{
	public class Neuron
	{
		private IDictionary<int, double> _input;

		public Neuron()
		{
			Output = double.NaN;
			Delta = double.NaN;
			Inputs = new List<Synapse>();
			Outputs = new List<Synapse>();
		}

		public double Output { get; set; }
		public double Delta { get; set; }
		public IActivationFunction Activation { get; set; }
		public IList<Synapse> Inputs { get; set; }
		public IList<Synapse> Outputs { get; set; }

		public double Compute(Vector input)
		{
			if (!double.IsNaN(Output)) {
				return Output;
			}

			_input = new Dictionary<int, double>();
			Output = 0.0;
			var sum = 0.0;

			for (var i = 0; i < Inputs.Count; i++) {
				var s = Inputs[i];
				var x = s.Source.Compute(input);
				_input[i] = x;
				sum += s.Weight * x;
			}

			Output = Activation.Compute(sum);

			return Output;
		}

		public void Update(double learningRate)
		{
			if (!double.IsNaN(Delta) && !double.IsNaN(Output) && _input != null) {
				for (var i = 0; i < Inputs.Count; i++) {
					var s = Inputs[i];

					s.Weight += learningRate * Output * (1.0 - Output) * Delta * _input[i];
				}

				for (var i = 0; i < Outputs.Count; i++) {
					var s = Outputs[i];
					s.Destination.Update(learningRate);
				}

				Delta = double.NaN;
				_input = null;
				Output = double.NaN;
			}
		}

		public double Error(double actual)
		{
			if (!double.IsNaN(Delta)) {
				return Delta;
			}

			if (Outputs.Count == 0) {
				Delta = actual - Output; // TODO error func
			} else {
				var sum = 0.0;

				for (var i = 0; i < Outputs.Count; i++) {
					var s = Outputs[i];

					sum += s.Weight * s.Destination.Error(actual);
				}

				Delta = sum;
			}

			return Delta;
		}
	}
}