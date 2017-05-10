using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
	public class SetSourceData : AClipEditor
	{
		public SetSourceData ()
		{
			Name = "SetSourceData";
			Description = @"Useful for UDF's for setting the starting SourceData (clipboard content)";
			exampleInput = "output text";
			exampleCommand = "SetSourceData \"output text\"";
			exampleOutput = "output text";
			DefineParameters();
		}

		#region implemented abstract members of AClipEditor

		public override void Edit ()
		{
			SourceData = _parameterList[0].Value;
		}

		public override void DefineParameters ()
		{
			_parameterList = new List<Parameter>();
			_parameterList.Add(new Parameter()
			{
				ParameterName = "Text",
				Sequence = 1,
				Validator = (a => true),
				DefaultValue = "",
				Required = true,
				Expecting = "any text"
			});
		}

		#endregion
	}
}

