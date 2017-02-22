using System.Collections.Generic;

namespace Brain.Neat
{
  public class NeuroEvolution
  {
    private IList<IBody> _bodies;
    private readonly Neat _neat;
    private readonly SpeciesManager _speciesManager;

    public int Generation { get; set; }

    public NeuroEvolution(Neat neat)
    {
      _neat = neat;
      _speciesManager = new SpeciesManager(neat);
    }

    public Organism Begin(IList<IBody> bodies)
    {
      Generation = 0;
      _bodies = bodies;
      _speciesManager.CreateInitialOrganisms(_bodies);

      UpdateGeneration();

      return _speciesManager.GetFittestOrganism();
    }

    public Organism Epoch()
    {
      _speciesManager.Repopulate(_bodies);
      UpdateGeneration();
      Generation++;

      return _speciesManager.GetFittestOrganism();
    }

    private void UpdateGeneration()
    {
      var n = 1;
      if (_neat.Structure.AllowRecurrentConnections) {
        n = _neat.Structure.MemoryResetBeforeTotalReset;
      }

      for (var i = 0; i < n; i++) {
        _speciesManager.Reset();
        while (!_speciesManager.FinishedTask) {
          _speciesManager.Update();
        }
      }
    }
  }
}