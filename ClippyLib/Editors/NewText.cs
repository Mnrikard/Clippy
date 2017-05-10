/*
 * Copyright 2013 Matthew Rikard
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
 * User: Matthew
 * Date: 5/28/2013
 *
 */
using System;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    public class NewText : AClipEditor
    {
		public NewText()
		{
			Name = "NewText";
			Description = @"Generates new text for Guids or Current dates
ItemType:
	ID        Generates a new GUID
	Date      Generates the current date as yyyy-MM-dd
	Time      Generates the current time as HH:mm:ss (24h)
	time      Generates the current time as hh:mm:ss [ap]m (12h)
	dT        Generates the current date/time as yyyy-MM-dd HH:mm:ss
	dt        Generates the current date/time as yyyy-M-d h:mm:ss [ap]m";
			exampleInput = "doesn't matter";
			exampleCommand = "newtext id";
			exampleOutput = "A new GUID";
			DefineParameters();
		}

		public override string ShortDescription { get { return "Generates new text for Guids or Current dates"; } }

		public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "ItemType",
                Sequence = 1,
                Validator = (a => Regex.IsMatch(a, @"^\s*(id|date|time|dt|/\.)\s*$", RegexOptions.IgnoreCase)),
                DefaultValue = "ID",
                Required = false,
                Expecting = "either ID, Date, Time, time, dT, or dt"
            });
        }

        public override void Edit()
        {
        	string itemType = _parameterList[0].Value;
        	switch(itemType.ToLower().Trim())
        	{
        		case "id":
        			SourceData = Guid.NewGuid().ToString().ToUpper();
        			break;
        		case "date":
        			SourceData = DateTime.Now.ToString("yyyy-MM-dd");
        			break;
        		case "time":
        			if(itemType.Trim()[0] == 'T')
        			{
        				SourceData = DateTime.Now.ToString("HH:mm:ss");
        			}
        			else
        			{
        				SourceData = DateTime.Now.ToString("hh:mm:ss tt");
        			}
        			break;
        		case "dt":
        			if(itemType.Trim()[1] == 'T')
					{
						SourceData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        			}
        			else
					{
						SourceData = DateTime.Now.ToString("yyyy-M-d h:mm:ss tt");
        			}
        			break;
        		case "/.":
        			SourceData = GetRandomSlashDot();
        			break;
        		default:
        			throw new Exception("Item Type: " + itemType + " not found");
        	}      	
        	
        }    

        private string GetRandomSlashDot()
        {
        	XmlDocument sdoc = new XmlDocument();
        	sdoc.Load("http://rss.slashdot.org/Slashdot/slashdot");

        	XmlNodeList items = sdoc.SelectNodes("//item");
        	Random r = new Random();
        	int which = r.Next(items.Count);
        	
        	return String.Format("{0}\r\n\r\n{1}", items[which].SelectSingleNode("title").InnerText, items[which].SelectSingleNode("description").InnerText);
        }
        
    }
}
