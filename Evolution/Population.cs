using System.Collections.Generic;

namespace Brain.Evolution
{
	public class Population
	{
		public Population(IChromosome first)
		{
			Chromosomes = new List<IChromosome>();
			Chromosomes.Add(first.Clone());

			for (var i = 0; i < MinSize; i++) {
				Chromosomes.Add(first.CreateNew());
			}
		}

		public int Size
		{
			get { return Chromosomes.Count; }
		}

		public IChromosome this[int index]
		{
			get { return Chromosomes[index]; }
			set { Chromosomes[index] = value; }
		}

		public List<IChromosome> Chromosomes { get; set; }
		public int MaxSize { get; set; }
		public int MinSize { get; set; }

		public void Reset(List<IChromosome> chromosomes)
		{
			Chromosomes = chromosomes;
		}

		public IChromosome FindBest()
		{
			var best = Chromosomes[0];

			for (var i = 1; i < Chromosomes.Count; i++) {
				var c = Chromosomes[i];

				if (c.Fitness > best.Fitness) {
					best = c;
				}
			}

			return best;
		}
	}
}