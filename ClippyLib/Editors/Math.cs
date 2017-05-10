using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClippyLib
{
	public class Math : AClipEditor
	{
		public Math()
		{
			Name = "Math";
			Description = "Solves simple math problems";
			exampleInput = "123+456";
			exampleCommand = "math";
			exampleOutput = "123+456=579";
			DefineParameters();

			string decimalPattern = @"(?!=\d\s*)\-?\d+(\.\d+)?";
			string rparen = "\\)";
			string lparen = "\\(";
			_insideParens = new Regex(String.Concat(lparen,"(?<insides>[^",lparen,rparen,"]+)",rparen));
			_sumUpList = new Regex(String.Concat("(?<top>",decimalPattern,")\\s+(?<bottom>",decimalPattern,")"));
		}

		private readonly Regex _insideParens;
		private readonly Regex _sumUpList;


		public override void DefineParameters()
		{
			_parameterList = new List<Parameter>();
			_parameterList.Add(new Parameter()
			                   {
				ParameterName = "Answer only",
				Sequence = 1,
				Validator = a => (a.StartsWith("answer", StringComparison.CurrentCultureIgnoreCase) || String.IsNullOrEmpty(a)),
				Expecting = "Empty or \"answer\" for answer only.",
				Required=false,
				DefaultValue=""
			});
		}

		public override void Edit()
		{
			string expression = _sumUpList.Replace(SourceData, "${top}+${bottom}");
			expression = _sumUpList.Replace(expression, "${top}+${bottom}");
			try
			{
				string answer = EvaluateExpression(expression);

				if(ParameterList[0].GetValueOrDefault().Equals("answer", StringComparison.CurrentCultureIgnoreCase))
					SourceData = answer;
				else if(SourceData.Contains("\n"))
					SourceData = String.Concat(SourceData,"\n",answer);
				else if(SourceData.Contains("="))
					SourceData = String.Concat(SourceData,answer);
				else
					SourceData = String.Concat(SourceData,"=",answer);
			}
			catch(Exception mathErr)
			{
				RespondToExe("Error: " + mathErr.Message);
			}
		}

		private string EvaluateExpression(string math)
		{
			string originalMath = math.Trim(' ','\t','=','\n','\r');
			math = originalMath;

			if(IsNullOrNumber(math))
			{
				return math;
			}

			Match paren = _insideParens.Match(math);
			while(paren.Success)
			{
				string insideMath = paren.Groups["insides"].Value;
				math = math.Replace(paren.Value, EvaluateExpression(insideMath));
				paren = _insideParens.Match(math);
			}

			math = (new Exponent()).EvaluatePattern(math);
			math = (new MultiplyDivide()).EvaluatePattern(math);
			math = (new Modulo()).EvaluatePattern(math);
			math = (new DivideWithRemainder()).EvaluatePattern(math);
			math = (new AddSubtract()).EvaluatePattern(math);

			if(math == originalMath)
			{
				throw new Exception(String.Concat("Could not parse ",math));
			}

			return EvaluateExpression(math);
		}




		private abstract class Operation
		{
			protected Regex EquationPattern;
			protected string decimalPattern = @"(?!=\d\s*)\-?\d+(\.\d+)?";
			protected string rparen = "\\)";
			protected string lparen = "\\(";

			protected abstract string Perform(string equation);

			public string EvaluatePattern(string math)
			{
				Match expr = EquationPattern.Match(math);
				while(expr.Success)
				{
					string answer = Perform(expr.Value);
					math = math.Replace(expr.Value, answer);
					expr = EquationPattern.Match(math);
				}

				return math;
			}

			protected decimal ParseDecimal(string math)
			{
				decimal evaled;
				if(!decimal.TryParse(math, out evaled))
				{
					string failureMessage = String.Concat("Cannot evaluate " + math + " to a decimal");
					throw new Exception(failureMessage);
				}
				return evaled;
			}
		}

		private class AddSubtract : Operation
		{
			public AddSubtract()
			{
				EquationPattern = new Regex(String.Concat("(?<left>",decimalPattern,")\\s*(?<oper>[\\+\\-])\\s*(?<right>",decimalPattern,")"));
			}

			protected override string Perform(string equation)
			{
				Match expr = EquationPattern.Match(equation);
				Decimal left = ParseDecimal(expr.Groups["left"].Value);
				Decimal right = ParseDecimal(expr.Groups["right"].Value);
				string oper = expr.Groups["oper"].Value;

				if(oper.Contains("+"))
					return (left+right).ToString();

				return (left-right).ToString();
			}

		}

		private class MultiplyDivide : Operation
		{
			public MultiplyDivide()
			{
				EquationPattern = new Regex(String.Concat("(?<left>",decimalPattern,")\\s*(?<oper>[\\*\\/])\\s*(?<right>",decimalPattern,")"));
			}

			protected override string Perform(string equation)
			{
				Match expr = EquationPattern.Match(equation);
				Decimal left = ParseDecimal(expr.Groups["left"].Value);
				Decimal right = ParseDecimal(expr.Groups["right"].Value);
				string oper = expr.Groups["oper"].Value;

				if(oper.Contains("*"))
					return (left * right).ToString();

				return (left / right).ToString();
			}
		}

		private class Exponent : Operation
		{
			public Exponent()
			{
				EquationPattern = new Regex(String.Concat("(?<left>",decimalPattern,")\\s*(?<oper>\\^)\\s*(?<right>",decimalPattern,")"));
			}

			protected override string Perform(string equation)
			{
				Match expr = EquationPattern.Match(equation);
				Decimal left = ParseDecimal(expr.Groups["left"].Value);
				Decimal right = ParseDecimal(expr.Groups["right"].Value);

				return System.Math.Pow((double)left,(double)right).ToString();
			}
		}

		private class Modulo : Operation
		{
			public Modulo()
			{
				EquationPattern = new Regex(String.Concat("(?<left>",decimalPattern,")\\s*(?<oper>\\%)\\s*(?<right>",decimalPattern,")"));
			}

			protected override string Perform(string equation)
			{
				Match expr = EquationPattern.Match(equation);
				Decimal left = ParseDecimal(expr.Groups["left"].Value);
				Decimal right = ParseDecimal(expr.Groups["right"].Value);

				return (left % right).ToString();
			}
		}

		
		private class DivideWithRemainder : Operation
		{
			public DivideWithRemainder()
			{
				EquationPattern = new Regex(String.Concat("(?<left>",decimalPattern,")\\s*(?<oper>\\/\\%)\\s*(?<right>",decimalPattern,")"));
			}

			protected override string Perform(string equation)
			{
				Match expr = EquationPattern.Match(equation);
				Decimal left = ParseDecimal(expr.Groups["left"].Value);
				Decimal right = ParseDecimal(expr.Groups["right"].Value);

				return String.Concat(System.Math.Floor(left/right),",",(left % right)).ToString();
			}
		}


	}
}

