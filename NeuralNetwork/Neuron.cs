using System;
using System.Collections.Generic;

namespace Brain.NeuralNetwork
{
	public class Neuron : IComparable<Neuron>
	{
		public Neuron()
		{
			Inputs = new List<Synapse>();
			Outputs = new List<Synapse>();
		}

		public int Id { get; set; }
		public IActivationFunction Activation { get; set; }
		public IList<Synapse> Inputs { get; set; }
		public IList<Synapse> Outputs { get; set; }
		public double Bias { get; set; }
		public double Input { get; set; }
		public double Output { get; set; }
		public double OutputDerivative { get; set; }
		public double InputDerivative { get; set; }
		public double InputDerivativeSum { get; set; }
		public int InputDerivativeCount { get; set; }

		public int CompareTo(Neuron other)
		{
			return Id.CompareTo(other.Id);
		}

		internal double Update()
		{
			Input = Bias;

			for (var i = 0; i < Inputs.Count; i++) {
				var s = Inputs[i];
				Input += s.Weight * s.Source.Output;
			}

			Output = Activation.Compute(Input);

			return Output;
		}

		public override bool Equals(object obj)
		{
			if (obj == null) {
				return false;
			}

			if (ReferenceEquals(this, obj)) {
				return true;
			}

			if (obj.GetType() != typeof(Neuron)) {
				return false;
			}

			return Id == ((Neuron) obj).Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}