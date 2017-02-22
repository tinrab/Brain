namespace Brain.Neat
{
  public class Organism
  {
    private readonly IBody _body;
    private double _fitness;
    private bool _isFitnessUpdated;
    private readonly Neat _neat;
    public Phenotype Phenotype { get; }

    public double FitnessModifier { get; set; }

    public double MaxFitness
    {
      get { return _body.MaxFitness; }
    }

    public NeatChromosome Chromosome
    {
      get { return Phenotype.Chromosome; }
    }

    public Organism(Neat neat, IBody body, Phenotype phenotype)
    {
      _neat = neat;
      _body = body;
      Phenotype = phenotype;
      FitnessModifier = 1.0;
    }

    public void Reset()
    {
      _body.Reset();
    }

    public void Update()
    {
      if (!_body.HasFinished()) {
        var inputs = _body.GetInputs();
        var outputs = Phenotype.Compute(inputs);
        _body.Activate(outputs);
        _isFitnessUpdated = false;
      }
    }

    public double CalculateFitness()
    {
      return CalculateRawFitness() * FitnessModifier;
    }

    public double CalculateRawFitness()
    {
      if (!_isFitnessUpdated) {
        _fitness = _body.Fitness;
        _isFitnessUpdated = true;
      }

      return _fitness;
    }

    public Phenotype BreedWith(Organism other, InnovationCacher currentGeneration)
    {
      Organism dominantParent;
      if (CalculateFitness() == other.CalculateFitness()) {
        dominantParent = Utility.RandomBoolean() ? this : other;
      } else {
        dominantParent = CalculateFitness() > other.CalculateFitness() ? this : other;
      }

      var child = dominantParent.Chromosome;

      var sizeOfSmallerParent = System.Math.Min(Chromosome.GeneCount, other.Chromosome.GeneCount);
      for (var i = 0; i < sizeOfSmallerParent && child.GetGeneAt(i) == other.Chromosome.GetGeneAt(i); ++i) {
        if (Utility.RandomBoolean()) {
          child.GetGeneAt(i).Weight = other.Chromosome.GetGeneAt(i).Weight;
        }
      }

      return new Phenotype(_neat, child, currentGeneration);
    }

    public bool HasFinished()
    {
      return _body.HasFinished();
    }
  }
}