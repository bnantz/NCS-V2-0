/**
Copyright (c) 2005, Nantz Consulting & Software 

- All rights reserved. -

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
Redistributions in binary form must produce the above copyright notice, this list of conditions and the following disclaimer 
in the documentation and/or other materials provided with the distribution. Neither the name of Cornerstone Consulting nor 
the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
OF SUCH DAMAGE.
**/
using System;

namespace NCS.Utilities.CommandLineHelper
{
	/// <summary>Implements a basic command-line switch by taking the
	/// switching name and the associated description.</summary>
	/// <remark>Only currently is implemented for properties, so all
	/// auto-switching variables should have a get/set method supplied.</remark>
	[AttributeUsage( AttributeTargets.Property )]
	public class CommandLineSwitchAttribute : System.Attribute
	{
		#region Private Variables
		private string m_name = "";
		private string m_description = "";
		#endregion

		#region Public Properties
		/// <summary>Accessor for retrieving the switch-name for an associated
		/// property.</summary>
		public string Name 			{ get { return m_name; } }

		/// <summary>Accessor for retrieving the description for a switch of
		/// an associated property.</summary>
		public string Description	{ get { return m_description; } }

		#endregion

		#region Constructors
		/// <summary>Attribute constructor.</summary>
		public CommandLineSwitchAttribute( string name,
													  string description )
		{
			m_name = name;
			m_description = description;
		}
		#endregion
	}

	/// <summary>
	/// This class implements an alias attribute to work in conjunction
	/// with the <see cref="CommandLineSwitchAttribute">CommandLineSwitchAttribute</see>
	/// attribute.  If the CommandLineSwitchAttribute exists, then this attribute
	/// defines an alias for it.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property )]
	public class CommandLineAliasAttribute : System.Attribute
	{
		#region Private Variables
		protected string m_Alias = "";
		#endregion

		#region Public Properties
		public string Alias 
		{
			get { return m_Alias; }
		}

		#endregion

		#region Constructors
		public CommandLineAliasAttribute( string alias ) 
		{
			m_Alias = alias;
		}
		#endregion
	}

}
