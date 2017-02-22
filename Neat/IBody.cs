namespace Brain.Neat
{
  public interface IBody
  {
    void Reset();
    void Update(double[] outputs);
    bool HasFinished();
    double[] ProvideNetworkWithInputs();

    int InputCount { get; }
    int OutputCount { get; }
    double Fitness { get; }
    double MaxFitness { get; }
  }
}