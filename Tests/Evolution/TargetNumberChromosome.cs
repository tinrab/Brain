using System;
using Brain.Evolution;

namespace Tests.Evolution
{
	public class TargetNumberChromosome : IChromosome
	{
		public const double Target = 42.0;

		public double Value { get; set; }
		public double Fitness { get; set; }

		public IChromosome CreateNew()
		{
			return new TargetNumberChromosome {
				Value = Brain.Util.RandomDouble() * 100.0
			};
		}

		public IChromosome Clone()
		{
			return new TargetNumberChromosome {
				Value = Value
			};
		}

		public void Mutate()
		{
			Value += Brain.Util.RandomDouble() * 2.0 - 1.0;
		}

		public void Evaluate()
		{
			Fitness = -Math.Abs(Target - Value);
		}
	}
}