using System.Collections.Generic;
using Brain.Math;

namespace Brain.NeuralNetwork
{
	public class Neuron
	{
		private double[] _input;

		public Neuron()
		{
			Inputs = new List<Synapse>();
			Outputs = new List<Synapse>();
		}

		public double Output { get; set; }
		public double Error { get; set; }
		public IActivationFunction Activation { get; set; }
		public IList<Synapse> Inputs { get; set; }
		public IList<Synapse> Outputs { get; set; }

		public virtual double Compute(Vector input)
		{
			if (!double.IsNaN(Output)) {
				return Output;
			}

			_input = new double[Inputs.Count];
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

		public virtual void Update(double learningRate)
		{
			if (!double.IsNaN(Error) && !double.IsNaN(Output) && _input != null) {
				for (var i = 0; i < Inputs.Count; i++) {
					var s = Inputs[i];

					s.Weight += learningRate * Output * (1.0 - Output) * Error * _input[i];
				}

				for (var i = 0; i < Outputs.Count; i++) {
					var s = Outputs[i];
					s.Destination.Update(learningRate);
				}

				Error = double.NaN;
				_input = null;
				Output = double.NaN;
			}
		}

		public virtual double ComputeError(double actual, IErrorFunction errorFunction)
		{
			if (!double.IsNaN(Error)) {
				return Error;
			}

			if (Outputs.Count == 0) {
				Error = actual - Output;
			} else {
				var sum = 0.0;

				for (var i = 0; i < Outputs.Count; i++) {
					var s = Outputs[i];
					sum += s.Weight * s.Destination.ComputeError(actual, errorFunction);
				}

				Error = sum;
			}

			return Error;
		}

		public virtual void Clear()
		{
			if (!double.IsNaN(Output)) {
				Output = double.NaN;

				for (var i = 0; i < Inputs.Count; i++) {
					var s = Inputs[i];
					s.Source.Clear();
				}
			}
		}
	}

	public class InputNeuron : Neuron
	{
		private readonly int _index;

		public InputNeuron(int index)
		{
			_index = index;
			Output = double.NaN;
			Error = double.NaN;
		}

		public override double Compute(Vector input)
		{
			Output = input[_index];
			return Output;
		}

		public override void Update(double learningRate)
		{
			for (var i = 0; i < Outputs.Count; i++) {
				var s = Outputs[i];
				s.Destination.Update(learningRate);
			}
		}

		public override double ComputeError(double actual)
		{
			for (var i = 0; i < Outputs.Count; i++) {
				var s = Outputs[i];
				s.Destination.ComputeError(actual);
			}
			return Error;
		}

		public override void Clear()
		{
			Output = double.NaN;
		}
	}

	public class BiasNeuron : Neuron
	{
		public BiasNeuron()
		{
			Output = double.NaN;
			Error = double.NaN;
		}

		public override void Update(double learningRate)
		{
			for (var i = 0; i < Outputs.Count; i++) {
				var s = Outputs[i];
				s.Destination.Update(learningRate);
			}
		}

		public override double ComputeError(double actual)
		{
			for (var i = 0; i < Outputs.Count; i++) {
				var s = Outputs[i];
				s.Destination.ComputeError(actual);
			}
			return Error;
		}

		public override void Clear()
		{
			Output = double.NaN;
		}

		public override double Compute(Vector input)
		{
			return 1.0;
		}
	}
}