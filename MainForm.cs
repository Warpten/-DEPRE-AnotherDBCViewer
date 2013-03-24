using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using FileStructures;
using FileStructures.DBC;
using FileStructures.DBC.Cataclysm;
using MyDBCViewer.Extensions;
using DBFilesClient.NET;

namespace MyDBCViewer
{
    public partial class MainForm : Form
    {
        public static string SelectedBuild;             //< String identifying the build (Cataclysm or WoTLK)
        public static object CurrentDBClientFile;       //< Current DBC/DB2 being read
        public static Type CurrentDBClientFileType;     //< Underlying type of the object above
        public static string SelectedFile;              //< File currently displayed           

        public MainForm()
        {
            InitializeComponent();
        }

        private void OnApplicationLoad(object sender, EventArgs e)
        {
            // Select latest build
            OnBuildSelection(_dbcVersionSelector.DropDownItems.LastItem(), null);

            foreach (var build in _dbcVersionSelector.DropDownItems) // Bind event hook on the build enum
                (build as ToolStripMenuItem).Click += new EventHandler(OnBuildSelection);

            #region Load every DBC
            // Populate the submenu with DBCs.
            try
            {
                string[] dbcNames = Directory.GetFiles("dbc/", "*.dbc", SearchOption.TopDirectoryOnly);

                if (dbcNames.Length != 0)
                {
                    // Easy sort
                    dbcNames = dbcNames.OrderBy(d => d).ToArray();

                    ToolStripMenuItem[] alphabeticalMenuItems = new ToolStripMenuItem[26];
                    for (int i = 0; i < 26; ++i)
                        alphabeticalMenuItems[i] = new ToolStripMenuItem(((char)Enumerable.Range('A', 'Z' - 'A' + 1).ElementAt(i)).ToString());

                    ToolStripMenuItem[] items = new ToolStripMenuItem[dbcNames.Length];
                    for (int i = 0; i < dbcNames.Length; ++i)
                    {
                        string dbcFileName = dbcNames[i].Replace(".dbc", "").Replace("dbc/", "");
                        int offset = (int)dbcFileName[0].ToString().ToUpper()[0] - 65;

                        items[i] = new ToolStripMenuItem(dbcFileName);
                        items[i].Click += new EventHandler(this.OnDbcFileSelection);

                        try
                        {
                            Type fileInfo = Assembly.GetExecutingAssembly().GetFormatType("FileStructures.DBC.{0}.{1}Entry", SelectedBuild, dbcFileName.Replace("-", ""));
                            if (fileInfo == null)
                                throw new Exception();
                            items[i].Image = Properties.Resources.CheckBox;
                        }
                        catch (Exception /*ex*/) { }

                        alphabeticalMenuItems[offset].DropDownItems.Add(items[i]);
                    }

                    for (int i = 0; i < 26; ++i)
                        if (alphabeticalMenuItems[i].HasDropDownItems)
                            loadDBCToolStripMenuItem.DropDownItems.Add(alphabeticalMenuItems[i]);
                }
            }
            catch (DirectoryNotFoundException /*ex*/)
            {
                MessageBox.Show("You need to put your .dbc files into the /dbc/ subdirectory!");
                Application.Exit();
            }
            #endregion

            #region Load every DB2 file
            try
            {
                string[] db2Names = Directory.GetFiles("db2/", "*.db2", SearchOption.TopDirectoryOnly);

                foreach (string file in db2Names)
                    loadDB2ToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem(file.Replace(".db2", "").Replace("db2/", "")));

                foreach (ToolStripMenuItem item in loadDB2ToolStripMenuItem.DropDownItems)
                {
                    item.Click += new EventHandler(OnDb2FileSelection);

                    Type fileInfo = Assembly.GetExecutingAssembly().GetFormatType("FileStructures.DB2.{0}.{1}Entry", SelectedBuild, item.Text.Replace("-", ""));
                    if (fileInfo != null)
                        item.Image = Properties.Resources.CheckBox;
                }
            }
            catch (Exception /*ex*/)
            {
                MessageBox.Show("You need to put your .db2 files into the /db2/ subdirectory!");
            }
            #endregion
        }

        private void OnBuildSelection(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in _dbcVersionSelector.DropDownItems)
                item.Image = null;

            (sender as ToolStripMenuItem).Image = Properties.Resources.CheckBox;
            SelectedBuild = (sender as ToolStripMenuItem).Name.Substring(1).Replace("Build", "");
            BuildIdStatusStrip.Text = "@" + SelectedBuild;

            foreach (ToolStripMenuItem alphabetical in loadDBCToolStripMenuItem.DropDownItems)
            {
                int validatedCount = 0;
                foreach (ToolStripMenuItem dbc in alphabetical.DropDownItems)
                    if (Assembly.GetExecutingAssembly().GetFormatType("FileStructures.DBC.{0}.{1}Entry", SelectedBuild, dbc.Text.Replace("-", "")) == null)
                        dbc.Image = null;
                    else
                    {
                        dbc.Image = Properties.Resources.CheckBox;
                        ++validatedCount;
                    }

                if (validatedCount == alphabetical.DropDownItems.Count)
                    alphabetical.Image = Properties.Resources.CheckBox;
                else alphabetical.Image = null;
            }

            foreach (ToolStripMenuItem dbc in loadDB2ToolStripMenuItem.DropDownItems)
                if (Assembly.GetExecutingAssembly().GetFormatType("FileStructures.DB2.{0}.{1}Entry", SelectedBuild, dbc.Text) == null)
                    dbc.Image = null;
                else
                    dbc.Image = Properties.Resources.CheckBox;
        }

        #region File loaders (DBC, DB2)
        private void OnDb2FileSelection(object sender, EventArgs e)
        {
            InternalFileSelectionHandler((sender as ToolStripMenuItem).Text, "DB2");
        }

        private void OnDbcFileSelection(object sender, EventArgs e)
        {
            InternalFileSelectionHandler((sender as ToolStripMenuItem).Text, "DBC");
        }

        protected void InternalFileSelectionHandler(string fileName, string fileType)
        {
            try
            {
                /// TODO: Nuke out as MUCH reflection as possible

                Type classType = Assembly.GetExecutingAssembly().GetFormatType("FileStructures.{2}.{0}.{1}Entry", SelectedBuild, fileName.Replace("-", ""), fileType);
                ClientFieldInfo[] columnsArray = BaseDbcFormat.GetStructure(classType);
                if (columnsArray.Length == 0)
                    throw new Exception("Missing definition!");

                Type target = typeof(DBCStorage<>);
                if (fileType == "DB2")
                    target = typeof(DB2Storage<>);

                // Load the file
                Type[] typeArgs = { classType };
                CurrentDBClientFileType = target.MakeGenericType(typeArgs);
                CurrentDBClientFile = Activator.CreateInstance(CurrentDBClientFileType);
                using (var strm = new FileStream(String.Format("{0}\\{1}.{0}", fileType.ToLower(), fileName), FileMode.Open))
                    target.MakeGenericType(typeArgs)
                        .GetMethod("Load", new Type[] { typeof(FileStream) })
                        .Invoke(CurrentDBClientFile, new object[] { strm });

                // Now that this is populated, build our columns
                LoadTableStructure(columnsArray, fileName, fileType);

                SetSelectedFile(fileName, fileType.ToLower());
            }
            catch (Exception /*ex*/)
            {
                Utils.BoxError("No definition found for {0}.{2} ({1})", fileName, SelectedBuild, fileType.ToLower());
            }
        }
        #endregion

        private void LoadTableStructure(ClientFieldInfo[] columnsArray, string fileName, string fileType)
        {
            _lvRecordList.Columns.Clear();
            _lvRecordList.Items.Clear();

            // Load the header
            foreach (var col in columnsArray)
            {
                int colWidth = 0; // Placeholder to hide them until everything is loaded

                if (col.ArraySize != 0)
                    for (int i = 0; i < col.ArraySize; ++i)
                        _lvRecordList.Columns.Add(col.Name + "[ " + i + " ]", colWidth, HorizontalAlignment.Left);
                else
                    _lvRecordList.Columns.Add(col.Name, colWidth, HorizontalAlignment.Left); // Autowidth based off items
            }

            /// TODO: Nuke out as MUCH reflection as possible

            // Get the records
            Type[] storageType = { Assembly.GetExecutingAssembly().GetFormatType("FileStructures.{0}.{1}.{2}Entry", fileType, SelectedBuild, fileName.Replace("-", "")) };
            CurrentDBClientFileType = typeof(DBCStorage<>).MakeGenericType(storageType);
            dynamic dbcRecords = CurrentDBClientFileType.GetProperty("Records").GetValue(CurrentDBClientFile);
            foreach (var record in dbcRecords)
                _lvRecordList.Items.Add(BaseDbcFormat.CreateTableRow(record, storageType[0], record.GetType()));

            foreach (var record in dbcRecords) // Can't find any other way to iterate over Records
            {
                ClientFieldInfo[] cfInfo = BaseDbcFormat.GetStructure(record.GetType());
                foreach (ColumnHeader col in _lvRecordList.Columns)
                {
                    bool autoWidth = false;
                    foreach (ClientFieldInfo cfi in cfInfo)
                    {
                        if (cfi.FieldType != typeof(string))
                        {
                            autoWidth = true;
                            break;
                        }
                    }
                    col.Width = autoWidth ? -2 : 120;
                }
                break;
            }
        }

        #region Background worker - NYI
        private void BackgroundLoadFile(object sender, DoWorkEventArgs e)
        {

        }

        private void BackgroundLoaderProgressInform(object sender, ProgressChangedEventArgs e)
        {

        }

        private void BackgroundLoaderProgressCompleteInform(object sender, RunWorkerCompletedEventArgs e)
        {

        }
        #endregion

        private void ExportToSQL(object sender, EventArgs e)
        {
            var output = new StreamWriter("output.sql");
            output.AutoFlush = true;

            List<string> parameterList = new List<string>();
            dynamic recordWrapper = GetCurrentFileRecords();
            foreach (var record in recordWrapper)
            {
                ClientFieldInfo[] fieldInfo = BaseDbcFormat.GetStructure(record.GetType());
                foreach (ClientFieldInfo info in fieldInfo)
                {
                    if (info.ArraySize != 0)
                        for (int i = 0; i < info.ArraySize; ++i)
                            parameterList.Add(String.Format("`{0}_{1}`", info.Name, i));
                    else
                        parameterList.Add(String.Format("`{0}`", info.Name));
                }
                break;
            }

            output.WriteLine("INSERT INTO `{0}_dbc` ({1}) VALUES", SelectedFile, String.Join(", ", parameterList.ToArray()));
            List<string> records = new List<string>();
            foreach (var record in recordWrapper)
            {
                ClientFieldInfo[] fieldInfo = BaseDbcFormat.GetStructure(record.GetType());
                List<string> fieldList = new List<string>();
                foreach (var info in fieldInfo)
                {
                    if (info.ArraySize != 0)
                        for (int i = 0; i < info.ArraySize; ++i)
                            fieldList.Add(record.GetType().GetField(info.Name).GetValue(record)[i].ToString());
                    else
                        fieldList.Add(record.GetType().GetField(info.Name).GetValue(record).ToString());
                }
                records.Add(String.Format("({0})", String.Join(",", fieldList.ToArray())));
            }
            output.Write(String.Join("," + Environment.NewLine, records.ToArray()) + ";");
            output.Close();
        }

        private dynamic GetCurrentFileRecords()
        {
            return CurrentDBClientFileType.GetProperty("Records").GetValue(CurrentDBClientFile);
        }

        private void SetSelectedFile(string fileType, string fileExt)
        {
            SelectedFile = fileType;
            CurrentFileStatusStrip.Text = String.Format("{0}.{1}", fileType, fileExt);
        }

    }
}
