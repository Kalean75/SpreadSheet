using FormulaEvaluator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EvaluatorTest
{
	/// <summary>
	///This is a test class for EvaluatorTest and is intended
	///to contain all EvaluatorTest Unit Tests
	///</summary>
	[TestClass]
	public class EvaluatorTests
	{

		[TestMethod(), Timeout(5000)]
		[TestCategory("1")]
		public void TestSingleNumber()
		{
			Assert.AreEqual(5, Evaluator.Evaluate("5", s => 0));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("2")]
		public void TestSingleVariable()
		{
			Assert.AreEqual(13, Evaluator.Evaluate("X5", s => 13));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("3")]
		public void TestAddition()
		{
			Assert.AreEqual(8, Evaluator.Evaluate("5+3", s => 0));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("4")]
		public void TestSubtraction()
		{
			Assert.AreEqual(8, Evaluator.Evaluate("18-10", s => 0));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("5")]
		public void TestMultiplication()
		{
			Assert.AreEqual(8, Evaluator.Evaluate("2*4", s => 0));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("6")]
		public void TestDivision()
		{
			Assert.AreEqual(8, Evaluator.Evaluate("16/2", s => 0));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("7")]
		public void TestArithmeticWithVariable()
		{
			Assert.AreEqual(6, Evaluator.Evaluate("2+X1", s => 4));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("8")]
		[ExpectedException(typeof(ArgumentException))]
		public void TestUnknownVariable()
		{
			Evaluator.Evaluate("2+X1", s => { throw new ArgumentException("Unknown variable"); });
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("9")]
		public void TestLeftToRight()
		{
			Assert.AreEqual(15, Evaluator.Evaluate("2*6+3", s => 0));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("10")]
		public void TestOrderOperations()
		{
			Assert.AreEqual(20, Evaluator.Evaluate("2+6*3", s => 0));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("11")]
		public void TestParenthesesTimes()
		{
			Assert.AreEqual(24, Evaluator.Evaluate("(2+6)*3", s => 0));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("12")]
		public void TestTimesParentheses()
		{
			Assert.AreEqual(16, Evaluator.Evaluate("2*(3+5)", s => 0));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("13")]
		public void TestPlusParentheses()
		{
			Assert.AreEqual(10, Evaluator.Evaluate("2+(3+5)", s => 0));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("14")]
		public void TestPlusComplex()
		{
			Assert.AreEqual(50, Evaluator.Evaluate("2+(3+5*9)", s => 0));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("15")]
		public void TestOperatorAfterParens()
		{
			Assert.AreEqual(0, Evaluator.Evaluate("(1*1)-2/2", s => 0));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("16")]
		public void TestComplexTimesParentheses()
		{
			Assert.AreEqual(26, Evaluator.Evaluate("2+3*(3+5)", s => 0));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("17")]
		public void TestComplexAndParentheses()
		{
			Assert.AreEqual(194, Evaluator.Evaluate("2+3*5+(3+4*8)*5+2", s => 0));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("18")]
		[ExpectedException(typeof(ArgumentException))]
		public void TestDivideByZero()
		{
			Evaluator.Evaluate("5/0", s => 0);
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("19")]
		[ExpectedException(typeof(ArgumentException))]
		public void TestSingleOperator()
		{
			Evaluator.Evaluate("+", s => 0);
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("20")]
		[ExpectedException(typeof(ArgumentException))]
		public void TestExtraOperator()
		{
			Evaluator.Evaluate("2+5+", s => 0);
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("21")]
		[ExpectedException(typeof(ArgumentException))]
		public void TestExtraParentheses()
		{
			Evaluator.Evaluate("2+5*7)", s => 0);
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("22")]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidVariable()
		{
			Evaluator.Evaluate("xx", s => 0);
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("23")]
		[ExpectedException(typeof(ArgumentException))]
		public void TestPlusInvalidVariable()
		{
			Evaluator.Evaluate("5+xx", s => 0);
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("25")]
		[ExpectedException(typeof(ArgumentException))]
		public void TestEmpty()
		{
			Evaluator.Evaluate("", s => 0);
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("26")]
		public void TestComplexMultiVar()
		{
			Assert.AreEqual(6, Evaluator.Evaluate("y1*3-8/2+4*(8-9*2)/14*x7", s => (s == "x7") ? 1 : 4));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("27")]
		public void TestComplexNestedParensRight()
		{
			Assert.AreEqual(6, Evaluator.Evaluate("x1+(x2+(x3+(x4+(x5+x6))))", s => 1));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("28")]
		public void TestComplexNestedParensLeft()
		{
			Assert.AreEqual(12, Evaluator.Evaluate("((((x1+x2)+x3)+x4)+x5)+x6", s => 2));
		}

		[TestMethod(), Timeout(5000)]
		[TestCategory("29")]
		public void TestRepeatedVar()
		{
			Assert.AreEqual(0, Evaluator.Evaluate("a4-a4*a4/a4", s => 3));
		}

		/// <summary>
		/// tests operations
		/// </summary>
		[TestMethod]
		public void TestOperations()
		{
			int result1 = (((4 * 4) / 2 * (2 * 2)) + (1 + 2)) / 2 + (2 - 1) - 1;
			Console.WriteLine("test");
			Console.WriteLine("Expected Output" + Evaluator.Evaluate("(((4 * 4) / 2 * (2 * 2)) + (1 + 2)) / 2 + (2 - 1) - 1", (string str) => int.Parse(str)));
			Assert.AreEqual(result1, Evaluator.Evaluate("(((4 * 4) / 2 * (2*2)) + (1 + 2))/2+(2-1) - 1", (string str) => int.Parse(str)));
		}
		/// <summary>
		/// tests basic addition
		/// </summary>
		[TestMethod]
		public void TestAddBasic()
		{
			int result = 4 + 16;
			Console.WriteLine("Test");
			Console.WriteLine("Expected Output " + result);
			Console.WriteLine("Actual Output " + Evaluator.Evaluate("4 + 16", (string str) => int.Parse(str)));
			Assert.AreEqual(result, Evaluator.Evaluate("4 +  16", (string str) => int.Parse(str)));
		}
		/// <summary>
		/// tests addition
		/// </summary>
		[TestMethod]
		public void TestAdd()
		{
			int result = 4 - 4 / 2 * 2 / 4 + 4 / 5 + 1 - 1;
			Console.WriteLine("test");
			Console.WriteLine("Expected Output " + result);
			Console.WriteLine("Actual Output " + Evaluator.Evaluate("4 + 4 / 2*2 / 4 + 4 / 5", (string str) => int.Parse(str)));
			Assert.AreEqual(result, Evaluator.Evaluate("4 - 4 / 2*2 / 4 + 4 / 5 + 1 - 1", (string str) => int.Parse(str)));
		}
		/// <summary>
		/// tests basic multiplication
		/// </summary>
		[TestMethod]
		public void TestMultiplyBasic()
		{
			int result = 4 * 10;
			Console.WriteLine("test");
			Console.WriteLine("Expected Output " + result);
			Console.WriteLine("Actual Output " + Evaluator.Evaluate("4 * 10", (string str) => int.Parse(str)));
			Assert.AreEqual(result, Evaluator.Evaluate("4 * 10", (string str) => int.Parse(str)));
		}
		/// <summary>
		/// tests multiplication
		/// </summary>
		[TestMethod]
		public void TestMultiply()
		{
			int result = 4 * 4 / 4 * 2 * 2;
			Console.WriteLine("test");
			Console.WriteLine("Expected Output " + result);
			Console.WriteLine("Actual Output " + Evaluator.Evaluate("4 * 4/4 * 2 * 2", (string str) => int.Parse(str)));
			Assert.AreEqual(result, Evaluator.Evaluate("4 * 4/4 * 2 * 2", (string str) => int.Parse(str)));
		}
		/// <summary>
		/// tests basic division
		/// </summary>
		public void TestDivisionBasic()
		{
			int result = 16 / 8;
			Console.WriteLine("test");
			Console.WriteLine("Expected Output " + result);
			Console.WriteLine("Actual Output " + Evaluator.Evaluate("16   /  8", (string str) => int.Parse(str)));
			Assert.AreEqual(result, Evaluator.Evaluate("16/8", (string str) => int.Parse(str)));
		}
		/// <summary>
		/// tests divison
		/// </summary>
		[TestMethod]
		public void TestmoreDivision()
		{
			int result = 16 + 1 / 4 * 2 / 2;
			Console.WriteLine("test");
			Console.WriteLine("Expected Output " + result);
			Console.WriteLine("Actual Output" + Evaluator.Evaluate("16  +1  /  4*2   /2", (string str) => int.Parse(str)));
			Assert.AreEqual(result, Evaluator.Evaluate("16+1  /4*2  /2", (string str) => int.Parse(str)));
		}
		/// <summary>
		/// tests basic subtraction
		/// </summary>
		[TestMethod]
		public void TestSubtractionbasic()
		{
			int result = 4 - 1;
			Console.WriteLine("test");
			Console.WriteLine("Expected Output " + result);
			Console.WriteLine("Actual Output " + Evaluator.Evaluate("4-1", (string str) => int.Parse(str)));
			Assert.AreEqual(result, Evaluator.Evaluate("4-1", (string str) => int.Parse(str)));
		}
		/// <summary>
		/// tests advanced subtraction
		/// </summary>
		[TestMethod]
		public void TestSubtractionadvanced()
		{
			int result = 4 - 1 - 1 - 1 - 1;
			Console.WriteLine("test");
			Console.WriteLine("Expected Output " + result);
			Console.WriteLine("Actual Output " + Evaluator.Evaluate("4-1-1 -1-1", (string str) => int.Parse(str)));
			Assert.AreEqual(result, Evaluator.Evaluate("4-1-1-1-1", (string str) => int.Parse(str)));
		}

		[TestMethod]
		public void TestDivZero()
		{
			Console.WriteLine("test");
			Assert.ThrowsException<ArgumentException>(() => Evaluator.Evaluate("4/0", (string str) => int.Parse(str)), "Throws improper exception");
		}
		/// <summary>
		/// EvaluatorTests parenthesis
		/// </summary>
		[TestMethod]
		public void MisMatchedright()
		{
			Console.WriteLine("test");
			Assert.ThrowsException<ArgumentException>(() => Evaluator.Evaluate("4 + 4) - 4) * 4", (string str) => int.Parse(str)), "Throws improper exception");
		}
		/// <summary>
		/// tests parenthesis
		/// </summary>
		[TestMethod]
		public void MisMatchedleft()
		{
			Console.WriteLine("test");
			Assert.ThrowsException<ArgumentException>(() => Evaluator.Evaluate("(4 + 4 - 4 * 4", (string str) => int.Parse(str)), "Throws improper exception");
		}/// <summary>
		 /// tests parenthesis
		 /// </summary>

		[TestMethod]
		public void MisMatched2()
		{
			Console.WriteLine("test");
			Assert.ThrowsException<ArgumentException>(() => Evaluator.Evaluate("(4 + 4) - 4) * 4", (string str) => int.Parse(str)), "Throws improper exception");
		}
		/// <summary>
		/// tests if a string is an invalid variable
		/// </summary>
		[TestMethod]
		public void testInvalidTokenString()
		{
			Console.WriteLine("test");
			Assert.ThrowsException<ArgumentException>(() => Evaluator.Evaluate("HAHAHA", (string str) => int.Parse(str)), "Throws improper exception");
		}
		/// <summary>
		/// tests if negative int is a valid token
		/// </summary>
		[TestMethod]
		public void testInvalidTokenNegative()
		{
			Console.WriteLine("test");
			Console.WriteLine("Expected Output " + "Error");
			Assert.ThrowsException<ArgumentException>(() => Evaluator.Evaluate("-5/5", (string str) => int.Parse(str)));

		}
		/// <summary>
		/// tests if a operator with no int in front throws exception
		/// </summary>
		[TestMethod]
		public void testInvalidoperation()
		{
			Console.WriteLine("test");
			Console.WriteLine("Expected Output " + "error");
			Assert.ThrowsException<ArgumentException>(() => Evaluator.Evaluate("+5 - 5", (string str) => int.Parse(str)));

		}
		/// <summary>
		/// tests various operations
		/// </summary>
		[TestMethod]
		public void TestOperations2()
		{
			int result6 = ((44 + 4) / 2) - 1 + 1;
			Console.WriteLine("test");
			Console.WriteLine("Expected Output " + result6);
			Console.WriteLine("Actual Output " + Evaluator.Evaluate("((44+4) / 2)  - 1 + 1", (string str) => int.Parse(str)));
			Assert.AreEqual(result6, Evaluator.Evaluate("((44 + 4) / 2) - 1 + 1", (string str) => int.Parse(str)));
		}
		/// <summary>
		/// tests a single integer
		/// </summary>
		[TestMethod]
		public void testsingleInt()
		{
			Console.WriteLine("test");
			Console.WriteLine("Expected Output " + 1);
			Console.WriteLine("Actual Output " + Evaluator.Evaluate("1", (string str) => int.Parse(str)));
			Assert.AreEqual(1, Evaluator.Evaluate("1", (string str) => int.Parse(str)));
		}
		/// <summary>
		/// Tests an exception if space between 2 numbers
		/// </summary>
		[TestMethod]
		public void testsinglespaceException()
		{

			Assert.ThrowsException<ArgumentException>(() => Evaluator.Evaluate("4 + 4 2", (string str) => int.Parse(str)), "Throws improper exception");
		}
		/// <summary>
		/// tests right side invalid parenthesis
		/// </summary>
		[TestMethod]
		public void testinvalidparenthesisright()
		{
			Assert.ThrowsException<ArgumentException>(() => Evaluator.Evaluate("4 + 2)", (string str) => int.Parse(str)), "Throws improper exception");
		}
		/// <summary>
		/// tests left side parenthesis mismatch
		/// </summary>
		[TestMethod]
		public void testinvalidparenthesisleft()
		{
			Assert.ThrowsException<ArgumentException>(() => Evaluator.Evaluate("((4 + 2)", (string str) => int.Parse(str)), "Throws improper exception");
		}
		/// <summary>
		/// tests variable and number
		/// </summary>
		[TestMethod]
		public void testVariable()
		{
			string str = "A6 + 2";
			Console.WriteLine("Expected Output " + 8);
			Console.WriteLine("Actual Output " + Evaluator.Evaluate(" A6 + B6", variableLookup));
			Assert.AreEqual(8, Evaluator.Evaluate(str, variableLookup));
		}
		/// <summary>
		/// Tests multiple variables and numbers
		/// </summary>
		[TestMethod]
		public void testvariable2()
		{
			string str = "A6 + B2 + C2 + D2 * 2+1+3";
			Console.WriteLine("Expected Output " + 34);
			Console.WriteLine("Actual Output " + Evaluator.Evaluate("A6 + B2 + C2 + D2 * 2+1+3", variableLookup));
			Assert.AreEqual(34, Evaluator.Evaluate(str, variableLookup));
		}
		/// <summary>
		/// tests if the variable is valid
		/// </summary>
		[TestMethod]
		public void TestinvalidVariable()
		{
			Console.WriteLine("test");
			Console.WriteLine("Expected Output: error");
			Assert.ThrowsException<ArgumentException>(() => Evaluator.Evaluate("A4A", variableLookup), "Throws improper exception");
		}
		/// <summary>
		/// Tests if a variable is valid in a longer format
		/// </summary>
		[TestMethod]
		public void testlongvariable()
		{
			string str = "AA6";
			Console.WriteLine("Expected Output " + 6);
			Console.WriteLine("Actual Output " + Evaluator.Evaluate("AA6", variableLookup));
			Assert.AreEqual(6, Evaluator.Evaluate(str, variableLookup));
		}
		/// <summary>
		/// Tests various variables of various lengths
		/// </summary>
		[TestMethod]
		public void testMultiplelongvariable()
		{
			int result = (6 * 6) / 6 - 6 * 2;
			string str = "(AA66 * BB2) / C22 - D12 * 2";
			Console.WriteLine("Expected Output " + result);
			Console.WriteLine("Actual Output " + Evaluator.Evaluate("(AA66 * BB2) / C22 - D12 * 2", variableLookup));
			Assert.AreEqual(result, Evaluator.Evaluate(str, variableLookup));
		}
		/// <summary>
		/// Tests if a given token is invalid
		/// </summary>
		[TestMethod]
		public void testinvalidToken()
		{
			Assert.ThrowsException<ArgumentException>(() => Evaluator.Evaluate("^", (string str) => int.Parse(str)), "Throws improper exception");
		}
		[TestMethod]
		public void testweirdparenthesis()
		{
			string str = "1 + 3 + 4 + 1 + 1 + 4 + 2";
			Assert.AreEqual(16, Evaluator.Evaluate(str, variableLookup));
		}
		/// <summary>
		/// tests the delegate
		/// </summary>
		[TestMethod]
		public void testvariableLookup()
		{
			Assert.AreEqual(6, variableLookup("A6"));
		}

		private static int variableLookup(string variable)
		{

			return 6;
		}
	}
}
