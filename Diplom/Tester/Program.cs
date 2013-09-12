using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using NetworkModel.ConnectionAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
	class Program
	{
		static void Main(string[] args)
		{
			// data points: we compute y perfectly but then add strong random noise to it
			var ydata = new double[] { 225, 243, 249, 262, 272, 280, 285, 290, 295, 317, 337, 343, 356, 362, 395, 465, 465 };

			// build matrices
			var X = Matrix.CreateFromColumns(new[] {
				new DenseVector(new double[]{10,10,10,11,12,12,12,12,12,14,14,15,15,15,17,20,20}),
				new DenseVector(new double[]{3,10,10,18,11,15,11,18,12,6,16,8,10,15,18,10,14}),
				new DenseVector(new double[]{1,13,17,10,2,7,11,14,18,3,16,5,14,18,9,12,12}),
				new DenseVector(new double[]{1,2,4,3,5,6,7,8,9,10,11,11,12,13,14,15,16})});
			var y = new DenseVector(ydata);
			var yData = new double[,]
			{
			{1,2,3,5,7,9},
			{1,2,6,7,4,11},
			{3,11,6,8,15,18}
			};
			// solve
			var p = X.QR().Solve(y);
			foreach (var pp in p)
			{
				Console.WriteLine(pp);
			}

			p = ConnectionFinder.MultipleRegression(ydata, X);

			DenseVector yVector = new DenseVector(ydata);

			Console.WriteLine(ConnectionFinder.RSquare(p, yVector, X));

			Console.ReadLine();
		}
	}
}
