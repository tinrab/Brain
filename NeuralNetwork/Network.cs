using System;
using Brain.Data;

namespace Brain.NeuralNetwork
{
    public class Network
    {
        private Node[][] _nodes;

        /// <summary>
        ///     Create a multi-layer neural network
        /// </summary>
        /// <param name="layerLengths">Number of neurons in each layer</param>
        /// <param name="activationFunction">Activation function for every hidden node</param>
        /// <param name="outputActivationFunction">Activation function for output nodes</param>
        /// <param name="regularizationFunction">Regularization function that computes a penalty for a given weight</param>
        /// <returns>New network</returns>
        public static Network Create(int[] layerLengths, IActivationFunction activationFunction,
            IActivationFunction outputActivationFunction, IRegularizationFunction regularizationFunction)
        {
            var initZero = false;
            var nodes = new Node[layerLengths.Length][];

            for (var i = 0; i < layerLengths.Length; i++) {
                var layer = new Node[layerLengths[i]];
                nodes[i] = layer;

                for (var j = 0; j < layerLengths[i]; j++) {
                    var node = new Node((i == layerLengths.Length - 1) ? outputActivationFunction : activationFunction, initZero);
                    layer[j] = node;

                    if (i < 1) {
                        continue;
                    }

                    // add edges
                    for (var k = 0; k < layerLengths[i - 1]; k++) {
                        var prevNode = nodes[i - 1][k];
                        var edge = new Edge(prevNode, node, regularizationFunction, initZero);
                        prevNode.Outputs.Add(edge);
                        node.Inputs.Add(edge);
                    }
                }
            }

            return new Network {_nodes = nodes};
        }

        public double ForwardPropagate(DataRow input)
        {
            var layer0 = _nodes[0];
            if (input.Length != layer0.Length) {
                throw new Exception("Invalid inputs length");
            }

            for (var i = 0; i < layer0.Length; i++) {
                layer0[i].Output = input[i];
            }

            for (var i = 1; i < _nodes.Length; i++) {
                var layer = _nodes[i];
                for (var j = 0; j < layer.Length; j++) {
                    layer[j].Update();
                }
            }

            return _nodes[_nodes.Length - 1][0].Output;
        }

        public void BackPropagate(double target, IErrorFunction errorFunction)
        {
            var outputNode = _nodes[_nodes.Length - 1][0];
            outputNode.OutputDerivative = errorFunction.Derivative(outputNode.Output, target);

            for (var i = _nodes.Length - 1; i >= 1; i--) {
                var layer = _nodes[i];

                for (var j = 0; j < layer.Length; j++) {
                    var node = layer[j];
                    node.InputDerivative = node.OutputDerivative * node.Activation.Derivative(node.TotalInput);
                    node.AccInputDer += node.InputDerivative;
                    node.NumAccumulateDers++;
                }

                for (var j = 0; j < layer.Length; j++) {
                    var node = layer[j];
                    for (var k = 0; k < node.Inputs.Count; k++) {
                        var edge = node.Inputs[k];
                        edge.ErrorDerivative = node.InputDerivative * edge.Source.Output;
                        edge.AccErrorDer += edge.ErrorDerivative;
                        edge.NumAccumulatedDers++;
                    }
                }

                if (i != 1) {
                    var prevLayer = _nodes[i - 1];
                    for (var j = 0; j < prevLayer.Length; j++) {
                        var node = prevLayer[j];
                        node.OutputDerivative = 0;
                        for (var k = 0; k < node.Outputs.Count; k++) {
                            var outputEdge = node.Outputs[k];
                            node.OutputDerivative += outputEdge.Weight * outputEdge.Destination.InputDerivative;
                        }
                    }
                }
            }
        }

        public void UpdateWeights(double learningRate, double regularizationRate)
        {
            for (var i = 1; i < _nodes.Length; i++) {
                var layer = _nodes[i];
                for (var j = 0; j < layer.Length; j++) {
                    var node = layer[j];

                    if (node.NumAccumulateDers > 0) {
                        node.Bias -= learningRate * node.AccInputDer / node.NumAccumulateDers;
                        node.AccInputDer = 0.0;
                        node.NumAccumulateDers = 0;
                    }

                    for (var k = 0; k < node.Inputs.Count; k++) {
                        var edge = node.Inputs[k];
                        var rd = edge.Regularization != null ? edge.Regularization.Derivative(edge.Weight) : 0.0;

                        if (edge.NumAccumulatedDers > 0) {
                            edge.Weight -= learningRate / edge.NumAccumulatedDers *
                                           (edge.AccErrorDer + regularizationRate * rd);
                            edge.AccErrorDer = 0.0;
                            edge.NumAccumulatedDers = 0;
                        }
                    }
                }
            }
        }
    }
}