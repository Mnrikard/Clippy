using System.Collections.Generic;
using System.Xml.Linq;

namespace ClippyLib
{
	public class UserFunction
	{
		/*<command key="cleaninsert">
				<description>inserts clean</description>
					<function><![CDATA[insert %0%]]></function>
					<parameter name="Tablename" default="xxx" required="True" parmdesc="" sequence="1" />
					</command>
			*/
		public UserFunction (XElement commandNode)
		{
			Name = commandNode.Attribute("key").Value;
			Description = commandNode.Element("description") == null ? string.Empty : commandNode.Element("description").Value;
			SubFunctions = new Queue<string>();

			foreach(XElement function in commandNode.Elements("function"))
			{
				SubFunctions.Enqueue(function.Value);
			}

			Parameters = new List<UserParameter>();
			foreach(XElement parameterNode in commandNode.Elements("parameter"))
			{
				Parameters.Add(new UserParameter(parameterNode));
			}
		}

		public UserFunction(string name, string description, string subfuncs, List<UserParameter> parameters)
		{
			Name = name;
			Description = description;
			Parameters = parameters;
			SubFunctions = new Queue<string>();

			foreach(string subfunction in subfuncs.Split('\n'))
			{
				SubFunctions.Enqueue(subfunction.Trim());
			}
		}

		public string Name{get; set;}
		public string Description {get; set;}
		public Queue<string> SubFunctions {get; set;}
		public List<UserParameter> Parameters {get; set;}

		public class UserParameter
		{
			public UserParameter(XElement parameterNode)
			{
				Name = parameterNode.Attribute("name").Value;
				DefaultValue = parameterNode.Attribute("default") == null ? string.Empty : parameterNode.Attribute("default").Value;
				Description = parameterNode.Attribute("parmdesc") == null ? string.Empty : parameterNode.Attribute("parmdesc").Value;

				if(parameterNode.Attribute("required") != null)
					Required = bool.Parse(parameterNode.Attribute("required").Value);
				if(parameterNode.Attribute("sequence") != null)
					Sequence = int.Parse(parameterNode.Attribute("sequence").Value);
			}

			public UserParameter(string name, string defaultValue, bool required, string description, int sequence)
			{
				Name = name;
				DefaultValue = defaultValue;
				Required = required;
				Description = description;
				Sequence = sequence;
			}

			public string Name {get;set;}
			public string DefaultValue {get;set;}
			public bool Required {get;set;}
			public string Description {get;set;}
			public int Sequence {get;set;}
		}

	}
}

