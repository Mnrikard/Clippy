using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    public class ChunkText : AClipEditor
    {
        #region boilerplate

        public override string EditorName
        {
            get { return "Chunk"; }
        }

        public override string ShortDescription
        {
            get { return "Chunks text into n-sized chunks"; }
        }

        public override string LongDescription
        {
            get
            {
                return @"Chunk
Syntax: Chunk [numberOfChars] [separator]
Chunks text into n-sized chunks

numberOfChars - An integer representing characters per line

separator - The output separator between chunks
defaults to new line character

Example:
    clippy chunk 20 \n
    will separate the source data into 20 character chunks separated
    by a new line character
";
            }
        }

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Number of Characters",
                Sequence = 1,
                Validator = (a => a.IsInteger()),
                Required = true,
                Expecting = "An integer"
            });
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Separator",
                Sequence = 2,
                Validator = (a => true),
                DefaultValue = "\n",
                Required = false,
                Expecting = "A string separator"
            });
        }

        #endregion

        //you don't need to override this
        public override void SetParameters(string[] args)
        {
            for(int i=1;i<ParameterList.Count;i++)
                ParameterList[i].Value = ParameterList[i].DefaultValue;
            if (args.Length > 1)
            {
                SetParameter(1, args[1]);
                if (args.Length > 2)
                {
                    SetParameter(2, args[2]);
                }
            }
        }

        public override void Edit()
        {
            SourceData = Chunk(SourceData, Int32.Parse(ParameterList[0].Value), ClipEscape(ParameterList[1].Value));
        }

        public static string Chunk(string text, int nchar, string sep)
        {
            string[] chunkparts = new string[(int)Math.Ceiling(text.Length / (double)nchar)];
            for (var i = 0; i < chunkparts.Length; i++)
            {
                if (nchar * (i + 1) > text.Length)
                {
                    chunkparts[i] = text.Substring(nchar * i);
                }
                else
                {
                    chunkparts[i] = text.Substring(nchar * i, nchar);
                }
            }
            text = String.Join(sep, chunkparts);
            return text;

        }
        
    }
}
