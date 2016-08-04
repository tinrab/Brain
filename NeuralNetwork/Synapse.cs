namespace Brain.NeuralNetwork
{
	public class Synapse
	{
		public Neuron Source { get; set; }
		public Neuron Destination { get; set; }
		public double Weight { get; set; }
	}
}