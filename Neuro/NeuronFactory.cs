using Brain.Neuro.ParameterGenerators;

namespace Brain.Neuro
{
  public class NeuronFactory
  {
    private int _neuronIdCounter;
    private readonly IParameterGenerator _parameterGenerator;

    public NeuronFactory()
    {
      _parameterGenerator = new BasicParameterGenerator();
    }

    public NeuronFactory(IParameterGenerator parameterGenerator)
    {
      _parameterGenerator = parameterGenerator;
    }

    public Neuron CreateNeuron(IActivationFunction activation)
    {
      return new Neuron {
        Id = _neuronIdCounter++,
        Activation = activation,
        Bias = _parameterGenerator.GenerateNeuronBias()
      };
    }

    public Neuron CreateOutputNeuron(IActivationFunction activation)
    {
      return new Neuron {
        Id = _neuronIdCounter++,
        Activation = activation,
        Bias = _parameterGenerator.GenerateNeuronBias()
      };
    }
  }
}