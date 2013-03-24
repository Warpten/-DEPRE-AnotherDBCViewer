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

                        if (Assembly.GetExecutingAssembly().GetFormatType("FileStructures.DBC.{0}.{1}Entry", SelectedBuild, dbcFileName.AsReflectionTypeIdentifier()) != null)
                            items[i].Image = Properties.Resources.CheckBox;

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
                    if (Assembly.GetExecutingAssembly().GetFormatType("FileStructures.DB2.{0}.{1}Entry", SelectedBuild, item.Text.AsReflectionTypeIdentifier()) != null)
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
            BuildIdStatusStrip.Text = "@ " + SelectedBuild;

            foreach (ToolStripMenuItem alphabetical in loadDBCToolStripMenuItem.DropDownItems)
            {
                int validatedCount = 0;
                foreach (ToolStripMenuItem dbc in alphabetical.DropDownItems)
                    if (Assembly.GetExecutingAssembly().GetFormatType("FileStructures.DBC.{0}.{1}Entry", SelectedBuild, dbc.Text.AsReflectionTypeIdentifier()) == null)
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
                if (Assembly.GetExecutingAssembly().GetFormatType("FileStructures.DB2.{0}.{1}Entry", SelectedBuild, dbc.Text.AsReflectionTypeIdentifier()) == null)
                    dbc.Image = null;
                else
                    dbc.Image = Properties.Resources.CheckBox;
        }

        #region File loaders (DBC, DB2)
        private void OnDb2FileSelection(object sender, EventArgs e)
        {
            InternalFileSelectionHandler((sender as ToolStripMenuItem).Text, "DB2", typeof(DB2Storage<>));
        }

        private void OnDbcFileSelection(object sender, EventArgs e)
        {
            InternalFileSelectionHandler((sender as ToolStripMenuItem).Text, "DBC", typeof(DBCStorage<>));
        }

        protected void InternalFileSelectionHandler(string fileName, string fileType, Type target)
        {
            BackgroundLoader.CancelAsync();
            BackgroundLoader.RunWorkerAsync(new BackgroundWorkerSettings(fileName, fileType, target));
        }
        #endregion

        #region Background workers
        private void BackgroundLoadFile(object sender, DoWorkEventArgs e)
        {
            BackgroundWorkerSettings settings = e.Argument as BackgroundWorkerSettings;

            try
            {
                BackgroundLoader.ReportProgress(0);

                /// TODO: Nuke out as MUCH reflection as possible
                Type[] classType = { Assembly.GetExecutingAssembly().GetFormatType("FileStructures.{2}.{0}.{1}Entry", SelectedBuild, settings.FileName.AsReflectionTypeIdentifier(), settings.FileType) };
                ClientFieldInfo[] columnsArray = BaseDbcFormat.GetStructure(classType[0]);
                if (columnsArray.Length == 0)
                    throw new Exception(String.Format("No definition found for {0}.{2} ({1})", settings.FileName, SelectedBuild, settings.FileType.ToLower()));

                BackgroundLoader.ReportProgress(10);

                // Load the file
                CurrentDBClientFileType = settings.TargetType.MakeGenericType(classType);
                CurrentDBClientFile = Activator.CreateInstance(CurrentDBClientFileType);
                using (var strm = new FileStream(String.Format("{0}\\{1}.{0}", settings.FileType.ToLower(), settings.FileName), FileMode.Open))
                    CurrentDBClientFileType
                        .GetMethod("Load", new Type[] { typeof(FileStream) })
                        .Invoke(CurrentDBClientFile, new object[] { strm });

                BackgroundLoader.ReportProgress(40);

                BackgroundWorkerResultWrapper result = new BackgroundWorkerResultWrapper(settings, classType[0]);

                #region Columns loading, set to hidden
                foreach (var col in columnsArray)
                    if (col.ArraySize != 0)
                        for (int i = 0; i < col.ArraySize; ++i)
                            result.AddColumnHeader(col.Name, 0, HorizontalAlignment.Left, i);
                    else
                        result.AddColumnHeader(col.Name, 0, HorizontalAlignment.Left);
                #endregion

                BackgroundLoader.ReportProgress(80);

                /// THIS IS THE MOST TIME-CONSUMING PROCESS, ALONG WITH RENDERING
                // Get the records
                CurrentDBClientFileType = typeof(DBCStorage<>).MakeGenericType(classType);
                using (dynamic dbcRecords = CurrentDBClientFileType.GetProperty("Records").GetValue(CurrentDBClientFile))
                    foreach (var record in dbcRecords)
                        result.AddRow(BaseDbcFormat.CreateTableRow(record, classType[0], record.GetType()));

                BackgroundLoader.ReportProgress(100);

                SetSelectedFile(settings.FileName, settings.FileType.ToLower());
                e.Result = result;
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
                e.Cancel = true;
            }
        }

        private void BackgroundLoaderProgressInform(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
                BackgroundWorkProgressBar.Visible = true;
            else if (e.ProgressPercentage == 100)
                BackgroundWorkProgressBar.Visible = false;

            BackgroundWorkProgressBar.Value = e.ProgressPercentage;
        }

        private void BackgroundLoaderProgressCompleteInform(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Utils.BoxError(e.Result as string);
                return;
            }

            BackgroundWorkerResultWrapper result = e.Result as BackgroundWorkerResultWrapper;

            _lvRecordList.Columns.Clear();
            _lvRecordList.Items.Clear();

            _lvRecordList.Columns.AddRange(result.Headers.ToArray());
            _lvRecordList.Items.AddRange(result.Rows.ToArray());

            foreach (ColumnHeader col in _lvRecordList.Columns)
                col.Width = BaseDbcFormat.IsFieldString(result.GetRecordType(), col.Name) ? 120 : -2;
        }
        #endregion

        private void ExportToSQL(object sender, EventArgs e)
        {
            SQLExportWorker.RunWorkerAsync();
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

        #region SQL Worker
        private void OutputSQL(object sender, DoWorkEventArgs e)
        {
            var output = new StreamWriter("output.sql");
            output.AutoFlush = true;

            SQLExportWorker.ReportProgress(0);

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

            SQLExportWorker.ReportProgress(10);

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

            SQLExportWorker.ReportProgress(60);
            
            output.Write(String.Join("," + Environment.NewLine, records.ToArray()) + ";");
            output.Close();
            
            SQLExportWorker.ReportProgress(100);
        }
        #endregion

        private void OnSQlWriteProgressUpdate(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
                sqlExportProgressBar.Visible = true;
            else if (e.ProgressPercentage == 100)
                sqlExportProgressBar.Visible = false;

            sqlExportProgressBar.Value = e.ProgressPercentage;
        }
    }

    public class BackgroundWorkerSettings
    {
        public string FileName;
        public string FileType;
        public Type TargetType;

        public BackgroundWorkerSettings(string Name, string Type, Type targetType)
        {
            FileName = Name;
            FileType = Type;
            TargetType = targetType;
        }
    }

    public class BackgroundWorkerResultWrapper
    {
        public List<ListViewItem> Rows;
        public List<ColumnHeader> Headers;
        private BackgroundWorkerSettings InitialSettings;
        public Type RecordType;

        public BackgroundWorkerResultWrapper(BackgroundWorkerSettings settings, Type recordType)
        {
            Rows = new List<ListViewItem>();
            Headers = new List<ColumnHeader>();
            InitialSettings = settings;
            RecordType = recordType;
        }

        public string GetFileName() { return InitialSettings.FileName; }
        public string GetFileType(bool lower = true) { return lower ? InitialSettings.FileType.ToLower() : InitialSettings.FileType; }
        public Type GetRecordType() { return RecordType; }

        public void AddRow(ListViewItem row) { Rows.Add(row); }
        public void AddColumnHeader(string Text, int width, HorizontalAlignment alignment, int arrayOffset = -1)
        {
            ColumnHeader header = new ColumnHeader();
            header.Name = Text;
            header.Text = Text;
            header.Width = width;
            header.TextAlign = alignment;
            if (arrayOffset != -1)
                header.Text = String.Format("{0}[{1}]", Text, arrayOffset);
            Headers.Add(header);
        }
    }
}
