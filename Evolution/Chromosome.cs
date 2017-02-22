using System;

namespace Brain.Evolution
{
  public abstract class Chromosome : IComparable<Chromosome>
  {
    protected Gene[] _genes;

    public Gene[] Genes
    {
      get { return _genes; }
    }

    public double Fitness { get; set; }

    public int Length
    {
      get { return Genes == null ? 0 : Genes.Length; }
    }

    public Gene this[int index]
    {
      get { return Genes[index]; }
      set { Genes[index] = value; }
    }

    public int CompareTo(Chromosome other)
    {
      return -Fitness.CompareTo(other.Fitness);
    }

    public override int GetHashCode()
    {
      return Fitness.GetHashCode();
    }

    public void Resize(int length)
    {
      Array.Resize(ref _genes, length);
    }

    public abstract Chromosome CreateNew();
    public abstract Chromosome Clone();
    public abstract void Mutate();
  }
}