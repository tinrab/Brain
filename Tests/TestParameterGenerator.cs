using Brain.NeuralNetwork;

namespace Tests
{
	public class TestParameterGenerator : IParameterGenerator
	{
		public double GenerateSynapseWeight()
		{
			return 1.0;
		}

		public double GenerateNeuronBias()
		{
			return 0.1;
		}
	}
}