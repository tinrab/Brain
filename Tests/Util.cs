using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Brain.Math;
using Brain.Neuro;

namespace Tests
{
	public static class Util
	{
		public static void PrintNetwork(Network network)
		{
			var s = new SortedSet<Neuron>();
			for (var i = 0; i < network.InputLayer.Length; i++) {
				s.Add(network.InputLayer[i]);
			}

			while (s.Count != 0) {
				var neurons = s.ToArray();
				s.Clear();

				for (var i = 0; i < neurons.Length; i++) {
					PrintNeuron(neurons[i]);
				}
				Console.WriteLine();

				for (var i = 0; i < neurons.Length; i++) {
					var n = neurons[i];
					for (var j = 0; j < n.Outputs.Count; j++) {
						s.Add(n.Outputs[j].Destination);
					}
				}
			}
		}

		private static void PrintNeuron(Neuron neuron, int indentation = 0)
		{
			var indent = "";
			for (var i = 0; i < indentation; i++) {
				indent += "   ";
			}

			Console.Write("{0}Neuron ({1})  ", indent, neuron.Id);
			/*
			for (int i = 0; i < neuron.Outputs.Count; i++) {
				var output = neuron.Outputs[i].Destination;

				PrintNeuron(output, indentation + 1);
			}*/
		}

		public static string[][] ReadCSVFile(string path)
		{
			using (var r = new StreamReader(File.OpenRead(path))) {
				var data = new List<string[]>();

				while (!r.EndOfStream) {
					var line = r.ReadLine();
					var values = line.Split(',');
					data.Add(values);
				}

				return data.ToArray();
			}
		}

		public static Matrix LoadIrisDataSet()
		{
			var csv = ReadCSVFile("Data/Iris.csv");
			var data = new double[csv.Length, 5];
			var classes = new Dictionary<string, double> {
				{"Iris-setosa", 0.0},
				{"Iris-versicolor", 1.0},
				{"Iris-virginica", 2.0}
			};
			for (var i = 0; i < data.GetLength(0); i++) {
				var row = csv[i];
				for (var j = 0; j < row.Length; j++) {
					var value = row[j];
					if (j == 4) {
						data[i, 4] = classes[value];
					} else {
						data[i, j] = double.Parse(value);
					}
				}
			}

			return new Matrix(data);
		}

		public static void PrintArray<T>(T[] array)
		{
			Console.WriteLine("[{0}]", string.Join(",", array));
		}
	}
}