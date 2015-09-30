using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    public class Template : AClipEditor
    {
		public Template()
		{
			Name = "";
			Description = "";
			exampleInput = "";
			exampleCommand = "";
			exampleOutput = "";
			DefineParameters();
		}

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "parm1",
                Sequence = 1,
                Validator = (a => true),
                DefaultValue = "\n",
                Required = false,
                Expecting = "a string delimiter"
            });
        }

        //you don't need to override this
        public override void SetParameters(string[] args)
        {
            for(int i=0;i<ParameterList.Count;i++)
                ParameterList[i].Value = ParameterList[i].DefaultValue;
            if (args.Length > 1)
            {
                ParameterList[0].Value = args[1];
                if (args.Length > 2)
                {
                    if (args[2].Equals("desc", StringComparison.CurrentCultureIgnoreCase))
                        ParameterList[1].Value = "desc";
                }
            }
        }

        public override void Edit()
        {
            //Set the SourceData variable here when complete
        }       
        
    }
}
