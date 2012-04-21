//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.Tests
{
	public class CustomTextFormatter : TextFormatter
	{
		public CustomTextFormatter(string template)
			: base(template)
		{
		}

		public override string Format(LogEntry log)
		{
			CustomLogEntry customEntry = (CustomLogEntry)log;

			StringBuilder templateBuilder = CreateTemplateBuilder();
			templateBuilder.Replace("{field1}", customEntry.AcmeCoField1);
			templateBuilder.Replace("{field2}", customEntry.AcmeCoField2);
			templateBuilder.Replace("{field3}", customEntry.AcmeCoField3);

			CustomToken custom = new CustomToken();
			custom.Format(templateBuilder, log);

			return base.Format(templateBuilder, log);
		}
	}
}

