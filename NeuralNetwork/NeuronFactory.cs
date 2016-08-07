namespace Brain.NeuralNetwork
{
	public static class NeuronFactory
	{
		public static Neuron CreateInputNeuron(int index)
		{
			return new InputNeuron(index);
		}

		public static Neuron CreateHiddenNeuron(IActivationFunction activation)
		{
			var n = new Neuron {
				Output = double.NaN,
				Error = double.NaN,
				Activation = activation
			};
			SynapseFactory.Link(CreateBiasNeuron(), n);
			return n;
		}

		public static Neuron CreateOutputNeuron(IActivationFunction activation)
		{
			return CreateHiddenNeuron(activation);
		}

		public static Neuron CreateBiasNeuron()
		{
			return new BiasNeuron();
		}
	}
}