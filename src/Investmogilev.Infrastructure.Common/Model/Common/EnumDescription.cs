// // -----------------------------------------------------------------------
// // <copyright file="EnumDescription.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.Common
{
	#region Using

	using System;
	using System.Reflection;

	#endregion

	public class EnumDescription : Attribute
	{
		private readonly string _value;

		public EnumDescription(string value)
		{
			_value = value;
		}

		public string Value
		{
			get { return _value; }
		}

		public static string GetEnumDescription(Enum value)
		{
			string output = null;
			Type type = value.GetType();
			FieldInfo fi = type.GetField(value.ToString());
			var attrs = fi.GetCustomAttributes(typeof (EnumDescription), false) as EnumDescription[];
			if (attrs.Length > 0)
			{
				output = attrs[0].Value;
			}
			return output;
		}
	}
}