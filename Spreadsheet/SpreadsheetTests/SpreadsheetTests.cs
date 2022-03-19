using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
/// <summary>
/// Author: Devin White
/// </summary>
namespace SpreadsheetTests
{
	[TestClass]
	public class SpreadsheetTests
	{

		/// <summary>
		/// Tests that GetCellContents works as expected
		/// </summary>
		[TestMethod()]
		[TestCategory("Cell Contents")]
		public void testgetCellContents()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.SetContentsOfCell("A1", "  =   b2+c2");
			spreadsheet.SetContentsOfCell("b2", "12");
			spreadsheet.SetContentsOfCell("c3", "hi Buddy");
			Assert.AreEqual(new Formula("b2+c2"), spreadsheet.GetCellContents("A1"));
			Assert.AreEqual(12.0, spreadsheet.GetCellContents("b2"));
			Assert.AreEqual("hi Buddy", spreadsheet.GetCellContents("c3"));

			//write to console for final verification values are as expected
			Console.WriteLine(spreadsheet.GetCellContents("A1"));
			Console.WriteLine(spreadsheet.GetCellContents("b2"));
			Console.WriteLine(spreadsheet.GetCellContents("c3"));
		}
		/// <summary>
		/// Tests that GetCellValue returns the expected value
		/// </summary>
		[TestMethod()]
		[TestCategory("Cell Contents")]
		public void testgetCellValue()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.SetContentsOfCell("b2", "12");
			spreadsheet.SetContentsOfCell("c2", "3");
			spreadsheet.SetContentsOfCell("A1", "  =   b2+c2");
			spreadsheet.SetContentsOfCell("B1", " 2 ");
			spreadsheet.SetContentsOfCell("B3", "=A1 + B1 ");
			spreadsheet.SetContentsOfCell("c3", "hi Buddy");
			Assert.AreEqual(12.0, spreadsheet.GetCellValue("b2"));
			Assert.AreEqual(3.0, spreadsheet.GetCellValue("c2"));
			Assert.AreEqual(15.0, spreadsheet.GetCellValue("A1"));
			Assert.AreEqual(17.0, spreadsheet.GetCellValue("B3"));
			spreadsheet.SetContentsOfCell("b2", "1");
			Assert.AreEqual(4.0, spreadsheet.GetCellValue("A1"));
			Assert.AreEqual(6.0, spreadsheet.GetCellValue("B3"));
			spreadsheet.SetContentsOfCell("c2", "1");
			Assert.AreEqual(2.0, spreadsheet.GetCellValue("A1"));
			Assert.AreEqual(4.0, spreadsheet.GetCellValue("B3"));
			Assert.AreEqual("hi Buddy", spreadsheet.GetCellValue("c3"));

			//write to console for final verification values are as expected
			Console.WriteLine(spreadsheet.GetCellContents("A1"));
			Console.WriteLine(spreadsheet.GetCellContents("b2"));
			Console.WriteLine(spreadsheet.GetCellContents("c3"));
		}
		/// <summary>
		/// Tests that a nonexistent/Empty Cell returns an empty string
		/// </summary>
		[TestMethod()]
		[TestCategory("Cell Contents")]
		public void testgetCellContentsempty()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			Assert.AreEqual("", spreadsheet.GetCellContents("c3"));
		}
		[TestMethod()]
		[TestCategory("Cell Contents")]
		[ExpectedException(typeof(InvalidNameException))]
		public void testinvalidNormalizedVariable()
		{
			Spreadsheet spreadsheet = new Spreadsheet(s => true, s => "6A", "default");
			spreadsheet.SetContentsOfCell("A6", "12");

		}
		/// <summary>
		/// tests that altering the spreadsheet changes changed value to true;
		/// </summary>
		[TestMethod()]
		[TestCategory("Cell Contents")]
		public void testChanged()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.SetContentsOfCell("A6", "12");
			Assert.AreEqual(true, spreadsheet.Changed);

		}
		/// <summary>
		/// tests that a circular dependeny changes changed value to false;
		/// </summary>
		[TestMethod()]
		[TestCategory("Cell Contents")]
		public void testnotChanged()
		{
			try
			{
				Spreadsheet spreadsheet = new Spreadsheet();
				spreadsheet.SetContentsOfCell("A6", "12");
				Assert.AreEqual(true, spreadsheet.Changed);
				spreadsheet.SetContentsOfCell("A6", "=A6");
				Assert.AreEqual(false, spreadsheet.Changed);
			}
			catch (CircularException)
			{ }

		}
		/// <summary>
		/// Tests the Save function works as expected
		/// </summary>
		[TestMethod()]
		[TestCategory("Save Spreadsheet")]
		public void testSaveSpreadsheet()
		{
			Spreadsheet spreadsheetOriginal = new Spreadsheet();
			spreadsheetOriginal.SetContentsOfCell("A6", "2");
			spreadsheetOriginal.Save("save4.txt");
			AbstractSpreadsheet spreadsheet = new Spreadsheet(s => true, s => s, "default");
			spreadsheet.SetContentsOfCell("A6", "12");
			spreadsheet.SetContentsOfCell("A2", "26");
			spreadsheet.SetContentsOfCell("A3", "=A6+A2");
			spreadsheet.SetContentsOfCell("A5", "Hey Buddyy");
			spreadsheet.Save("save4.txt");
			AbstractSpreadsheet spreadsheet2 = new Spreadsheet("save4.txt", s => true, s => s, "default");
			Assert.AreEqual(12.0, spreadsheet2.GetCellContents("A6"));
			Assert.AreEqual(12.0, spreadsheet2.GetCellValue("A6"));
			Assert.AreEqual(new Formula("A6+A2"), spreadsheet2.GetCellContents("A3"));
			Assert.AreEqual(38.0, spreadsheet2.GetCellValue("A3"));
			Console.WriteLine(String.Join(",", spreadsheet2.GetNamesOfAllNonemptyCells()));

		}
		/// <summary>
		/// Tests whether trying to read a nonexistent file throws the expected error
		/// </summary>
		[TestMethod()]
		[TestCategory("Save Spreadsheet")]
		[ExpectedException(typeof(SpreadsheetReadWriteException))]
		public void GetSpreadSheetFileDoesNotExist()
		{
			AbstractSpreadsheet spreadsheet = new Spreadsheet("WhimmyWhimWhamMan.txt", s => true, s => s, "default");

		}
		/// <summary>
		/// Tests whether contents of cell reset in the event setting the conetents to a new formula
		/// causes a circularException
		/// </summary>
		[TestMethod()]
		[TestCategory("Exceptions")]
		[ExpectedException(typeof(CircularException))]
		public void SetCircularExceptionReset()
		{
			List<string> compareList = new List<string>();
			AbstractSpreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.SetContentsOfCell("A1", "12");
			compareList.Add("12");
			Assert.AreEqual(12.0, spreadsheet.GetCellContents("A1"));
			spreadsheet.SetContentsOfCell("A1", "=A1");

		}
		/// <summary>
		/// Tests whether turning a cell with a formula to a cell with a string removes dependencies
		/// </summary>
		[TestMethod()]
		[TestCategory("Cell Contents")]
		public void SetCellContentsFormulaToString()
		{
			List<string> CompareList1 = new List<string>();
			List<string> CompareList2 = new List<string>();
			List<string> CompareList3 = new List<string>();
			AbstractSpreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.SetContentsOfCell("A6", "=A2 + B2");
			Assert.IsInstanceOfType(spreadsheet.GetCellValue("A6"), typeof(FormulaError));
			spreadsheet.SetContentsOfCell("A2", "=6 + 7");
			spreadsheet.SetContentsOfCell("B2", "6");
			CompareList1.Add("A2");
			CompareList1.Add("A6");
			Assert.AreEqual(19.0, spreadsheet.GetCellValue("A6"));
			CollectionAssert.AreEquivalent(CompareList1, (System.Collections.ICollection)spreadsheet.SetContentsOfCell("A2", "=6 + 7"));
			CompareList1.Clear();
			CompareList2.Add("B2");
			CompareList2.Add("A6");
			CollectionAssert.AreEquivalent(CompareList2, (System.Collections.ICollection)spreadsheet.SetContentsOfCell("B2", "6"));
			CompareList2.Clear();
			CompareList3.Add("A6");
			CollectionAssert.AreEquivalent(CompareList3, (System.Collections.ICollection)spreadsheet.SetContentsOfCell("A6", "=A2 + B2"));
			spreadsheet.SetContentsOfCell("A6", "youknowtherulesandsodoI");
			CompareList2.Add("B2");
			CollectionAssert.AreEquivalent(CompareList2, (System.Collections.ICollection)spreadsheet.SetContentsOfCell("B2", "6"));
			CompareList1.Add("A2");
			CollectionAssert.AreEquivalent(CompareList1, (System.Collections.ICollection)spreadsheet.SetContentsOfCell("A2", "=6 + 7"));
		}
		/// <summary>
		/// Tests whether an invalid name in GetCellValue throws the expected exception
		/// </summary>
		[TestMethod()]
		[TestCategory("Cell Value")]
		[ExpectedException(typeof(InvalidNameException))]
		public void GetCellValueInvalidName()
		{
			AbstractSpreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.GetCellValue("6A");
		}
		/// <summary>
		/// Tests whether saving to a nonexistent directory throws the expected exception
		/// </summary>
		[TestMethod()]
		[TestCategory("read Write")]
		[ExpectedException(typeof(SpreadsheetReadWriteException))]
		public void CellSaveReadWriteException()
		{
			AbstractSpreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.Save("/some/nonsense/path.xml");
		}
		/// <summary>
		/// Tests whether trying to read an invalid variable name throws an error
		/// </summary>
		[TestMethod()]
		[TestCategory("Cell Value")]
		[ExpectedException(typeof(SpreadsheetReadWriteException))]
		public void CellReadInvalidVariable()
		{
			//Create Writer
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = "  ";
			XmlWriter writer = XmlWriter.Create("InvalidVar.xml", settings);
			using (writer)
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("Spreadsheet");
				writer.WriteAttributeString("Version", "default");//<Spreadsheet>
				writer.WriteStartElement("Cell");//<cells>
				writer.WriteElementString("Name", "6A"); //<name>
				writer.WriteElementString("Content", "12");
				writer.WriteElementString("Value", "12");
				writer.WriteEndElement(); //ends Cells
				writer.WriteEndDocument();
			}
			AbstractSpreadsheet spreadsheet = new Spreadsheet("invalidVar.txt", s => true, s => s, "default");
		}
		/// <summary>
		/// Tests whether trying to read an invalid variable name throws an error Again
		/// </summary>
		[TestMethod()]
		[TestCategory("Cell Value")]
		[ExpectedException(typeof(SpreadsheetReadWriteException))]
		public void CellReadInvalidVariable2()
		{

			AbstractSpreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.SetContentsOfCell("A2", "12");
			spreadsheet.Save("InvalidVar2.xml");
			AbstractSpreadsheet spreadsheet2 = new Spreadsheet("InvalidVar2.xml", s => false, s => s, "default");

		}
		/// <summary>
		/// Tests whether an invalid path causes as readwriteException
		/// </summary>
		[TestMethod()]
		[TestCategory("Cell Value")]
		[ExpectedException(typeof(SpreadsheetReadWriteException))]
		public void GetSavedReadWriteException()
		{
			AbstractSpreadsheet spreadsheet = new Spreadsheet("/some/nonsense/path.xml", s => true, s => s, "default");
		}
		/// <summary>
		/// Tests whether GetCellscontents with an invalid name throws the expected excetpion
		/// </summary>
		[TestMethod()]
		[TestCategory("Cell Value")]
		[ExpectedException(typeof(InvalidNameException))]
		public void GetCellContentsInvalidName()
		{
			AbstractSpreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.GetCellContents("6A");
		}
		/// <summary>
		/// Tests whether empty cells return an empty string
		/// </summary>
		[TestMethod()]
		[TestCategory("Cell Value")]
		public void GetCellValueEmptyCell()
		{
			AbstractSpreadsheet spreadsheet = new Spreadsheet();
			Assert.AreEqual("", spreadsheet.GetCellValue("A6"));
		}
		/// <summary>
		/// Tests whether the getspreadsheetfile works as expected
		/// </summary>
		[TestMethod()]
		[TestCategory("Save Spreadsheet")]
		public void GetSpreadSheetFile()
		{
			Spreadsheet spreadsheetOriginal = new Spreadsheet();
			spreadsheetOriginal.Save("save2.txt");
			Spreadsheet spreadsheet1 = new Spreadsheet("save2.txt", s => true, s => s, "default");
			AbstractSpreadsheet spreadsheet = new Spreadsheet(s => true, s => s, "default");
			spreadsheet.SetContentsOfCell("A6", "12");
			spreadsheet.SetContentsOfCell("A2", "26");
			spreadsheet.SetContentsOfCell("A3", "12");
			spreadsheet.SetContentsOfCell("A4", "26");
			spreadsheet.SetContentsOfCell("A5", "12");
			spreadsheet.SetContentsOfCell("A1", "26");
			spreadsheet.SetContentsOfCell("A7", "=A6");
			spreadsheet.SetContentsOfCell("A8", "26");
			spreadsheet.SetContentsOfCell("A9", "12");
			spreadsheet.SetContentsOfCell("A10", "26");
			spreadsheet.SetContentsOfCell("A11", "=A7 + A2");
			spreadsheet.SetContentsOfCell("A12", "=A11 + A8");
			spreadsheet.Save("save2.txt");
			AbstractSpreadsheet spreadsheet2 = new Spreadsheet("save2.txt", s => true, s => s, "default");
			Assert.AreEqual(12.0, spreadsheet2.GetCellContents("A6"));
			Assert.AreEqual(12.0, spreadsheet2.GetCellValue("A6"));
			Assert.AreEqual(26.0, spreadsheet2.GetCellContents("A2"));
			Assert.AreEqual(64.0, spreadsheet2.GetCellValue("A12"));
			Console.WriteLine(String.Join(",", spreadsheet2.GetNamesOfAllNonemptyCells()));
		}
		/// <summary>
		/// Tests whether if versions in reader and spreadsheet are mismatched, throws the proper exception
		/// </summary>
		[TestMethod()]
		[TestCategory("Save Spreadsheet")]
		[ExpectedException(typeof(SpreadsheetReadWriteException))]
		public void GetSpreadSheetFileReadWriteExceptionNotMatchingVersions()
		{
			AbstractSpreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.SetContentsOfCell("A6", "12");
			spreadsheet.SetContentsOfCell("A2", "26");
			spreadsheet.SetContentsOfCell("A3", "12");
			spreadsheet.SetContentsOfCell("A4", "26");
			spreadsheet.SetContentsOfCell("A5", "12");
			spreadsheet.SetContentsOfCell("A1", "26");
			spreadsheet.SetContentsOfCell("A7", "A6");
			spreadsheet.SetContentsOfCell("A8", "26");
			spreadsheet.SetContentsOfCell("A9", "12");
			spreadsheet.SetContentsOfCell("A10", "26");
			spreadsheet.SetContentsOfCell("A11", "A7 + A2");
			spreadsheet.SetContentsOfCell("A12", "A11 + A8");
			spreadsheet.Save("save3.txt");
			AbstractSpreadsheet spreadsheet2 = new Spreadsheet("save3.txt", s => true, s => s, "ElectricBoogaloo");
		}

		///The following are PS4 tests modified to work with PS5
		/// <summary>
		/// Tests that changing a formula to an empty cell changes the dependency graph
		/// </summary>
		[TestMethod(), Timeout(2000)]
		[TestCategory("Cell Contents")]
		public void testSetCellContentsemptyDependents()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			List<object> namedCells = new List<object>();

			namedCells.Add("c3");
			spreadsheet.SetContentsOfCell("c3", "=A2+B2");
			int counter = 0;

			//check if the returned list of SetCellContents(c3) returns a list of {c3}
			foreach (string dependent in spreadsheet.SetContentsOfCell("c3", "=A2+B2"))
			{
				Assert.AreEqual(namedCells[counter], dependent);
				counter++;
				Console.WriteLine(dependent); //print for verification
			}
			namedCells.Clear();
			namedCells.Add("A2");
			namedCells.Add("c3");
			counter = 0;
			//List should return {A2, c3}
			foreach (string dependent in spreadsheet.SetContentsOfCell("A2", ""))
			{
				//Assert.AreEqual(namedCells[counter], dependent);
				counter++;
				Console.WriteLine(dependent); //print for verification
			}
			namedCells.Clear();
			namedCells.Add("B2");
			namedCells.Add("c3");
			counter = 0;
			//CList should return {B2, c3}
			foreach (string dependent in spreadsheet.SetContentsOfCell("B2", ""))
			{
				Assert.AreEqual(namedCells[counter], dependent);
				counter++;
				Console.WriteLine(dependent); //print for verification
			}

			//Change c3 to empty cell
			spreadsheet.SetContentsOfCell("c3", "");
			namedCells.Clear();
			namedCells.Add("A2");
			counter = 0;

			//List should now only return {A2}
			foreach (string dependent in spreadsheet.SetContentsOfCell("A2", ""))
			{
				//Assert.AreEqual(namedCells[counter], dependent);
				counter++;
				Console.WriteLine(dependent); //print for verification
			}

			namedCells.Clear();
			namedCells.Add("B2");
			counter = 0;
			//List should now only return {B2}
			foreach (string dependent in spreadsheet.SetContentsOfCell("B2", ""))
			{
				Assert.AreEqual(namedCells[counter], dependent);
				counter++;
				Console.WriteLine(dependent); //print for verification
			}
		}
		/// <summary>
		/// Tests whether SetCellContents works as expected
		/// </summary>
		[TestMethod(), Timeout(2000)]
		[TestCategory("Set Cells")]
		public void testgetSetCellContentsBasic()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			List<string> namedCells = new List<string>();
			namedCells.Add("A1"); //add to list for comparison
			Console.WriteLine(String.Join(",", spreadsheet.SetContentsOfCell("A1", "3")));
			int counter = 0;
			foreach (string cellname in spreadsheet.SetContentsOfCell("A1", "3")) //iterates through cells and list of known cell names and compares them
			{
				Assert.AreEqual(namedCells[counter], cellname);
				counter++;
				Console.WriteLine(cellname); //print for verification
			}
			namedCells.Clear();
			namedCells.Add("B1");
			counter = 0;
			foreach (string cellname in spreadsheet.SetContentsOfCell("B1", "=A1 + A1")) //iterates through cells and list of known cell names and compares them
			{
				Assert.AreEqual(namedCells[counter], cellname);
				counter++;
				Console.WriteLine(cellname); //print for verification
			}
			namedCells.Clear();
			namedCells.Add("A1");
			namedCells.Add("B1");
			counter = 0;
			foreach (string cellname in spreadsheet.SetContentsOfCell("A1", "=4 + 5")) //iterates through cells and list of known cell names and compares them
			{
				Assert.AreEqual(namedCells[counter], cellname);
				counter++;
				Console.WriteLine(cellname); //print for verification
			}

		}
		/// <summary>
		/// Tests that GetCellContents with a double returns a doulbe
		/// </summary>
		[TestMethod(), Timeout(2000)]
		[TestCategory("Cell Contents")]
		public void testgetCellContentsDouble()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			Console.WriteLine(String.Join(",", spreadsheet.SetContentsOfCell("C1", "3")));
			Console.WriteLine(spreadsheet.GetCellContents("C1").ToString());
			Assert.AreEqual(3.0, spreadsheet.GetCellContents("C1"));
		}
		/// <summary>
		/// Tests whether GetNamesOfAllNonemptyCells returns the expected values;
		/// </summary>
		[TestMethod(), Timeout(2000)]
		[TestCategory("Non Empty Cells")]
		public void TestnonemptyCells()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.SetContentsOfCell("a1", "12");
			spreadsheet.SetContentsOfCell("b1", "a2+b2");
			List<string> namedCells = new List<string>(); //List for easy comparison of each expected item
			namedCells.Add("a1");
			namedCells.Add("b1");
			int counter = 0;
			//Foreach loop compares known list with expected returned list
			foreach (string cellname in spreadsheet.GetNamesOfAllNonemptyCells()) //iterates through cells and list of known cell names and compares them
			{
				Assert.AreEqual(namedCells[counter], cellname);
				counter++;
			}
			spreadsheet.SetContentsOfCell("C1", "Hello");
			namedCells.Add("C1");
			counter = 0;
			//Foreach loop compares known list with expected returned list
			foreach (string cellname in spreadsheet.GetNamesOfAllNonemptyCells()) //iterates through cells and list of known cell names and compares them
			{
				Assert.AreEqual(namedCells[counter], cellname);
				counter++;
			}
			foreach (string cellname in spreadsheet.GetNamesOfAllNonemptyCells()) //iterates through cells and list of known cell names and compares them
			{
				Console.WriteLine(cellname); ///Print all names for final verification
			}

		}
		/// <summary>
		/// Tests whether setCellContents throws a circular execption if a set of cells ends up with a circular dependency
		/// </summary>
		[TestMethod(), Timeout(2000)]
		[TestCategory("Set Cells")]
		[ExpectedException(typeof(CircularException))]
		public void TestSetCellContentsCircularException()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.SetContentsOfCell("a2", "=b2+c2");
			spreadsheet.SetContentsOfCell("b2", "=b3+c2");
			spreadsheet.SetContentsOfCell("c2", "=z2 + e2");
			spreadsheet.SetContentsOfCell("e2", "=d2+a2");
		}
		/// <summary>
		/// More testing of SetCellContents to check if the return value is as expected
		/// </summary>
		//B1 contains A1*2, and C1 contains B1+A1
		[TestMethod(), Timeout(2000)]
		[TestCategory("Set Cells")]
		public void TestSetCellContentsDependentsChange()
		{
			int counter = 0;
			Spreadsheet spreadsheet = new Spreadsheet();
			List<string> setCellList = new List<string>();
			setCellList.Add("A1");
			setCellList.Add("B1");
			setCellList.Add("C1");

			spreadsheet.SetContentsOfCell("B1", "=A1*2");
			spreadsheet.SetContentsOfCell("C1", "=B1+A1");
			spreadsheet.SetContentsOfCell("A1", "3");
			//Foreach loop compares known list with expected returned list
			foreach (string item in spreadsheet.SetContentsOfCell("A1", "3"))
			{
				Assert.AreEqual(setCellList[counter], item);
				counter++;
				Console.WriteLine(item);
			}
			///Check that list returns is {A1, B1, C1}
			Console.WriteLine(String.Join(",", spreadsheet.SetContentsOfCell("A1", "3")));
			///Check that B1 List returns {B1, C1}
			Console.WriteLine(String.Join(",", spreadsheet.SetContentsOfCell("B1", "=D1*2")));
			///Check that D1 List returns {D1, B1, C1}
			Console.WriteLine(String.Join(",", spreadsheet.SetContentsOfCell("D1", "3")));

			counter = 0;
			setCellList.Clear();
			setCellList.Add("A1");
			setCellList.Add("C1");
			setCellList.Add("Z1");
			spreadsheet.SetContentsOfCell("Z1", "=D1 + A1");
			spreadsheet.SetContentsOfCell("A1", "  =B1");
			///Changed A1 to depend on B1
			foreach (string item in spreadsheet.SetContentsOfCell("A1", "  =B1"))
			{
				Assert.AreEqual(setCellList[counter], item);
				counter++;
				Console.WriteLine(item);
			}

			counter = 0;
			setCellList.Clear();
			spreadsheet.SetContentsOfCell("C1", "3.0");
			spreadsheet.SetContentsOfCell("A1", " =B1 + C1");
			spreadsheet.SetContentsOfCell("E1", " =A1");
			spreadsheet.SetContentsOfCell("F1", " =D1");
			Console.WriteLine(String.Join(",", spreadsheet.SetContentsOfCell("D1", "3")));

		}
		/// <summary>
		/// Tests SetCellcontents with a series of formulas to see if the return values are correct
		/// </summary>
		[TestMethod(),]
		[TestCategory("Set Cells")]
		public void TestsetCellContentsFormula()
		{

			//Add cells to List and Spreadsheet for comparison
			int counter = 0;
			List<string> setCellList = new List<string>();
			setCellList.Add("A1");
			setCellList.Add("B1");
			setCellList.Add("C1");
			Spreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.SetContentsOfCell("B1", "=A1*2");
			spreadsheet.SetContentsOfCell("C1", "=B1+A1");

			Console.WriteLine("Test 1");
			//Foreach loop compares known list with expected returned list
			foreach (string item in spreadsheet.SetContentsOfCell("A1", "3"))
			{
				Assert.AreEqual(setCellList[counter], item);
				counter++;
				Console.WriteLine(item);
			}

			//Reset counter and List and Add more to list and Spreadsheet for comparison
			counter = 0;
			setCellList.Clear();
			setCellList.Add("D3");
			setCellList.Add("C3");
			setCellList.Add("b2");
			spreadsheet.SetContentsOfCell("C3", "=D3");
			spreadsheet.SetContentsOfCell("D3", "12");
			Console.WriteLine("Test 2");
			//Foreach loop compares known list with expected returned list
			foreach (string item in spreadsheet.SetContentsOfCell("D3", "12"))
			{
				Assert.AreEqual(setCellList[counter], item);
				counter++;
				Console.WriteLine(item);
			}
			//Reset counter and List and Add more to list and Spreadsheet for comparison
			setCellList.Clear();
			setCellList.Add("A6");
			setCellList.Add("Z5");
			setCellList.Add("C4");
			spreadsheet.SetContentsOfCell("A6", "=C4+Z5");
			counter = 0;
			Console.WriteLine("Test 3");
			//Foreach loop compares known list with expected returned list
			foreach (string item in spreadsheet.SetContentsOfCell("A6", "=C4+Z5"))
			{
				Assert.AreEqual(setCellList[counter], item);
				counter++;
				Console.WriteLine(item);
			}
		}
		/// <summary>
		/// Tests whether setCellContents with a double works as expected
		/// </summary>
		[TestMethod(), Timeout(2000)]
		[TestCategory("Set Cells")]
		public void TestsetCellContentsDouble()
		{
			int counter = 0;
			List<object> setCellList = new List<object>();
			Spreadsheet spreadsheet = new Spreadsheet();
			setCellList.Add("A6");
			foreach (string item in spreadsheet.SetContentsOfCell("A6", "12"))
			{
				Assert.AreEqual(setCellList[counter], item);
				counter++;
				Console.WriteLine(item);
			}
		}
		/// <summary>
		/// Tests whether setCellContents with a removed cell works as expected
		/// </summary>
		[TestMethod(), Timeout(2000)]
		[TestCategory("Set Cells")]
		public void TestsetCellContentsRemove()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			List<object> setCellList = new List<object>();
			setCellList.Add("B1");
			setCellList.Add("A1");
			spreadsheet.SetContentsOfCell("A1", "=B1");
			int counter = 0;

			//Foreach loop compares known list with expected returned list
			foreach (string item in spreadsheet.SetContentsOfCell("B1", "2"))
			{
				Assert.AreEqual(setCellList[counter], item);
				counter++;
				Console.WriteLine(item);
			}
			///Remove A1 to see if B1 will then only return B1
			spreadsheet.SetContentsOfCell("A1", "");
			setCellList.Remove("A1");
			counter = 0;
			//Foreach loop compares known list with expected returned list
			foreach (string item in spreadsheet.SetContentsOfCell("B1", "2"))
			{
				Assert.AreEqual(setCellList[counter], item);
				counter++;
				Console.WriteLine(item);
			}

			counter = 0;
			///Tests that an empty cell will return itself
			setCellList.Clear();
			setCellList.Add("A1");
			foreach (string item in spreadsheet.SetContentsOfCell("A1", ""))
			{
				Assert.AreEqual(setCellList[counter], item);
				counter++;
				Console.WriteLine(item);
			}
		}
		/// <summary>
		/// Tests that setting the contents to an empty string removes the cell
		/// </summary>
		[TestMethod(), Timeout(2000)]
		[TestCategory("Non Empty Cells")]
		public void TestRemoveEmptyCell()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.SetContentsOfCell("Z00", "Cat");
			spreadsheet.SetContentsOfCell("Z0", "Cool Cat");
			Assert.AreEqual("Cat", spreadsheet.GetCellContents("Z00"));
			foreach (string emptyCell in spreadsheet.GetNamesOfAllNonemptyCells())
			{
				Console.WriteLine(emptyCell);
			}
			spreadsheet.SetContentsOfCell("Z00", "");
			foreach (string emptyCell in spreadsheet.GetNamesOfAllNonemptyCells())
			{
				Console.WriteLine(emptyCell);
			}
		}
		/// <summary>
		/// tests whether setcellcontents with an invalid name throws the expected exception
		/// </summary>
		[TestMethod(), Timeout(2000)]
		[TestCategory("Set Cells")]
		[ExpectedException(typeof(InvalidNameException))]
		public void testinvalidNameSetCellContents()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.SetContentsOfCell("6a", "7b");
		}
		/// <summary>
		/// tests whether a null name throws the expected exception
		/// </summary>
		[TestMethod(), Timeout(2000)]
		[TestCategory("Set Cells")]
		[ExpectedException(typeof(InvalidNameException))]
		public void testinvalidNameNull()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.SetContentsOfCell(null, "b7");
		}
		/// <summary>
		/// Tests whether a null string throws the expected exception
		/// </summary>
		[TestMethod(), Timeout(2000)]
		[TestCategory("Set Cells")]
		[ExpectedException(typeof(ArgumentNullException))]
		public void testErrorNullText()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.SetContentsOfCell("6a", (string)null);
		}
		/// <summary>
		/// Tests whether a formula of null throws the expected Exception
		/// </summary>
		[TestMethod(), Timeout(2000)]
		[TestCategory("Set Cells")]
		[ExpectedException(typeof(ArgumentNullException))]
		public void testErrorSetNullFormula()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.SetContentsOfCell("a6", null);
		}
		/// <summary>
		/// Tests whether get cell contents throws the expected exception with a null value
		/// </summary>
		[TestMethod(), Timeout(2000)]
		[TestCategory("Cell Contents")]
		[ExpectedException(typeof(InvalidNameException))]
		public void testGetCellContentsNull()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.GetCellContents(null);
		}
		/// <summary>
		/// Tests whether an invalid name in GetCellContents throws an exception
		/// </summary>
		[TestMethod(), Timeout(2000)]
		[TestCategory("Cell Contents")]
		[ExpectedException(typeof(InvalidNameException))]
		public void testGetCellContentsInvalidName()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.GetCellContents("65a");
		}
		/// <summary>
		/// Tests whether an invalid name in GetDirectDependents throws an exception
		/// </summary>
		[TestMethod()]
		[TestCategory("Cell Contents")]
		[ExpectedException(typeof(InvalidNameException))]
		public void testGetDirectDependentsInvalidName()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			spreadsheet.GetCellContents("65a");
		}
		/// <summary>
		/// Stress Test by adding a bunch of cells and invoking various methods
		/// </summary>
		[TestMethod()]
		[TestCategory("Stress Tests")]
		public void StressTest()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			List<string> NonEmptyCellList = new List<string>();
			//Set a bunch of cells, add those nonempty cells to the list of nonempty cells
			for (int i = 2; i < 500; i++)
			{
				//Create a bunch of new cells in the spreadsheet(About 13,000)
				spreadsheet.SetContentsOfCell("A" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("B" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("C" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("D" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("E" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("F" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("G" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("H" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("I" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("J" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("K" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("L" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("M" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("N" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("O" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("P" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("Q" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("R" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("S" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("T" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("U" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("V" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("W" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("X" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("Y" + i, "=A1 + A1");
				spreadsheet.SetContentsOfCell("Z" + i, "=A1 + A1");


				//Create a List of known "Cells" in the Spreadsheet for comparison of return value
				NonEmptyCellList.Add("A" + i);
				NonEmptyCellList.Add("B" + i);
				NonEmptyCellList.Add("C" + i);
				NonEmptyCellList.Add("D" + i);
				NonEmptyCellList.Add("E" + i);
				NonEmptyCellList.Add("F" + i);
				NonEmptyCellList.Add("G" + i);
				NonEmptyCellList.Add("H" + i);
				NonEmptyCellList.Add("I" + i);
				NonEmptyCellList.Add("J" + i);
				NonEmptyCellList.Add("K" + i);
				NonEmptyCellList.Add("L" + i);
				NonEmptyCellList.Add("M" + i);
				NonEmptyCellList.Add("N" + i);
				NonEmptyCellList.Add("O" + i);
				NonEmptyCellList.Add("P" + i);
				NonEmptyCellList.Add("Q" + i);
				NonEmptyCellList.Add("R" + i);
				NonEmptyCellList.Add("S" + i);
				NonEmptyCellList.Add("T" + i);
				NonEmptyCellList.Add("U" + i);
				NonEmptyCellList.Add("V" + i);
				NonEmptyCellList.Add("W" + i);
				NonEmptyCellList.Add("X" + i);
				NonEmptyCellList.Add("Y" + i);
				NonEmptyCellList.Add("Z" + i);
			}
			///Test setcellContents returns as expected
			NonEmptyCellList.Add("A1");
			CollectionAssert.AreEquivalent(NonEmptyCellList, (System.Collections.ICollection)spreadsheet.SetContentsOfCell("A1", "12"));
			int counter = NonEmptyCellList.Count - 1;
			foreach (string dependent in spreadsheet.SetContentsOfCell("A1", "12"))
			{
				Assert.AreEqual(NonEmptyCellList[counter], dependent);
				counter--;
			}

			///Tests getNamesOfAllNonemptyCells returns as expected
			counter = 0;
			foreach (string dependent in spreadsheet.GetNamesOfAllNonemptyCells())
			{
				Assert.AreEqual(NonEmptyCellList[counter], dependent);
				counter++;
			}
		}
		[TestMethod()]
		[TestCategory("Stress Tests")]
		public void InsaneCellValueTest()
		{
			Spreadsheet spreadsheet = new Spreadsheet();
			List<string> NonEmptyCellList = new List<string>();
			//Set a bunch of cells, add those nonempty cells to the list of nonempty cells
			for (int i = 2; i < 500; i++)
			{
				//Create a bunch of new cells in the spreadsheet(About 13,000)
				spreadsheet.SetContentsOfCell("A" + i, "=A" + (i - 1) + "+" + "B" + (i - 1));
				spreadsheet.SetContentsOfCell("B" + i, "=B" + (i - 1) + "+" + "C" + (i - 1));
				spreadsheet.SetContentsOfCell("C" + i, "=C" + (i - 1) + "+" + "D" + (i - 1));
				spreadsheet.SetContentsOfCell("D" + i, "=D" + (i - 1) + "+" + "E" + (i - 1));
				spreadsheet.SetContentsOfCell("E" + i, "=E" + (i - 1) + "+" + "F" + (i - 1));
				spreadsheet.SetContentsOfCell("F" + i, "=F" + (i - 1) + "+" + "G" + (i - 1));
				spreadsheet.SetContentsOfCell("G" + i, "=G" + (i - 1) + "+" + "H" + (i - 1));
				spreadsheet.SetContentsOfCell("H" + i, "=H" + (i - 1) + "+" + "I" + (i - 1));
				spreadsheet.SetContentsOfCell("I" + i, "=I" + (i - 1) + "+" + "J" + (i - 1));
				spreadsheet.SetContentsOfCell("J" + i, "=J" + (i - 1) + "+" + "K" + (i - 1));
				spreadsheet.SetContentsOfCell("K" + i, "=K" + (i - 1) + "+" + "L" + (i - 1));
				spreadsheet.SetContentsOfCell("L" + i, "=L" + (i - 1) + "+" + "M" + (i - 1));
				spreadsheet.SetContentsOfCell("M" + i, "=M" + (i - 1) + "+" + "N" + (i - 1));
				spreadsheet.SetContentsOfCell("N" + i, "=N" + (i - 1) + "+" + "O" + (i - 1));
				spreadsheet.SetContentsOfCell("O" + i, "=O" + (i - 1) + "+" + "P" + (i - 1));
				spreadsheet.SetContentsOfCell("P" + i, "=P" + (i - 1) + "+" + "Q" + (i - 1));
				spreadsheet.SetContentsOfCell("Q" + i, "=Q" + (i - 1) + "+" + "R" + (i - 1));
				spreadsheet.SetContentsOfCell("R" + i, "=R" + (i - 1) + "+" + "S" + (i - 1));
				spreadsheet.SetContentsOfCell("S" + i, "=S" + (i - 1) + "+" + "T" + (i - 1));
				spreadsheet.SetContentsOfCell("T" + i, "=T" + (i - 1) + "+" + "U" + (i - 1));
				spreadsheet.SetContentsOfCell("U" + i, "=U" + (i - 1) + "+" + "V" + (i - 1));
				spreadsheet.SetContentsOfCell("V" + i, "=V" + (i - 1) + "+" + "W" + (i - 1));
				spreadsheet.SetContentsOfCell("W" + i, "=W" + (i - 1) + "+" + "X" + (i - 1));
				spreadsheet.SetContentsOfCell("X" + i, "=X" + (i - 1) + "+" + "Y" + (i - 1));
				spreadsheet.SetContentsOfCell("Y" + i, "=Y" + (i - 1) + "+" + "Z" + (i - 1));
				spreadsheet.SetContentsOfCell("Z" + i, "=" + i);
			}
			spreadsheet.SetContentsOfCell("A1", "1");
			spreadsheet.SetContentsOfCell("B1", "2");
			spreadsheet.SetContentsOfCell("C1", "3");
			spreadsheet.SetContentsOfCell("D1", "3");
			spreadsheet.SetContentsOfCell("E1", "4");
			spreadsheet.SetContentsOfCell("F1", "5");
			spreadsheet.SetContentsOfCell("G1", "6");
			spreadsheet.SetContentsOfCell("H1", "7");
			spreadsheet.SetContentsOfCell("I1", "8");
			spreadsheet.SetContentsOfCell("J1", "9");
			spreadsheet.SetContentsOfCell("K1", "10");
			spreadsheet.SetContentsOfCell("L1", "11");
			spreadsheet.SetContentsOfCell("M1", "12");
			spreadsheet.SetContentsOfCell("N1", "13");
			spreadsheet.SetContentsOfCell("O1", "14");
			spreadsheet.SetContentsOfCell("P1", "15");
			spreadsheet.SetContentsOfCell("Q1", "16");
			spreadsheet.SetContentsOfCell("R1", "17");
			spreadsheet.SetContentsOfCell("S1", "18");
			spreadsheet.SetContentsOfCell("T1", "19");
			spreadsheet.SetContentsOfCell("U1", "20");
			spreadsheet.SetContentsOfCell("V1", "21");
			spreadsheet.SetContentsOfCell("W1", "22");
			spreadsheet.SetContentsOfCell("X1", "23");
			spreadsheet.SetContentsOfCell("Y1", "24");
			spreadsheet.SetContentsOfCell("Z1", "25");
			Console.WriteLine(spreadsheet.GetCellValue("A499").ToString());
			Console.WriteLine(spreadsheet.GetCellValue("B499").ToString());
			spreadsheet.SetContentsOfCell("A1", "100000000000000000000000000000000000000000000000");
			spreadsheet.SetContentsOfCell("B1", "100000000000000000000000000000000000000000000000");
			Console.WriteLine(spreadsheet.GetCellValue("A499").ToString());
			Console.WriteLine(spreadsheet.GetCellValue("B499").ToString());
			spreadsheet.SetContentsOfCell("A1", "=Z1");
			Console.WriteLine(spreadsheet.GetCellValue("A499").ToString());
			Console.WriteLine(spreadsheet.GetCellValue("B499").ToString());


		}
		/*Class Grading Tests
		 * These were Not written by Me!!!
		 * But were modified by me to work correctly with PS5
		 */


		/// <summary>
		///This is a test class for SpreadsheetTest and is intended
		///to contain all SpreadsheetTest Unit Tests
		///</summary>
		[TestClass()]
		public class SpreadsheetTest
		{

			// EMPTY SPREADSHEETS
			[TestMethod(), Timeout(2000)]
			[TestCategory("1")]
			[ExpectedException(typeof(InvalidNameException))]
			public void TestEmptyGetNull()
			{
				Spreadsheet s = new Spreadsheet();
				s.GetCellContents(null);
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("2")]
			[ExpectedException(typeof(InvalidNameException))]
			public void TestEmptyGetContents()
			{
				Spreadsheet s = new Spreadsheet();
				s.GetCellContents("1AA");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("3")]
			public void TestGetEmptyContents()
			{
				Spreadsheet s = new Spreadsheet();
				Assert.AreEqual("", s.GetCellContents("A2"));
			}

			// SETTING CELL TO A DOUBLE
			[TestMethod(), Timeout(2000)]
			[TestCategory("4")]
			[ExpectedException(typeof(InvalidNameException))]
			public void TestSetNullDouble()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell(null, "1.5");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("5")]
			[ExpectedException(typeof(InvalidNameException))]
			public void TestSetInvalidNameDouble()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("1A1A", "1.5");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("6")]
			public void TestSimpleSetDouble()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("Z7", "1.5");
				Assert.AreEqual(1.5, (double)s.GetCellContents("Z7"), 1e-9);
			}

			// SETTING CELL TO A STRING
			[TestMethod(), Timeout(2000)]
			[TestCategory("7")]
			[ExpectedException(typeof(ArgumentNullException))]
			public void TestSetNullStringVal()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("A8", (string)null);
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("8")]
			[ExpectedException(typeof(InvalidNameException))]
			public void TestSetNullStringName()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell(null, "hello");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("9")]
			[ExpectedException(typeof(InvalidNameException))]
			public void TestSetSimpleString()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("1AZ", "hello");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("10")]
			public void TestSetGetSimpleString()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("Z7", "hello");
				Assert.AreEqual("hello", s.GetCellContents("Z7"));
			}

			// SETTING CELL TO A FORMULA
			[TestMethod(), Timeout(2000)]
			[TestCategory("11")]
			[ExpectedException(typeof(ArgumentNullException))]
			public void TestSetNullFormVal()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("A8", null);
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("12")]
			[ExpectedException(typeof(InvalidNameException))]
			public void TestSetNullFormName()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell(null, "=2");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("13")]
			[ExpectedException(typeof(InvalidNameException))]
			public void TestSetSimpleForm()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("1AZ", "=2");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("14")]
			public void TestSetGetForm()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("Z7", "=3");
				Formula f = (Formula)s.GetCellContents("Z7");
				Assert.AreEqual(new Formula("3"), f);
				Assert.AreNotEqual(new Formula("2"), f);
			}

			// CIRCULAR FORMULA DETECTION
			[TestMethod(), Timeout(2000)]
			[TestCategory("15")]
			[ExpectedException(typeof(CircularException))]
			public void TestSimpleCircular()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("A1", "=A2");
				s.SetContentsOfCell("A2", "=A1");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("16")]
			[ExpectedException(typeof(CircularException))]
			public void TestComplexCircular()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("A1", "=A2+A3");
				s.SetContentsOfCell("A3", "=A4+A5");
				s.SetContentsOfCell("A5", "=A6+A7");
				s.SetContentsOfCell("A7", "=A1+A1");
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("17")]
			[ExpectedException(typeof(CircularException))]
			public void TestUndoCircular()
			{
				Spreadsheet s = new Spreadsheet();
				try
				{
					s.SetContentsOfCell("A1", "=A2+A3");
					s.SetContentsOfCell("A2", "15");
					s.SetContentsOfCell("A3", "30");
					s.SetContentsOfCell("A2", "=A3*A1");
				}
				catch (CircularException e)
				{
					Assert.AreEqual(15, (double)s.GetCellContents("A2"), 1e-9);
					throw e;
				}
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("17b")]
			[ExpectedException(typeof(CircularException))]
			public void TestUndoCellsCircular()
			{
				Spreadsheet s = new Spreadsheet();
				try
				{
					s.SetContentsOfCell("A1", "=A2");
					s.SetContentsOfCell("A2", "=A1");
				}
				catch (CircularException e)
				{
					Assert.AreEqual("", s.GetCellContents("A2"));
					Assert.IsTrue(new HashSet<string> { "A1" }.SetEquals(s.GetNamesOfAllNonemptyCells()));
					throw e;
				}
			}

			// NONEMPTY CELLS
			[TestMethod(), Timeout(2000)]
			[TestCategory("18")]
			public void TestEmptyNames()
			{
				Spreadsheet s = new Spreadsheet();
				Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("19")]
			public void TestExplicitEmptySet()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("B1", "");
				Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
			}

			[TestMethod()]
			[TestCategory("20")]
			public void TestSimpleNamesString()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("B1", "hello");
				Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
			}

			[TestMethod()]
			[TestCategory("21")]
			public void TestSimpleNamesDouble()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("B1", "52.25");
				Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("22")]
			public void TestSimpleNamesFormula()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("B1", "=3.5");
				Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("23")]
			public void TestMixedNames()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("A1", "17.2");
				s.SetContentsOfCell("C1", "hello");
				s.SetContentsOfCell("B1", "=3.5");
				Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "A1", "B1", "C1" }));
			}

			// RETURN VALUE OF SET CELL CONTENTS
			[TestMethod(), Timeout(2000)]
			[TestCategory("24")]
			public void TestSetSingletonDouble()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("B1", "hello");
				s.SetContentsOfCell("C1", "=5");
				Assert.IsTrue(s.SetContentsOfCell("A1", "17.2").SequenceEqual(new List<string>() { "A1" }));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("25")]
			public void TestSetSingletonString()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("A1", "17.2");
				s.SetContentsOfCell("C1", "=5");
				Assert.IsTrue(s.SetContentsOfCell("B1", "hello").SequenceEqual(new List<string>() { "B1" }));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("26")]
			public void TestSetSingletonFormula()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("A1", "17.2");
				s.SetContentsOfCell("B1", "hello");
				Assert.IsTrue(s.SetContentsOfCell("C1", "=5").SequenceEqual(new List<string>() { "C1" }));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("27")]
			public void TestSetChain()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("A1", "=A2+A3");
				s.SetContentsOfCell("A2", "6");
				s.SetContentsOfCell("A3", "=A2+A4");
				s.SetContentsOfCell("A4", "=A2+A5");
				Assert.IsTrue(s.SetContentsOfCell("A5", "82.5").SequenceEqual(new List<string>() { "A5", "A4", "A3", "A1" }));
			}

			// CHANGING CELLS
			[TestMethod(), Timeout(2000)]
			[TestCategory("28")]
			public void TestChangeFtoD()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("A1", "=A2+A3");
				s.SetContentsOfCell("A1", "2.5");
				Assert.AreEqual(2.5, (double)s.GetCellContents("A1"), 1e-9);
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("29")]
			public void TestChangeFtoS()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("A1", "=A2+A3");
				s.SetContentsOfCell("A1", "Hello");
				Assert.AreEqual("Hello", (string)s.GetCellContents("A1"));
			}

			[TestMethod(), Timeout(2000)]
			[TestCategory("30")]
			public void TestChangeStoF()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("A1", "Hello");
				s.SetContentsOfCell("A1", "=23");
				Assert.AreEqual(new Formula("23"), s.GetCellContents("A1"));
				Assert.AreNotEqual(24, s.GetCellContents("A1"));
			}

			// STRESS TESTS
			[TestMethod(), Timeout(2000)]
			[TestCategory("31")]
			public void TestStress1()
			{
				Spreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("A1", "=B1+B2");
				s.SetContentsOfCell("B1", "=C1-C2");
				s.SetContentsOfCell("B2", "=C3*C4");
				s.SetContentsOfCell("C1", "=D1*D2");
				s.SetContentsOfCell("C2", "=D3*D4");
				s.SetContentsOfCell("C3", "=D5*D6");
				s.SetContentsOfCell("C4", "=D7*D8");
				s.SetContentsOfCell("D1", "=E1");
				s.SetContentsOfCell("D2", "=E1");
				s.SetContentsOfCell("D3", "=E1");
				s.SetContentsOfCell("D4", "=E1");
				s.SetContentsOfCell("D5", "=E1");
				s.SetContentsOfCell("D6", "=E1");
				s.SetContentsOfCell("D7", "=E1");
				s.SetContentsOfCell("D8", "=E1");
				IList<String> cells = s.SetContentsOfCell("E1", "0");
				Assert.IsTrue(new HashSet<string>() { "A1", "B1", "B2", "C1", "C2", "C3", "C4", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "E1" }.SetEquals(cells));
			}

			// Repeated for extra weight
			[TestMethod(), Timeout(2000)]
			[TestCategory("32")]
			public void TestStress1a()
			{
				TestStress1();
			}
			[TestMethod(), Timeout(2000)]
			[TestCategory("33")]
			public void TestStress1b()
			{
				TestStress1();
			}
			[TestMethod(), Timeout(2000)]
			[TestCategory("34")]
			public void TestStress1c()
			{
				TestStress1();
			}

			[TestMethod()]
			[TestCategory("35")]
			public void TestStress2()
			{
				Spreadsheet s = new Spreadsheet();
				ISet<String> cells = new HashSet<string>();
				for (int i = 1; i < 200; i++)
				{
					cells.Add("A" + i);
					Assert.IsTrue(cells.SetEquals(s.SetContentsOfCell("A" + i, "=A" + (i + 1))));
				}
			}
			[TestMethod()]
			[TestCategory("36")]
			public void TestStress2a()
			{
				TestStress2();
			}
			[TestMethod()]
			[TestCategory("37")]
			public void TestStress2b()
			{
				TestStress2();
			}
			[TestMethod()]
			[TestCategory("38")]
			public void TestStress2c()
			{
				TestStress2();
			}

			[TestMethod()]
			[TestCategory("39")]
			public void TestStress3()
			{
				Spreadsheet s = new Spreadsheet();
				for (int i = 1; i < 200; i++)
				{
					s.SetContentsOfCell("A" + i, "=A" + (i + 1));
				}
				try
				{
					s.SetContentsOfCell("A150", "=A50");
					Assert.Fail();
				}
				catch (CircularException)
				{
				}
			}

			[TestMethod()]
			[TestCategory("40")]
			public void TestStress3a()
			{
				TestStress3();
			}
			[TestMethod()]
			[TestCategory("41")]
			public void TestStress3b()
			{
				TestStress3();
			}
			[TestMethod()]
			[TestCategory("42")]
			public void TestStress3c()
			{
				TestStress3();
			}

			[TestMethod()]
			[TestCategory("43")]
			public void TestStress4()
			{
				Spreadsheet s = new Spreadsheet();
				for (int i = 0; i < 500; i++)
				{
					s.SetContentsOfCell("A1" + i, "=A1" + (i + 1));
				}
				LinkedList<string> firstCells = new LinkedList<string>();
				LinkedList<string> lastCells = new LinkedList<string>();
				for (int i = 0; i < 250; i++)
				{
					firstCells.AddFirst("A1" + i);
					lastCells.AddFirst("A1" + (i + 250));
				}
				Assert.IsTrue(s.SetContentsOfCell("A1249", "25.0").SequenceEqual(firstCells));
				Assert.IsTrue(s.SetContentsOfCell("A1499", "0").SequenceEqual(lastCells));
			}
			[TestMethod()]
			[TestCategory("44")]
			public void TestStress4a()
			{
				TestStress4();
			}
			[TestMethod()]
			[TestCategory("45")]
			public void TestStress4b()
			{
				TestStress4();
			}
			[TestMethod()]
			[TestCategory("46")]
			public void TestStress4c()
			{
				TestStress4();
			}

			[TestMethod()]
			[TestCategory("47")]
			public void TestStress5()
			{
				RunRandomizedTest(47, 2519);
			}

			[TestMethod()]
			[TestCategory("48")]
			public void TestStress6()
			{
				RunRandomizedTest(48, 2521);
			}

			[TestMethod()]
			[TestCategory("49")]
			public void TestStress7()
			{
				RunRandomizedTest(49, 2526);
			}

			[TestMethod()]
			[TestCategory("50")]
			public void TestStress8()
			{
				RunRandomizedTest(50, 2521);
			}

			/// <summary>
			/// Sets random contents for a random cell 10000 times
			/// </summary>
			/// <param name="seed">Random seed</param>
			/// <param name="size">The known resulting spreadsheet size, given the seed</param>
			public void RunRandomizedTest(int seed, int size)
			{
				Spreadsheet s = new Spreadsheet();
				Random rand = new Random(seed);
				for (int i = 0; i < 10000; i++)
				{
					try
					{
						switch (rand.Next(3))
						{
							case 0:
								s.SetContentsOfCell(randomName(rand), "3.14");
								break;
							case 1:
								s.SetContentsOfCell(randomName(rand), "hello");
								break;
							case 2:
								s.SetContentsOfCell(randomName(rand), randomFormula(rand));
								break;
						}
					}
					catch (CircularException)
					{
					}
				}
				ISet<string> set = new HashSet<string>(s.GetNamesOfAllNonemptyCells());
				Assert.AreEqual(size, set.Count);
			}

			/// <summary>
			/// Generates a random cell name with a capital letter and number between 1 - 99
			/// </summary>
			/// <param name="rand"></param>
			/// <returns></returns>
			private String randomName(Random rand)
			{
				return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(rand.Next(26), 1) + (rand.Next(99) + 1);
			}

			/// <summary>
			/// Generates a random Formula
			/// </summary>
			/// <param name="rand"></param>
			/// <returns></returns>
			private String randomFormula(Random rand)
			{
				String f = randomName(rand);
				for (int i = 0; i < 10; i++)
				{
					switch (rand.Next(4))
					{
						case 0:
							f += "+";
							break;
						case 1:
							f += "-";
							break;
						case 2:
							f += "*";
							break;
						case 3:
							f += "/";
							break;
					}
					switch (rand.Next(2))
					{
						case 0:
							f += 7.2;
							break;
						case 1:
							f += randomName(rand);
							break;
					}
				}
				return f;
			}

		}
	}
	namespace SpreadsheetTester
	{
		/// <summary>
		///This is a test class for SpreadsheetTest and is intended
		///to contain all SpreadsheetTest Unit Tests
		///</summary>
		[TestClass()]
		public class GradingTests
		{

			// Verifies cells and their values, which must alternate.
			public void VV(AbstractSpreadsheet sheet, params object[] constraints)
			{
				for (int i = 0; i < constraints.Length; i += 2)
				{
					if (constraints[i + 1] is double)
					{
						Assert.AreEqual((double)constraints[i + 1], (double)sheet.GetCellValue((string)constraints[i]), 1e-9);
					}
					else
					{
						Assert.AreEqual(constraints[i + 1], sheet.GetCellValue((string)constraints[i]));
					}
				}
			}


			// For setting a spreadsheet cell.
			public IEnumerable<string> Set(AbstractSpreadsheet sheet, string name, string contents)
			{
				List<string> result = new List<string>(sheet.SetContentsOfCell(name, contents));
				return result;
			}

			// Tests IsValid
			[TestMethod, Timeout(2000)]
			[TestCategory("1")]
			public void IsValidTest1()
			{
				AbstractSpreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("A1", "x");
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("2")]
			[ExpectedException(typeof(InvalidNameException))]
			public void IsValidTest2()
			{
				AbstractSpreadsheet ss = new Spreadsheet(s => s[0] != 'A', s => s, "");
				ss.SetContentsOfCell("A1", "x");
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("3")]
			public void IsValidTest3()
			{
				AbstractSpreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("B1", "= A1 + C1");
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("4")]
			[ExpectedException(typeof(FormulaFormatException))]
			public void IsValidTest4()
			{
				AbstractSpreadsheet ss = new Spreadsheet(s => s[0] != 'A', s => s, "");
				ss.SetContentsOfCell("B1", "= A1 + C1");
			}

			// Tests Normalize
			[TestMethod, Timeout(2000)]
			[TestCategory("5")]
			public void NormalizeTest1()
			{
				AbstractSpreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("B1", "hello");
				Assert.AreEqual("", s.GetCellContents("b1"));
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("6")]
			public void NormalizeTest2()
			{
				AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "");
				ss.SetContentsOfCell("B1", "hello");
				Assert.AreEqual("hello", ss.GetCellContents("b1"));
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("7")]
			public void NormalizeTest3()
			{
				AbstractSpreadsheet s = new Spreadsheet();
				s.SetContentsOfCell("a1", "5");
				s.SetContentsOfCell("A1", "6");
				s.SetContentsOfCell("B1", "= a1");
				Assert.AreEqual(5.0, (double)s.GetCellValue("B1"), 1e-9);
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("8")]
			public void NormalizeTest4()
			{
				AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "");
				ss.SetContentsOfCell("a1", "5");
				ss.SetContentsOfCell("A1", "6");
				ss.SetContentsOfCell("B1", "= a1");
				Assert.AreEqual(6.0, (double)ss.GetCellValue("B1"), 1e-9);
			}

			// Simple tests
			[TestMethod, Timeout(2000)]
			[TestCategory("9")]
			public void EmptySheet()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				VV(ss, "A1", "");
			}


			[TestMethod, Timeout(2000)]
			[TestCategory("10")]
			public void OneString()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				OneString(ss);
			}

			public void OneString(AbstractSpreadsheet ss)
			{
				Set(ss, "B1", "hello");
				VV(ss, "B1", "hello");
			}


			[TestMethod, Timeout(2000)]
			[TestCategory("11")]
			public void OneNumber()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				OneNumber(ss);
			}

			public void OneNumber(AbstractSpreadsheet ss)
			{
				Set(ss, "C1", "17.5");
				VV(ss, "C1", 17.5);
			}


			[TestMethod, Timeout(2000)]
			[TestCategory("12")]
			public void OneFormula()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				OneFormula(ss);
			}

			public void OneFormula(AbstractSpreadsheet ss)
			{
				Set(ss, "A1", "4.1");
				Set(ss, "B1", "5.2");
				Set(ss, "C1", "= A1+B1");
				VV(ss, "A1", 4.1, "B1", 5.2, "C1", 9.3);
			}


			[TestMethod, Timeout(2000)]
			[TestCategory("13")]
			public void ChangedAfterModify()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				Assert.IsFalse(ss.Changed);
				Set(ss, "C1", "17.5");
				Assert.IsTrue(ss.Changed);
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("13b")]
			public void UnChangedAfterSave()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				Set(ss, "C1", "17.5");
				ss.Save("changed.txt");
				Assert.IsFalse(ss.Changed);
			}


			[TestMethod, Timeout(2000)]
			[TestCategory("14")]
			public void DivisionByZero1()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				DivisionByZero1(ss);
			}

			public void DivisionByZero1(AbstractSpreadsheet ss)
			{
				Set(ss, "A1", "4.1");
				Set(ss, "B1", "0.0");
				Set(ss, "C1", "= A1 / B1");
				Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("15")]
			public void DivisionByZero2()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				DivisionByZero2(ss);
			}

			public void DivisionByZero2(AbstractSpreadsheet ss)
			{
				Set(ss, "A1", "5.0");
				Set(ss, "A3", "= A1 / 0.0");
				Assert.IsInstanceOfType(ss.GetCellValue("A3"), typeof(FormulaError));
			}



			[TestMethod, Timeout(2000)]
			[TestCategory("16")]
			public void EmptyArgument()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				EmptyArgument(ss);
			}

			public void EmptyArgument(AbstractSpreadsheet ss)
			{
				Set(ss, "A1", "4.1");
				Set(ss, "C1", "= A1 + B1");
				Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
			}


			[TestMethod, Timeout(2000)]
			[TestCategory("17")]
			public void StringArgument()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				StringArgument(ss);
			}

			public void StringArgument(AbstractSpreadsheet ss)
			{
				Set(ss, "A1", "4.1");
				Set(ss, "B1", "hello");
				Set(ss, "C1", "= A1 + B1");
				Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
			}


			[TestMethod, Timeout(2000)]
			[TestCategory("18")]
			public void ErrorArgument()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				ErrorArgument(ss);
			}

			public void ErrorArgument(AbstractSpreadsheet ss)
			{
				Set(ss, "A1", "4.1");
				Set(ss, "B1", "");
				Set(ss, "C1", "= A1 + B1");
				Set(ss, "D1", "= C1");
				Assert.IsInstanceOfType(ss.GetCellValue("D1"), typeof(FormulaError));
			}


			[TestMethod, Timeout(2000)]
			[TestCategory("19")]
			public void NumberFormula1()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				NumberFormula1(ss);
			}

			public void NumberFormula1(AbstractSpreadsheet ss)
			{
				Set(ss, "A1", "4.1");
				Set(ss, "C1", "= A1 + 4.2");
				VV(ss, "C1", 8.3);
			}


			[TestMethod, Timeout(2000)]
			[TestCategory("20")]
			public void NumberFormula2()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				NumberFormula2(ss);
			}

			public void NumberFormula2(AbstractSpreadsheet ss)
			{
				Set(ss, "A1", "= 4.6");
				VV(ss, "A1", 4.6);
			}


			// Repeats the simple tests all together
			[TestMethod, Timeout(2000)]
			[TestCategory("21")]
			public void RepeatSimpleTests()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				Set(ss, "A1", "17.32");
				Set(ss, "B1", "This is a test");
				Set(ss, "C1", "= A1+B1");
				OneString(ss);
				OneNumber(ss);
				OneFormula(ss);
				DivisionByZero1(ss);
				DivisionByZero2(ss);
				StringArgument(ss);
				ErrorArgument(ss);
				NumberFormula1(ss);
				NumberFormula2(ss);
			}

			// Four kinds of formulas
			[TestMethod, Timeout(2000)]
			[TestCategory("22")]
			public void Formulas()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				Formulas(ss);
			}

			public void Formulas(AbstractSpreadsheet ss)
			{
				Set(ss, "A1", "4.4");
				Set(ss, "B1", "2.2");
				Set(ss, "C1", "= A1 + B1");
				Set(ss, "D1", "= A1 - B1");
				Set(ss, "E1", "= A1 * B1");
				Set(ss, "F1", "= A1 / B1");
				VV(ss, "C1", 6.6, "D1", 2.2, "E1", 4.4 * 2.2, "F1", 2.0);
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("23")]
			public void Formulasa()
			{
				Formulas();
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("24")]
			public void Formulasb()
			{
				Formulas();
			}


			// Are multiple spreadsheets supported?
			[TestMethod, Timeout(2000)]
			[TestCategory("25")]
			public void Multiple()
			{
				AbstractSpreadsheet s1 = new Spreadsheet();
				AbstractSpreadsheet s2 = new Spreadsheet();
				Set(s1, "X1", "hello");
				Set(s2, "X1", "goodbye");
				VV(s1, "X1", "hello");
				VV(s2, "X1", "goodbye");
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("26")]
			public void Multiplea()
			{
				Multiple();
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("27")]
			public void Multipleb()
			{
				Multiple();
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("28")]
			public void Multiplec()
			{
				Multiple();
			}

			// Reading/writing spreadsheets
			[TestMethod, Timeout(2000)]
			[TestCategory("29")]
			[ExpectedException(typeof(SpreadsheetReadWriteException))]
			public void SaveTest1()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				ss.Save(Path.GetFullPath("/missing/save.txt"));
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("30")]
			[ExpectedException(typeof(SpreadsheetReadWriteException))]
			public void SaveTest2()
			{
				AbstractSpreadsheet ss = new Spreadsheet(Path.GetFullPath("/missing/save.txt"), s => true, s => s, "");
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("31")]
			public void SaveTest3()
			{
				AbstractSpreadsheet s1 = new Spreadsheet();
				Set(s1, "A1", "hello");
				s1.Save("save1.txt");
				s1 = new Spreadsheet("save1.txt", s => true, s => s, "default");
				Assert.AreEqual("hello", s1.GetCellContents("A1"));
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("32")]
			[ExpectedException(typeof(SpreadsheetReadWriteException))]
			public void SaveTest4()
			{
				using (StreamWriter writer = new StreamWriter("save2.txt"))
				{
					writer.WriteLine("This");
					writer.WriteLine("is");
					writer.WriteLine("a");
					writer.WriteLine("test!");
				}
				AbstractSpreadsheet ss = new Spreadsheet("save2.txt", s => true, s => s, "");
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("33")]
			[ExpectedException(typeof(SpreadsheetReadWriteException))]
			public void SaveTest5()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				ss.Save("save3.txt");
				ss = new Spreadsheet("save3.txt", s => true, s => s, "version");
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("34")]
			public void SaveTest6()
			{
				AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s, "hello");
				ss.Save("save4.txt");
				Assert.AreEqual("hello", new Spreadsheet().GetSavedVersion("save4.txt"));
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("35")]
			public void SaveTest7()
			{
				XmlWriterSettings settings = new XmlWriterSettings();
				settings.Indent = true;
				settings.IndentChars = "  ";
				using (XmlWriter writer = XmlWriter.Create("save5.txt", settings))
				{
					writer.WriteStartDocument();
					writer.WriteStartElement("spreadsheet");
					writer.WriteAttributeString("version", "");

					writer.WriteStartElement("cell");
					writer.WriteElementString("name", "A1");
					writer.WriteElementString("contents", "hello");
					writer.WriteEndElement();

					writer.WriteStartElement("cell");
					writer.WriteElementString("name", "A2");
					writer.WriteElementString("contents", "5.0");
					writer.WriteEndElement();

					writer.WriteStartElement("cell");
					writer.WriteElementString("name", "A3");
					writer.WriteElementString("contents", "4.0");
					writer.WriteEndElement();

					writer.WriteStartElement("cell");
					writer.WriteElementString("name", "A4");
					writer.WriteElementString("contents", "= A2 + A3");
					writer.WriteEndElement();

					writer.WriteEndElement();
					writer.WriteEndDocument();
				}
				AbstractSpreadsheet ss = new Spreadsheet("save5.txt", s => true, s => s, "");
				VV(ss, "A1", "hello", "A2", 5.0, "A3", 4.0, "A4", 9.0);
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("36")]
			public void SaveTest8()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				Set(ss, "A1", "hello");
				Set(ss, "A2", "5.0");
				Set(ss, "A3", "4.0");
				Set(ss, "A4", "= A2 + A3");
				ss.Save("save6.txt");
				using (XmlReader reader = XmlReader.Create("save6.txt"))
				{
					int spreadsheetCount = 0;
					int cellCount = 0;
					bool A1 = false;
					bool A2 = false;
					bool A3 = false;
					bool A4 = false;
					string name = null;
					string contents = null;

					while (reader.Read())
					{
						if (reader.IsStartElement())
						{
							switch (reader.Name)
							{
								case "spreadsheet":
									Assert.AreEqual("default", reader["version"]);
									spreadsheetCount++;
									break;

								case "cell":
									cellCount++;
									break;

								case "name":
									reader.Read();
									name = reader.Value;
									break;

								case "contents":
									reader.Read();
									contents = reader.Value;
									break;
							}
						}
						else
						{
							switch (reader.Name)
							{
								case "cell":
									if (name.Equals("A1")) { Assert.AreEqual("hello", contents); A1 = true; }
									else if (name.Equals("A2")) { Assert.AreEqual(5.0, Double.Parse(contents), 1e-9); A2 = true; }
									else if (name.Equals("A3")) { Assert.AreEqual(4.0, Double.Parse(contents), 1e-9); A3 = true; }
									else if (name.Equals("A4")) { contents = contents.Replace(" ", ""); Assert.AreEqual("=A2+A3", contents); A4 = true; }
									else Assert.Fail();
									break;
							}
						}
					}
					Assert.AreEqual(1, spreadsheetCount);
					Assert.AreEqual(4, cellCount);
					Assert.IsTrue(A1);
					Assert.IsTrue(A2);
					Assert.IsTrue(A3);
					Assert.IsTrue(A4);
				}
			}


			// Fun with formulas
			[TestMethod, Timeout(2000)]
			[TestCategory("37")]
			public void Formula1()
			{
				Formula1(new Spreadsheet());
			}
			public void Formula1(AbstractSpreadsheet ss)
			{
				Set(ss, "a1", "= a2 + a3");
				Set(ss, "a2", "= b1 + b2");
				Assert.IsInstanceOfType(ss.GetCellValue("a1"), typeof(FormulaError));
				Assert.IsInstanceOfType(ss.GetCellValue("a2"), typeof(FormulaError));
				Set(ss, "a3", "5.0");
				Set(ss, "b1", "2.0");
				Set(ss, "b2", "3.0");
				VV(ss, "a1", 10.0, "a2", 5.0);
				Set(ss, "b2", "4.0");
				VV(ss, "a1", 11.0, "a2", 6.0);
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("38")]
			public void Formula2()
			{
				Formula2(new Spreadsheet());
			}
			public void Formula2(AbstractSpreadsheet ss)
			{
				Set(ss, "a1", "= a2 + a3");
				Set(ss, "a2", "= a3");
				Set(ss, "a3", "6.0");
				VV(ss, "a1", 12.0, "a2", 6.0, "a3", 6.0);
				Set(ss, "a3", "5.0");
				VV(ss, "a1", 10.0, "a2", 5.0, "a3", 5.0);
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("39")]
			public void Formula3()
			{
				Formula3(new Spreadsheet());
			}
			public void Formula3(AbstractSpreadsheet ss)
			{
				Set(ss, "a1", "= a3 + a5");
				Set(ss, "a2", "= a5 + a4");
				Set(ss, "a3", "= a5");
				Set(ss, "a4", "= a5");
				Set(ss, "a5", "9.0");
				VV(ss, "a1", 18.0);
				VV(ss, "a2", 18.0);
				Set(ss, "a5", "8.0");
				VV(ss, "a1", 16.0);
				VV(ss, "a2", 16.0);
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("40")]
			public void Formula4()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				Formula1(ss);
				Formula2(ss);
				Formula3(ss);
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("41")]
			public void Formula4a()
			{
				Formula4();
			}


			[TestMethod, Timeout(2000)]
			[TestCategory("42")]
			public void MediumSheet()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				MediumSheet(ss);
			}

			public void MediumSheet(AbstractSpreadsheet ss)
			{
				Set(ss, "A1", "1.0");
				Set(ss, "A2", "2.0");
				Set(ss, "A3", "3.0");
				Set(ss, "A4", "4.0");
				Set(ss, "B1", "= A1 + A2");
				Set(ss, "B2", "= A3 * A4");
				Set(ss, "C1", "= B1 + B2");
				VV(ss, "A1", 1.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 3.0, "B2", 12.0, "C1", 15.0);
				Set(ss, "A1", "2.0");
				VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 4.0, "B2", 12.0, "C1", 16.0);
				Set(ss, "B1", "= A1 / A2");
				VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("43")]
			public void MediumSheeta()
			{
				MediumSheet();
			}


			[TestMethod, Timeout(2000)]
			[TestCategory("44")]
			public void MediumSave()
			{
				AbstractSpreadsheet ss = new Spreadsheet();
				MediumSheet(ss);
				ss.Save("save7.txt");
				ss = new Spreadsheet("save7.txt", s => true, s => s, "default");
				VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
			}

			[TestMethod, Timeout(2000)]
			[TestCategory("45")]
			public void MediumSavea()
			{
				MediumSave();
			}


			// A long chained formula. Solutions that re-evaluate 
			// cells on every request, rather than after a cell changes,
			// will timeout on this test.
			// This test is repeated to increase its scoring weight
			[TestMethod, Timeout(6000)]
			[TestCategory("46")]
			public void LongFormulaTest()
			{
				object result = "";
				LongFormulaHelper(out result);
				Assert.AreEqual("ok", result);
			}

			[TestMethod, Timeout(6000)]
			[TestCategory("47")]
			public void LongFormulaTest2()
			{
				object result = "";
				LongFormulaHelper(out result);
				Assert.AreEqual("ok", result);
			}

			[TestMethod, Timeout(6000)]
			[TestCategory("48")]
			public void LongFormulaTest3()
			{
				object result = "";
				LongFormulaHelper(out result);
				Assert.AreEqual("ok", result);
			}

			[TestMethod, Timeout(6000)]
			[TestCategory("49")]
			public void LongFormulaTest4()
			{
				object result = "";
				LongFormulaHelper(out result);
				Assert.AreEqual("ok", result);
			}

			[TestMethod, Timeout(6000)]
			[TestCategory("50")]
			public void LongFormulaTest5()
			{
				object result = "";
				LongFormulaHelper(out result);
				Assert.AreEqual("ok", result);
			}

			public void LongFormulaHelper(out object result)
			{
				try
				{
					AbstractSpreadsheet s = new Spreadsheet();
					s.SetContentsOfCell("sum1", "= a1 + a2");
					int i;
					int depth = 100;
					for (i = 1; i <= depth * 2; i += 2)
					{
						s.SetContentsOfCell("a" + i, "= a" + (i + 2) + " + a" + (i + 3));
						s.SetContentsOfCell("a" + (i + 1), "= a" + (i + 2) + "+ a" + (i + 3));
					}
					s.SetContentsOfCell("a" + i, "1");
					s.SetContentsOfCell("a" + (i + 1), "1");
					Assert.AreEqual(Math.Pow(2, depth + 1), (double)s.GetCellValue("sum1"), 1.0);
					s.SetContentsOfCell("a" + i, "0");
					Assert.AreEqual(Math.Pow(2, depth), (double)s.GetCellValue("sum1"), 1.0);
					s.SetContentsOfCell("a" + (i + 1), "0");
					Assert.AreEqual(0.0, (double)s.GetCellValue("sum1"), 0.1);
					result = "ok";
				}
				catch (Exception e)
				{
					result = e;
				}
			}

		}
	}

}
