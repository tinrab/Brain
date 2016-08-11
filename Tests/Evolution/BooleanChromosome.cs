using Brain.Evolution;

namespace Tests.Evolution
{
	internal class BooleanChromosome : Chromosome
	{
		public const int Length = 100;

		public BooleanChromosome()
		{
			_genes = new Gene[Length];
			for (var i = 0; i < _genes.Length; i++) {
				_genes[i] = new Gene(Brain.Util.RandomBoolean());
			}
		}

		public bool[] Value
		{
			get {
				var v = new bool[_genes.Length];
				for (var i = 0; i < v.Length; i++) {
					v[i] = (bool) _genes[i].Value;
				}
				return v;
			}
			set {
				_genes = new Gene[value.Length];
				for (var i = 0; i < value.Length; i++) {
					_genes[i] = new Gene(value[i]);
				}
			}
		}

		public override Chromosome CreateNew()
		{
			return new BooleanChromosome();
		}

		public override Chromosome Clone()
		{
			return new BooleanChromosome {
				Value = Value
			};
		}

		public override void Mutate()
		{
			var v = Value;

			var i = Brain.Util.RandomInt(v.Length);
			v[i] = !v[i];

			Value = v;
		}

		public void Evaluate(bool[] target)
		{
			var v = Value;
			Fitness = 0.0;
			if (v.Length == target.Length) {
				for (var i = 0; i < v.Length; i++) {
					if (v[i] != target[i]) {
						Fitness -= 10.0;
					}
				}
			} else {
				Fitness = -1000.0;
			}
		}
	}
}