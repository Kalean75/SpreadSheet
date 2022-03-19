
namespace SpreadsheetGUI
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CellNameTextBox = new System.Windows.Forms.ToolStripTextBox();
			this.CellContentsTextBox = new System.Windows.Forms.ToolStripTextBox();
			this.CellValueTextBox = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showEditButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showHelpButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showFileButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showCellNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showCellValueBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.randomStatGeneratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
			this.printDocument1 = new System.Drawing.Printing.PrintDocument();
			this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.CellNameTextBox,
            this.CellContentsTextBox,
            this.CellValueTextBox,
            this.toolStripMenuItem2,
            this.toolsToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(3, 1, 0, 1);
			this.menuStrip1.Size = new System.Drawing.Size(771, 25);
			this.menuStrip1.TabIndex = 6;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem1,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem1,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 23);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// newToolStripMenuItem1
			// 
			this.newToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem1.Image")));
			this.newToolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
			this.newToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newToolStripMenuItem1.Size = new System.Drawing.Size(146, 22);
			this.newToolStripMenuItem1.Text = "&New";
			this.newToolStripMenuItem1.Click += new System.EventHandler(this.newToolStripMenuItem1_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
			this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.LoadButton_Click);
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(143, 6);
			// 
			// saveToolStripMenuItem1
			// 
			this.saveToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem1.Image")));
			this.saveToolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
			this.saveToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem1.Size = new System.Drawing.Size(146, 22);
			this.saveToolStripMenuItem1.Text = "&Save";
			this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveTool_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.saveAsToolStripMenuItem.Text = "Save &As";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsTool_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator3,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.selectAllToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 23);
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(141, 6);
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
			this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.cutToolStripMenuItem.Text = "Cu&t";
			this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
			this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.copyToolStripMenuItem.Text = "&Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
			this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.pasteToolStripMenuItem.Text = "&Paste";
			this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(141, 6);
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.selectAllToolStripMenuItem.Text = "Select &All";
			this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(113, 6);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
			this.aboutToolStripMenuItem.Text = "&About...";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// CellNameTextBox
			// 
			this.CellNameTextBox.Enabled = false;
			this.CellNameTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.CellNameTextBox.Name = "CellNameTextBox";
			this.CellNameTextBox.ReadOnly = true;
			this.CellNameTextBox.Size = new System.Drawing.Size(62, 23);
			this.CellNameTextBox.Text = "Cell Name";
			this.CellNameTextBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// CellContentsTextBox
			// 
			this.CellContentsTextBox.AutoSize = false;
			this.CellContentsTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.CellContentsTextBox.Name = "CellContentsTextBox";
			this.CellContentsTextBox.Size = new System.Drawing.Size(202, 23);
			this.CellContentsTextBox.Click += new System.EventHandler(this.CellContentsTextBox_Click);
			// 
			// CellValueTextBox
			// 
			this.CellValueTextBox.Enabled = false;
			this.CellValueTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.CellValueTextBox.Name = "CellValueTextBox";
			this.CellValueTextBox.ReadOnly = true;
			this.CellValueTextBox.Size = new System.Drawing.Size(152, 23);
			this.CellValueTextBox.Text = "Cell Value";
			this.CellValueTextBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(12, 23);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customizeToolStripMenuItem,
            this.randomStatGeneratorToolStripMenuItem});
			this.toolsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 23);
			this.toolsToolStripMenuItem.Text = "&Tools";
			// 
			// customizeToolStripMenuItem
			// 
			this.customizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showEditButtonToolStripMenuItem,
            this.showHelpButtonToolStripMenuItem,
            this.showFileButtonToolStripMenuItem,
            this.showCellNameToolStripMenuItem,
            this.showCellValueBoxToolStripMenuItem});
			this.customizeToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
			this.customizeToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
			this.customizeToolStripMenuItem.Text = "&Customize";
			// 
			// showEditButtonToolStripMenuItem
			// 
			this.showEditButtonToolStripMenuItem.Checked = true;
			this.showEditButtonToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showEditButtonToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.showEditButtonToolStripMenuItem.Name = "showEditButtonToolStripMenuItem";
			this.showEditButtonToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.showEditButtonToolStripMenuItem.Text = "Show Edit Button";
			this.showEditButtonToolStripMenuItem.Click += new System.EventHandler(this.showEditButtonToolStripMenuItem_Click);
			// 
			// showHelpButtonToolStripMenuItem
			// 
			this.showHelpButtonToolStripMenuItem.Checked = true;
			this.showHelpButtonToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showHelpButtonToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.showHelpButtonToolStripMenuItem.Name = "showHelpButtonToolStripMenuItem";
			this.showHelpButtonToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.showHelpButtonToolStripMenuItem.Text = "Show Help Button";
			this.showHelpButtonToolStripMenuItem.Click += new System.EventHandler(this.showHelpButtonToolStripMenuItem_Click);
			// 
			// showFileButtonToolStripMenuItem
			// 
			this.showFileButtonToolStripMenuItem.Checked = true;
			this.showFileButtonToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showFileButtonToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.showFileButtonToolStripMenuItem.Name = "showFileButtonToolStripMenuItem";
			this.showFileButtonToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.showFileButtonToolStripMenuItem.Text = "Show File Button";
			this.showFileButtonToolStripMenuItem.Click += new System.EventHandler(this.showFileButtonToolStripMenuItem_Click);
			// 
			// showCellNameToolStripMenuItem
			// 
			this.showCellNameToolStripMenuItem.Checked = true;
			this.showCellNameToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showCellNameToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.showCellNameToolStripMenuItem.Name = "showCellNameToolStripMenuItem";
			this.showCellNameToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.showCellNameToolStripMenuItem.Text = "Show Cell Name Box";
			this.showCellNameToolStripMenuItem.Click += new System.EventHandler(this.showCellNameToolStripMenuItem_Click);
			// 
			// showCellValueBoxToolStripMenuItem
			// 
			this.showCellValueBoxToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.showCellValueBoxToolStripMenuItem.Checked = true;
			this.showCellValueBoxToolStripMenuItem.CheckOnClick = true;
			this.showCellValueBoxToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showCellValueBoxToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.showCellValueBoxToolStripMenuItem.Name = "showCellValueBoxToolStripMenuItem";
			this.showCellValueBoxToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.showCellValueBoxToolStripMenuItem.Text = "Show Cell Value Box";
			this.showCellValueBoxToolStripMenuItem.Click += new System.EventHandler(this.showCellValueBoxToolStripMenuItem_Click);
			// 
			// randomStatGeneratorToolStripMenuItem
			// 
			this.randomStatGeneratorToolStripMenuItem.Name = "randomStatGeneratorToolStripMenuItem";
			this.randomStatGeneratorToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
			this.randomStatGeneratorToolStripMenuItem.Text = "Random Stat Generator";
			this.randomStatGeneratorToolStripMenuItem.Click += new System.EventHandler(this.randomStatGeneratorToolStripMenuItem_Click);
			// 
			// printPreviewDialog1
			// 
			this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
			this.printPreviewDialog1.Document = this.printDocument1;
			this.printPreviewDialog1.Enabled = true;
			this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
			this.printPreviewDialog1.Name = "printPreviewDialog1";
			this.printPreviewDialog1.Visible = false;
			// 
			// spreadsheetPanel1
			// 
			this.spreadsheetPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.spreadsheetPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 25);
			this.spreadsheetPanel1.Name = "spreadsheetPanel1";
			this.spreadsheetPanel1.Size = new System.Drawing.Size(771, 401);
			this.spreadsheetPanel1.TabIndex = 0;
			this.spreadsheetPanel1.SelectionChanged += new SS.SelectionChangedHandler(this.spreadsheetPanel1_SelectionChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(771, 426);
			this.Controls.Add(this.spreadsheetPanel1);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "New Spreadsheet";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private SS.SpreadsheetPanel spreadsheetPanel1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripTextBox CellContentsTextBox;
		private System.Windows.Forms.ToolStripTextBox CellNameTextBox;
		private System.Windows.Forms.ToolStripTextBox CellValueTextBox;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem showEditButtonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showHelpButtonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showFileButtonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showCellValueBoxToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showCellNameToolStripMenuItem;
		private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
		private System.Drawing.Printing.PrintDocument printDocument1;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem randomStatGeneratorToolStripMenuItem;
	}
}

