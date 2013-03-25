using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBFilesClient.NET;
using FileStructures;
using MyDBCViewer.Extensions;
using MyDBCViewer.Properties;

namespace MyDBCViewer
{
    public partial class MainForm : Form
    {
        public static object CurrentStorage;   //< DBCStorage<T>
        public static Type CurrentStorageType; //< typeof(DBCStorage<T>)
        public static Type CurrentDbFileType;  //< U in typeof(T<U>)
        public static string SelectedBuild; //< String identifying the build (Cataclysm or WoTLK)
        public static string SelectedFile; //< File currently displayed

        public MainForm()
        {
            InitializeComponent();
        }

        // ReSharper disable InconsistentNaming
        private void ExportToSQL(object sender, EventArgs e)
        // ReSharper restore InconsistentNaming
        {
            SQLExportWorker.RunWorkerAsync();
        }

        private dynamic GetCurrentStorage()
        {
            return CurrentStorageType.GetProperty("Records").GetValue(CurrentStorage);
        }

        private void OnApplicationLoad(object sender, EventArgs e)
        {
            // Select latest build
            OnBuildSelection(_dbcVersionSelector.DropDownItems.LastItem(), null);

            foreach (ToolStripMenuItem build in _dbcVersionSelector.DropDownItems) // Bind event hook on the build enum
                build.Click += OnBuildSelection;

            #region Load every DBC

            try
            {
                string[] dbcNames = Directory.GetFiles("dbc/", "*.dbc", SearchOption.TopDirectoryOnly);

                if (dbcNames.Length != 0)
                {
                    // Easy sort
                    dbcNames = dbcNames.OrderBy(d => d).ToArray();

                    var alphabeticalMenuItems = new ToolStripMenuItem[26];
                    for (int i = 0; i < 26; ++i)
                        alphabeticalMenuItems[i] =
                            new ToolStripMenuItem(((char)Enumerable.Range('A', 'Z' - 'A' + 1).ElementAt(i)).ToString(CultureInfo.InvariantCulture));

                    var items = new ToolStripMenuItem[dbcNames.Length];
                    for (int i = 0; i < dbcNames.Length; ++i)
                    {
                        string dbcFileName = dbcNames[i].Replace(".dbc", "").Replace("dbc/", "");
                        int offset = dbcFileName[0].ToString(CultureInfo.InvariantCulture).ToUpper()[0] - 65;

                        items[i] = new ToolStripMenuItem(dbcFileName);

                        if (
                            Assembly.GetExecutingAssembly()
                                    .GetFormatType("FileStructures.DBC.{0}.{1}Entry", SelectedBuild,
                                                   dbcFileName.AsReflectionTypeIdentifier()) != null)
                        {
                            items[i].Image = Resources.CheckBox;
                            items[i].Click += OnDbcFileSelection;
                        }

                        alphabeticalMenuItems[offset].DropDownItems.Add(items[i]);
                    }

                    for (int i = 0; i < 26; ++i)
                        if (alphabeticalMenuItems[i].HasDropDownItems)
                            loadDBCToolStripMenuItem.DropDownItems.Add(alphabeticalMenuItems[i]);
                }
            }
            catch (DirectoryNotFoundException /*ex*/)
            {
                MessageBox.Show(@"You need to put your .dbc files into the /dbc/ subdirectory!");
                Application.Exit();
            }

            #endregion Load every DBC

            #region Load every DB2 file

            try
            {
                string[] db2Names = Directory.GetFiles("db2/", "*.db2", SearchOption.TopDirectoryOnly);

                foreach (string file in db2Names)
                    loadDB2ToolStripMenuItem.DropDownItems.Add(
                        new ToolStripMenuItem(file.Replace(".db2", "").Replace("db2/", "")));

                foreach (ToolStripMenuItem item in loadDB2ToolStripMenuItem.DropDownItems)
                {
                    if (
                        Assembly.GetExecutingAssembly()
                                .GetFormatType("FileStructures.DB2.{0}.{1}Entry", SelectedBuild,
                                               item.Text.AsReflectionTypeIdentifier()) != null)
                    {
                        item.Image = Resources.CheckBox;
                        item.Click += OnDb2FileSelection;
                    }
                }
            }
            catch (Exception /*ex*/)
            {
                MessageBox.Show(@"You need to put your .db2 files into the /db2/ subdirectory!");
            }

            #endregion Load every DB2 file
        }

        private void OnBuildSelection(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in _dbcVersionSelector.DropDownItems)
                item.Image = null;

            ((ToolStripMenuItem)sender).Image = Resources.CheckBox;
            SelectedBuild = ((ToolStripMenuItem)sender).Name.Substring(1).Replace("Build", "");
            BuildIdStatusStrip.Text = @"@ " + SelectedBuild;

            foreach (ToolStripMenuItem alphabetical in loadDBCToolStripMenuItem.DropDownItems)
            {
                int validatedCount = 0;
                foreach (ToolStripMenuItem dbc in alphabetical.DropDownItems)
                    if (
                        Assembly.GetExecutingAssembly()
                                .GetFormatType("FileStructures.DBC.{0}.{1}Entry", SelectedBuild,
                                               dbc.Text.AsReflectionTypeIdentifier()) == null)
                        dbc.Image = null;
                    else
                    {
                        dbc.Image = Resources.CheckBox;
                        ++validatedCount;
                    }

                alphabetical.Image = validatedCount == alphabetical.DropDownItems.Count ? Resources.CheckBox : null;
            }

            foreach (ToolStripMenuItem dbc in loadDB2ToolStripMenuItem.DropDownItems)
                dbc.Image = Assembly.GetExecutingAssembly()
                                    .GetFormatType("FileStructures.DB2.{0}.{1}Entry", SelectedBuild,
                                                   dbc.Text.AsReflectionTypeIdentifier()) == null ? null : Resources.CheckBox;
        }

        private void OnSQlWriteProgressUpdate(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
                sqlExportProgressBar.Visible = true;
            else if (e.ProgressPercentage == 100)
                sqlExportProgressBar.Visible = false;

            sqlExportProgressBar.Value = e.ProgressPercentage;
        }

        private void SetSelectedFile(string fileType, string fileExt)
        {
            SelectedFile = fileType;
            CurrentFileStatusStrip.Text = String.Format("{0}.{1}", fileType, fileExt);
        }

        #region SQL Background Worker

        // ReSharper disable InconsistentNaming
        private void OutputSQL(object sender, DoWorkEventArgs e)
        // ReSharper restore InconsistentNaming
        {
            var output = new StreamWriter("output.sql") { AutoFlush = true };

            SQLExportWorker.ReportProgress(0);

            var parameterList = new List<string>();
            dynamic recordWrapper = GetCurrentStorage();
            ClientFieldInfo[] fileInfo = BaseDbcFormat.GetStructure(CurrentDbFileType);

            foreach (var info in fileInfo)
            {
                if (info.ArraySize != 0)
                    for (int i = 0; i < info.ArraySize; ++i)
                        parameterList.Add(String.Format("`{0}_{1}`", info.Name, i));
                else
                    parameterList.Add(String.Format("`{0}`", info.Name));
            }

            output.WriteLine("INSERT INTO `{0}Records` ({1}) VALUES", SelectedFile,
                             String.Join(", ", parameterList.ToArray()));
            parameterList.Clear(); // Reuse it
            SQLExportWorker.ReportProgress(50);

            int recordIndex = 0;
            int lastRecord = recordWrapper.Count - 1;
            foreach (var record in recordWrapper)
            {
                var fieldList = new List<string>();
                foreach (var info in fileInfo)
                {
                    bool isStringField = (info.FieldType == typeof(string));
                    FieldInfo fi = record.GetType().GetField(info.Name);
                    if (info.ArraySize != 0)
                    {
                        for (int i = 0; i < info.ArraySize; ++i)
                        {
                            string value = fi.GetValue(record)[i].ToString();
                            if (isStringField)
                                fieldList.Add("\"" + value.Escape() + "\"");
                            else
                                fieldList.Add(value);
                        }
                    }
                    else
                    {
                        string value = fi.GetValue(record).ToString();
                        if (isStringField)
                            fieldList.Add("\"" + value.Escape() + "\"");
                        else
                            fieldList.Add(value);
                    }
                }
                output.WriteLine("({0})" + (recordIndex == lastRecord ? ";" : ","),
                                 String.Join(",", fieldList.ToArray()));
                output.Flush();
                ++recordIndex;
            }

            output.Close();
            SQLExportWorker.ReportProgress(100);
        }

        #endregion SQL Background Worker

        #region File loaders (DBC, DB2)

        protected void InternalFileSelectionHandler(string fileName, string fileType, Type target)
        {
            BackgroundLoader.CancelAsync();
            BackgroundLoader.RunWorkerAsync(new BackgroundWorkerSettings(fileName, fileType, target));
        }

        private void OnDb2FileSelection(object sender, EventArgs e)
        {
            InternalFileSelectionHandler(((ToolStripMenuItem)sender).Text, "DB2", typeof(DB2Storage<>));
        }

        private void OnDbcFileSelection(object sender, EventArgs e)
        {
            InternalFileSelectionHandler(((ToolStripMenuItem)sender).Text, "DBC", typeof(DBCStorage<>));
        }

        #endregion File loaders (DBC, DB2)

        #region Background DBC/DB2 loader

        private void BackgroundLoaderProgressInform(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
                BackgroundWorkProgressBar.Visible = true;
            BackgroundWorkProgressBar.Value = e.ProgressPercentage;
        }

        private async void BackgroundLoadFile(object sender, DoWorkEventArgs e)
        {
            var settings = (BackgroundWorkerSettings)e.Argument;

            try
            {
                BackgroundLoader.ReportProgress(0);

                //! TODO: Nuke out as much reflection as possible
                CurrentDbFileType = Assembly.GetExecutingAssembly()
                                            .GetFormatType("FileStructures.{2}.{0}.{1}Entry", SelectedBuild,
                                                           settings.FileName.AsReflectionTypeIdentifier(),
                                                           settings.FileType);

                ClientFieldInfo[] columnsArray = BaseDbcFormat.GetStructure(CurrentDbFileType);

                if (columnsArray.Length == 0)
                    throw new Exception(String.Format("No definition found for {0}.{2} ({1})", settings.FileName,

                                                      SelectedBuild, settings.FileType.ToLower()));

                BackgroundLoader.ReportProgress(10);

                // Load the file
                CurrentStorageType = settings.TargetType.MakeGenericType(new[] { CurrentDbFileType });

                CurrentStorage = Activator.CreateInstance(CurrentStorageType);
                using (
                    var strm =
                        new FileStream(String.Format("{0}\\{1}.{0}", settings.FileType.ToLower(), settings.FileName),
                                       FileMode.Open))
                    CurrentStorageType
                        .GetMethod("Load", new[] { typeof(FileStream) })
                        .Invoke(CurrentStorage, new object[] { strm });

                BackgroundLoader.ReportProgress(40);
                var result = new BackgroundWorkerResultWrapper(settings, CurrentDbFileType);

                #region Columns loading, set to hidden

                foreach (var col in columnsArray)
                    if (col.ArraySize != 0)
                        for (var i = 0; i < col.ArraySize; ++i)
                            result.AddColumnHeader(col.Name, 0, HorizontalAlignment.Left, i);
                    else
                        result.AddColumnHeader(col.Name, 0, HorizontalAlignment.Left);

                #endregion Columns loading, set to hidden

                BackgroundLoader.ReportProgress(60);

                // Get the records
                using (dynamic dbcRecords = CurrentStorageType.GetProperty("Records").GetValue(CurrentStorage))
                    foreach (var record in dbcRecords)
                        result.AddRow(BaseDbcFormat.CreateTableRow(record, CurrentDbFileType, record.GetType()));

                SetSelectedFile(settings.FileName, settings.FileType.ToLower());
                BackgroundLoader.ReportProgress(100);

                await Task.Run(() =>
                {
                    _lvRecordList.Clear();
                    _lvRecordList.BeginUpdate();
                    _lvRecordList.Columns.AddRange(result.Headers.ToArray());
                    _lvRecordList.Items.AddRange(result.Rows.ToArray());

                    foreach (ColumnHeader col in _lvRecordList.Columns)
                        col.Width = BaseDbcFormat.IsFieldString(result.GetRecordType(), col.Name) ? 120 : -2;
                    RecordsCountLabel.Text = String.Format(@"Records: {0}", result.Rows.Count);
                    _lvRecordList.EndUpdate();
                    BackgroundWorkProgressBar.Visible = false;
                });
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
                e.Cancel = true;
                Utils.BoxError(ex.Message);
            }
        }

        #endregion Background DBC/DB2 loader
    }

    public class BackgroundWorkerResultWrapper
    {
        // ReSharper disable InconsistentNaming
        private readonly BackgroundWorkerSettings InitialSettings;

        // ReSharper restore InconsistentNaming
        public List<ColumnHeader> Headers;

        public Type RecordType;
        public List<ListViewItem> Rows;

        public BackgroundWorkerResultWrapper(BackgroundWorkerSettings settings, Type recordType)
        {
            Rows = new List<ListViewItem>();
            Headers = new List<ColumnHeader>();
            InitialSettings = settings;
            RecordType = recordType;
        }

        public void AddColumnHeader(string text, int width, HorizontalAlignment alignment, int arrayOffset = -1)
        {
            var header = new ColumnHeader { Name = text, Text = text, Width = width, TextAlign = alignment };
            if (arrayOffset != -1)
                header.Text = String.Format("{0}[{1}]", text, arrayOffset);
            Headers.Add(header);
        }

        public void AddRow(ListViewItem row)
        {
            Rows.Add(row);
        }

        public string GetFileName()
        {
            return InitialSettings.FileName;
        }

        public string GetFileType(bool lower = true)
        {
            return lower ? InitialSettings.FileType.ToLower() : InitialSettings.FileType;
        }

        public Type GetRecordType()
        {
            return RecordType;
        }
    }

    public class BackgroundWorkerSettings
    {
        public string FileName;
        public string FileType;
        public Type TargetType;

        public BackgroundWorkerSettings(string name, string type, Type targetType)
        {
            FileName = name;
            FileType = type;
            TargetType = targetType;
        }
    }
}