using System;
using System.Text.RegularExpressions;

namespace ClippyLib
{
    public class SuperRegex : Regex
    {
        #region ctor
        public SuperRegex(string pattern)
            : base(pattern)
        {
        }
        public SuperRegex(string pattern, RegexOptions options)
            : base(pattern, options)
        {
        }
        #endregion

        public string SuperReplace(string input, string replacement)
        {
            MatchEvaluator singleRep = new MatchEvaluator(SingleRepper);
            _baseRepString = replacement;
            return base.Replace(input, singleRep);
        }

        private Match _currentMatch;
        private string _currentRepString = string.Empty;
        private string _baseRepString = String.Empty;

        private string SingleRepper(Match m)
        {
            if (_baseRepString.Contains("\\u$") || _baseRepString.Contains("\\U$") || _baseRepString.Contains("\\l$") || _baseRepString.Contains("\\L$"))
            {
                MatchEvaluator repModUp = new MatchEvaluator(this.ReplacementModifierUp);
                MatchEvaluator repModDown = new MatchEvaluator(this.ReplacementModifierDown);
                MatchEvaluator repModNumUp = new MatchEvaluator(this.ReplacementModifierNumUp);
                MatchEvaluator repModNumDown = new MatchEvaluator(this.ReplacementModifierNumDown);
                _currentMatch = m;
                _currentRepString = Regex.Replace(_baseRepString, @"\\[Uu]\$\{(?<grpname>[^\}]+)\}", repModUp);
                _currentRepString = Regex.Replace(_currentRepString, @"\\[Ll]\$\{(?<grpname>[^\}]+)\}", repModDown);
                _currentRepString = Regex.Replace(_currentRepString, @"\\[Uu]\$(?<backtick>\d+)", repModNumUp);
                _currentRepString = Regex.Replace(_currentRepString, @"\\[Ll]\$(?<backtick>\d+)", repModNumDown);
            }

            return Regex.Replace(m.Value, base.pattern, _currentRepString, base.Options);
        }

        internal string ReplacementModifierUp(Match m)
        {
            return _currentMatch.Groups[m.Groups["grpname"].Value].Value.ToUpper();
        }

        internal string ReplacementModifierDown(Match m)
        {
            return _currentMatch.Groups[m.Groups["grpname"].Value].Value.ToLower();
        }

        internal string ReplacementModifierNumUp(Match m)
        {
            return _currentMatch.Groups[Int32.Parse(m.Groups["backtick"].Value)].Value.ToUpper();
        }

        internal string ReplacementModifierNumDown(Match m)
        {
            return _currentMatch.Groups[Int32.Parse(m.Groups["backtick"].Value)].Value.ToLower();
        }
    }

}
