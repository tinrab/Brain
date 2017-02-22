using System;
using System.Collections.Generic;
using System.Linq;

namespace Brain.Neat
{
  public class SpeciesManager
  {
    private readonly InnovationCacher _innovationCacher;
    private readonly Neat _neat;
    private bool _sorted;
    private IList<Species> _species;

    public bool FinishedTask { get; set; }

    public int SpeciesCount
    {
      get { return _species.Count; }
    }

    public int PopulationCount
    {
      get
      {
        var count = 0;
        for (var i = 0; i < _species.Count; i++) {
          count += _species[i].Size;
        }
        return count;
      }
    }

    public SpeciesManager(Neat neat)
    {
      _neat = neat;
      _innovationCacher = new InnovationCacher();
      _species = new List<Species>();
    }

    public void CreateInitialOrganisms(IList<IBody> bodies)
    {
      _species.Clear();
      for (var i = 0; i < bodies.Count; i++) {
        var body = bodies[i];
        var standardGenes = new NeatChromosome(_neat, body.InputCount, body.OutputCount);
        _innovationCacher.AssignHistory(standardGenes);
        var phenotype = new Phenotype(_neat, standardGenes);
        var organism = new Organism(_neat, body, phenotype);
        FillOrganismIntoSpecies(organism);
      }
    }

    public Species GetSpecies(int index)
    {
      return _species[index];
    }

    public void Repopulate(IList<IBody> bodies)
    {
      var populationCount = PopulationCount;
      var averageFitness = GetAverageFitness();
      var newGeneration = new List<Organism>(bodies.Count);
      var currentBodyIndex = 0;

      _innovationCacher.Clear();
      for (var i = 0; i < _species.Count; i++) {
        var sp = _species[i];
        var offspringCount = sp.GetOffspringCount(averageFitness);
        offspringCount = System.Math.Min(offspringCount, sp.Size);
        sp.RemoveWorst();

        if (offspringCount >= 1 && sp.Size > _neat.Reproduction.MinSpeciesSizeForChampConservation) {
          var champPhenotype = sp.GetFittest().Phenotype;
          newGeneration.Add(new Organism(_neat, bodies[currentBodyIndex], champPhenotype));
          currentBodyIndex++;
          offspringCount--;
        }

        for (var j = 0; j < offspringCount; j++) {
          var child = BreedInSpecies(sp);
          newGeneration.Add(new Organism(_neat, bodies[currentBodyIndex], child));
          currentBodyIndex++;
        }
      }

      while (newGeneration.Count < populationCount) {
        var child = BreedInSpecies(GetFittestSpecies());
        newGeneration.Add(new Organism(_neat, bodies[currentBodyIndex], child));
        currentBodyIndex++;
      }

      ClearSpeciesPopulation();
      for (var i = 0; i < newGeneration.Count; i++) {
        var child = newGeneration[i];
        FillOrganismIntoSpecies(child);
      }
      DeleteEmptySpecies();
    }

    public Phenotype BreedInSpecies(Species species)
    {
      var parent1 = species.GetOrganismToBreed();
      Organism parent2 = null;

      if (Utility.RandomDouble() < _neat.Reproduction.InterspecialReproductionProbability) {
        parent2 = _species[Utility.RandomInt(0, _species.Count)].GetFittest();
      } else {
        parent2 = species.GetOrganismToBreed();
      }

      return parent2.BreedWith(parent1, _innovationCacher);
    }

    public Species GetFittestSpecies()
    {
      if (_species.Count == 0) {
        throw new Exception("empty population");
      }

      SortSpecies();
      return _species.First();
    }

    public Organism GetFittestOrganism()
    {
      return GetFittestSpecies().GetFittest();
    }

    public void Reset()
    {
      for (var i = 0; i < _species.Count; i++) {
        _species[i].Reset();
      }

      FinishedTask = false;
    }

    public void Update()
    {
      FinishedTask = true;
      for (var i = 0; i < _species.Count; i++) {
        var sp = _species[i];
        sp.Update();
        if (FinishedTask) {
          FinishedTask = sp.FinishedTask;
        }
      }
      _sorted = false;
    }

    public double GetTotalFitness()
    {
      var total = 0.0;
      for (var i = 0; i < _species.Count; i++) {
        total += _species[i].GetTotalFitness();
      }

      return total;
    }

    public double GetAverageFitness()
    {
      return GetTotalFitness() / PopulationCount;
    }

    private void FillOrganismIntoSpecies(Organism organism)
    {
      _sorted = false;
      var compatible = false;

      for (var i = 0; i < _species.Count; i++) {
        var sp = _species[i];
        if (sp.IsCompatible(organism.Chromosome)) {
          sp.AddOrganism(organism);
          compatible = true;
          break;
        }
      }

      if (!compatible) {
        _species.Add(new Species(_neat, organism));
      }
    }

    private void DeleteEmptySpecies()
    {
      _species = _species.Where(sp => !sp.Empty).ToArray();
    }

    private void SortSpecies()
    {
      if (_sorted) {
        return;
      }

      // TODO: check order
      _species = _species.OrderBy(sp => sp.GetFittest().CalculateFitness()).ToList();
      _sorted = true;
    }

    private void ClearSpeciesPopulation()
    {
      for (var i = 0; i < _species.Count; i++) {
        _species[i].Clear();
      }
    }
  }
}