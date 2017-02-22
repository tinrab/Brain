using System;
using System.Collections.Generic;
using Brain.Math;

namespace Brain.Neuro
{
  public class NetworkTrainer
  {
    private readonly IErrorFunction _errorFunction;
    private readonly double _learningRate;
    private readonly Network _network;
    private readonly double _regularizationRate;

    public NetworkTrainer(Network network, double learningRate = 0.1, double regularizationRate = 0.1)
    {
      _network = network;
      _errorFunction = ErrorFunction.Square;
      _learningRate = learningRate;
      _regularizationRate = regularizationRate;
    }

    public NetworkTrainer(Network network, double learningRate, double regularizationRate, IErrorFunction errorFunction)
    {
      _network = network;
      _errorFunction = errorFunction;
      _learningRate = learningRate;
      _regularizationRate = regularizationRate;
    }

    public void Train(Matrix examples, Vector labels, int maxIterations)
    {
      var labelMatrix = new Matrix(labels.Length, 1);

      for (var i = 0; i < labels.Length; i++) {
        labelMatrix[i, 0] = labels[i];
      }

      Train(examples, labelMatrix, maxIterations);
    }

    public void Train(Matrix examples, Matrix labels, int maxIterations)
    {
      if (_network.OutputLayer.Length != labels.Columns) {
        throw new Exception("Invalid label size");
      }

      while (maxIterations-- >= 0) {
        for (var i = 0; i < examples.Rows; i++) {
          _network.Compute(examples[i]);
          Back(labels[i]);
          Update();
        }
      }
    }

    public void Back(double actual)
    {
      Back(new Vector(actual));
    }

    public void Back(Vector actual)
    {
      // BFS starting at the end
      var s = new Stack<Neuron>();
      for (var i = 0; i < _network.OutputLayer.Length; i++) {
        var n = _network.OutputLayer[i];
        n.OutputDerivative = _errorFunction.Derivative(n.Output, actual[i]);
        s.Push(n);
      }

      while (s.Count != 0) {
        var layer = s.ToArray();
        s.Clear();

        // compute error derivative
        for (var i = 0; i < layer.Length; i++) {
          var n = layer[i];
          n.InputDerivative = n.OutputDerivative * n.Activation.Derivative(n.Input);
          n.InputDerivativeSum += n.InputDerivative;
          n.InputDerivativeCount++;
        }

        // error derivative with respect to input weights
        for (var i = 0; i < layer.Length; i++) {
          var n = layer[i];
          for (var j = 0; j < n.Inputs.Count; j++) {
            var inputLink = n.Inputs[j];
            inputLink.ErrorDerivative = n.InputDerivative * inputLink.Source.Output;
            inputLink.ErrorDerivativeSum += inputLink.ErrorDerivative;
            inputLink.DerivativeCount++;
          }
        }

        for (var i = 0; i < layer.Length; i++) {
          var n = layer[i];
          for (var j = 0; j < n.Inputs.Count; j++) {
            s.Push(n.Inputs[j].Source);
          }
        }

        if (s.Count != 0) {
          var prevLayer = s.ToArray();

          for (var i = 0; i < prevLayer.Length; i++) {
            var n = prevLayer[i];
            if (n.Inputs.Count == 0) {
              goto OUT;
            }
          }

          for (var i = 0; i < prevLayer.Length; i++) {
            var n = prevLayer[i];
            n.OutputDerivative = 0.0;
            for (var j = 0; j < n.Outputs.Count; j++) {
              var outSynapse = n.Outputs[j];
              n.OutputDerivative += outSynapse.Weight * outSynapse.Destination.InputDerivative;
            }
          }
        }

        OUT:
        ;
      }
    }

    public void Update()
    {
      var s = new Stack<Neuron>();
      for (var i = 0; i < _network.InputLayer.Length; i++) {
        var inputNeuron = _network.InputLayer[i];
        for (var j = 0; j < inputNeuron.Outputs.Count; j++) {
          s.Push(inputNeuron.Outputs[j].Destination);
        }
      }

      while (s.Count != 0) {
        var layer = s.ToArray();
        s.Clear();

        for (var i = 0; i < layer.Length; i++) {
          var n = layer[i];

          // update bias
          if (n.InputDerivativeCount > 0) {
            n.Bias -= _learningRate * n.InputDerivativeSum / n.InputDerivativeCount;
            n.InputDerivativeSum = 0.0;
            n.InputDerivativeCount = 0;
          }

          // update input weights
          for (var j = 0; j < n.Inputs.Count; j++) {
            var inSynapse = n.Inputs[j];
            if (inSynapse.DerivativeCount > 0) {
              var rd = inSynapse.Regularization == null ? 0.0 : inSynapse.Regularization.Derivative(inSynapse.Weight);
              inSynapse.Weight -= _learningRate / inSynapse.DerivativeCount *
                                  (inSynapse.ErrorDerivativeSum + _regularizationRate * rd);
              inSynapse.ErrorDerivativeSum = 0.0;
              inSynapse.DerivativeCount = 0;
            }
          }

          for (var j = 0; j < n.Outputs.Count; j++) {
            s.Push(n.Outputs[j].Destination);
          }
        }
      }
    }
  }
}