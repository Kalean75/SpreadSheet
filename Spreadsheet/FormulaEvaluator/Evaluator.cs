using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{/// <summary>
/// Author: Devin White
/// </summary>
	public static class Evaluator
	{
		public delegate int Lookup(String v);
		private static Stack<int> valueStack = new Stack<int>();
		private static Stack<string> operatorStack = new Stack<string>();
		private static bool isVariable = false; // if token is a variable, flips to true

		/// <summary>
		/// Evaluates a given string, splits it into tokens. Performs operations in infx and returns a value
		/// </summary>
		/// <param name="exp">String to be split into tokens and evaluated</param>
		/// <param name="variableEvaluator">Function to search variables and return a value</param>
		/// <returns></returns>
		public static int Evaluate(String exp, Lookup variableEvaluator)
		{
			stackCleaner();
			string[] substrings = Regex.Split(exp.Trim(), "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
			String operatorVal;
			int counter = substrings.Length - 1;
			foreach (string token in substrings)
			{
				CheckvalidToken(token); //evaluates tokens and throws error if invalid
				switch (token)
				{
					case "(":
					case "*":
					case "/":
						operatorStack.Push(token);
						break;

					case "+":
					case "-":
						if (valueStack.Count == 0)
						{
							stackCleaner();
							throw new ArgumentException("Cannot have an operator with no integers");
						}
						EvaluateOperation(token);
						break;

					case ")":
						//if right parenthesis are mismatched(e.g  (4+4)), throws an error
						if (!operatorStack.Contains("("))
						{
							stackCleaner();
							throw new ArgumentException("Mismatched parenthesis");
						}
						EvaluateParenthesis();
						break;
				}

				if (int.TryParse(token, out int value)) //if t is an integer
				{
					CalculateNumber(value);
				} //if token is integer

				else if (isVariable) //if token is a variable
				{
					isVariable = false; //resets to false to check next token
					int variable = variableEvaluator(token);
					CalculateNumber(variable);
				}

				//Performs operation on final token
				if (counter == 0)
				{
					if (operatorStack.Count == 0 && valueStack.Count == 1)
					{
						return valueStack.Pop();
					}
					else if (valueStack.Count >= 2 && operatorStack.Count == 1)
					{
						operatorVal = operatorStack.Pop();
						int val = valueStack.Pop();
						if (operatorVal == "+")
						{
							return (valueStack.Pop() + val);
						}
						else if (operatorVal == "-")
						{
							return (valueStack.Pop() - val);

						}
					}
					else //if left parenthesis mismatched, throws error and clears stack
						stackCleaner();
					throw new ArgumentException("Invalid argument. Parenthesis mismatched or operator in invalid location");
				} //when last token is processed
				counter--;
			}
			return valueStack.Pop();
		}
		/// <summary>
		/// If operation throws an error & program fails, cleans operators and values in stacks
		/// </summary>
		private static void stackCleaner()
		{
			while (valueStack.Count > 0)
			{
				valueStack.Pop();
			}
			while (operatorStack.Count > 0)
			{
				operatorStack.Pop();
			}
		} // If an argument is thrown clears the stacks (since they're static)

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
					int vals = valueStack.Pop();
					valueStack.Push(valueStack.Pop() + vals);
				}
				else if (oper == "-")
				{
					int vals = valueStack.Pop();
					valueStack.Push(valueStack.Pop() - vals);

				}
				else operatorStack.Push(oper);
			}

			if (operatorStack.Count > 0 && operatorStack.Peek() == "(")
			{
				operatorStack.Pop();
			}

			if (operatorStack.Count > 0 && valueStack.Count > 1)
			{
				if (operatorStack.Peek() == "*")
				{
					operatorStack.Pop();
					int vals = valueStack.Pop();
					valueStack.Push(valueStack.Pop() * vals);
				}
				else if (operatorStack.Peek() == "/")
				{
					operatorStack.Pop();
					int vals = valueStack.Pop();
					if (vals == 0)
					{
						stackCleaner();
						throw new ArgumentException("can't divide by zero");
					}
					valueStack.Push(valueStack.Pop() / vals);
				}
			}

		} //if token = ")" evaluates items inside parenthesis

		/// <summary>
		///	If token is a "+" or "-" evaluates the operation and then pushes onto the stack
		/// </summary>
		/// <param name="token">The current token to evaluate</param>
		private static void EvaluateOperation(string token)
		{
			if (operatorStack.Count > 0 && valueStack.Count > 1)
			{
				if (operatorStack.Peek() == "+" || operatorStack.Peek() == "-")
				{
					string oper = operatorStack.Pop();
					int vals = valueStack.Pop();
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

						default:
							operatorStack.Push(token);
							valueStack.Push(vals);
							break;

					}
				}
				else operatorStack.Push(token);
			}
			else operatorStack.Push(token);
		} // evaluates + or - operations

		/// <summary>
		/// If token is an integer or Variable. Evaluates and the pushes onto the stack
		/// </summary>
		/// <param name="value">The integer value of the token</param>
		private static void CalculateNumber(int value)
		{
			if (valueStack.Count == 0)
			{
				valueStack.Push(value);
			}
			else if (operatorStack.Count != 0 && valueStack.Count > 0)
			{
				if (operatorStack.Peek() == "*")
				{
					operatorStack.Pop();
					int vals = valueStack.Pop();
					valueStack.Push(vals * value);
				}

				else if (operatorStack.Peek() == "/")
				{
					operatorStack.Pop();
					int vals = valueStack.Pop();
					if (value == 0)
					{
						stackCleaner();
						throw new ArgumentException("can't divide by zero");
					}
					valueStack.Push(vals / value);
				}
				else valueStack.Push(value);
			}
		} // evaluates token if an integer or variable

		/// <summary>
		/// Method for validating input. Checks if a token is a valid input
		/// </summary>
		/// <param name="token">token to be validated</param>
		private static void CheckvalidToken(string token)//checks if token is valid, if not, throws exception
		{
			token = token.Trim();
			//checks if token string is an int value
			if (int.TryParse(token, out int value))
			{
				if (value >= 0)
				{
					token = "pass"; // passes validation if an integer
				}
			}

			//Checks if token is a variable of Letters followed by numbers
			//e.g AABB6 or A6
			Regex variableCheck = new Regex("^[a-zA-Z]+[0-9]+$");
			if (variableCheck.IsMatch(token))
			{
				token = "pass";
				isVariable = true;
			}

			switch (token)
			{
				case "+":
				case "-":
				case "*":
				case "/":
				case "(":
				case ")":
				case ",":
				case "":
				case "         ":
				case "pass":
					return; //if one of the above, passes the check
				default:
					stackCleaner();
					throw new ArgumentException("Invalid token " + token);

			}
		}
	}
}

