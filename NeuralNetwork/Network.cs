using Brain.Math;

namespace Brain.NeuralNetwork
{
	public class Network
	{
		public Neuron[] Inputs { get; set; }
		public Neuron Output { get; set; }

		public void Train(Matrix examples, Vector labels, double learningRate, int maxIterations)
		{
			while (maxIterations-- > 0) {
				for (var i = 0; i < examples.Rows; i++) {
					var example = examples.GetRow(i);
					var label = labels[i];

					Compute(example);
					Back(label);
					Update(learningRate);
				}
			}
		}

		public double Compute(Vector input)
		{
			return Output.Compute(input);
		}

		private void Back(double actual)
		{
			for (var i = 0; i < Inputs.Length; i++) {
				Inputs[i].Error(actual);
			}
		}

		private void Update(double learningRate)
		{
			for (var i = 0; i < Inputs.Length; i++) {
				Inputs[i].Update(learningRate);
			}
		}
	}
}