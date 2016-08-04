namespace Brain.NeuralNetwork
{
	public static class SynapseFactory
	{
		public static Synapse Link(Neuron source, Neuron destination)
		{
			var s = new Synapse {
				Source = source,
				Destination = destination,
				Weight = Util.RandomDouble()
			};

			source.Outputs.Add(s);
			destination.Inputs.Add(s);

			return s;
		}
	}
}