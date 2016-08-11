using System;
using Brain.Evolution;

namespace Tests.Evolution
{
	public class TargetNumberChromosome : Chromosome
	{
		public const double Target = 42.0;

		public TargetNumberChromosome()
		{
			_genes = new[] {new Gene(Brain.Util.RandomDouble() * 100.0 - 50.0)};
		}

		public double Value
		{
			get { return (double) _genes[0].Value; }
			set { _genes[0].Value = value; }
		}

		public override Chromosome CreateNew()
		{
			return new TargetNumberChromosome {
				Value = Brain.Util.RandomDouble() * 100.0 - 50.0
			};
		}

		public override Chromosome Clone()
		{
			return new TargetNumberChromosome {
				Value = Value
			};
		}

		public override void Mutate()
		{
			Value += Brain.Util.RandomDouble() * 10.0 - 5.0;
		}

		public void Evaluate()
		{
			Fitness = -System.Math.Abs(Target - Value);
		}
	}
}