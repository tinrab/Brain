using System;
using System.Collections.Generic;
using Brain.Math;

namespace Brain.Neat
{
  public class Phenotype
  {
    private readonly Dictionary<int, IList<NeatNeuron>> _layerMap;
    private readonly Neat _neat;
    private IList<NeatNeuron> _neurons;
    public NeatChromosome Chromosome { get; }

    public Phenotype(Neat neat, NeatChromosome chromosome)
    {
      _layerMap = new Dictionary<int, IList<NeatNeuron>>();
      _neat = neat;
      Chromosome = chromosome;

      BuildFromGenes();
    }

    public Phenotype(Neat neat, NeatChromosome chromosome, InnovationCacher currentGeneration)
    {
      _layerMap = new Dictionary<int, IList<NeatNeuron>>();
      _neat = neat;
      Chromosome = chromosome;
      MutateAndBuild(currentGeneration);
    }

    public double[] Compute(double[] inputs)
    {
      SetInputs(inputs);
      return GetOutputs();
    }

    public void Reset()
    {
      for (var i = 0; i < _neurons.Count; i++) {
        _neurons[i].Reset();
      }
    }

    private void SetInputs(double[] inputs)
    {
      var inputNeurons = GetInputNeurons();
      if (inputNeurons.Count != inputs.Length) {
        throw new Exception("Invalid input size");
      }

      for (var i = 0; i < inputNeurons.Count; i++) {
        inputNeurons[i].Value = inputs[i];
      }
    }

    private double[] GetOutputs()
    {
      for (var i = 1; i < _layerMap.Count - 1; ++i) {
        var neurons = _layerMap[i];
        for (var j = 0; j < neurons.Count; j++) {
          neurons[j].Compute();
        }
      }

      var outputNeurons = GetOutputNeurons();
      var outputs = new double[outputNeurons.Count];

      for (var i = 0; i < outputNeurons.Count; i++) {
        outputs[i] = outputNeurons[i].Compute();
      }

      return outputs;
    }

    private IList<NeatNeuron> GetOutputNeurons()
    {
      var neurons = new List<NeatNeuron>(Chromosome.OutputCount);

      for (var i = 0; i < Chromosome.OutputCount; i++) {
        neurons.Add(_neurons[Chromosome.GetGeneAt(i).To]);
      }

      return neurons;
    }

    private IList<NeatNeuron> GetInputNeurons()
    {
      var neurons = new List<NeatNeuron>(Chromosome.InputCount);

      for (var i = 0; i < Chromosome.InputCount; i++) {
        neurons.Add(_neurons[i + _neat.Structure.BiasNeuronCount]);
      }

      return neurons;
    }

    private void MutateAndBuild(InnovationCacher currentGeneration)
    {
      if (ShouldAddConnection()) {
        BuildFromGenes();
        AddRandomConnection(currentGeneration);
      } else {
        if (ShouldAddNeuron()) {
          AddRandomNeuron(currentGeneration);
        } else {
          ShuffleWeights();
        }

        BuildFromGenes();
      }
    }

    private void BuildFromGenes()
    {
      _neurons = new List<NeatNeuron>(Chromosome.NeuronCount);
      for (var i = 0; i < Chromosome.NeuronCount; i++) {
        var neuron = new NeatNeuron {
          Activation = _neat.Neural.ActivationFunction
        };
        _neurons.Add(neuron);
      }

      for (var i = 0; i < Chromosome.GeneCount; i++) {
        var gene = Chromosome.GetGeneAt(i);

        if (gene.Enabled) {
          var inConnection = new NeatNeuron.Connection();
          inConnection.Weight = gene.Weight;
          inConnection.Recursive = gene.Recursive;
          inConnection.Neuron = _neurons[gene.From];
          _neurons[gene.To].Connections.Add(inConnection);

          var outConnection = new NeatNeuron.Connection();
          outConnection.Weight = gene.Weight;
          outConnection.Recursive = gene.Recursive;
          outConnection.Neuron = _neurons[gene.To];
          outConnection.OutGoing = true;
          _neurons[gene.From].Connections.Add(outConnection);
        }
      }

      for (var i = 0; i < _neat.Structure.BiasNeuronCount; i++) {
        _neurons[i].Value = 1.0;
      }

      CategorizeNeuronsIntoLayers();
    }

    private bool ShouldAddConnection()
    {
      if (Utility.RandomDouble() > _neat.Mutation.ConnectionMutationProbability) {
        return false;
      }

      var inputLayerSize = Chromosome.InputCount + _neat.Structure.BiasNeuronCount;
      var outputLayerSize = Chromosome.OutputCount;
      var hiddenLayerSize = Chromosome.NeuronCount - inputLayerSize - outputLayerSize;

      var startingConnections = inputLayerSize * outputLayerSize;
      var hiddenConnections = hiddenLayerSize * (hiddenLayerSize - 1);

      if (!_neat.Structure.AllowRecurrentConnections) {
        hiddenConnections /= 2;
      }

      var connectionsFromInputs = inputLayerSize * hiddenLayerSize;
      var connectionsToOutputs = outputLayerSize * hiddenLayerSize;

      if (_neat.Structure.AllowRecurrentConnections) {
        connectionsToOutputs *= 2;
      }

      var possibleConnections = startingConnections + hiddenConnections + connectionsFromInputs + connectionsToOutputs;
      return Chromosome.GeneCount < possibleConnections;
    }

    private bool ShouldAddNeuron()
    {
      return Utility.RandomDouble() < _neat.Mutation.NeuralMutationProbability;
    }

    private bool ShouldMutateWeight()
    {
      return Utility.RandomDouble() < _neat.Mutation.WeightMutationProbability;
    }

    private void AddRandomNeuron(InnovationCacher currentGeneration)
    {
      var randGene = GetRandomEnabledGene();
      var index = Chromosome.NeuronCount;

      var g1 = new NeatGene(_neat, randGene.From, index);
      g1.Weight = 1.0;
      g1.Recursive = randGene.Recursive;
      currentGeneration.AssignHistory(g1);

      var g2 = new NeatGene(_neat, index, randGene.To);
      g2.Weight = randGene.Weight;
      g2.Recursive = randGene.Recursive;
      currentGeneration.AssignHistory(g2);

      randGene.Enabled = false;

      Chromosome.AppendGene(g1);
      Chromosome.AppendGene(g2);
    }

    private void AddRandomConnection(InnovationCacher currentGeneration)
    {
      var neuronPair = GetTwoUnconnectedNeurons();
      var fromNeuron = neuronPair[0];
      var toNeuron = neuronPair[1];

      var newConnectionGene = new NeatGene(_neat, _neurons.IndexOf(fromNeuron), _neurons.IndexOf(toNeuron));

      if (fromNeuron.Layer > toNeuron.Layer) {
        if (!_neat.Structure.AllowRecurrentConnections) {
          throw new Exception("Illegal recurrent connection");
        }

        newConnectionGene.Recursive = true;
      }

      currentGeneration.AssignHistory(newConnectionGene);

      var inConnection = new NeatNeuron.Connection();
      inConnection.Recursive = newConnectionGene.Recursive;
      inConnection.Neuron = fromNeuron;
      inConnection.Weight = newConnectionGene.Weight;
      toNeuron.Connections.Add(inConnection);

      var outConnection = new NeatNeuron.Connection();
      outConnection.Recursive = newConnectionGene.Recursive;
      outConnection.Neuron = toNeuron;
      outConnection.Weight = newConnectionGene.Weight;
      outConnection.OutGoing = true;
      fromNeuron.Connections.Add(outConnection);

      Chromosome.AppendGene(newConnectionGene);
      CategorizeNeuronsIntoLayers();
    }

    private NeatNeuron[] GetTwoUnconnectedNeurons()
    {
      var possibleFromNeurons = new List<NeatNeuron>(_neurons);

      var inputRange = Chromosome.InputCount + _neat.Structure.BiasNeuronCount;
      var possibleToNeurons = new List<NeatNeuron>();

      for (var i = inputRange; i < _neurons.Count; i++) {
        possibleToNeurons.Add(_neurons[i]);
      }

      Utility.Shuffle(possibleFromNeurons);
      Utility.Shuffle(possibleToNeurons);

      for (var i = 0; i < possibleFromNeurons.Count; i++) {
        var from = possibleFromNeurons[i];
        for (var j = 0; j < possibleToNeurons.Count; j++) {
          var to = possibleToNeurons[j];
          if (CanConnect(from, to)) {
            return new[] {
              from,
              to
            };
          }
        }
      }

      throw new Exception("All neurons already connected");
    }

    private NeatGene GetRandomEnabledGene()
    {
      var num = Utility.RandomInt(Chromosome.GeneCount - 1);
      var r = num;

      while (r < Chromosome.GeneCount && !Chromosome.GetGeneAt(r).Enabled) {
        ++r;
      }

      if (r == Chromosome.GeneCount) {
        r = num;
        while (r >= 0 && !Chromosome.GetGeneAt(r).Enabled) {
          --r;
        }
      }

      return Chromosome.GetGeneAt(r);
    }

    private bool CanConnect(NeatNeuron a, NeatNeuron b)
    {
      var same = a == b;
      var canConnect = !same && !AreOutputs(a, b) && !AreConnected(a, b);

      if (!_neat.Structure.AllowRecurrentConnections) {
        var isRecurrent = a.Layer > b.Layer;
        canConnect = canConnect && !isRecurrent;
      }

      return canConnect;
    }

    private bool AreOutputs(NeatNeuron a, NeatNeuron b)
    {
      var aOut = false;
      var bOut = false;

      var outputNeurons = GetOutputNeurons();
      for (var i = 0; i < outputNeurons.Count; i++) {
        var n = outputNeurons[i];
        if (n == a) {
          aOut = true;
        } else if (n == b) {
          bOut = true;
        }

        if (aOut && bOut) {
          return true;
        }
      }

      return false;
    }

    private bool AreConnected(NeatNeuron a, NeatNeuron b)
    {
      for (var i = 0; i < b.Connections.Count; i++) {
        var c = b.Connections[i];
        if (!c.OutGoing && a == c.Neuron) {
          return true;
        }
      }
      return false;
    }

    private void ShuffleWeights()
    {
      for (var i = 0; i < Chromosome.GeneCount; i++) {
        if (ShouldMutateWeight()) {
          MutateWeightOfGeneAt(i);
        }
      }
    }

    private void MutateWeightOfGeneAt(int index)
    {
      if (Utility.RandomDouble() < _neat.Mutation.TotalWeightResetProbability) {
        Chromosome.GetGeneAt(index).SetRandomWeight();
      } else {
        PerturbWeightAt(index);
      }
    }

    private void PerturbWeightAt(int index)
    {
      var perturbRange = 2.5;
      var perturbance = Utility.RandomDouble(-perturbRange, perturbRange);

      var w = Chromosome.GetGeneAt(index).Weight;
      Chromosome.GetGeneAt(index).Weight = MathUtil.Clamp(w + perturbance, _neat.Neural.MinWeight,
        _neat.Neural.MaxWeight);
    }

    private void CategorizeNeuronsIntoLayers()
    {
      for (var i = 0; i < _neat.Structure.BiasNeuronCount; i++) {
        CategorizeNeuronBranchIntoLayers(_neurons[i]);
      }
      var inputNeurons = GetInputNeurons();
      for (var i = 0; i < inputNeurons.Count; i++) {
        CategorizeNeuronBranchIntoLayers(inputNeurons[i]);
      }

      var highestLayer = 0;
      var outputNeurons = GetOutputNeurons();

      for (var i = 0; i < outputNeurons.Count; i++) {
        var n = outputNeurons[i];
        highestLayer = System.Math.Max(n.Layer, highestLayer);
      }

      for (var i = 0; i < outputNeurons.Count; i++) {
        var n = outputNeurons[i];
        n.Layer = highestLayer;
      }

      for (var i = 0; i < _neurons.Count; i++) {
        var n = _neurons[i];
        if (_layerMap.ContainsKey(n.Layer)) {
          _layerMap[n.Layer].Add(n);
        } else {
          _layerMap[n.Layer] = new List<NeatNeuron> {
            n
          };
        }
      }
    }

    private void CategorizeNeuronBranchIntoLayers(NeatNeuron current, int depth = 0)
    {
      current.Layer = depth;
      var nextLayer = current.Layer + 1;

      for (var i = 0; i < current.Connections.Count; i++) {
        var c = current.Connections[i];

        if (nextLayer > c.Neuron.Layer && c.OutGoing != c.Recursive) {
          CategorizeNeuronBranchIntoLayers(c.Neuron, nextLayer);
        }
      }
    }
  }
}