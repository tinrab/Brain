using Brain.Evolution;

namespace Brain.Neat
{
  public class NeatGene : Gene
  {
    private readonly Neat _neat;

    public int From { get; set; }
    public int To { get; set; }
    public double Weight { get; set; }
    public bool Enabled { get; set; }
    public bool Recursive { get; set; }
    public int History { get; set; }

    public NeatGene(Neat neat, int from, int to) : base(null)
    {
      _neat = neat;
      From = from;
      To = to;
      Enabled = true;
      SetRandomWeight();
    }

    public void SetRandomWeight()
    {
      Weight = Utility.RandomDouble(_neat.Neural.MinWeight, _neat.Neural.MaxWeight);
    }

    public bool Equals(NeatGene other)
    {
      if (other == null) {
        return false;
      }

      return History == other.History || From == other.From && To == other.To;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) {
        return false;
      }

      if (ReferenceEquals(this, obj)) {
        return true;
      }

      if (obj.GetType() != GetType()) {
        return false;
      }

      return Equals((NeatGene) obj);
    }

    public static bool operator ==(NeatGene left, NeatGene right)
    {
      if (ReferenceEquals(left, right)) {
        return true;
      }

      if ((object)left == null && (object)right == null) {
        return true;
      }

      if ((object) left == null || (object)right == null) {
        return false;
      }

      return left.Equals(right);
    }

    public static bool operator !=(NeatGene left, NeatGene right)
    {
      return !(left == right);
    }
  }
}