using System.Collections.Generic;
using System.Linq;

namespace Brain.Neat
{
  public class Species
  {
    private double _bestFitness;
    private readonly Neat _neat;
    private IList<Organism> _population;
    private Organism _representative;
    private bool _sorted;
    private int _stagnantGenerationCount;

    public bool FinishedTask { get; private set; }

    public int Size
    {
      get { return _population.Count; }
    }

    public bool Empty
    {
      get { return _population.Count == 0; }
    }

    public bool Stagnant
    {
      get { return _stagnantGenerationCount >= _neat.Speciation.StagnantSpeciesClearThreshold; }
    }

    public Species(Neat neat, Organism representative)
    {
      _neat = neat;
      _representative = representative;
      _population = new List<Organism>();
    }

    public void AddOrganism(Organism organism)
    {
      _population.Add(organism);
      _sorted = false;
      SetPopulationFitnessModifier();
    }

    public bool IsCompatible(NeatChromosome chromosome)
    {
      var distance = _representative.Chromosome.GetGeneticalDistanceFrom(chromosome);
      return !IsAboveCompatibilityThreshold(distance);
    }

    public double GetAverageFitness()
    {
      return GetTotalFitness() / Size;
    }

    public double GetTotalFitness()
    {
      var total = 0.0;
      for (var i = 0; i < _population.Count; i++) {
        total += _population[i].CalculateFitness();
      }

      return total;
    }

    public int GetOffspringCount(double averageFitness)
    {
      if (Stagnant) {
        return 0;
      }

      if (averageFitness == 0.0) {
        return Size;
      }

      var offspringCount = 0.0;
      for (var i = 0; i < _population.Count; i++) {
        var org = _population[i];
        offspringCount += org.CalculateFitness() / averageFitness;
      }

      return Utility.RoundToInt(offspringCount);
    }

    public void Update()
    {
      FinishedTask = true;
      for (var i = 0; i < _population.Count; i++) {
        var org = _population[i];
        org.Update();
        if (FinishedTask) {
          FinishedTask = org.HasFinished();
        }
      }

      _sorted = false;
    }

    public void Reset()
    {
      for (var i = 0; i < _population.Count; i++) {
        _population[i].Reset();
      }
      FinishedTask = false;
    }

    public void SetPopulationFitnessModifier()
    {
      var modifier = 1.0 / _population.Count;

      for (var i = 0; i < _population.Count; i++) {
        _population[i].FitnessModifier = modifier;
      }
    }

    public void Clear()
    {
      var best = GetFittest().CalculateFitness();

      if (best > _bestFitness) {
        _bestFitness = best;
        _stagnantGenerationCount = 0;
      } else {
        _stagnantGenerationCount++;
      }

      ElectRepresentative();
      _population.Clear();
    }

    public void RemoveWorst()
    {
      var threshold = _neat.Reproduction.ReproductionThreshold;
      var size = (double) Size;
      var potentialParents = (int) (size * threshold);

      var minParents = _neat.Reproduction.MinParents;

      _population = _population.Take(System.Math.Max(potentialParents, minParents)).ToList();
    }

    public Organism GetFittest()
    {
      Sort();
      return _population.First();
    }

    public void Sort()
    {
      if (!_sorted) {
        _population = _population.OrderByDescending(org => org.CalculateFitness()).ToList();
        _sorted = true;
      }
    }

    public Organism GetOrganismToBreed()
    {
      var totalFitness = GetTotalFitness();

      if (totalFitness == 0.0) {
        return _population[Utility.RandomInt(0, _population.Count)];
      }

      var p = 0.0;
      while (true) {
        for (var i = 0; i < _population.Count; i++) {
          var org = _population[i];
          p = p + org.CalculateFitness() / totalFitness;

          if (Utility.RandomDouble() < p) {
            return org;
          }
        }
      }
    }

    private void ElectRepresentative()
    {
      if (!Empty) {
        SelectRandomRepresentative(); // or SelectFittestOrganismAsRepresentative()
      }
    }

    private void SelectRandomRepresentative()
    {
      _representative = _population[Utility.RandomInt(0, _population.Count)];
    }

    private void SelectFittestOrganismAsRepresentative()
    {
      _representative = GetFittest();
    }

    private bool IsAboveCompatibilityThreshold(double x)
    {
      return x > _neat.Speciation.CompatibilityThreshold;
    }
  }
}