namespace Brain.NeuralNetwork
{
	public interface IParameterGenerator
	{
		double GenerateSynapseWeight();
		double GenerateNeuronBias();
	}

	public class BasicParameterGenerator : IParameterGenerator
	{
		public double GenerateSynapseWeight()
		{
			return Util.RandomDouble() - 0.5;
		}

		public double GenerateNeuronBias()
		{
			return 0.1;
		}
	}

	public class PositiveUniformParameterGenerator : IParameterGenerator
	{
		public double GenerateSynapseWeight()
		{
			return Util.RandomDouble();
		}

		public double GenerateNeuronBias()
		{
			return 0.1;
		}
	}
}