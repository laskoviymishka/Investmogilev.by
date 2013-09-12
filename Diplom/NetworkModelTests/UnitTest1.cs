using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkModel.ConnectionAlgorithm;

namespace NetworkModelTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void NetworkInitialization()
		{
			double[] y = new double[] { 10, 12, 14, 15, 12, 17 };
			double[,] xDatas = new double[,] 
			{
				{1,2,3,5,7,9},
				{1,2,6,7,4,11},
				{3,11,6,8,15,18}
			};

			ConnectionFinder test = new ConnectionFinder();
			//var t = ConnectionFinder.MultipleRegression(y, xDatas);
			//Assert.IsTrue(t.Count == 3);
		}
	}
}
