/// <summary>
/// Author Devin White
/// Test Class for Formula
/// </summary>
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// Author Devin W. Some Test methods written by CS3500 staff
/// </summary>
namespace FormulaTest
{
	[TestClass]
	public class FormulaTest
	{
		/// <summary>
		/// Normalizer Method for testing
		/// </summary>
		/// <param name="token">token to normalize</param>
		/// <returns>Returns normalized token
		/// </returns>
		public string normalize(string token)
		{
			return token;
		}
		/// <summary>
		/// Normalizer Method for testing, makes Uppercase
		/// </summary>
		/// <param name="token">token to normalize</param>
		/// <returns>Returns normalized token</returns>
		public string normalizeUpper(string token)
		{
			return token.ToUpper();
		}
		/// <summary>
		/// Validator method for testing
		/// </summary>
		/// <param name="token">Token to validate</param>
		/// <returns>True or false</returns>
		public bool isvalid(string token)
		{
			if (token == "KHAZAM")
			{
				return false;
			}
			return true;
		}
		/// <summary>
		/// Lookup tester method
		/// </summary>
		/// <param name="token">token to lookup</param>
		/// <returns>6 for all variables</returns>
		public double lookup(string token)
		{
			if (token == "A6")
			{
				throw new ArgumentException();
			}
			else return 6;
		}
		/// <summary>
		/// Tests the GetVariables method
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("GetVariables")]
		public void testGetVariables()
		{
			Formula formula = new Formula("x+X*z");
			IEnumerator<string> enumerator = formula.GetVariables().GetEnumerator();
			Assert.IsTrue(enumerator.MoveNext());
			Assert.IsTrue(enumerator.MoveNext());
			Assert.IsTrue(enumerator.MoveNext());
			Assert.IsFalse(enumerator.MoveNext());



			Formula formula2 = new Formula("x+X*z", normalizeUpper, isvalid);
			IEnumerator<string> enumerator2 = formula2.GetVariables().GetEnumerator();
			Assert.IsTrue(enumerator2.MoveNext());
			Assert.IsTrue(enumerator2.MoveNext());
			Assert.IsFalse(enumerator2.MoveNext());
		}
		/// <summary>
		/// Checks Normalizer with mix of lower and Upper with duplicates to ensure it works completely
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("GetVariables")]

		public void testGetVariablesWithLowercase()
		{
			Formula formula = new Formula("a + A + B + b + C+ c + D + d + e + e + f+f + g + h + I", normalizeUpper, isvalid);
			IEnumerator<string> enumerator = formula.GetVariables().GetEnumerator();
			Assert.IsTrue(enumerator.MoveNext()); //A
			Assert.IsTrue(enumerator.MoveNext());//B
			Assert.IsTrue(enumerator.MoveNext());//C
			Assert.IsTrue(enumerator.MoveNext());//D
			Assert.IsTrue(enumerator.MoveNext());//E
			Assert.IsTrue(enumerator.MoveNext());//F
			Assert.IsTrue(enumerator.MoveNext());//G
			Assert.IsTrue(enumerator.MoveNext());//H
			Assert.IsTrue(enumerator.MoveNext());//I
			Assert.IsFalse(enumerator.MoveNext());
		}
		/// <summary>
		/// Tetss Equals override
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Equals")]
		public void testEquals()
		{
			Formula formula = new Formula("x2 + y3", normalize, isvalid);
			Formula formula2 = new Formula("x2+y3", normalize, isvalid);
			Formula formula3 = new Formula("2.0000", normalize, isvalid);
			Formula formula4 = new Formula("2", normalize, isvalid);
			Formula formula5 = new Formula("(x2 +1e19)*3.00000/   y3 ", normalize, isvalid);
			Formula formula6 = new Formula("(x2 + 10000000000000000000) * 3 / y3 ", normalize, isvalid);

			Assert.IsTrue(formula.Equals(formula2));
			Assert.IsTrue(formula3.Equals(formula4));
			Assert.IsTrue(formula5.Equals(formula6));
			Assert.IsFalse(formula5.Equals(formula));
		}
		/// <summary>
		/// Tests whether a not formula object does not equal a formula
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Equals")]
		public void testEqualsobjNotFormula()
		{
			Formula formula = new Formula("x2+y3", normalize, isvalid);
			StringBuilder stringBuilder = new StringBuilder("x2+y3");
			Assert.IsFalse(stringBuilder.Equals(formula));
			Assert.IsFalse(formula.Equals(stringBuilder));

		}
		/// <summary>
		/// tests Equals override again with different, more complex numbers
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Equals")]
		public void testEquals2()
		{
			Formula formula = new Formula("x22 + y33", normalize, isvalid);
			Formula formula2 = new Formula("x22+y33", normalize, isvalid);
			Formula formula3 = new Formula("3.000500", normalize, isvalid); ///checks if 3.00500 is equal to 3.005
			Formula formula4 = new Formula("3.0005", normalize, isvalid); /// checks if 3.005 is equal to 3.00500
			Formula formula5 = new Formula("(x_X2 +1e5)*3.00000/   y3 ", normalize, isvalid);
			Formula formula6 = new Formula("(x_X2 + 100000) * 3 / y3 ", normalize, isvalid);

			Assert.IsTrue(formula.Equals(formula2));
			Assert.IsTrue(formula3.Equals(formula4));
			Assert.IsTrue(formula5.Equals(formula6));
			Assert.IsFalse(formula5.Equals(formula));
		}
		/// <summary>
		/// Tets the GetHashCode() override
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("HashCode")]
		public void testHashCode()
		{
			Formula formula = new Formula("x2 + y3 + 1 + 4 + 6", normalize, isvalid);
			Formula formula2 = new Formula("x2+y3 + 1 + 4  +6", normalize, isvalid);
			Formula formula3 = new Formula("2.0000", normalize, isvalid);
			Formula formula4 = new Formula("2.0", normalize, isvalid);
			Formula formula5 = new Formula("(x2 +1e19)*3.00000/   y3 ", normalize, isvalid);
			Formula formula6 = new Formula("(x2 + 10000000000000000000) * 3 / y3 ", normalize, isvalid);
			Formula formula7 = new Formula("ABC+2", s => "X", isvalid);
			Formula formula8 = new Formula("X2 +2", S => "X", isvalid);
			Formula formula9 = new Formula("y3+x2 + 6 + 1 + 4", normalize, isvalid);


			Assert.IsTrue(formula.Equals(formula2));
			Assert.AreEqual(formula.GetHashCode(), formula2.GetHashCode());
			Assert.IsTrue(formula3.Equals(formula4));
			Assert.AreEqual(formula3.GetHashCode(), formula4.GetHashCode());
			Assert.IsTrue(formula6.Equals(formula5));
			Assert.AreEqual(formula5.GetHashCode(), formula6.GetHashCode());
			Assert.AreNotEqual(formula.GetHashCode(), formula6.GetHashCode());
			Assert.AreNotEqual(formula.GetHashCode(), formula4.GetHashCode());
			Assert.AreEqual(formula7, formula8);
			Assert.AreNotEqual(formula.GetHashCode(), formula9.GetHashCode()); //Tests same tokens in same order

		}
		[TestMethod(), Timeout(5000)]
		[TestCategory("HashCode")]
		public void equals4()
		{
			Formula formula = new Formula("X2 + y3 + 1 + 4 + 6", normalizeUpper, isvalid);
			Formula formula2 = new Formula("x2+Y3 + 1 + 4  +6", normalizeUpper, isvalid);
			Assert.IsTrue(formula.Equals(formula2));
		}
		/// <summary>
		/// Tests that the toString method removes spaces
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("ToString")]
		public void TestToString()
		{
			Formula formula = new Formula("5+7+(5)", normalize, isvalid);
			Formula formula2 = new Formula("5+7+(5)");
			Console.WriteLine(formula.ToString());
			Assert.AreEqual("5+7+(5)", formula.ToString());

		}
		/// <summary>
		/// Tests that the toString method removes spaces
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("ToString")]
		public void Testparen()
		{
			Formula formula = new Formula("x2    +    y2", normalize, isvalid);
			Console.WriteLine(formula.ToString());
			Assert.AreEqual("x2+y2", formula.ToString());

		}
		/// <summary>
		/// Tests whether a single variable is valid
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		public void testVariable()
		{
			Formula formula = new Formula("ABRA1", normalize, isvalid);
		}
		/// <summary>
		/// tests a single invalid variable
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void testinValidVariable()
		{
			Formula formula = new Formula("1KADABRA", normalize, isvalid);
		}
		/// <summary>
		/// Tests an invalid variable with a valid variable
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void testValidwithInvalidVariable()
		{
			Formula formula = new Formula("ALA+KHAZAM", normalize, isvalid);
		}
		/// <summary>
		/// Tests whether a number with many decimal places is valid
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		public void testValue()
		{
			Formula formula = new Formula("120353123125.123123124535", normalize, isvalid);
		}
		/// <summary>
		/// Tests whether a lone set of parenthesis is valid
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void LoneParenthesis()
		{
			Formula formula = new Formula("()");
		}
		/// <summary>
		/// Tests whether scientific notation is valid
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		public void testScientificNotation()
		{
			Formula formula = new Formula("1e12");
			Assert.AreEqual(1e12, formula.Evaluate(lookup));
		}
		/// <summary>
		/// Tests if the != sign works as expected
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		public void TestoperatornotEquals()
		{
			Formula formula = new Formula("x2 + y2", normalize, isvalid);
			Formula formula2 = new Formula("z2+Y2", normalize, isvalid);
			Formula formula3 = new Formula("x2+y2", normalize, isvalid);
			Console.WriteLine(formula);
			Console.WriteLine(formula2);
			Assert.IsTrue(formula != formula2);
			Assert.IsFalse(null != null);
			Assert.IsFalse(formula3 != formula);
			Assert.IsTrue(null != formula);
		}
		/// <summary>
		/// Tests if the != sign works as expected with a non formula object
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		public void TestoperatornotEqualsNonFormulaObj()
		{
			Formula formula = new Formula("x2 + y2", normalize, isvalid);
			Formula formula2 = new Formula("A1 + B1 + C1 + 1 + 2 + 3", normalize, isvalid);
			Formula formula3 = new Formula("A1+B1+C1+1e0+2.0+3.0", normalize, isvalid);
			StringBuilder str = new StringBuilder("A1 + B1 + C1 + 1 + 2 + 3");

			Assert.AreNotEqual(str, formula2);
			Assert.IsFalse(formula2.ToString() == str.ToString());

		}
		/// <summary>
		/// Tests if the == sign works as expected
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		public void TestoperatorEquals()
		{
			Formula formula = new Formula("x2 + y2", normalizeUpper, isvalid);
			Formula formula2 = new Formula("X2+Y2", normalizeUpper, isvalid);
			Assert.IsTrue(formula == formula2);
			Assert.IsTrue(null == null);
			Assert.IsFalse(null == formula);
		}
		/// <summary>
		/// Tests basic invalid token
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void testInvalidToken()
		{
			Formula formula = new Formula("2A", normalize, isvalid);
		}
		/// <summary>
		/// Tests if an operator following a parenthesis throws an exception
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void testParenthesisOperatorFollowingRule()
		{
			Formula formula = new Formula("( *c 2 a + 2)", normalize, isvalid);
		}
		/// <summary>
		/// Tests "Any token that immediately follows a number, 
		/// a variable, or a closing parenthesis 
		/// must be either an operator or a closing parenthesis."
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void testExtraFollowingRule()
		{
			Formula formula = new Formula("1 1", normalize, isvalid);
		}
		/// <summary>
		/// Tests whether right parenthesis match left parenthesis
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void testRightParenthesisrule()
		{
			Formula formula = new Formula("(1))", normalize, isvalid);
		}
		/// <summary>
		/// Tests empty token rule
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void testnoValidToken()
		{
			Formula formula = new Formula("", normalize, isvalid);
		}
		/// <summary>
		/// Tests Balanced Parenthesis rule
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void testBalancedParenthesis()
		{
			Formula formula = new Formula("(((a2 + b3)+ 2) + 4))", normalize, isvalid);
		}
		/// <summary>
		/// Tests if a single left parenthesis will cause a mismatch
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void testleftParenMismatch()
		{
			Formula formula = new Formula("(a2", normalize, isvalid);
		}
		/// <summary>
		/// Tests whether starting token is a number, variable, or (
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Valid Token")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void testStartingTokenRule()
		{
			Formula formula = new Formula("", normalize, isvalid);
		}
		/// <summary>
		/// Tests the Evaluator throws a formula Error
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void testEvaluator()
		{
			Formula formula = new Formula("4*B2+(A6+3)", normalize, isvalid);
			Assert.IsInstanceOfType(formula.Evaluate(lookup), typeof(FormulaError));
		}
		/// <summary>
		/// Tests the normalizer works as expected
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void testNormalize()
		{
			Formula formula = new Formula("x6", s => "X", isvalid);
			Assert.AreEqual("X", formula.ToString());
		}
		/// <summary>
		/// Tests Dividing by zero throws a formula error
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void testDivByZero()
		{
			Formula formula = new Formula("(5/0)", normalize, isvalid);
			Assert.IsInstanceOfType(formula.Evaluate(lookup), typeof(FormulaError));

		}
		/// <summary>
		/// Tests Dividing by zero with a formula lookup throws a formula error
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void testDivByZeroVariable()
		{
			Formula formula = new Formula("(5/ x6)", normalize, isvalid);
			Assert.IsInstanceOfType(formula.Evaluate(s => 0), typeof(FormulaError));

		}
		/// <summary>
		/// Tests a complex operation will return a FormulaError
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestComplexDivByZeroariable()
		{
			Formula formula = new Formula("(((4 * 4) / 2 * (2 * 2.22)) + (1.89 + 2)) / A6 + (2 - 1) /2", normalize, isvalid);
			Assert.IsInstanceOfType(formula.Evaluate(s => 0), typeof(FormulaError));

		}
		/// <summary>
		/// tests Complex Operation
		/// </summary>
		[TestMethod]
		[TestCategory("Evaluator")]
		public void TestComplexOperations()
		{
			double result1 = (((4 * 4) / 2 * (2 * 2.22)) + (1.89 + 2)) / 2 + (2 - 1) - 1;
			Formula formula = new Formula("(((4 * 4) / 2 * (2 * 2.22)) + (1.89 + 2)) / 2 + (2 - 1) - 1");
			Assert.AreEqual(result1, formula.Evaluate(lookup));
		}
		/*[TestMethod]
		[TestCategory("Evaluator")]
		public void TestVariableUndefined()
		{

			Formula formula = new Formula("2/A6");
			Assert.IsInstanceOfType(formula.Evaluate(s => 0), typeof(FormulaError));
		}*/

		/// <summary>
		/// tests Complex Operation with Variables
		/// </summary>
		[TestMethod]
		[TestCategory("Evaluator")]
		public void TestComplexOperationsVariable()
		{
			double result1 = (((((6 * 4.5) / 2) * (2 * 6) + (1 - 2)) / 6) / (2 - 6) + 1);
			Formula formula = new Formula("(((((A6 * 4.5) / 2) * (2 * B22) + (1 - 2)) / P) / (2 - X9) + 1)");
			Assert.AreEqual(result1, formula.Evaluate(s => 6));
		}
		///The following are Unit Tests from PS1
		/// They were NOT WRITTEN BY ME
		/// However, slightly modified to account for doubles
		/// and new Variable rules becaue they're useful
		/// for testing evaluator
		/// <summary>
		/// tests a single number
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestSingleNumber()
		{
			Formula formula = new Formula("5", normalize, isvalid);
			Assert.AreEqual(5.0, formula.Evaluate(lookup));
		}
		/// <summary>
		/// Tests a single variable
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestSingleVariable()
		{
			Formula formula = new Formula("X5", normalize, isvalid);
			Assert.AreEqual(13.0, formula.Evaluate(s => 13));
		}
		/// <summary>
		/// Tests basic addition
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestAddition()
		{
			Formula formula = new Formula("5+3", normalize, isvalid);
			Formula formula2 = new Formula("5+3.5", normalize, isvalid);
			Assert.AreEqual(8.0, formula.Evaluate(lookup));
			Assert.AreEqual(8.5, formula2.Evaluate(lookup));
		}
		/// <summary>
		/// Tests basic subtraction
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestSubtraction()
		{
			Formula formula = new Formula("18-10", normalize, isvalid);
			Assert.AreEqual(8.0, formula.Evaluate(lookup));
		}
		/// <summary>
		/// Tets basic multiplication
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestMultiplication()
		{
			Formula formula = new Formula("2*4", normalize, isvalid);
			Assert.AreEqual(8.0, formula.Evaluate(lookup));
		}
		/// <summary>
		/// Tets basic division
		/// </summary>
		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestDivision()
		{
			Formula formula = new Formula("16/2", normalize, isvalid);
			Assert.AreEqual(8.0, formula.Evaluate(lookup));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestArithmeticWithVariable()
		{
			Formula formula = new Formula("x2+1", normalize, isvalid);
			Assert.AreEqual(5.0, formula.Evaluate(s => 4));
		}
		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestLeftToRight()
		{
			Formula formula = new Formula("2*6+3", normalize, isvalid);
			Assert.AreEqual(15.0, formula.Evaluate(lookup));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestOrderOperations()
		{
			Formula formula = new Formula("2+6*3", normalize, isvalid);
			Assert.AreEqual(20.0, formula.Evaluate(lookup));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestParenthesesTimes()
		{
			Formula formula = new Formula("(2+6)*3", normalize, isvalid);
			Assert.AreEqual(24.0, formula.Evaluate(lookup));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestTimesParentheses()
		{
			Formula formula = new Formula("2*(3+5)", normalize, isvalid);
			Assert.AreEqual(16.0, formula.Evaluate(lookup));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestPlusParentheses()
		{
			Formula formula = new Formula("2+(3+5)", normalize, isvalid);
			Assert.AreEqual(10.0, formula.Evaluate(lookup));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestPlusComplex()
		{
			Formula formula = new Formula("2+(3+5*9)", normalize, isvalid);
			Assert.AreEqual(50.0, formula.Evaluate(lookup));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestOperatorAfterParens()
		{
			Formula formula = new Formula("(1*1)-2/2", normalize, isvalid);
			Assert.AreEqual(0.0, formula.Evaluate(lookup));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestComplexTimesParentheses()
		{
			Formula formula = new Formula("2+3*(3+5)", normalize, isvalid);
			Assert.AreEqual(26.0, formula.Evaluate(lookup));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestComplexAndParentheses()
		{
			Formula formula = new Formula("2+3*5+(3+4*8)*5+2", normalize, isvalid);
			Assert.AreEqual(194.0, formula.Evaluate(lookup));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestDivideByZero()
		{
			Formula formula = new Formula("5/0", normalize, isvalid);
			formula.Evaluate(lookup);
			Console.WriteLine(formula);
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void TestSingleOperator()
		{
			Formula formula = new Formula("+", normalize, isvalid);

		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void TestExtraOperator()
		{
			Formula formula = new Formula("2+5+", normalize, isvalid);
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void TestExtraParentheses()
		{
			Formula formula = new Formula("2+5*7)", normalize, isvalid);
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void TestInvalidVariable()
		{
			Formula formula = new Formula("2xx", normalize, isvalid);
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void TestPlusInvalidVariable()
		{
			Formula formula = new Formula("5+2xx", normalize, isvalid);
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		[ExpectedException(typeof(FormulaFormatException))]
		public void TestEmpty()
		{
			Formula formula = new Formula("", normalize, isvalid);
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestComplexMultiVar()
		{
			Formula formula = new Formula("y1*3-8/2+4*(8-9*2)/14*x7", normalize, isvalid);
			Assert.AreEqual(5.142857142857142, formula.Evaluate(s => (s == "x7") ? 1 : 4));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestComplexNestedParensRight()
		{
			Formula formula = new Formula("x1+(x2+(x3+(x4+(x5+x6))))", normalize, isvalid);
			Assert.AreEqual(6.0, formula.Evaluate(s => 1));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestComplexNestedParensLeft()
		{
			Formula formula = new Formula("((((x1+x2)+x3)+x4)+x5)+x6", normalize, isvalid);
			Assert.AreEqual(12.0, formula.Evaluate(s => 2));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("Evaluator")]
		public void TestRepeatedVar()
		{
			Formula formula = new Formula("a4-a4*a4/a4", normalize, isvalid);
			Assert.AreEqual(0.0, formula.Evaluate(s => 3));
		}
		[TestClass]
		public class GradingTests
		{

			// Normalizer tests
			[TestMethod(), Timeout(2000)]
			[TestCategory("1")]
			public void TestNormalizerGetVars()
			{
				Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
				HashSet<string> vars = new HashSet<string>(f.GetVariables());

				Assert.IsTrue(vars.SetEquals(new HashSet<string> { "X1" }));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("2")]
			public void TestNormalizerEquals()
			{
				Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
				Formula f2 = new Formula("2+X1", s => s.ToUpper(), s => true);

				Assert.IsTrue(f.Equals(f2));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("3")]
			public void TestNormalizerToString()
			{
				Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
				Formula f2 = new Formula(f.ToString());

				Assert.IsTrue(f.Equals(f2));
			}

			// Validator tests
			[TestMethod(), Timeout(2000)]
			[TestCategory("4")]
			[ExpectedException(typeof(FormulaFormatException))]
			public void TestValidatorFalse()
			{
				Formula f = new Formula("2+x1", s => s, s => false);
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("5")]
			public void TestValidatorX1()
			{
				Formula f = new Formula("2+x", s => s, s => (s == "x"));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("6")]
			[ExpectedException(typeof(FormulaFormatException))]
			public void TestValidatorX2()
			{
				Formula f = new Formula("2+y1", s => s, s => (s == "x"));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("7")]
			[ExpectedException(typeof(FormulaFormatException))]
			public void TestValidatorX3()
			{
				Formula f = new Formula("2+x1", s => s, s => (s == "x"));
			}


			// Simple tests that return FormulaErrors
			[TestMethod(), Timeout(2000)]
			[TestCategory("8")]
			public void TestUnknownVariable()
			{
				Formula f = new Formula("2+X1");
				Assert.IsInstanceOfType(f.Evaluate(s => { throw new ArgumentException("Unknown variable"); }), typeof(FormulaError));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("9")]
			public void TestDivideByZero()
			{
				Formula f = new Formula("5/0");
				Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("10")]
			public void TestDivideByZeroVars()
			{
				Formula f = new Formula("(5 + X1) / (X1 - 3)");
				Assert.IsInstanceOfType(f.Evaluate(s => 3), typeof(FormulaError));
			}


			// Tests of syntax errors detected by the constructor
			[TestMethod(), Timeout(2000)]
			[TestCategory("11")]
			[ExpectedException(typeof(FormulaFormatException))]
			public void TestSingleOperator()
			{
				Formula f = new Formula("+");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("12")]
			[ExpectedException(typeof(FormulaFormatException))]
			public void TestExtraOperator()
			{
				Formula f = new Formula("2+5+");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("13")]
			[ExpectedException(typeof(FormulaFormatException))]
			public void TestExtraCloseParen()
			{
				Formula f = new Formula("2+5*7)");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("14")]
			[ExpectedException(typeof(FormulaFormatException))]
			public void TestExtraOpenParen()
			{
				Formula f = new Formula("((3+5*7)");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("15")]
			[ExpectedException(typeof(FormulaFormatException))]
			public void TestNoOperator()
			{
				Formula f = new Formula("5x");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("16")]
			[ExpectedException(typeof(FormulaFormatException))]
			public void TestNoOperator2()
			{
				Formula f = new Formula("5+5x");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("17")]
			[ExpectedException(typeof(FormulaFormatException))]
			public void TestNoOperator3()
			{
				Formula f = new Formula("5+7+(5)8");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("18")]
			[ExpectedException(typeof(FormulaFormatException))]
			public void TestNoOperator4()
			{
				Formula f = new Formula("5 5");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("19")]
			[ExpectedException(typeof(FormulaFormatException))]
			public void TestDoubleOperator()
			{
				Formula f = new Formula("5 + + 3");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("20")]
			[ExpectedException(typeof(FormulaFormatException))]
			public void TestEmpty()
			{
				Formula f = new Formula("");
			}

			// Some more complicated formula evaluations
			[TestMethod(), Timeout(2000)]
			[TestCategory("21")]
			public void TestComplex1()
			{
				Formula f = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
				Assert.AreEqual(5.14285714285714, (double)f.Evaluate(s => (s == "x7") ? 1 : 4), 1e-9);
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("22")]
			public void TestRightParens()
			{
				Formula f = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
				Assert.AreEqual(6, (double)f.Evaluate(s => 1), 1e-9);
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("23")]
			public void TestLeftParens()
			{
				Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
				Assert.AreEqual(12, (double)f.Evaluate(s => 2), 1e-9);
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("53")]
			public void TestRepeatedVar()
			{
				Formula f = new Formula("a4-a4*a4/a4");
				Assert.AreEqual(0, (double)f.Evaluate(s => 3), 1e-9);
			}

			// Test of the Equals method
			[TestMethod(), Timeout(2000)]
			[TestCategory("24")]
			public void TestEqualsBasic()
			{
				Formula f1 = new Formula("X1+X2");
				Formula f2 = new Formula("X1+X2");
				Assert.IsTrue(f1.Equals(f2));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("25")]
			public void TestEqualsWhitespace()
			{
				Formula f1 = new Formula("X1+X2");
				Formula f2 = new Formula(" X1  +  X2   ");
				Assert.IsTrue(f1.Equals(f2));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("26")]
			public void TestEqualsDouble()
			{
				Formula f1 = new Formula("2+X1*3.00");
				Formula f2 = new Formula("2.00+X1*3.0");
				Assert.IsTrue(f1.Equals(f2));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("27")]
			public void TestEqualsComplex()
			{
				Formula f1 = new Formula("1e-2 + X5 + 17.00 * 19 ");
				Formula f2 = new Formula("   0.0100  +     X5+ 17 * 19.00000 ");
				Assert.IsTrue(f1.Equals(f2));
			}


			[TestMethod(), Timeout(2000)]
			[TestCategory("28")]
			public void TestEqualsNullAndString()
			{
				Formula f = new Formula("2");
				Assert.IsFalse(f.Equals(null));
				Assert.IsFalse(f.Equals(""));
			}


			// Tests of == operator
			[TestMethod(), Timeout(2000)]
			[TestCategory("29")]
			public void TestEq()
			{
				Formula f1 = new Formula("2");
				Formula f2 = new Formula("2");
				Assert.IsTrue(f1 == f2);
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("30")]
			public void TestEqFalse()
			{
				Formula f1 = new Formula("2");
				Formula f2 = new Formula("5");
				Assert.IsFalse(f1 == f2);
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("31")]
			public void TestEqNull()
			{
				Formula f1 = new Formula("2");
				Formula f2 = new Formula("2");
				Assert.IsFalse(null == f1);
				Assert.IsFalse(f1 == null);
				Assert.IsTrue(f1 == f2);
			}


			// Tests of != operator
			[TestMethod(), Timeout(2000)]
			[TestCategory("32")]
			public void TestNotEq()
			{
				Formula f1 = new Formula("2");
				Formula f2 = new Formula("2");
				Assert.IsFalse(f1 != f2);
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("33")]
			public void TestNotEqTrue()
			{
				Formula f1 = new Formula("2");
				Formula f2 = new Formula("5");
				Assert.IsTrue(f1 != f2);
			}


			// Test of ToString method
			[TestMethod(), Timeout(2000)]
			[TestCategory("34")]
			public void TestString()
			{
				Formula f = new Formula("2*5");
				Assert.IsTrue(f.Equals(new Formula(f.ToString())));
			}


			// Tests of GetHashCode method
			[TestMethod(), Timeout(2000)]
			[TestCategory("35")]
			public void TestHashCode()
			{
				Formula f1 = new Formula("2*5");
				Formula f2 = new Formula("2*5");
				Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
			}

			// Technically the hashcodes could not be equal and still be valid,
			// extremely unlikely though. Check their implementation if this fails.
			[TestMethod(), Timeout(2000)]
			[TestCategory("36")]
			public void TestHashCodeFalse()
			{
				Formula f1 = new Formula("2*5");
				Formula f2 = new Formula("3/8*2+(7)");
				Assert.IsTrue(f1.GetHashCode() != f2.GetHashCode());
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("37")]
			public void TestHashCodeComplex()
			{
				Formula f1 = new Formula("2 * 5 + 4.00 - _x");
				Formula f2 = new Formula("2*5+4-_x");
				Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
			}


			// Tests of GetVariables method
			[TestMethod(), Timeout(2000)]
			[TestCategory("38")]
			public void TestVarsNone()
			{
				Formula f = new Formula("2*5");
				Assert.IsFalse(f.GetVariables().GetEnumerator().MoveNext());
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("39")]
			public void TestVarsSimple()
			{
				Formula f = new Formula("2*X2");
				List<string> actual = new List<string>(f.GetVariables());
				HashSet<string> expected = new HashSet<string>() { "X2" };
				Assert.AreEqual(actual.Count, 1);
				Assert.IsTrue(expected.SetEquals(actual));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("40")]
			public void TestVarsTwo()
			{
				Formula f = new Formula("2*X2+Y3");
				List<string> actual = new List<string>(f.GetVariables());
				HashSet<string> expected = new HashSet<string>() { "Y3", "X2" };
				Assert.AreEqual(actual.Count, 2);
				Assert.IsTrue(expected.SetEquals(actual));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("41")]
			public void TestVarsDuplicate()
			{
				Formula f = new Formula("2*X2+X2");
				List<string> actual = new List<string>(f.GetVariables());
				HashSet<string> expected = new HashSet<string>() { "X2" };
				Assert.AreEqual(actual.Count, 1);
				Assert.IsTrue(expected.SetEquals(actual));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("42")]
			public void TestVarsComplex()
			{
				Formula f = new Formula("X1+Y2*X3*Y2+Z7+X1/Z8");
				List<string> actual = new List<string>(f.GetVariables());
				HashSet<string> expected = new HashSet<string>() { "X1", "Y2", "X3", "Z7", "Z8" };
				Assert.AreEqual(actual.Count, 5);
				Assert.IsTrue(expected.SetEquals(actual));
			}

			// Tests to make sure there can be more than one formula at a time
			[TestMethod(), Timeout(2000)]
			[TestCategory("43")]
			public void TestMultipleFormulae()
			{
				Formula f1 = new Formula("2 + a1");
				Formula f2 = new Formula("3");
				Assert.AreEqual(2.0, f1.Evaluate(x => 0));
				Assert.AreEqual(3.0, f2.Evaluate(x => 0));
				Assert.IsFalse(new Formula(f1.ToString()) == new Formula(f2.ToString()));
				IEnumerator<string> f1Vars = f1.GetVariables().GetEnumerator();
				IEnumerator<string> f2Vars = f2.GetVariables().GetEnumerator();
				Assert.IsFalse(f2Vars.MoveNext());
				Assert.IsTrue(f1Vars.MoveNext());
			}

			// Repeat this test to increase its weight
			[TestMethod(), Timeout(2000)]
			[TestCategory("44")]
			public void TestMultipleFormulaeB()
			{
				TestMultipleFormulae();
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("45")]
			public void TestMultipleFormulaeC()
			{
				TestMultipleFormulae();
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("46")]
			public void TestMultipleFormulaeD()
			{
				TestMultipleFormulae();
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("47")]
			public void TestMultipleFormulaeE()
			{
				TestMultipleFormulae();
			}

			// Stress test for constructor
			[TestMethod(), Timeout(2000)]
			[TestCategory("48")]
			public void TestConstructor()
			{
				Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
			}

			// This test is repeated to increase its weight
			[TestMethod(), Timeout(2000)]
			[TestCategory("49")]
			public void TestConstructorB()
			{
				Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("50")]
			public void TestConstructorC()
			{
				Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("51")]
			public void TestConstructorD()
			{
				Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
			}

			// Stress test for constructor
			[TestMethod(), Timeout(2000)]
			[TestCategory("52")]
			public void TestConstructorE()
			{
				Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
			}
		}
	}
}
