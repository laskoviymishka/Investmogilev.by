using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkModel.BaseModels
{
	public interface INetwork
	{
		IEnumerable<INetwork> Neighbors { get; }

		bool CouldPassTo(INetwork destenation);

		IEnumerable<INetwork> ShortestPassTo(INetwork destenation);

		void AddConnection(INetwork conectedNode);

		void RemoveConnection(INetwork connectedNode);
	}
}
