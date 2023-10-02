# SpreadSheet

### Author: 
Devin White

## Instructions for Use:

#### Navigation:

Cells can be navigated to either by clicking or uses the arrow keys

Clicking will navigate to the clicked cell.

Arrow keys will navigate to new cells as follows:

**Right:** Moves right 1 cell Unless at the rightmost edge:

**Left:** Moves left 1 cell unless at the Leftmost edge:

**Up:** Moves up 1 cell unless at the top of spreadsheet.

**Down:** Moves down 1 cell unless at the bottom of spreadsheet.

**Note. Navigating with the arrow keys will populate the previous(cell before move) Cell with what is in the textbox
Clicking a new Cell will not populate the previous cell. Enter or the textbox MUST be clicked in that case.**

### File Menu:

**New:** Opens a new Empty spreadsheet

**Open:** Opens a .sprd file and populates the contents of the spreadsheet with those of the files
Updates the spreadsheet name

**Save:** If the file has a name(was saved) will automatically save the file in the same path/file. Otherwise, goes to the Saveas Dialog

**SaveAs:** Allows the user to specify the name, location, and file type, then save it.


### Edit Menu:

**Cut:** Clears the selected text in the cell or textbox, then adds it to the clipboard

**Copy:** Copies the selected text to keyboard

**If copy is used while selected on a cell(focused on a cell), will copy the CONTENTS of the cell rather than the value**

**If copy is used while focus is on textbox, will copy the contents of the textbox**

**Paste:** Pastes the contents of the clipboard

**If focus is on a cell, will copy the contents to that cell and update the spreadsheet accordingly**

**If focus is on textbox, will copy contents to textbox.**

### Select All:

Selects the entirety of the Text in the textbox

**Help:** Opens a new form with Instructions for Using the spreadsheet

**About:** Opens README file

### UI Elements

**Cell name:** Displays the name of the cell. Can be toggled visible or invisible

**Cell Contents:** Editable textbox that displays cell contents. Contents of Cell can be edited by entering the desired information
and clicking the textbox or hitting the ENTER key

**Cell Value:** Displays the numerical Value of the cell. This can be a string, double, or FormulaError is formula contains
a cell with either no value or that is out of range

### Tools:

**Customize:** Contains various options for slightly editing the UI elements

**Random Stat Generator:** Generates the typical D & D stats randomly

**Show Edit Button:** Toggles the Edit button on and off

**Show Help Button:** Toggles the help button on and off

**Show File button:** Toggles the file button on and off

**Show Cell name Box:** Toggles cell name box on and Off

**Show Cell Value Box:** Toggles Cell value box on and off.

**The Cell contents box, the Tools box, and the spreadsheet cells can't be toggled on or Off**

### Additional Features:

Buttons to Hide/Show everything besides the cell contents and Help button

Added copy/paste/Cut functionality, and let them work when the focus is on either the cell or the contents box

Added SelectAll functionality: which Selects the entirety of the Text in the textbox

Added a random stat generator because I could. It's under tools

Want to add printing functionality, along with a Tic Tac toe game. But ran out of time.


Sources for Implementation from Official Microsoft Documentation
