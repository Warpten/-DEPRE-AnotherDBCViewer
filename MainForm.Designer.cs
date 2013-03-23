namespace MyDBCViewer
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDBCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._dbcVersionSelector = new System.Windows.Forms.ToolStripMenuItem();
            this._WoTLKBuild = new System.Windows.Forms.ToolStripMenuItem();
            this._CataclysmBuild = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toSQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._lvRecordList = new System.Windows.Forms.ListView();
            this.BackgroundLoader = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(3, 1);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(97, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDBCToolStripMenuItem,
            this._dbcVersionSelector});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadDBCToolStripMenuItem
            // 
            this.loadDBCToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.loadDBCToolStripMenuItem.Name = "loadDBCToolStripMenuItem";
            this.loadDBCToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadDBCToolStripMenuItem.Text = "Load DBC";
            // 
            // _dbcVersionSelector
            // 
            this._dbcVersionSelector.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._WoTLKBuild,
            this._CataclysmBuild});
            this._dbcVersionSelector.Name = "_dbcVersionSelector";
            this._dbcVersionSelector.Size = new System.Drawing.Size(152, 22);
            this._dbcVersionSelector.Text = "DBC Version";
            // 
            // _WoTLKBuild
            // 
            this._WoTLKBuild.Name = "_WoTLKBuild";
            this._WoTLKBuild.Size = new System.Drawing.Size(152, 22);
            this._WoTLKBuild.Text = "3.3.5 12340";
            // 
            // _CataclysmBuild
            // 
            this._CataclysmBuild.Name = "_CataclysmBuild";
            this._CataclysmBuild.Size = new System.Drawing.Size(152, 22);
            this._CataclysmBuild.Text = "4.3.4 15595";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toSQLToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // toSQLToolStripMenuItem
            // 
            this.toSQLToolStripMenuItem.Name = "toSQLToolStripMenuItem";
            this.toSQLToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.toSQLToolStripMenuItem.Text = "To SQL";
            // 
            // _lvRecordList
            // 
            this._lvRecordList.AllowColumnReorder = true;
            this._lvRecordList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lvRecordList.FullRowSelect = true;
            this._lvRecordList.GridLines = true;
            this._lvRecordList.HideSelection = false;
            this._lvRecordList.Location = new System.Drawing.Point(3, 28);
            this._lvRecordList.MinimumSize = new System.Drawing.Size(914, 402);
            this._lvRecordList.MultiSelect = false;
            this._lvRecordList.Name = "_lvRecordList";
            this._lvRecordList.Size = new System.Drawing.Size(914, 402);
            this._lvRecordList.TabIndex = 8;
            this._lvRecordList.UseCompatibleStateImageBehavior = false;
            this._lvRecordList.View = System.Windows.Forms.View.Details;
            // 
            // BackgroundLoader
            // 
            this.BackgroundLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundLoadFile);
            this.BackgroundLoader.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundLoaderProgressInform);
            this.BackgroundLoader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundLoaderProgressCompleteInform);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 433);
            this.Controls.Add(this._lvRecordList);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "DBCViewer";
            this.Load += new System.EventHandler(this.OnApplicationLoad);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadDBCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toSQLToolStripMenuItem;
        private System.Windows.Forms.ListView _lvRecordList;
        private System.ComponentModel.BackgroundWorker BackgroundLoader;
        private System.Windows.Forms.ToolStripMenuItem _dbcVersionSelector;
        private System.Windows.Forms.ToolStripMenuItem _WoTLKBuild;
        private System.Windows.Forms.ToolStripMenuItem _CataclysmBuild;
    }
}

