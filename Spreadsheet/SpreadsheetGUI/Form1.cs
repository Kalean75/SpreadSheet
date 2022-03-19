using SpreadsheetUtilities;
using SS;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
//Author: Devin White
//Spreadsheet gui
namespace SpreadsheetGUI
{
	public partial class Form1 : Form
	{
		private Spreadsheet spreadsheet;
		private string FileName { get; set; }
		/// <summary>
		/// Constructor
		/// </summary>
		public Form1()
		{
			InitializeComponent();
			spreadsheetPanel1.SetSelection(0, 0);
			spreadsheet = new Spreadsheet(s => true, s => s.ToUpper(), "ps6");
			CellNameTextBox.Text = displayName(spreadsheetPanel1);
			FileName = "";
			CellContentsTextBox.Focus();
		}

		/// <summary>
		/// Displays the value within the spreadsheet cell
		/// </summary>
		/// <param name="ss">Spreadsheet panel</param>
		/// <param name="value">Value to display</param>
		/// <returns>Contents of the cell</returns>
		private string DisplayInCell(SpreadsheetPanel ss, string value)
		{
			try
			{
				int row, col;
				String contents = value;
				ss.GetSelection(out col, out row);
				ss.GetValue(col, row, out value);
				//SetCellContents
				spreadsheet.SetContentsOfCell(displayName(spreadsheetPanel1), contents);
				//set value of cell to cell contents
				ss.SetValue(col, row, spreadsheet.GetCellValue(displayName(spreadsheetPanel1)).ToString());
				return contents;
			}
			// Captures exceptions then shows a dialog box
			catch (Exception e)
			{
				if (e is CircularException)
				{
					MessageBox.Show("Error editing TextBox: Circular Exception");
				}
				else
				{
					MessageBox.Show("Error Editing Cell");
				}
				return value;
			}
		}
		/// <summary>
		/// Displays the Contents of the selected cell
		/// </summary>
		/// <param name="ss">Spreadsheet panel</param>
		/// <returns>Returns the contents of the cell</returns>
		private string displayContents(SpreadsheetPanel ss)
		{
			int row, col;
			object contents;
			ss.GetSelection(out col, out row);
			contents = spreadsheet.GetCellContents(displayName(spreadsheetPanel1));
			if (contents is Formula)
			{
				contents = "=" + contents;
			}
			return contents.ToString();
		}
		/// <summary>
		/// Displays the value of the selected cell
		/// </summary>
		/// <param name="ss">Spreadsheetpanel</param>
		/// <returns>The value of the selected cell</returns>
		private object displayValue(SpreadsheetPanel ss)
		{
			int row, col;
			String contents;
			ss.GetSelection(out col, out row);
			ss.GetValue(col, row, out contents);
			return spreadsheet.GetCellValue(displayName(spreadsheetPanel1));
		}
		/// <summary>
		/// Displays the name of the selected Cell
		/// </summary>
		/// <param name="ss"> Spreadsheetpanel</param>
		/// <returns> The name of the selected cell</returns>
		private string displayName(SpreadsheetPanel ss)
		{
			int row, col;
			String variable;
			int cellnum;
			ss.GetSelection(out col, out row);
			cellnum = (row + 1);
			variable = FindColumn(col, cellnum);
			return variable;
		}
		/// <summary>
		/// Finds the Letter of the variable based on column number then transfers that plus the row to a variable
		/// </summary>
		/// <param name="columNum">The column Number</param>
		/// <param name="rowNum">The row number</param>
		/// <returns></returns>
		private string FindColumn(int columNum, int rowNum)
		{
			columNum += 1;
			string columnName = "Cell Name";
			switch (columNum.ToString())
			{
				case "1":
					columnName = "A";
					break;
				case "2":
					columnName = "B";
					break;
				case "3":
					columnName = "C";
					break;
				case "4":
					columnName = "D";
					break;
				case "5":
					columnName = "E";
					break;
				case "6":
					columnName = "F";
					break;
				case "7":
					columnName = "G";
					break;
				case "8":
					columnName = "H";
					break;
				case "9":
					columnName = "I";
					break;
				case "10":
					columnName = "J";
					break;
				case "11":
					columnName = "K";
					break;
				case "12":
					columnName = "L";
					break;
				case "13":
					columnName = "M";
					break;
				case "14":
					columnName = "N";
					break;
				case "15":
					columnName = "O";
					break;
				case "16":
					columnName = "P";
					break;
				case "17":
					columnName = "Q";
					break;
				case "18":
					columnName = "R";
					break;
				case "19":
					columnName = "S";
					break;
				case "20":
					columnName = "T";
					break;
				case "21":
					columnName = "U";
					break;
				case "22":
					columnName = "V";
					break;
				case "23":
					columnName = "W";
					break;
				case "24":
					columnName = "X";
					break;
				case "25":
					columnName = "Y";
					break;
				case "26":
					columnName = "Z";
					break;

			}
			columnName = columnName + rowNum;

			return columnName;
		}
		/// <summary>
		/// Changes the values of all dependent columns when the column they are dependent on changes
		/// </summary>
		/// <param name="variableName">The name of the cells to chagnge</param>
		private void ChangeDependentColumns(string variableName)
		{
			string rowNumString = "";
			for (int i = 0; i < variableName.Length; i++)
			{
				if (Char.IsDigit(variableName[i]))
					rowNumString += variableName[i];
			}
			int.TryParse(rowNumString, out int rowNum);
			rowNum -= 1;
			int columnNum = 0;

			//Translates the variable to a column number
			switch (variableName[0].ToString())
			{
				case "A":
					columnNum = 0;
					break;
				case "B":
					columnNum = 1;
					break;
				case "C":
					columnNum = 2;
					break;
				case "D":
					columnNum = 3;
					break;
				case "E":
					columnNum = 4;
					break;
				case "F":
					columnNum = 5;
					break;
				case "G":
					columnNum = 6;
					break;
				case "H":
					columnNum = 7;
					break;
				case "I":
					columnNum = 8;
					break;
				case "J":
					columnNum = 9;
					break;
				case "K":
					columnNum = 10;
					break;
				case "L":
					columnNum = 11;
					break;
				case "M":
					columnNum = 12;
					break;
				case "N":
					columnNum = 13;
					break;
				case "O":
					columnNum = 14;
					break;
				case "P":
					columnNum = 15;
					break;
				case "Q":
					columnNum = 16;
					break;
				case "R":
					columnNum = 17;
					break;
				case "S":
					columnNum = 18;
					break;
				case "T":
					columnNum = 19;
					break;
				case "U":
					columnNum = 20;
					break;
				case "V":
					columnNum = 21;
					break;
				case "W":
					columnNum = 22;
					break;
				case "X":
					columnNum = 23;
					break;
				case "Y":
					columnNum = 24;
					break;
				case "Z":
					columnNum = 25;
					break;
			}
			spreadsheetPanel1.SetValue(columnNum, rowNum, spreadsheet.GetCellValue(variableName).ToString());

		}
		/// <summary>
		/// When Cell Contents box is clicked, sets contents of cell.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CellContentsTextBox_Click(object sender, EventArgs e)
		{
			ToolStripTextBox objTextBox = (ToolStripTextBox)sender;
			string theText = objTextBox.Text;
			CellContentsTextBox.Text = DisplayInCell(spreadsheetPanel1, theText);
			CellValueTextBox.Text = displayValue(spreadsheetPanel1).ToString();
			foreach (string cellname in spreadsheet.GetNamesOfAllNonemptyCells().ToList())
			{
				if (spreadsheet.GetCellContents(cellname) is Formula)
				{
					ChangeDependentColumns(cellname);
				}
			}
		}
		/// <summary>
		/// PreviewKeydown for override
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void control_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			e.IsInputKey = true;
		}
		/// <summary>
		/// Overrides Keydown. 
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{

			//Populates contents of cell when the enter key is pressed
			if (keyData == Keys.Enter)
			{
				string theText = CellContentsTextBox.Text;
				CellContentsTextBox.Text = DisplayInCell(spreadsheetPanel1, theText);
				CellValueTextBox.Text = displayValue(spreadsheetPanel1).ToString();
				foreach (string cellname in spreadsheet.GetNamesOfAllNonemptyCells().ToList())
				{
					if (spreadsheet.GetCellContents(cellname) is Formula)
					{
						ChangeDependentColumns(cellname);
					}
				}
				return true;
			}
			//Moves selected cell down
			if (keyData == Keys.Down)
			{
				spreadsheetPanel1.GetSelection(out int col, out int row);
				if (row != 98)
				{
					AlterPreviousOnMove();
					spreadsheetPanel1.SetSelection(col, row + 1);
					changeText();
				}
				return true;
			}
			//Moves selected cell up
			if (keyData == Keys.Up)
			{
				spreadsheetPanel1.GetSelection(out int col, out int row);
				if (row != 0)
				{
					AlterPreviousOnMove();
					spreadsheetPanel1.SetSelection(col, row - 1);
					changeText();
				}
				return true;
			}
			//Moves selected cell right
			if (keyData == Keys.Right)
			{
				spreadsheetPanel1.GetSelection(out int col, out int row);
				if (col != 98)
				{
					AlterPreviousOnMove();
					spreadsheetPanel1.SetSelection(col + 1, row);
					changeText();
				}
				return true;
			}
			//Moves selected cell left
			if (keyData == Keys.Left)
			{
				spreadsheetPanel1.GetSelection(out int col, out int row);
				if (col != 0)
				{
					AlterPreviousOnMove();
					spreadsheetPanel1.SetSelection(col - 1, row);
					changeText();
				}
				return true;
			}
			Focus();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		/// <summary>
		/// Helper method. Alters dependent cells when selection is moved with arrow keys
		/// </summary>
		private void AlterPreviousOnMove()
		{
			string theText = CellContentsTextBox.Text;
			CellContentsTextBox.Text = DisplayInCell(spreadsheetPanel1, theText);
			foreach (string cellname in spreadsheet.GetNamesOfAllNonemptyCells().ToList())
			{
				if (spreadsheet.GetCellContents(cellname) is Formula)
				{
					ChangeDependentColumns(cellname);
				}
			}
		}

		/// <summary>
		/// Changes Text
		/// </summary>
		private void changeText()
		{
			CellNameTextBox.Text = displayName(spreadsheetPanel1);
			CellValueTextBox.Text = displayValue(spreadsheetPanel1).ToString();
			CellContentsTextBox.Text = displayContents(spreadsheetPanel1);
		}

		/// <summary>
		///  Performs actions when the selection is changed
		/// </summary>
		/// <param name="sender"></param>
		private void spreadsheetPanel1_SelectionChanged(SpreadsheetPanel sender)
		{
			CellContentsTextBox.Focus();
			if (CellNameTextBox.Text != "")
			{

			}
			CellNameTextBox.Text = displayName(spreadsheetPanel1);
			CellValueTextBox.Text = displayValue(spreadsheetPanel1).ToString();
			CellContentsTextBox.Text = displayContents(spreadsheetPanel1);
		}
		/// <summary>
		/// Closes the spreadsheet and goes to FormClosing
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}
		/// <summary>
		/// Performs Actions when the form is closed
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">eventargs</param>
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			//If the spreadsheet has been Changed, prompts the user asking if they wish to close it.
			if (spreadsheet.Changed)
			{
				DialogResult dr = MessageBox.Show("There is Unsaved Data, Exiting will cause loss of data", "Exit?", MessageBoxButtons.OKCancel);
				switch (dr)
				{
					case DialogResult.OK:
						e.Cancel = false;
						break;
					case DialogResult.Cancel:
						e.Cancel = true;
						break;
				}
			}
		}
		/// <summary>
		/// Saves to the file with the same name
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">eventargs</param>
		private void saveTool_Click(object sender, EventArgs e)
		{
			//If file doesn't have a name goes to save as tool, else automatically updates the save
			if (FileName == "")
			{
				saveAsTool_Click(sender, e);
			}
			else
			{
				spreadsheet.Save(FileName);
			}

		}
		/// <summary>
		/// Saves the file and lets the user select the name and file type
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">eventargs</param>
		private void saveAsTool_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			using (saveFileDialog)
			{
				saveFileDialog.FileName = FileName;
				saveFileDialog.DefaultExt = ".sprd";
				saveFileDialog.AddExtension = true;
				saveFileDialog.FilterIndex = 1;
				saveFileDialog.Filter = "Spreadsheet File (*.sprd)|*.sprd|Text File (*.txt)|*.txt|All files (*.*)|*.*";
				saveFileDialog.Title = "Save a spreadsheet";
				saveFileDialog.ShowDialog();
				if (saveFileDialog.FileName.Equals(FileName))
				{
					saveFileDialog.OverwritePrompt = false;
				}
				FileName = saveFileDialog.FileName;

				// If the file name is not an empty string open it for saving.
				if (FileName != "")
				{
					spreadsheet.Save(FileName);
					Form1_Load(sender, e);
				}
			}
		}
		/// <summary>
		/// Opens a file and populates the contents of the cell with it.
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">eventargs</param>
		private void LoadButton_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.InitialDirectory = "c:\\";
				openFileDialog.DefaultExt = ".sprd";
				openFileDialog.AddExtension = true;
				openFileDialog.Filter = "Sprd Files (*.sprd)|*.sprd|txt files (*.txt)|*.txt|All files (*.*)|*.*";
				openFileDialog.FilterIndex = 1;
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					foreach (string cellname in spreadsheet.GetNamesOfAllNonemptyCells().ToList())
					{
						clearcells(cellname);
					}
					//Get the path of specified file
					FileName = openFileDialog.FileName;
					spreadsheet.GetSavedVersion(FileName);
					foreach (string cellname in spreadsheet.GetNamesOfAllNonemptyCells().ToList())
					{
						string contents = spreadsheet.GetCellContents(cellname).ToString();
						ChangeDependentColumns(cellname);
					}
					foreach (string cellname in spreadsheet.GetNamesOfAllNonemptyCells().ToList())
					{
						ChangeDependentColumns(cellname);
					}
				}
				spreadsheetPanel1.GetSelection(out int col, out int row);
				spreadsheetPanel1.GetValue(col, row, out string value);
				CellContentsTextBox.Text = value;
			}

		}
		/// <summary>
		/// Changes name and other actions when form loads
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_Load(object sender, EventArgs e)
		{
			CellContentsTextBox.Focus();
			if (FileName == "")
			{
				this.Text = "New Spreadsheet";
			}
			else
			{
				this.Text = Path.GetFileName(FileName);
			}
		}
		/// <summary>
		/// Copies Contents of Cell or TextBox
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">eventargs</param>
		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string copiedText;
			try
			{
				spreadsheetPanel1.GetSelection(out int col, out int row);
				if (spreadsheetPanel1.Focused == true)
				{
					copiedText = displayContents(spreadsheetPanel1);
				}
				else
				{
					copiedText = CellNameTextBox.Text;
				}
				Clipboard.SetText(copiedText);
			}
			catch (Exception)
			{
			}

		}
		/// <summary>
		/// Pastes Content of Clipboard
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string pasteText = Clipboard.GetText();
			DisplayInCell(spreadsheetPanel1, pasteText);
			this.ActiveControl.Text = pasteText;
			CellContentsTextBox.Text = pasteText;
		}
		/// <summary>
		/// Copies text to clipboard then removes the text from the cell
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string copiedText;
			try
			{
				spreadsheetPanel1.GetSelection(out int col, out int row);
				if (spreadsheetPanel1.Focused == true)
				{
					copiedText = displayContents(spreadsheetPanel1);
					DisplayInCell(spreadsheetPanel1, "");
					CellContentsTextBox.Text = displayContents(spreadsheetPanel1);
					CellValueTextBox.Text = displayValue(spreadsheetPanel1).ToString();
				}
				else
				{
					copiedText = CellContentsTextBox.Text;
					DisplayInCell(spreadsheetPanel1, "");
					CellContentsTextBox.Text = displayContents(spreadsheetPanel1);
					CellValueTextBox.Text = displayValue(spreadsheetPanel1).ToString();
				}
				Clipboard.SetText(copiedText);
			}
			catch (Exception)
			{
			}

		}
		/// <summary>
		/// Selects all the text in the Contents box
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CellContentsTextBox.SelectAll();
			CellContentsTextBox.Focus();
		}
		/// <summary>
		/// Opens a new Form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void newToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			GuiApplicationContext.getAppContext().RunForm(new Form1());
		}
		/// <summary>
		/// Shows or hides the edit button
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">eventargs</param>
		private void showEditButtonToolStripMenuItem_Click(object sender, EventArgs e)
		{

			if (showEditButtonToolStripMenuItem.Checked == false)
			{
				showEditButtonToolStripMenuItem.Checked = true;
				editToolStripMenuItem.Visible = true;
			}
			else
			{
				showEditButtonToolStripMenuItem.Checked = false;
				editToolStripMenuItem.Visible = false;
			}
		}
		/// <summary>
		/// Hides and shows the help button
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		private void showHelpButtonToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (showHelpButtonToolStripMenuItem.Checked == false)
			{
				showHelpButtonToolStripMenuItem.Checked = true;
				helpToolStripMenuItem.Visible = true;
			}
			else
			{
				showHelpButtonToolStripMenuItem.Checked = false;
				helpToolStripMenuItem.Visible = false;
			}
		}
		/// <summary>
		/// Hides and shows the File button
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		private void showFileButtonToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (showFileButtonToolStripMenuItem.Checked == false)
			{
				showFileButtonToolStripMenuItem.Checked = true;
				fileToolStripMenuItem.Visible = true;
			}
			else
			{
				showFileButtonToolStripMenuItem.Checked = false;
				fileToolStripMenuItem.Visible = false;
			}
		}
		/// <summary>
		/// Hides and shows the Cell Name button
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		private void showCellNameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (showCellNameToolStripMenuItem.Checked == false)
			{
				showCellNameToolStripMenuItem.Checked = true;
				CellNameTextBox.Visible = true;
			}
			else
			{
				showCellNameToolStripMenuItem.Checked = false;
				CellNameTextBox.Visible = false;
			}
		}
		/// <summary>
		/// Hides and shows the Cell Value button
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">EventArgs</param>
		private void showCellValueBoxToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (showCellValueBoxToolStripMenuItem.Checked == true)
			{
				showCellValueBoxToolStripMenuItem.Checked = true;
				CellValueTextBox.Visible = true;
			}
			else
			{
				showCellValueBoxToolStripMenuItem.Checked = false;
				CellValueTextBox.Visible = false;
			}
		}
		/// <summary>
		/// Opens Help menu for instructions
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">Eventargs</param>
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GuiApplicationContext.getAppContext().RunForm(new Form2());
		}
		private void randomStatGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			statRandomizer();
		}
		private void statRandomizer()
		{
			var rand = new Random();
			int str = rand.Next(1, 20);
			int dex = rand.Next(1, 20);
			int cons = rand.Next(1, 20);
			int intel = rand.Next(1, 20);
			int wis = rand.Next(1, 20);
			int charisma = rand.Next(1, 20);
			String Strength = "Strength";
			String Dex = "Dexterity";
			String Cons = "Constitution";
			String Int = "Intelligence";
			String Wis = "Wisdom";
			String Charisma = "Charisma";
			spreadsheetPanel1.SetValue(0, 1, Strength);
			spreadsheetPanel1.SetValue(0, 2, Dex);
			spreadsheetPanel1.SetValue(0, 3, Cons);
			spreadsheetPanel1.SetValue(0, 4, Int);
			spreadsheetPanel1.SetValue(0, 5, Wis);
			spreadsheetPanel1.SetValue(0, 6, Charisma);
			spreadsheetPanel1.SetValue(1, 1, str.ToString());
			spreadsheetPanel1.SetValue(1, 2, dex.ToString());
			spreadsheetPanel1.SetValue(1, 3, cons.ToString());
			spreadsheetPanel1.SetValue(1, 4, intel.ToString());
			spreadsheetPanel1.SetValue(1, 5, wis.ToString());
			spreadsheetPanel1.SetValue(1, 6, charisma.ToString());
		}
		private void clearcells(string variableName)
		{
			string rowNumString = "";
			for (int i = 0; i < variableName.Length; i++)
			{
				if (Char.IsDigit(variableName[i]))
					rowNumString += variableName[i];
			}
			int.TryParse(rowNumString, out int rowNum);
			rowNum -= 1;
			int columnNum = 0;

			//Translates the variable to a column number
			switch (variableName[0].ToString())
			{
				case "A":
					columnNum = 0;
					break;
				case "B":
					columnNum = 1;
					break;
				case "C":
					columnNum = 2;
					break;
				case "D":
					columnNum = 3;
					break;
				case "E":
					columnNum = 4;
					break;
				case "F":
					columnNum = 5;
					break;
				case "G":
					columnNum = 6;
					break;
				case "H":
					columnNum = 7;
					break;
				case "I":
					columnNum = 8;
					break;
				case "J":
					columnNum = 9;
					break;
				case "K":
					columnNum = 10;
					break;
				case "L":
					columnNum = 11;
					break;
				case "M":
					columnNum = 12;
					break;
				case "N":
					columnNum = 13;
					break;
				case "O":
					columnNum = 14;
					break;
				case "P":
					columnNum = 15;
					break;
				case "Q":
					columnNum = 16;
					break;
				case "R":
					columnNum = 17;
					break;
				case "S":
					columnNum = 18;
					break;
				case "T":
					columnNum = 19;
					break;
				case "U":
					columnNum = 20;
					break;
				case "V":
					columnNum = 21;
					break;
				case "W":
					columnNum = 22;
					break;
				case "X":
					columnNum = 23;
					break;
				case "Y":
					columnNum = 24;
					break;
				case "Z":
					columnNum = 25;
					break;
			}
			spreadsheetPanel1.SetValue(columnNum, rowNum, "");
			spreadsheet.SetContentsOfCell(variableName, "");
		}
	}
}
