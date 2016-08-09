namespace Brain.Neuro
{
	public interface IParameterGenerator
	{
		double GenerateSynapseWeight();
		double GenerateNeuronBias();
	}

	public class ActionParameterGenerator : IParameterGenerator
	{
		public delegate T Func<T>();

		private readonly Func<double> _generateNeuronBias;
		private readonly Func<double> _generateSynapseWeight;

		public ActionParameterGenerator(Func<double> generateNeuronBias, Func<double> generateSynapseWeight)
		{
			_generateNeuronBias = generateNeuronBias;
			_generateSynapseWeight = generateSynapseWeight;
		}

		public double GenerateSynapseWeight()
		{
			return _generateSynapseWeight();
		}

		public double GenerateNeuronBias()
		{
			return _generateNeuronBias();
		}
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