namespace Brain.NeuralNetwork
{
	public static class NetworkFactory
	{
		/// <summary>
		///    Create a multilayer perceptron
		/// </summary>
		/// <param name="inputSize">Input size</param>
		/// <param name="hiddenLayerSizes">Number of neurons for each hidden layer</param>
		/// <param name="activationFunction">Activation function for every hidden node</param>
		/// <param name="regularizationFunction">Regularization function</param>
		/// <param name="neuronFactory">Neuron factory used to create neurons</param>
		/// <param name="synapseFactory">Synapse factory used to create synapses</param>
		/// <returns>New network</returns>
		public static Network CreateMultilayerPerceptron(int inputSize, int[] hiddenLayerSizes,
			IActivationFunction activationFunction,
			IRegularizationFunction regularizationFunction,
			NeuronFactory neuronFactory,
			SynapseFactory synapseFactory)
		{
			var prevLayer = new Neuron[inputSize];

			// create input layer
			var inputNeurons = new Neuron[inputSize];
			for (var i = 0; i < inputSize; i++) {
				var n = neuronFactory.CreateNeuron(activationFunction);
				inputNeurons[i] = n;
				prevLayer[i] = n;
			}

			// create hidden layers
			for (var i = 0; i < hiddenLayerSizes.Length; i++) {
				var layer = new Neuron[hiddenLayerSizes[i]];

				for (var j = 0; j < layer.Length; j++) {
					var n = neuronFactory.CreateNeuron(activationFunction);
					layer[j] = n;

					for (var k = 0; k < prevLayer.Length; k++) {
						var pn = prevLayer[k];
						synapseFactory.Link(pn, n, regularizationFunction);
					}
				}

				prevLayer = layer;
			}

			// create output neuron
			var outputNeuron = neuronFactory.CreateOutputNeuron();
			for (var i = 0; i < prevLayer.Length; i++) {
				var pn = prevLayer[i];
				synapseFactory.Link(pn, outputNeuron, regularizationFunction);
			}

			return new Network {
				InputNeurons = inputNeurons,
				OutputNeuron = outputNeuron
			};
		}
	}
}