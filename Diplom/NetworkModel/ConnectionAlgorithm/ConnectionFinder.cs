using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;
using MathNet.Numerics.Statistics;
using NetworkModel.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkModel.ConnectionAlgorithm
{
	public class ConnectionFinder
	{
		public static IEnumerable<INetwork> GenerateNetwork(DataLine[] lines)
		{
			IList<INetwork> result = new List<INetwork>();

			foreach (var line in lines)
			{
				result.Add(GenerateNetworkNode(line, lines));
			}

			return result;
		}


		private static INetwork GenerateNetworkNode(DataLine node, DataLine[] lines)
		{
			double[] yData = new double[node.Points.Count() - 1];
			double[,] xDatas = new double[node.Points.Count() - 1, lines.Count()];
			Dictionary<int, DataLine> linesDictionary = new Dictionary<int, DataLine>();

			int iterator = 0;

			foreach (var point in node.Points)
			{
				if (point != node.Points.First())
				{
					xDatas[iterator, lines.Count()] = point.Value;
					yData[iterator++] = point.Value;
				}
			}

			int row = 0;
			foreach (var line in lines)
			{
				if (line != node)
				{
					int column = 0;
					foreach (var point in line.Points)
					{
						if (point != line.Points.Last())
						{
							xDatas[row, column++] = point.Value;
						}
					}
					linesDictionary[row] = line;
					row++;
				}
			}



			throw new NotImplementedException();
		}

		public static Vector<double> MultipleRegression(double[] y, Matrix<double> x)
		{
			DenseVector yVector = new DenseVector(y);
			return x.QR().Solve(yVector);
		}

		public static double RSquare(Vector<double> regression, Vector<double> y, Matrix<double> x)
		{
			double errors = 0;
			double variance =0;
			DescriptiveStatistics statistics = new DescriptiveStatistics(y);

			for (int i = 0; i < y.Count(); i++)
			{
				errors += Math.Pow(y[i] - x.Row(i) * regression, 2);
			}

			foreach (double item in y)
			{
				variance += Math.Pow((item - statistics.Mean), 2);
			}

			return 1 - errors/variance;
		}
	}
}
