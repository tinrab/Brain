namespace Brain.NeuralNetwork
{
	public static class NetworkFactory
	{
		/// <summary>
		///     Create a multilayer perceptron
		/// </summary>
		/// <param name="layerSizes">Number of neurons per layer</param>
		/// <param name="activationFunction">Activation function for every hidden node</param>
		/// <returns>New network</returns>
		public static Network CreateMultilayerPerceptron(int[] layerSizes, IActivationFunction activationFunction)
		{
		/*
			var network = new Network();
			network.Inputs = new Neuron[layerSizes[0]];
			network.Outputs = new Neuron[layerSizes[layerSizes.Length - 1]];

			Neuron[] prevLayer = null;

			for (var i = 0; i < layerSizes.Length; i++) {
				Neuron[] layer;
				if (i == 0) {
					layer = network.Inputs;
				} else if (i == layerSizes.Length - 1) {
					layer = network.Outputs;
				} else {
					layer = new Neuron[layerSizes[i]];
				}

				for (var j = 0; j < layer.Length; j++) {
					var n = new Neuron(i == layerSizes.Length - 1 ? ActivationFunction.Linear : activationFunction);
					layer[j] = n;

					if (prevLayer != null) {
						for (var k = 0; k < layerSizes[i - 1]; k++) {
							var prevNeuron = prevLayer[k];
							var s = new Synapse(prevNeuron, n);
							s.Weight = Util.RandomDouble() - 0.5;
							prevNeuron.Outputs.Add(s);
							n.Inputs.Add(s);
						}
					}
				}

				prevLayer = layer;
			}

			return network;*/
			return null;
		}
	}
}