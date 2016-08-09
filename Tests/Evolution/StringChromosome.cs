using Brain.Evolution;

namespace Tests.Evolution
{
	public class StringChromosome : IChromosome
	{
		public string Genes { get; set; }

		public double Fitness
		{
			get {
				var f = 0.0;
				return f;
			}
		}

		public IChromosome CreateNew()
		{
			return new StringChromosome();
		}

		public IChromosome Clone()
		{
			return new StringChromosome {
				Genes = Genes
			};
		}

		public void Mutate()
		{
			
		}
	}
}