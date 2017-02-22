using System.Collections.Generic;
using Brain.Evolution;

namespace Brain.Neat
{
  public class NeatChromosome : Chromosome
  {
    private readonly List<NeatGene> _genes;
    private readonly Neat _neat;

    public int InputCount { get; private set; }
    public int OutputCount { get; private set; }
    public int NeuronCount { get; private set; }

    public int GeneCount
    {
      get { return _genes.Count; }
    }

    public NeatChromosome(Neat neat, int inputCount, int outputCount)
    {
      _neat = neat;
      InputCount = inputCount;
      OutputCount = outputCount;
      _genes = new List<NeatGene>();

      for (var i = 0; i < inputCount + neat.Structure.BiasNeuronCount; i++) {
        for (var j = 0; j < outputCount; j++) {
          var gene = new NeatGene(neat, i, j + inputCount + neat.Structure.BiasNeuronCount);
          AppendGene(gene);
        }
      }
    }

    public NeatGene GetGeneAt(int index)
    {
      return _genes[index];
    }

    public void AppendGene(NeatGene gene)
    {
      AdjustNeuronCount(gene);
      _genes.Add(gene);
    }

    public void InsertGeneAt(NeatGene gene, int index)
    {
      AdjustNeuronCount(gene);
      _genes.Insert(index, gene);
    }

    public double GetGeneticalDistanceFrom(NeatChromosome other)
    {
      var totalWeightDifference = 0.0;
      var overlapingGenes = 0;

      var smallerSize = System.Math.Min(GeneCount, other.GeneCount);

      for (var i = 0;
        i < smallerSize && GetGeneAt(i)
          .History == other.GetGeneAt(i)
          .History;
        ++i) {
        totalWeightDifference += System.Math.Abs(GetGeneAt(i)
                                                   .Weight - other.GetGeneAt(i)
                                                   .Weight);
        overlapingGenes++;
      }

      var disjointGenes = GeneCount + other.GeneCount - 2 * overlapingGenes;

      var disjointGenesInfluence = (double) disjointGenes;
      var averageWeightDifference = totalWeightDifference / overlapingGenes;

      disjointGenesInfluence *= _neat.Speciation.ImportanceOfDisjointGenes;
      averageWeightDifference *= _neat.Speciation.ImportanceOfAverageWeightDifference;

      return disjointGenesInfluence + averageWeightDifference;
    }

    public bool ContainsGene(NeatGene gene)
    {
      return _genes.Contains(gene);
    }

    private void AdjustNeuronCount(NeatGene gene)
    {
      if (gene.To + 1 > NeuronCount) {
        NeuronCount = gene.To + 1;
      }
    }

    public override Chromosome CreateNew()
    {
      return null;
    }

    public override Chromosome Clone()
    {
      return null;
    }

    public override void Mutate() {}
  }
}