using System;
using System.Collections.Generic;
using Brain.Math;

namespace Brain.Neuro
{
  public class Network
  {
    public Neuron[] InputLayer { get; set; }
    public Neuron[] OutputLayer { get; set; }

    public int[] Classify(Matrix inputs)
    {
      var classes = new int[inputs.Rows];

      for (var i = 0; i < inputs.Rows; i++) {
        classes[i] = Classify(inputs[i]);
      }

      return classes;
    }

    public int Classify(Vector input)
    {
      Compute(input);
      var c = 0;
      var max = double.NegativeInfinity;

      for (var i = 0; i < OutputLayer.Length; i++) {
        var o = OutputLayer[i].Output;
        if (o > max) {
          c = i;
          max = o;
        }
      }

      return c;
    }

    public Matrix Compute(Matrix inputs)
    {
      var result = new Matrix(inputs.Rows, OutputLayer.Length);

      for (var i = 0; i < inputs.Rows; i++) {
        var row = inputs.GetRow(i);
        result[i] = Compute(row);
      }

      return result;
    }

    public Vector Compute(Vector input)
    {
      if (input.Length != InputLayer.Length) {
        throw new Exception("Invalid input vector length");
      }

      for (var i = 0; i < InputLayer.Length; i++) {
        var n = InputLayer[i];
        n.Output = input[i];
      }

      var s = new Stack<Neuron>();
      // add neurons from layer 1 to the stack
      for (var i = 0; i < InputLayer.Length; i++) {
        var inputNeuron = InputLayer[i];
        for (var j = 0; j < inputNeuron.Outputs.Count; j++) {
          s.Push(inputNeuron.Outputs[j].Destination);
        }
      }

      while (s.Count != 0) {
        var layer = s.ToArray();
        s.Clear();

        for (var i = 0; i < layer.Length; i++) {
          var n = layer[i];
          n.Update();

          for (var j = 0; j < n.Outputs.Count; j++) {
            s.Push(n.Outputs[j].Destination);
          }
        }
      }

      var result = new Vector(OutputLayer.Length);
      for (var i = 0; i < OutputLayer.Length; i++) {
        result[i] = OutputLayer[i].Output;
      }

      return result;
    }
  }
}