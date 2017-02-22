using System.Collections.Generic;

namespace Brain.Evolution
{
  public class Population
  {
    public int Size
    {
      get { return Chromosomes.Count; }
    }

    public Chromosome this[int index]
    {
      get { return Chromosomes[index]; }
      set { Chromosomes[index] = value; }
    }

    public List<Chromosome> Chromosomes { get; set; }
    public int MaxSize { get; set; }
    public int MinSize { get; set; }

    public Population(Chromosome first, int minSize, int maxSize)
    {
      MinSize = minSize;
      MaxSize = maxSize;
      Chromosomes = new List<Chromosome>();

      Chromosomes.Add(first.Clone());
      while (Chromosomes.Count < MinSize) {
        Chromosomes.Add(first.CreateNew());
      }
    }

    public void Reset(List<Chromosome> chromosomes)
    {
      Chromosomes = chromosomes;
    }

    public Chromosome FindFittest()
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