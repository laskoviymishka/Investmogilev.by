using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkModel.BaseModels
{
	public class BaseNetwork : INetwork
	{
		#region Fields

		protected readonly IList<INetwork> _neighbors;

		#endregion

		#region Constructor

		public BaseNetwork()
		{
			_neighbors = new List<INetwork>();
		}

		public BaseNetwork(IEnumerable<INetwork> neghborNodes)
		{
			_neighbors = new List<INetwork>(neghborNodes);
		}

		#endregion

		#region INetwork implementation

		public IEnumerable<INetwork> Neighbors
		{
			get
			{
				return _neighbors;
			}
		}

		public bool CouldPassTo(INetwork destenation)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<INetwork> ShortestPassTo(INetwork destenation)
		{
			//алгоритм дейкстры сюда запилить
			throw new NotImplementedException();
		}


		public void AddConnection(INetwork conectedNode)
		{
			_neighbors.Add(conectedNode);
		}

		public void RemoveConnection(INetwork connectedNode)
		{
			if (_neighbors.Contains(connectedNode))
			{
				_neighbors.Remove(connectedNode);
			}
			else
			{
				throw new ArgumentException("there is no such node in neighborhood");
			}
		}

		#endregion
	}
}
