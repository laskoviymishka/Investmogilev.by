using NetworkModel.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkModel.ConnectionAlgorithm
{
	public interface ICalculateNetwork
	{
		IEnumerable<INetwork> GenerateNetwork(DataLine[] lines);
	}
}
