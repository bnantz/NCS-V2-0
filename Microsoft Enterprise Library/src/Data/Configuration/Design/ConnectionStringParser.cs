//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	sealed class ConnectionStringNameValuePair
	{
		private readonly string name;
		private readonly string value;

		public ConnectionStringNameValuePair(string name, string value)
		{
			this.name = name;
			this.value = value;
		}

		public string Name 
		{
			get { return name;  }
		}
		
		public string Value 
		{
			get { return value;  }
		}
	}

	static class ConnectionStringParser
	{		

		public static ICollection<ConnectionStringNameValuePair> Parse(string connectionString)
		{
			List<ConnectionStringNameValuePair> pairs = new List<ConnectionStringNameValuePair>();
			string[] splitString = connectionString.Split(';');
			for (int index = 0; index < splitString.Length; ++index)
			{
				string[] nameValuePair = splitString[index].Split('=');
				if (nameValuePair.Length == 2) pairs.Add(new ConnectionStringNameValuePair(nameValuePair[0], nameValuePair[1]));
			}
			return pairs;
		}
		
		public static string Build(ICollection<ConnectionStringNameValuePair> pairs)
		{
			StringBuilder builder = new StringBuilder();
			foreach (ConnectionStringNameValuePair pair in pairs)
			{
				builder.AppendFormat("{0}={1};", pair.Name, pair.Value);
			}
			return builder.ToString();
		}
	}
}
