using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
/// <summary>
/// Author Devin White
/// </summary>
namespace SS
{
	///Spreadsheet edit for PS5
	/// <summary>
	/// Represents a spreadsheet with an infinite number of empty cells
	/// </summary>
	public class Spreadsheet : AbstractSpreadsheet
	{
		private DependencyGraph dependencyGraph;
		private Dictionary<string, Cell> cells;
		private Func<string, double> lookup;
		private static Func<string, string> norm;
		private static Func<string, bool> isVal;
		string ver;

		public override bool Changed { get; protected set; }

		/// <summary>
		/// Constructs an empty spreadsheet. Normalizes Variable to itself
		/// Any variable that passes the built in validator is true
		/// the version is set to "default"
		/// </summary>
		public Spreadsheet()
			: base(s => true, s => s, "default") ///constructor for spreadsheet
		{
			norm = Normalize;
			isVal = IsValid;
			ver = Version;
			lookup = Lookup;
			dependencyGraph = new DependencyGraph();
			cells = new Dictionary<string, Cell>();
			Changed = false;
		}
		/// <summary>
		/// Spreadsheet Constructor which uses 3 arguments. the function isValid,
		/// the function Normalize, and the spreadsheet version
		/// </summary>
		/// <param name="isValid">The Valid variable checker </param>
		/// <param name="normalize">The normalizer function to be used</param>
		/// <param name="version">The spreadsheet Version</param>
		public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
			: base(isValid, normalize, version)///constructor for spreadsheet
		{
			ver = Version;
			norm = Normalize;
			isVal = IsValid;
			lookup = Lookup;
			dependencyGraph = new DependencyGraph();
			cells = new Dictionary<string, Cell>();
			Changed = false;
		}
		/// <summary>
		/// Spreadsheet Constructor which uses 4 arguments. The filepath, the function isValid,
		/// the function Normalize, and the spreadsheet version
		/// </summary>
		/// <param name="filepath">Filepath for saving or reading</param>
		/// <param name="isValid">The Valid variable checker </param>
		/// <param name="normalize">The normalizer function to be used</param>
		/// <param name="version">The spreadsheet Version</param>
		public Spreadsheet(string filepath, Func<string, bool> isValid, Func<string, string> normalize, string version) ///constructor for spreadsheet
		: base(isValid, normalize, version)
		{
			ver = Version;
			norm = Normalize;
			isVal = IsValid;
			lookup = Lookup;
			dependencyGraph = new DependencyGraph();
			cells = new Dictionary<string, Cell>();
			if (GetSavedVersion(filepath) != Version)
			{
				throw new SpreadsheetReadWriteException("Spreadseet Version and File version do not match");
			}
			Changed = false;
		}
		/// <summary>
		///If name is null or invalid, throws an InvalidNameException.
		/// 
		/// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
		/// value should be either a string, a double, or a Formula.
		/// </summary>
		/// <param name="name">The name of the cell</param>
		/// <returns>Returns the contents(not value) of the cell. This can be a double, Formula, or String</returns>
		public override object GetCellContents(string name)
		{
			if (!IsVariable(name))
			{
				throw new InvalidNameException();
			}
			name = norm(name);
			if (cells.ContainsKey(name))
			{
				//Find the type of the cell(Formula, string, double)
				return cells[name].Contents;
			}
			else return ""; //if cell is empty, will always return an empty string since it isn't stored.
		}
		/// <summary>
		/// Enumerates the names of all the non-empty cells in the spreadsheet.
		/// </summary>
		/// <returns>The names of the cells in the spreadsheet that have contents</returns>
		public override IEnumerable<string> GetNamesOfAllNonemptyCells()
		{
			return cells.Keys;
		}
		/// <summary>
		/// If name is null or invalid, throws an InvalidNameException.
		/// 
		/// Otherwise, the contents of the named cell becomes a number.  The method returns a
		/// list consisting of name plus the names of all other cells whose value depends, 
		/// directly or indirectly, on the named cell.
		/// 
		/// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
		/// list {A1, B1, C1} is returned.
		/// </summary>
		/// <param name="name">The name of the cell</param>
		/// <param name="number">the value within the cell</param>
		/// <returns>returns a list consisting of cell name + dependent cells</returns>
		protected override IList<string> SetCellContents(string name, double number)
		{
			List<string> contents = new List<string>();
			cells[name] = new Cell(number, number);
			// if the Cell previously had a formula/dependencies, remove them
			// And update the dependencyGraph
			foreach (string variable in dependencyGraph.GetDependees(name))
			{
				dependencyGraph.RemoveDependency(variable, name);
			}
			RecalculateDependentCells(name, contents);
			return contents;
		}
		/// <summary>
		/// If text is null, throws an ArgumentNullException.
		/// 
		/// Otherwise, if name is null or invalid, throws an InvalidNameException.
		/// 
		/// Otherwise, the contents of the named cell becomes text.  The method returns a
		/// list consisting of name plus the names of all other cells whose value depends, 
		/// directly or indirectly, on the named cell.
		/// 
		/// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
		/// list {A1, B1, C1} is returned.
		/// </summary>
		/// <param name="name">The name of the cell</param>
		/// <param name="number">the value within the cell</param>
		/// <returns>returns a list consisting of cell name + dependent cells</returns>
		protected override IList<string> SetCellContents(string name, string text)
		{
			List<string> contents = new List<string>();
			cells[name] = new Cell(text, text);
			// if the Cell previously had a formula/dependencies, remove them
			// And update the dependencyGraph
			foreach (string variable in dependencyGraph.GetDependees(name))
			{
				dependencyGraph.RemoveDependency(variable, name);
			}
			RecalculateDependentCells(name, contents);
			return contents;
		}
		/// <summary>
		/// If the formula parameter is null, throws an ArgumentNullException.
		/// 
		/// Otherwise, if name is null or invalid, throws an InvalidNameException.
		/// 
		/// Otherwise, if changing the contents of the named cell to be the formula would cause a 
		/// circular dependency, throws a CircularException, and no change is made to the spreadsheet.
		/// 
		/// Otherwise, the contents of the named cell becomes formula.  The method returns a
		/// list consisting of name plus the names of all other cells whose value depends,
		/// directly or indirectly, on the named cell.
		/// 
		/// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
		/// list {A1, B1, C1} is returned.
		/// </summary>
		/// <param name="name">The name of the cell</param>
		/// <param name="number">the value within the cell</param>
		/// <returns>returns a list consisting of cell name + dependent cells</returns>
		protected override IList<string> SetCellContents(string name, Formula formula)
		{
			//stores dependees just in case change creates circular exception
			List<string> backupDependees = new List<string>();
			List<string> contents = new List<string>();
			object backupContents = "";
			object backupValue = "";
			if (cells.ContainsKey(name))
			{
				backupContents = cells[name].Contents;
				backupValue = cells[name].Value;
			}
			backupDependees.AddRange(dependencyGraph.GetDependees(name));
			try
			{
				dependencyGraph.ReplaceDependees(name, formula.GetVariables()); //Get variables in the formula
				cells[name] = new Cell(formula, formula.Evaluate(lookup));
				RecalculateDependentCells(name, contents);
			}
			///If new contents create a circularException, revert contents then throw exception
			catch (CircularException)
			{
				contents.Clear();
				dependencyGraph.ReplaceDependees(name, backupDependees);
				contents.Add(name);
				contents.AddRange(backupDependees);
				if (cells.ContainsKey(name))
				{
					cells[name] = new Cell(backupContents, backupValue);
				}
				SetContentsOfCell(name, backupContents.ToString());
				Changed = false;
				throw new CircularException();
			}
			///return a list of cell name + dependents
			return contents;
		}

		/// <summary>
		/// Helper method for SetCellContents
		/// Recalculates all the cells dependent on the changed cell and adds them to the list to be returned
		/// Also sets the changed variable to true upon success
		/// </summary>
		/// <param name="name">The name of the cell</param>
		/// <param name="contents">The Contents of the cell</param>
		private void RecalculateDependentCells(string name, List<string> contents)
		{
			foreach (string cell in GetCellsToRecalculate(name))
			{
				contents.Add(cell);
				if (cells[cell].Contents is Formula)
				{
					Formula formula2 = new Formula(cells[cell].Contents.ToString(), norm, isVal);
					cells[cell] = new Cell(cells[cell].Contents, formula2.Evaluate(lookup));
				}
			}
			Changed = true;
		}

		/// <summary>
		/// Returns an enumeration, without duplicates, of the names of all cells whose
		/// values depend directly on the value of the named cell.  In other words, returns
		/// an enumeration, without duplicates, of the names of all cells that contain
		/// formulas containing name.
		/// 
		/// For example, suppose that
		/// A1 contains 3
		/// B1 contains the formula A1 * A1
		/// C1 contains the formula B1 + A1
		/// D1 contains the formula B1 - C1
		/// The direct dependents of A1 are B1 and C1
		/// </summary>
		/// <param name="name">Name of the cell</param>
		/// <returns>The direct dependents of the cell</returns>
		protected override IEnumerable<string> GetDirectDependents(string name)
		{
			return dependencyGraph.GetDependents(name);
		}

		/// <summary>
		/// Checks if the token is a valid variable
		/// </summary>
		/// <param name="name">Name being evaluated</param>
		/// <returns>True if token is a variable, False if not</returns>
		private static bool IsVariable(string name)
		{
			//The string starts with one or more letters and is followed by one or more numbers.
			//The(application programmer's) IsValid function returns true for that string, and should be called only for
			//variable strings that are valid first by (1) above.
			//If the name is null or not a variable, returns false and throws and Invalid name Exception
			Regex varPattern = new Regex("^[a-zA-Z][a-zA-Z]*[0-9][0-9]*");
			if (name == null)
			{
				return false;
			}
			else if (varPattern.IsMatch(name))
			{
				//Normalize the Variable to user's normalization
				name = norm(name);

				//If valid after Normalization, is a valid variable
				if (isVal(name) && varPattern.IsMatch(name))
				{
					return true;
				}
				return false;
			}
			else return false;
		}
		/// <summary>
		/// Returns the version information of the spreadsheet saved in the named file.
		/// If there are any problems opening, reading, or closing the file, the method
		/// should throw a SpreadsheetReadWriteException with an explanatory message.
		/// </summary>
		/// <param name="filename">The name of the file to retreive</param>
		/// <returns>returns the version of the read file</returns>
		public override string GetSavedVersion(string filename)
		{
			string name = "";
			string content = "";
			bool changed = false;
			string version = "";

			try
			{
				using (XmlReader reader = XmlReader.Create(filename))
				{
					while (reader.Read())
					{
						if (reader.IsStartElement())
						{
							if (reader.GetAttribute("version") == ver || reader.GetAttribute("version") != null)
							{
								version = reader.GetAttribute("version");
							}
							switch (reader.Name)
							{
								//Reads the Cell Names
								case "name":
									reader.Read();
									name = reader.Value;
									break;
								//Reads Cell Contents
								case "contents":
									changed = true;
									reader.Read();
									content = reader.Value;
									break;
								//Reads Cell Value
								case "value":
									reader.Read();
									break;
							}
							//Changed switches from true or false to indicate a cell's name=> value pair
							if (name != "" && changed == true)
							{
								changed = false;
								if (!IsVariable(name))
								{
									throw new SpreadsheetReadWriteException("Trying to read an invalid variable");
								}
								SetContentsOfCell(name, content);
							}
						}
					}
					return version;
				}
			}
			catch (Exception)
			{
				throw new SpreadsheetReadWriteException("Error Reading the spreadsheet");
			}
		}

		/// <summary>
		/// Writes the contents of this spreadsheet to the named file using an XML format.
		/// The XML elements should be structured as follows:
		/// 
		/// <spreadsheet version="version information goes here">
		/// 
		/// <cell>
		/// <name>cell name goes here</name>
		/// <contents>cell contents goes here</contents>    
		/// </cell>
		/// 
		/// </spreadsheet>
		/// 
		/// There should be one cell element for each non-empty cell in the spreadsheet.  
		/// If the cell contains a string, it should be written as the contents.  
		/// If the cell contains a double d, d.ToString() should be written as the contents.  
		/// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
		/// 
		/// If there are any problems opening, writing, or closing the file, the method should throw a
		/// SpreadsheetReadWriteException with an explanatory message.
		/// </summary>
		/// <param name="filename">The name of the file to save</param>
		public override void Save(string filename)
		{
			try
			{
				/*<spreadsheet version =“version">
				 * <cell>
				 * <name>
				 * cell name goes here
				 * </name>
				 * <contents>
				 * cell contents goes here
				 * </contents> 
				 * <cell>
				 * </spreadsheet>*/

				// Indentation for easier reading
				XmlWriterSettings settings = new XmlWriterSettings();
				settings.Indent = true;
				settings.IndentChars = "  ";
				XmlWriter writer = XmlWriter.Create(filename, settings);
				// Creates an XmlWriter inside this block, and automatically Disposes it at the end.
				using (writer)
				{
					writer.WriteStartDocument();
					writer.WriteStartElement("spreadsheet");
					writer.WriteAttributeString("version", ver);//<Spreadsheet>

					foreach (string cell in GetNamesOfAllNonemptyCells())
					{
						writer.WriteStartElement("cell");//<cells>
						writer.WriteElementString("name", cell); //<name>
						if (GetCellContents(cell) is Formula)
						{
							writer.WriteElementString("contents", "=" + GetCellContents(cell).ToString());
						}
						else
						{
							writer.WriteElementString("contents", GetCellContents(cell).ToString());//<value>
						}
						writer.WriteElementString("value", GetCellContents(cell).ToString());
						writer.WriteEndElement(); //ends Cells

					}
					writer.WriteEndDocument();
					Changed = false;
				}
			}
			catch (Exception)
			{
				throw new SpreadsheetReadWriteException("Error Saving the spreadsheet");
			}

		}
		/// <summary>
		/// If name is null or invalid, throws an InvalidNameException.
		/// 
		/// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
		/// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
		/// </summary>
		/// <param name="name">The name of the cell to retrieve contents form</param>
		/// <returns>the contents of the cell</returns>
		public override object GetCellValue(string name)
		{
			if (!IsVariable(name))
			{
				throw new InvalidNameException();
			}

			if (cells.ContainsKey(name))
			{
				return cells[name].Value;
			}
			else return ""; //empty cell always returns empty string
		}
		/// <summary>
		/// Determines whether the Content of the Cell is a Formula, Double, or string
		/// then calls the appropriate method to add it to the spreadsheet
		/// </summary>
		/// <param name="name">Name of the cell</param>
		/// <param name="content">Contents of the cell, can be a Formula, string, or double</param>
		/// <returns>Returns of list consisting of the cell + its dependees</returns>
		public override IList<string> SetContentsOfCell(string name, string content)
		{
			List<string> contents = new List<string>();
			//Cell Insides can't be null
			if (content == null)
			{
				throw new ArgumentNullException("argument is null");
			}
			//If name is not a valid variable throws Exception
			else if (!IsVariable(name))
			{
				throw new InvalidNameException();
			}
			//trims content to check for = sign
			content = content.Trim();
			if (!content.Equals(""))
			{
				//sets the value of the cell
				if (Double.TryParse(content, out double value)) //if Contents of cell are a formula, must find variables within that formula and add to dependency graph
				{

					return SetCellContents(name, value); // calls double setCellContents
				}
				//Formula signified by equals preceding all other nonwhitespace tokens
				else if (content[0] == '=')
				{
					Formula formula = new Formula(content.Replace("=", ""), norm, isVal);
					return SetCellContents(name, formula); ///Calls formula GetCellContents
				}
				//If not a double or formula, contains a string
				else
				{
					return SetCellContents(name, content);
				}
			}
			else //remove empty cells 
			{
				cells.Remove(name);
				//If Cell existed and was made empty, removes it and any dependees it may have had from the dependency Graph
				if (dependencyGraph.HasDependees(name))
				{
					foreach (string dependency in dependencyGraph.GetDependees(name))
					{
						dependencyGraph.RemoveDependency(dependency, name);
					}
				}
				contents.AddRange(GetCellsToRecalculate(name)); //Adds direct and indirect dependents to the list
				return contents;
			}
		}
		/// <summary>
		/// Returns the names of all NonEmpty Cells
		/// Cells with contents of "" are considered Empty
		/// </summary>
		/// <param name="variable">The variable to lookup the value of</param>
		/// <returns>The names of all cells with content</returns>
		private double Lookup(string variable)
		{
			foreach (string name in GetNamesOfAllNonemptyCells())
			{
				if (variable == name.ToString())
				{
					if (cells[name].Value is double)
					{
						return (double)cells[name].Value;
					}
				}
			}
			throw new ArgumentException("Variable has an invalid(non-double) value");

		}
		/// <summary>
		/// Author: Devin White
		/// </summary>
		private class Cell
		{
			public object Contents { get; private set; }
			public object Value { get; private set; }
			/// <summary>
			/// Represents a single cell in the spreadsheet
			/// </summary>
			/// <param name="contents">The contents of the cell. Can be a double, String, or Formula</param>
			/// <param name="value">The Value of the cell. Can be either a double, string, or FormulaError</param>
			public Cell(object contents, object value)
			{
				Contents = contents;
				Value = value;
			}
		}
	}
}
