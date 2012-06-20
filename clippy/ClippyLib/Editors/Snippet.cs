using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Win32;

namespace ClippyLib.Editors
{
    public class Snippet : AClipEditor
    {
        #region boilerplate

        public override string EditorName
        {
            get { return "Snippet"; }
        }

        public override string ShortDescription
        {
            get { return "Returns a string of text defined in the snippet.xml file."; }
        }

        public override string LongDescription
        {
            get
            {
                return @"Snippet
Syntax: clippy snippet [snippetName]
Description

returns the snippet of code/text defined in the snippet.xml file

snippetName - the Name node of the snippet desired
If left out, will display all snippets and their description and then request
you to choose.
";
            }
        }

        private static XmlDocument SnippetsXml()
        {
            RegistryKey hkcu = Registry.CurrentUser;
            RegistryKey rkUdfLocation = hkcu.OpenSubKey("Software\\Rikard\\Clippy", false);
            object snipLocation = rkUdfLocation.GetValue("snippetsLocation");
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(snipLocation.ToString());
            return xdoc;
        }

        private List<string> _snippetNames = null;
        private List<string> SnippetNames
        {
            get
            {
                if (_snippetNames == null)
                {
                    if (_snippets == null)
                    {
                        _snippets = SnippetsXml();
                    }
                    XmlNodeList snames = _snippets.SelectNodes("//Snippet/@Name");
                    _snippetNames = new List<string>();
                    foreach (XmlNode snameNode in snames)
                    {
                        _snippetNames.Add(snameNode.InnerText);
                    }
                }
                return _snippetNames;
            }
        }

        private bool IsSnippet(string sname)
        {
            foreach (string snippetname in SnippetNames)
            {
                if(snippetname.Equals(sname,StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        private XmlDocument _snippets = null;

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Snippet to use",
                Sequence = 1,
                Validator = (a => true),
                DefaultValue = "",
                Required = true,
                Expecting = "A snippet Name defined in snippet.xml"
            });
        }

        #endregion

        //you don't need to override this
        public override void SetParameters(string[] args)
        {
            if (args.Length > 1)
            {
                SetParameter(1, args[1]);
            }
            else
            {
                RespondToExe(DisplaySnippetsToChoose(),false);
            }
        }

        private string DisplaySnippetsToChoose()
        {
            System.Text.StringBuilder output = new System.Text.StringBuilder();
            if (_snippets == null)
            {
                _snippets = SnippetsXml();
            }
            XmlNodeList snippets = _snippets.SelectNodes("/Snippets/Snippet");
            foreach (XmlNode snippet in snippets)
            {
                output.AppendFormat("{0} - {1}\r\n",
                    GetTextFromNode(snippet, "@Name", String.Empty),
                    GetTextFromNode(snippet, "Description", String.Empty)
                );
            }
            return output.ToString();
        }

        private string GetTextFromNode(XmlNode nd, string xpath, string defaultValue)
        {
            XmlNode innerNode = nd.SelectSingleNode(xpath);
            if (innerNode == null)
                return defaultValue;
            return innerNode.InnerText;
        }

        public override void Edit()
        {
            if (_snippets == null)
            {
                _snippets = SnippetsXml();
            }
            XmlNode snippet = _snippets.SelectSingleNode("/Snippets/Snippet[translate(@Name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"" + ParameterList[0].Value.ToLower() + "\"]/Content");
            if (snippet == null)
                RespondToExe(String.Format("Snippet requested ({0}) not found", ParameterList[0].Value));
            else
                SourceData = snippet.InnerText;
        }       
        
    }
}
