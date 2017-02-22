using System.Collections.Generic;

namespace Brain.Evolution
{
  public class GeneticAlgorithm
  {
    private const double DefaultCrossoverProbability = 0.8;
    private const double DefaultMutationProbability = 0.8;
    private List<Chromosome> _offspring;
    private List<Chromosome> _selected;

    public Chromosome FittestChromosome { get; set; }
    public Population Population { get; set; }
    public ISelection Selection { get; set; }
    public ICrossover Crossover { get; set; }
    public IReinsertion Reinsertion { get; set; }
    public double CrossoverProbability { get; set; }
    public double MutationProbability { get; set; }

    public GeneticAlgorithm(Population population, ISelection selection, ICrossover crossover, IReinsertion reinsertion)
    {
      Population = population;
      Selection = selection;
      Crossover = crossover;
      Reinsertion = reinsertion;
      CrossoverProbability = DefaultCrossoverProbability;
      MutationProbability = DefaultMutationProbability;
    }

    public void BeginGeneration()
    {
      _selected = Select();
      _offspring = Cross(_selected);
      Mutate(_offspring);
    }

    public void EndGeneration()
    {
      var nextGeneration = Reinsertion.Select(Population, _selected, _offspring);
      var best = Population.FindFittest();

      if (FittestChromosome == null || best.Fitness > FittestChromosome.Fitness) {
        FittestChromosome = best;
      }

      Population.Reset(nextGeneration);
    }

    private List<Chromosome> Cross(List<Chromosome> parents)
    {
      var offspring = new List<Chromosome>();

      for (var i = 0; i < parents.Count - Crossover.RequiredParents; i += Crossover.RequiredParents) {
        var selected = parents.GetRange(i, Crossover.RequiredParents);
        if (selected.Count == Crossover.RequiredParents && Utility.RandomDouble() <= CrossoverProbability) {
          offspring.AddRange(Crossover.Cross(selected));
        }
      }

      return offspring;
    }

    private void Mutate(List<Chromosome> chromosomes)
    {
      for (var i = 0; i < chromosomes.Count; i++) {
        if (Utility.RandomDouble() <= MutationProbability) {
          chromosomes[i].Mutate();
        }
      }
    }

    private List<Chromosome> Select()
    {
      return Selection.Select(Population.Chromosomes, Population.MinSize);
    }
  }
}