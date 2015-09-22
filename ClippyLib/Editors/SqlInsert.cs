﻿/*
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
using System.Text;

namespace ClippyLib.Editors
{
    public class SqlInsert : AClipEditor
	{
		private string _insertStatement;
		private int _rowOfInsert;
		private int _maxRowsPerInsert = 1000;

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
			StringBuilder output = new StringBuilder();

			string[] lines = SourceData.Split('\n');
			string[] columnNames = GetColumns(lines[0]);
			DefineInsertStatement(columnNames);
			WriteInsertStatement(output);

            for (int i = 1; i < lines.Length; i++)
            {				               
                if (_rowOfInsert >= _maxRowsPerInsert)
                {
					WriteInsertStatement(output);
                }
                else if(i > 1)
                {
                    output.Append(",");
                }
				
				string[] cols = GetColumns(lines[i]);
                WriteSingleRow (output, cols);
            }

            SourceData = output.ToString();
        }

		private void WriteInsertStatement(StringBuilder writeTo)
		{
			writeTo.Append(_insertStatement);
			_rowOfInsert = 0;
		}

		private void DefineInsertStatement (string[] columnNames)
		{
			string tableName = ParameterList [0].Value.Replace (".", "].[").Replace ("[[", "[").Replace ("]]", "]");
			_insertStatement = String.Format ("insert into [{0}] ({1})\nvalues\n ", tableName, String.Join (", ", columnNames));
		}

		private string[] GetColumns(string line)
		{
			return Regex.Split(line, ParameterList[1].GetEscapedValue(), RegexOptions.IgnoreCase);
		}
        
		private void WriteSingleRow (StringBuilder output, string[] cols)
		{
			output.Append("(");
			for (int j = 0; j < cols.Length; j++) 
			{
				if (j > 0) 
				{
					output.Append(", ");
				}

				string apostropheOrNot = IsNullOrNumber(cols[j]) ? String.Empty : "'";
				output.AppendFormat("{0}{1}{0}", apostropheOrNot, cols[j].Replace("'", "''"));
			}
			output.Append(")\n");
			_rowOfInsert++;
		}

		private bool IsNullOrNumber(string column)
		{
			double numberTester;
			return (column == "NULL" || Double.TryParse(column, out numberTester));
		}
    }
}
