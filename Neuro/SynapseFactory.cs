using Brain.Neuro.ParameterGenerators;

namespace Brain.Neuro
{
  public class SynapseFactory
  {
    private readonly IParameterGenerator _parameterGenerator;

    public SynapseFactory()
    {
      _parameterGenerator = new BasicParameterGenerator();
    }

    public SynapseFactory(IParameterGenerator parameterGenerator)
    {
      _parameterGenerator = parameterGenerator;
    }

    public Synapse Link(Neuron source, Neuron destination, IRegularizationFunction regularization = null)
    {
      var s = new Synapse {
        Source = source,
        Destination = destination,
        Regularization = regularization,
        Weight = _parameterGenerator.GenerateSynapseWeight()
      };

      source.Outputs.Add(s);
      destination.Inputs.Add(s);

      return s;
    }
  }
}