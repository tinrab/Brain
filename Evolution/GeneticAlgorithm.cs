using System.Collections.Generic;

namespace Brain.Evolution
{
	public class GeneticAlgorithm
	{
		public GeneticAlgorithm(Population population)
		{
			Population = population;
		}

		public IChromosome BestChromosome { get; set; }
		public Population Population { get; set; }
		public ISelection Selection { get; set; }
		public ICrossover Crossover { get; set; }
		public IReinsertion Reinsertion { get; set; }
		public double CrossoverProbability { get; set; }
		public double MutationProbability { get; set; }

		public void Evolve()
		{
			var selected = Select();
			var offspring = Cross(selected);
			Mutate(offspring);
			var nextGeneration = Reinsertion.Select(Population, selected, offspring);

			BestChromosome = Population.FindBest();
			Population.Reset(nextGeneration);
		}

		private List<IChromosome> Cross(List<IChromosome> parents)
		{
			var offspring = new List<IChromosome>();

			for (var i = 0; i < Population.MinSize; i += Crossover.RequiredParents) {
				var selected = parents.GetRange(i, Crossover.RequiredParents);
				if (selected.Count == Crossover.RequiredParents && Util.RandomDouble() <= CrossoverProbability) {
					offspring.AddRange(Crossover.Cross(selected));
				}
			}

			return offspring;
		}

		private void Mutate(List<IChromosome> chromosomes)
		{
			for (var i = 0; i < chromosomes.Count; i++) {
				if (Util.RandomDouble() <= MutationProbability) {
					chromosomes[i].Mutate();
				}
			}
		}

		private List<IChromosome> Select()
		{
			return Selection.Select(Population.Chromosomes, Population.MinSize);
		}
	}
}