using System.Collections.Generic;

namespace Brain.Neat
{
  public class InnovationCacher
  {
    private readonly IList<NeatGene> _cache;
    private int _nextHistory;

    public InnovationCacher()
    {
      _cache = new List<NeatGene>();
    }

    public void AssignHistory(NeatChromosome chromosome)
    {
      for (var i = 0; i < chromosome.GeneCount; i++) {
        AssignHistory(chromosome.GetGeneAt(i));
      }
    }

    public void AssignHistory(NeatGene gene)
    {
      var innov = FindGene(gene);

      if (innov == null) {
        gene.History = _nextHistory++;
        _cache.Add(gene);
      } else {
        gene.History = innov.History;
      }
    }

    public void Clear()
    {
      _cache.Clear();
    }

    private NeatGene FindGene(NeatGene gene)
    {
      for (var i = 0; i < _cache.Count; i++) {
        if (_cache[i] == gene) {
          return _cache[i];
        }
      }

      return null;
    }
  }
}