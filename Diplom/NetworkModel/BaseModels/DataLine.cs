using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkModel.BaseModels
{
	public class DataLine
	{
		#region Fields

		private readonly DataPoint[] _points;

		private readonly object _entity;

		#endregion

		#region Constructor

		public DataLine(object entity, IEnumerable<DataPoint> points)
		{
			_entity = entity;
			_points = points.ToArray();
		}

		#endregion

		#region Properties

		public DataPoint[] Points { get { return _points; } }
		public object LineEntity { get { return _entity; } }

		#endregion
	}
}
