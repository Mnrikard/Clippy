/*
 * 
 * Copyright 2012-2015 Matthew Rikard
 * This file is part of Clippy.
 * 
 *  Clippy is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  Clippy is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with Clippy.  If not, see <http://www.gnu.org/licenses/>.
 *
*/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    public class SqlUpdate : AClipEditor
    {
		public SqlUpdate()
		{
			Name = "Update";
			Description = "Converts a delimited string into a sql update statement";
			exampleInput = "column1\tcolumn2\tcolumn3\n1\ttext\t1900-1-1";
			exampleCommand = "update tablename column1";
			exampleOutput = "update tablename set column2='text', column3='1900-1-1' where column1=1";
			DefineParameters();
		}

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "TableName",
                Sequence = 1,
                Validator = (a => true),
                Required = true,
                DefaultValue = "xxxxxx",
                Expecting = "A table name"
            });
            _parameterList.Add(new Parameter()
            {
                ParameterName = "PrimaryKey",
                Sequence = 2,
                Validator = (a => true),
                Required = true,
                Expecting = "A column name"
            });
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Delimiter",
                Sequence = 3,
                Validator = (a => true),
                Required = false,
                DefaultValue="\t",
                Expecting = "A string delimiter"
            });
        }

        public override void SetParameters(string[] arguments)
        {
            base.SetParameters(arguments);
            if (!ParameterList[1].IsValued)
            {
                SetParameter(2, ParameterList[1].DefaultValue);
            }
        }

        public override void Edit()
        {
            string[] lines = SourceData.Split('\n');
            System.Text.StringBuilder output = new System.Text.StringBuilder();
            double currint = 0;
            string topper = String.Empty;
            int tapout = 1000;
            int rowcount = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string[] cols = Regex.Split(lines[i], Regex.Escape(ClipEscape(ParameterList[1].Value)), RegexOptions.IgnoreCase);
                if (i == 0)
                {
                    string tablename = ParameterList[0].Value.Replace(".", "].[").Replace("[[", "[").Replace("]]", "]");
                    topper = String.Format("insert into [{0}] ({1})\nvalues\n", tablename, String.Join(", ", cols));
                    rowcount = 0;
                }
                else
                {
                    if (++rowcount == tapout || i==1)
                    {
                        output.Append(topper);
                        output.Append(" (");
                        rowcount = 0;
                    }
                    else
                    {
                        output.Append(",(");
                    }

                    for (int j = 0; j < cols.Length; j++)
                    {
                        if (j > 0)
                        {
                            output.Append(", ");
                        }
                        if (Double.TryParse(cols[j], out currint) || cols[j] == "NULL")
                        {
                            output.Append(cols[j]);
                        }
                        else
                        {
                            output.Append("'" + cols[j].Replace("'", "''") + "'");
                        }
                    }
                    output.Append(")\n");
                }
            }
            SourceData = output.ToString();
        }       
        
    }
}
