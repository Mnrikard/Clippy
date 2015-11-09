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
			_simpleMultDiv = new Regex(String.Concat("(?<left>",decimalPattern,")\\s*(?<oper>[\\*\\/])\\s*(?<right>",decimalPattern,")"));
			_simpleAddSubtract = new Regex(String.Concat("(?<left>",decimalPattern,")\\s*(?<oper>[\\+\\-])\\s*(?<right>",decimalPattern,")"));
			_sumUpList = new Regex(String.Concat("(?<top>",decimalPattern,")\\s+(?<bottom>",decimalPattern,")"));
		}

		private readonly Regex _insideParens;
		private readonly Regex _simpleMultDiv;
		private readonly Regex _simpleAddSubtract;
		private readonly Regex _sumUpList;


		public override void DefineParameters()
		{
			_parameterList = new List<Parameter>();
		}

		public override void Edit()
		{
			string expression = _sumUpList.Replace(SourceData, "${top}+${bottom}");
			expression = _sumUpList.Replace(expression, "${top}+${bottom}");
			try
			{
				string answer = EvaluateExpression(expression);

				if(SourceData.Contains("\n"))
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

			math = EvaluatePattern(math, _simpleMultDiv);
			math = EvaluatePattern(math, _simpleAddSubtract);

			if(math == originalMath)
			{
				throw new Exception(String.Concat("Could not parse ",math));
			}

			return EvaluateExpression(math);
		}

		private string EvaluatePattern(string math, Regex pattern)
		{
			Match expr = pattern.Match(math);
			while(expr.Success)
			{
				Func<decimal,decimal,decimal> operation = DetermineFunction(expr.Groups["oper"].Value);
				string answer = operation(ParseDecimal(expr.Groups["left"].Value), ParseDecimal(expr.Groups["right"].Value)).ToString();
				math = math.Replace(expr.Value, answer);
				expr = pattern.Match(math);
			}

			return math;
		}

		private Func<decimal,decimal,decimal> DetermineFunction(string oper)
		{
			switch(oper.Trim())
			{
				case "*":
					return Multiply;
				case "/":
					return Divide;
				case "+":
					return Add;
				case "-":
					return Subtract;
			}
			throw new Exception(String.Concat("Unknown operator: ", oper));
		}

		private decimal Add(decimal left, decimal right)
		{
			return left + right;
		}
		
		private decimal Subtract(decimal left, decimal right)
		{
			return left - right;
		}
		
		private decimal Multiply(decimal left, decimal right)
		{
			return left * right;
		}
		
		private decimal Divide(decimal left, decimal right)
		{
			return left / right;
		}

		
		private decimal ParseDecimal(string math)
		{
			decimal evaled;
			if(!decimal.TryParse(EvaluateExpression(math), out evaled))
			{
				string failureMessage = String.Concat("Cannot evaluate " + math + " to a decimel");
				RespondToExe(failureMessage);
				throw new Exception(failureMessage);
			}
			return evaled;
		}

	}
}

