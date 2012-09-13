using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    public class SqlInsert : AClipEditor
    {
        #region boilerplate

        public override string EditorName
        {
            get { return "Insert"; }
        }

        public override string ShortDescription
        {
            get { return "Converts a delimited string into a sql insert statement"; }
        }

        public override string LongDescription
        {
            get
            {
                return @"Insert
Syntax: clippy insert [tablename] [delimiter]
Converts a delimited string (can be multiple rows) into an Sql insert statement

tablename - The name of the table to which you are inserting

delimiter - The string delimiter between columns
Defaults to tab

Example:
    clippy insert ""PersonnelRecords""
    will take tab delimited result set from a grid (including column headers)
    and create an insert statement out of it to insert into the table
    called PersonnelRecords.
";
            }
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
                ParameterName = "Delimiter",
                Sequence = 2,
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

        #endregion


        public override void Edit()
        {
            string[] lines = SourceData.Split('\n');
            string top = String.Empty;
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
                    output.Append(topper);
                }
                else
                {
                    if (rowcount++ == tapout)
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
