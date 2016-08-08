namespace Brain.NeuralNetwork
{
	public class NeuronFactory
	{
		private readonly IParameterGenerator _parameterGenerator;
		private int _neuronIdCounter;

		public NeuronFactory()
		{
			_parameterGenerator = new BasicParameterGenerator();
		}

		public NeuronFactory(IParameterGenerator parameterGenerator)
		{
			_parameterGenerator = parameterGenerator;
		}

		public Neuron CreateNeuron(IActivationFunction activation)
		{
			return new Neuron {
				Id = _neuronIdCounter++,
				Activation = activation,
				Bias = _parameterGenerator.GenerateNeuronBias()
			};
		}

		public Neuron CreateOutputNeuron()
		{
			return new Neuron {
				Id = _neuronIdCounter++,
				Activation = ActivationFunction.Linear,
				Bias = _parameterGenerator.GenerateNeuronBias()
			};
		}
	}
}