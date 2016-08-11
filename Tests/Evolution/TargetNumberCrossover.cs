using System.Collections.Generic;
using Brain.Evolution;

namespace Tests.Evolution
{
	internal class TargetNumberCrossover : ICrossover
	{
		public int RequiredParents
		{
			get { return 2; }
		}

		public List<Chromosome> Cross(List<Chromosome> parents)
		{
			var a = (TargetNumberChromosome) parents[0];
			var b = (TargetNumberChromosome) parents[1];
			var offspring = new List<Chromosome> {
				new TargetNumberChromosome {
					Value = (a.Value + b.Value) / 2.0
				}
			};

			return offspring;
		}
	}
}