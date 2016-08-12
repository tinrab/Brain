using System;
using System.Collections.Generic;

namespace Brain.Evolution.Selections
{
	public class TournamentSelection : ISelection
	{
		public TournamentSelection(int size, bool allowRetry = true)
		{
			Size = size;
			AllowRetry = allowRetry;
		}

		public int Size { get; set; }
		public bool AllowRetry { get; set; }

		public List<Chromosome> Select(List<Chromosome> chromosomes, int count)
		{
			if (Size >= chromosomes.Count) {
				throw new Exception("Tournament size is too large");
			}

			var players = new List<Chromosome>(chromosomes);
			var selected = new List<Chromosome>();

			players.Sort();

			while (selected.Count < count) {
				var randomIndexes = Util.RandomUniqueInts(Size, 0, chromosomes.Count);
				var winner = -1;

				for (var i = 0; i < players.Count; i++) {
					if (Array.IndexOf(randomIndexes, i) != -1) {
						winner = i;
						break;
					}
				}

				selected.Add(players[winner]);

				if (!AllowRetry) {
					players.RemoveAt(winner);
				}
			}

			return selected;
		}
	}
}