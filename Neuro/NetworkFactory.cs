namespace Brain.Neuro
{
  public static class NetworkFactory
  {
    /// <summary>
    ///   Create a multilayer perceptron
    /// </summary>
    /// <param name="layerSizes">Number of neurons per layer</param>
    /// <param name="activationFunction">Activation function for every hidden node</param>
    /// <param name="outputActivationFunction">Activation function for last layer</param>
    /// <param name="regularizationFunction">Regularization function</param>
    /// <param name="neuronFactory">Neuron factory used to create neurons</param>
    /// <param name="synapseFactory">Synapse factory used to create synapses</param>
    /// <returns>New network</returns>
    public static Network CreateMultilayerPerceptron(int[] layerSizes, IActivationFunction activationFunction,
      IActivationFunction outputActivationFunction, IRegularizationFunction regularizationFunction,
      NeuronFactory neuronFactory, SynapseFactory synapseFactory)
    {
      var prevLayer = new Neuron[layerSizes[0]];

      // create input layer
      var inputNeurons = new Neuron[layerSizes[0]];
      for (var i = 0; i < inputNeurons.Length; i++) {
        var n = neuronFactory.CreateNeuron(activationFunction);
        inputNeurons[i] = n;
        prevLayer[i] = n;
      }

      // create hidden layers
      for (var i = 1; i < layerSizes.Length - 1; i++) {
        var layer = new Neuron[layerSizes[i]];

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

      // create output layer
      var outputNeurons = new Neuron[layerSizes[layerSizes.Length - 1]];

      for (var i = 0; i < outputNeurons.Length; i++) {
        var outputNeuron = neuronFactory.CreateOutputNeuron(outputActivationFunction);
        outputNeurons[i] = outputNeuron;
        for (var j = 0; j < prevLayer.Length; j++) {
          var pn = prevLayer[j];
          synapseFactory.Link(pn, outputNeuron, regularizationFunction);
        }
      }

      return new Network {InputLayer = inputNeurons, OutputLayer = outputNeurons};
    }
  }
}