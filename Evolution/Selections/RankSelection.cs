using System.Collections.Generic;

namespace Brain.Evolution.Selections
{
	public class RankSelection : ISelection
	{
		public List<Chromosome> Select(List<Chromosome> chromosomes, int count)
		{
			var selected = new List<Chromosome>();

			chromosomes.Sort();
			var ranges = chromosomes.Count * (chromosomes.Count + 1) / 2.0;
			var rangeMax = new double[chromosomes.Count];
			var s = 0.0;

			for (int i = 0, n = chromosomes.Count; i < chromosomes.Count; i++,n--) {
				s += n / ranges;
				rangeMax[i] = s;
			}

			for (var i = 0; i < count; i++) {
				var prob = Util.RandomDouble();

				for (var j = 0; j < chromosomes.Count; j++) {
					if (prob <= rangeMax[j]) {
						selected.Add(chromosomes[j].Clone());
						break;
					}
				}
			}

			return selected;
		}
	}
}