using Brain.Math;

namespace Brain.NeuralNetwork
{
	public class Network
	{
		public Neuron[] Inputs { get; set; }
		public Neuron Output { get; set; }

		public void Train(Matrix examples, Vector labels, double learningRate, int maxIterations)
		{
			while (maxIterations > 0) {
				for (var i = 0; i < examples.Rows; i++) {
					var example = examples.GetRow(i);
					var actual = labels[i];

					Compute(example);
					Back(actual);
					Update(learningRate);

					maxIterations--;
				}
			}
		}

		public double Compute(Vector input)
		{
			Output.Clear();
			return Output.Compute(input);
		}

		private void Back(double actual)
		{
			for (var i = 0; i < Inputs.Length; i++) {
				Inputs[i].ComputeError(actual);
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