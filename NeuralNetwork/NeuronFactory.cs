namespace Brain.NeuralNetwork
{
	public static class NeuronFactory
	{
		public static Neuron CreateInputNeuron(IActivationFunction activation)
		{
			return new Neuron {
				Activation = activation
			};
		}

		public static Neuron CreateHiddenNeuron(IActivationFunction activation)
		{
			var n = new Neuron {
				Activation = activation
			};

			SynapseFactory.Link(CreateBiasNeuron(), n);

			return n;
		}

		public static Neuron CreateOutputNeuron()
		{
			return new Neuron {
				Activation = ActivationFunction.Linear
			};
		}

		public static Neuron CreateBiasNeuron()
		{
			return new Neuron {
				Output = 1.0
			};
		}
	}
}