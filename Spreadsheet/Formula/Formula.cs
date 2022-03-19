
///Author: Devin white
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Author Devin White
/// </summary>
namespace SpreadsheetUtilities
{
	/// <summary>
	/// Represents formulas written in standard infix notation using standard precedence
	/// rules.  The allowed symbols are non-negative numbers written using double-precision 
	/// floating-point syntax (without unary preceeding '-' or '+'); 
	/// variables that consist of a letter or underscore followed by 
	/// zero or more letters, underscores, or digits; parentheses; and the four operator 
	/// symbols +, -, *, and /.  
	/// 
	/// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
	/// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
	/// and "x 23" consists of a variable "x" and a number "23".
	/// 
	/// Associated with every formula are two delegates:  a normalizer and a validator.  The
	/// normalizer is used to convert variables into a canonical form, and the validator is used
	/// to add extra restrictions on the validity of a variable (beyond the standard requirement 
	/// that it consist of a letter or underscore followed by zero or more letters, underscores,
	/// or digits.)  Their use is described in detail in the constructor and method comments.
	/// </summary>
	public class Formula
	{
		private static Stack<double> valueStack; //Value stack
		private static Stack<string> operatorStack; //Operator Stacl
		private readonly string mainFormula;
		private String normalizedString;
		private List<string> normalizedVariables;

		/// <summary>
		/// Creates a Formula from a string that consists of an infix expression written as
		/// described in the class comment.  If the expression is syntactically invalid,
		/// throws a FormulaFormatException with an explanatory Message.
		/// 
		/// The associated normalizer is the identity function, and the associated validator
		/// maps every string to true.  
		/// </summary>
		public Formula(String formula) :
			this(formula, s => s, s => true)
		{

			normalizedString = (CheckValidFormula(formula, s => s, s => true));
			normalizedVariables = (getNormalizedVariables(normalizedString));
			mainFormula = normalizedString;
		}

		/// <summary>
		/// Creates a Formula from a string that consists of an infix expression written as
		/// described in the class comment.  If the expression is syntactically incorrect,
		/// throws a FormulaFormatException with an explanatory Message.
		/// 
		/// The associated normalizer and validator are the second and third parameters,
		/// respectively.  
		/// 
		/// If the formula contains a variable v such that normalize(v) is not a legal variable, 
		/// throws a FormulaFormatException with an explanatory message. 
		/// 
		/// If the formula contains a variable v such that isValid(normalize(v)) is false,
		/// throws a FormulaFormatException with an explanatory message.
		/// 
		/// Suppose that N is a method that converts all the letters in a string to upper case, and
		/// that V is a method that returns true only if a string consists of one letter followed
		/// by one digit.  Then:
		/// 
		/// new Formula("x2+y3", N, V) should succeed
		/// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
		/// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
		/// </summary>
		public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
		{
			normalizedString = (CheckValidFormula(formula, normalize, isValid));
			normalizedVariables = (getNormalizedVariables(normalizedString));
			mainFormula = normalizedString;
		}

		/// <summary>
		/// Evaluates this Formula, using the lookup delegate to determine the values of
		/// variables.  When a variable symbol v needs to be determined, it should be looked up
		/// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
		/// the constructor.)
		/// 
		/// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
		/// in a string to upper case:
		/// 
		/// new Formula("x+7", N, s => true).Evaluate(L) is 11
		/// new Formula("x+7").Evaluate(L) is 9
		/// 
		/// Given a variable symbol as its parameter, lookup returns the variable's value 
		/// (if it has one) or throws an ArgumentException (otherwise).
		/// 
		/// If no undefined variables or divisions by zero are encountered when evaluating 
		/// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
		/// The Reason property of the FormulaError should have a meaningful explanation.
		///
		/// This method should never throw an exception.
		/// </summary>
		public object Evaluate(Func<string, double> lookup)
		{
			try
			{
				valueStack = new Stack<double>();
				operatorStack = new Stack<string>();
				valueStack.Clear();
				operatorStack.Clear();
				String operatorVal;
				foreach (string token in GetTokens(mainFormula))
				{

					switch (token) ///Evaluates the token and either pushes to stack or performs operations
					{
						case "(":
						case "*":
						case "/":
							operatorStack.Push(token);
							break;

						case "+":
						case "-":
							EvaluateOperation(token);
							break;

						case ")":
							EvaluateParenthesis();
							break;
					}

					if (double.TryParse(token, out double value)) //if t is an integer
					{
						if (operatorStack.Count > 0 && operatorStack.Peek() == "/")
						{
							if (value == 0)
							{
								return new FormulaError("Can't divide by zero");
							}
						}
						CalculateNumber(value);
					} //if token is integer

					else if (isVariable(token))
					{
						double variable = lookup(token);
						if (operatorStack.Count > 0 && operatorStack.Peek() == "/")
						{
							if (variable == 0)
							{
								return new FormulaError("Can't divide by zero");
							}
						}
						CalculateNumber(variable);
					}


				}
				///When The final token is processed....
				if (double.IsInfinity(valueStack.Peek()))
				{
					return new FormulaError("can't divide by zero");
				}
				else if (operatorStack.Count == 0 && valueStack.Count == 1)
				{
					return valueStack.Pop();
				}
				else
				{
					operatorVal = operatorStack.Pop();
					double val = valueStack.Pop();
					if (operatorVal == "+")
					{
						return (valueStack.Pop() + val);
					}
					else
					{
						return (valueStack.Pop() - val);
					}
				}
			}
			catch (ArgumentException) ///if lookup throws an Argument Exception due to invalid Variable, catches then throws FormulaError instead
			{
				return new FormulaError("Variable Invalid. Lookup doesn't return a value for the given variable");
			}

		}


		/// <summary>
		/// Enumerates the normalized versions of all of the variables that occur in this 
		/// formula.  No normalization may appear more than once in the enumeration, even 
		/// if it appears more than once in this Formula.
		/// 
		/// For example, if N is a method that converts all the letters in a string to upper case:
		/// 
		/// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
		/// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
		/// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
		/// </summary>
		public IEnumerable<String> GetVariables()
		{
			foreach (string variable in normalizedVariables)
			{
				yield return variable;
			}
		}

		/// <summary>
		/// Returns a string containing no spaces which, if passed to the Formula
		/// constructor, will produce a Formula f such that this.Equals(f).  All of the
		/// variables in the string should be normalized.
		/// 
		/// For example, if N is a method that converts all the letters in a string to upper case:
		/// 
		/// new Formula("x + y", N, s => true).ToString() should return "X+Y"
		/// new Formula("x + Y").ToString() should return "x+Y"
		/// </summary>
		public override string ToString()
		{
			return normalizedString;
		}

		/// <summary>
		/// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
		/// whether or not this Formula and obj are equal.
		/// 
		/// Two Formulae are considered equal if they consist of the same tokens in the
		/// same order.  To determine token equality, all tokens are compared as strings 
		/// except for numeric tokens and variable tokens.
		/// Numeric tokens are considered equal if they are equal after being "normalized" 
		/// by C#'s standard conversion from string to double, then back to string. This 
		/// eliminates any inconsistencies due to limited floating point precision.
		/// Variable tokens are considered equal if their normalized forms are equal, as 
		/// defined by the provided normalizer.
		/// 
		/// For example, if N is a method that converts all the letters in a string to upper case:
		///  
		/// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
		/// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
		/// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
		/// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
		/// </summary>
		public override bool Equals(object obj)
		{
			if (!(obj is Formula) || obj is null)
			{
				return false;
			}
			else return obj.ToString() == normalizedString;

		}

		/// <summary>
		/// Reports whether f1 == f2, using the notion of equality from the Equals method.
		/// Note that if both f1 and f2 are null, this method should return true.  If one is
		/// null and one is not, this method should return false.
		/// </summary>
		public static bool operator ==(Formula f1, Formula f2)
		{
			if (ReferenceEquals(f1, null))
			{
				return ReferenceEquals(f2, null);
			}
			else return f1.Equals(f2);
		}

		/// <summary>
		/// Reports whether f1 != f2, using the notion of equality from the Equals method.
		/// Note that if both f1 and f2 are null, this method should return false.  If one is
		/// null and one is not, this method should return true.
		/// </summary>
		public static bool operator !=(Formula f1, Formula f2)
		{
			if (ReferenceEquals(f1, null))
			{
				return !ReferenceEquals(f2, null);
			}
			else return !f1.Equals(f2);

		}

		/// <summary>
		/// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
		/// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
		/// randomly-generated unequal Formulae have the same hash code should be extremely small.
		/// </summary>
		public override int GetHashCode()
		{
			return mainFormula.GetHashCode();
		}

		/// <summary>
		/// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
		/// right paren; one of the four operator symbols; a string consisting of a letter or underscore
		/// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
		/// match one of those patterns.  There are no empty tokens, and no token contains white space.
		/// </summary>
		private static IEnumerable<string> GetTokens(String formula)
		{
			// Patterns for individual tokens
			String lpPattern = @"\(";
			String rpPattern = @"\)";
			String opPattern = @"[\+\-*/]";
			String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
			String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
			String spacePattern = @"\s+";

			// Overall pattern
			String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
											lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

			// Enumerate matching tokens that don't consist solely of white space.
			foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
			{
				if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
				{
					yield return s;
				}
			}

		}
		/// <summary>
		/// Checks if tokens in a formula are valid
		/// </summary>
		/// <param name="formula">The formula to grab tokens from</param>
		/// <param name="normalize">Normalizer for variables</param>
		/// <param name="isValid">Checks whether Variable is valid</param>
		/// <returns>True if token is valid or False if it is not</returns>
		private static string CheckValidFormula(string formula, Func<string, string> normalize, Func<string, bool> isValid)//checks if token is valid, if not, throws exception
		{
			int tokenSize = countTokens(formula); //Checks for first and last token
			int counter = 0;
			string previousToken = " "; //Keeps track of the previous token for errors
			int openingparenCount = 0; //a count of opening parens seen thus far
			int closingparencount = 0; ///a count of closing parens seen thus far
			StringBuilder normalizedFormula = new StringBuilder();
			foreach (string token in GetTokens(formula))
			{

				counter++; // Counter starts at beginning to match countTokens
				if (!isVariable(token))
				{

					normalizedFormula.Append(getValue(token));
				}
				else if (isValidVariable(token, normalize, isValid))
				{
					normalizedFormula.Append(normalize(token));
				}

				if (token == "(")
				{
					openingparenCount++;
				}
				else if (token == ")")
				{
					closingparencount++;
				}
				if (isValidVariable(token, normalize, isValid))
				{
					if (isValue(previousToken))
					{
						throw new FormulaFormatException("Syntactically incorrect variable.");
					}
				}
				//checks if token string is an int value
				//Checks if token is a variable of Letters followed by numbers
				//e.g AABB6 or A6
				/*Starting Token Rule
				The first token of an expression must be a number, 
				a variable, or an opening parenthesis.*/
				if (counter == 1)
				{
					if (token == "(" || isValue(token) ||
					isValidVariable(token, normalize, isValid))
					{
						previousToken = token;
					}
					else
					{
						throw new FormulaFormatException("The first token of an expression must be a number, a variable, or an opening parenthesis");
					}
				}

				/*Right Parentheses Rule
				When reading tokens from left to right, 
				at no point should the number of 
				closing parentheses seen so far 
				be greater than the number of opening parentheses 
				seen so far.*/
				else if (closingparencount > openingparenCount)
				{
					throw new FormulaFormatException("More right parenthesis than left parenthesis");
				}
				/*Parenthesis/Operator Following Rule
				Any token that immediately follows an 
				opening parenthesis or an operator must be either a number, 
				a variable, or an opening parenthesis.*/
				else if (isOperator(previousToken) || previousToken == "(")
				{
					if (token == "(" || isValue(token)
						|| isValidVariable(token, normalize, isValid))
					{
						previousToken = token;
					}
					else
					{
						throw new FormulaFormatException("Any token that immediately follows an opening parenthesis or an operator must be either a number, a variable, or an opening parenthesis.");
					}
				}

				/*Extra Following Rule
				Any token that immediately follows a number, 
				a variable, or a closing parenthesis 
				must be either an operator or a closing parenthesis.*/
				else if (previousToken == ")" ||
					isValue(previousToken) ||
					isValidVariable(previousToken, normalize, isValid))
				{
					if (isOperator(token) || token == ")")
					{
						previousToken = token;
					}
					else
					{
						throw new FormulaFormatException("Any token that immediately follows a number, a variable, or a closing parenthesis must be either an operator or a closing parenthesis.");
					}
				}

				/*Ending Token Rule
				The last token of an expression must be a number, 
				a variable, or a closing parenthesis.*/
				if (counter == tokenSize)
				{
					if (openingparenCount != closingparencount)
					{
						throw new FormulaFormatException("Mismatched Parenthesis");
					}
					if (token == ")" ||
					isValue(token) || isValidVariable(token, normalize, isValid))
					{

						return normalizedFormula.ToString(); ;
					}
					/*Balanced Parentheses Rule
					The total number of opening parentheses must 
					equal the total number of closing parentheses.*/
					else
					{
						throw new FormulaFormatException("The last token of an expression must be a number, a variable, or a closing parenthesis");
					}
				}
			}

			throw new FormulaFormatException("There must be at least one token");
		}

		/// <summary>
		/// Checks whether token is a variable
		/// </summary>
		/// <param name="token">The token to be checked</param>
		/// <param name="normalize">The normalizer</param>
		/// <param name="isValid">Checks whether token is valid</param>
		/// <returns>True if token is variable or False if it is not</returns>
		private static bool isValidVariable(string token, Func<string, string> normalize, Func<string, bool> isValid)
		{
			if (isVariable(token))
			{
				string normalizedvar = normalize(token);
				if (isValid(normalizedvar))
				{
					return true;
				}
				else throw new FormulaFormatException("Invalid Variable");
			}
			else return false;
		}
		/// <summary>
		/// Checks if the token is a variable
		/// </summary>
		/// <param name="token">Token being evaluated</param>
		/// <returns>True if token is a variable, False if not</returns>
		private static bool isVariable(string token)
		{
			Regex varPattern = new Regex(@"^[a-zA-Z_](?: [a-zA-Z_]|\d)*");
			if (varPattern.IsMatch(token))
			{
				return true;
			}
			else return false;
		}
		/// <summary>
		/// Collects all the Normalized Variables from the Normalized String
		/// </summary>
		/// <param name="formula">The formula to gather the normalized Variables from</param>
		/// <returns></returns>
		private List<string> getNormalizedVariables(string formula)
		{
			List<string> normalizedVariables = new List<string>();
			foreach (string variable in GetTokens(formula))
			{

				if (isVariable(variable))
				{
					if (!normalizedVariables.Contains(variable))
					{
						normalizedVariables.Add(variable);
					}
				}
			}
			return normalizedVariables;
		}
		/// <summary>
		/// Checks whether the provided token is an operator, +, -, *, /
		/// </summary>
		/// <param name="token">The token to be checked</param>
		/// <returns>True if token is Operator or False if it is not</returns>
		private static bool isOperator(string token)
		{
			if (token == "+" || token == "-" || token == "*"
						|| token == "/")
			{
				return true;
			}
			else return false;
		}
		/// <summary>
		/// Checks if the provided token is a double value
		/// </summary>
		/// <param name="token">The token to be checked</param>
		/// <returns>True if token is a double or False if it is not</returns>
		private static bool isValue(string token)
		{
			if (double.TryParse(token, out double value))
			{
				return true;
			}
			else return false;
		}
		/// <summary>
		/// Gets the value of a double, trims unnecessary decimal places, then converts back to a string
		/// </summary>
		/// <param name="token">token to get the value from</param>
		/// <returns>Standardized string of the double</returns>
		private static string getValue(string token)
		{
			if (double.TryParse(token, out double value))
			{
				return value.ToString();
			}
			else return token;
		}
		/// <summary>
		/// Evaluates operation if the token is a closing parenthesis ")"
		/// performs order of operations, calculates, and pushes onto the value stack
		/// </summary>
		private static void EvaluateParenthesis()
		{
			if (valueStack.Count > 1)
			{
				string oper = operatorStack.Pop();
				if (oper == "+")
				{
					double vals = valueStack.Pop();
					valueStack.Push(valueStack.Pop() + vals);
				}
				else if (oper == "-")
				{
					double vals = valueStack.Pop();
					valueStack.Push(valueStack.Pop() - vals);

				}
				else operatorStack.Push(oper);
			}

			if (operatorStack.Peek() == "(")
			{
				operatorStack.Pop();
			}

			if (operatorStack.Count > 0 && valueStack.Count > 1)
			{
				if (operatorStack.Peek() == "*")
				{
					operatorStack.Pop();
					double vals = valueStack.Pop();
					valueStack.Push(valueStack.Pop() * vals);
				}
				else if (operatorStack.Peek() == "/")
				{
					operatorStack.Pop();
					double vals = valueStack.Pop();
					valueStack.Push(valueStack.Pop() / vals);
				}
			}

		} //if token = ")" evaluates items inside parenthesis
		/// <summary>
		/// Performs calculation if the token has a double value
		/// </summary>
		/// <param name="value">The value of the token</param>
		private static void CalculateNumber(double value)
		{
			if (valueStack.Count == 0)
			{
				valueStack.Push(value);
			}
			else if (valueStack.Count > 0)
			{
				if (operatorStack.Peek() == "*")
				{
					operatorStack.Pop();
					double vals = valueStack.Pop();
					valueStack.Push(vals * value);
				}

				else if (operatorStack.Peek() == "/")
				{
					operatorStack.Pop();
					double vals = valueStack.Pop();
					valueStack.Push(vals / value);
				}
				else valueStack.Push(value);
			}
		}

		/// <summary>
		/// Evaluates the Operator
		/// </summary>
		/// <param name="token">The token to perform operations on</param>
		private static void EvaluateOperation(string token)
		{
			if (operatorStack.Count > 0 && valueStack.Count > 1)
			{
				if (operatorStack.Peek() == "+" || operatorStack.Peek() == "-")
				{
					string oper = operatorStack.Pop();
					double vals = valueStack.Pop();
					switch (oper)
					{
						case "+":
							valueStack.Push(valueStack.Pop() + vals);
							operatorStack.Push(token);
							break;

						case "-":
							valueStack.Push(valueStack.Pop() - vals);
							operatorStack.Push(token);
							break;

					}
				}
				else operatorStack.Push(token);
			}
			else operatorStack.Push(token);
		} // evaluates + or - operations
		/// <summary>
		/// Counts the number of valid tokens within the given formula
		/// </summary>
		/// <param name="formula">Formula to count Tokens from</param>
		/// <returns>Returns the number of valid tokens within the given formula</returns>
		private static int countTokens(string formula)
		{
			List<string> counterList = new List<string>();
			foreach (string token in GetTokens(formula))
			{
				counterList.Add(token);
			}
			return counterList.Count;
		}
	}

	/// <summary>
	/// Used to report syntactic errors in the argument to the Formula constructor.
	/// </summary>
	public class FormulaFormatException : Exception
	{
		/// <summary>
		/// Constructs a FormulaFormatException containing the explanatory message.
		/// </summary>
		public FormulaFormatException(String message)
			: base(message)
		{
		}
	}

	/// <summary>
	/// Used as a possible return value of the Formula.Evaluate method.
	/// </summary>
	public struct FormulaError
	{
		/// <summary>
		/// Constructs a FormulaError containing the explanatory reason.
		/// </summary>
		/// <param name="reason">The Reason for the Error</param>
		public FormulaError(String reason)
			: this()
		{
			Reason = reason;
		}

		/// <summary>
		///  The reason why this FormulaError was created.
		/// </summary>
		public string Reason { get; private set; }
	}
}
