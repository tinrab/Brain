namespace Brain.NeuralNetwork
{
    internal class Edge
    {
        public Edge(Node source, Node destination, IRegularizationFunction regularization, bool initZero)
        {
            Source = source;
            Destination = destination;
            Regularization = regularization;
            Weight = initZero ? 0.0 : Util.RandomDouble() - 0.5;
        }

        public Node Source { get; set; }
        public Node Destination { get; set; }
        public double Weight { get; set; }
        public double ErrorDerivative { get; set; }
        public double AccErrorDer { get; set; }
        public int NumAccumulatedDers { get; set; }
        public IRegularizationFunction Regularization { get; set; }
    }
}