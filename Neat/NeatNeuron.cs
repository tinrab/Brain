using System.Collections.Generic;
using Brain.Neuro;

namespace Brain.Neat
{
  // TODO: use Brain.Neuro.Neuron instead
  public class NeatNeuron
  {
    public IActivationFunction Activation { get; set; }
    public IList<Connection> Connections { get; set; }
    public int Layer { get; set; }
    public double Value { get; set; }

    public NeatNeuron() : this(new List<Connection>()) {}

    public NeatNeuron(IList<Connection> connections)
    {
      Connections = connections;
    }

    public double Compute()
    {
      var incoming = 0.0;

      for (var i = 0; i < Connections.Count; i++) {
        var c = Connections[i];
        if (!c.OutGoing) {
          incoming += c.Neuron.Value * c.Weight;
        }
      }

      Value = Activation.Compute(incoming);
      return Value;
    }

    public void Reset()
    {
      Value = 0.0;
    }

    public class Connection
    {
      public NeatNeuron Neuron { get; set; }
      public double Weight { get; set; }
      public bool Recursive { get; set; }
      public bool OutGoing { get; set; }
    }
  }
}