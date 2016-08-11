namespace Brain.Neuro.ParameterGenerators
{
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