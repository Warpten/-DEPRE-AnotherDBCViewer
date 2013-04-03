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
        public string SelectedBuild; //< String identifying the build (Cataclysm or WoTLK)
        public string SelectedFile; //< File currently displayed
        public FileInfoStorage Storage; //< Contains information about every record currently held.

        public MainForm()
        {
            InitializeComponent();
        }

        private void ExportToSQL(object sender, EventArgs e)
        {
            SQLExportWorker.RunWorkerAsync();
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

                    // int validatedCount = 0;
                    var items = new ToolStripMenuItem[dbcNames.Length];
                    for (int i = 0; i < dbcNames.Length; ++i)
                    {
                        string dbcFileName = dbcNames[i].Replace(".dbc", "").Replace("dbc/", "");
                        int offset = dbcFileName[0].ToString(CultureInfo.InvariantCulture).ToUpper()[0] - 65;

                        items[i] = new ToolStripMenuItem(dbcFileName);

                        if (Assembly.GetExecutingAssembly().GetFormatType("FileStructures.DBC.{0}.{1}Entry", SelectedBuild, dbcFileName.AsReflectionTypeIdentifier()) != null)
                        {
                            items[i].Image = Resources.CheckBox;
                            items[i].Click += OnDbcFileSelection;
                            // ++validatedCount;
                        }

                        alphabeticalMenuItems[offset].DropDownItems.Add(items[i]);
                        // alphabeticalMenuItems[offset].Image = (validatedCount == alphabeticalMenuItems[offset].DropDownItems.Count - 1) ? Resources.CheckBox : null;
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

            CheckForIllegalCrossThreadCalls = false;
            _lvRecordList.DoubleBuffering(true);
        }

        private void OnBuildSelection(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in _dbcVersionSelector.DropDownItems)
                item.Image = null;

            ((ToolStripMenuItem)sender).Image = Resources.CheckBox;
            SelectedBuild = ((ToolStripMenuItem)sender).Name.Substring(1).Replace("Build", "");
            BuildIdStatusStrip.Text = @"@ " + SelectedBuild;

            foreach (ToolStripMenuItem alphabetical in loadDBCToolStripMenuItem.DropDownItems)
                foreach (ToolStripMenuItem dbc in alphabetical.DropDownItems)
                    dbc.Image = Assembly.GetExecutingAssembly()
                                        .GetFormatType("FileStructures.DBC.{0}.{1}Entry", SelectedBuild,
                                                       dbc.Text.AsReflectionTypeIdentifier()) == null ? null : Resources.CheckBox;

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
            string tableName = Storage.GetFileName().AsReflectionTypeIdentifier() + "Records";

            var output = new StreamWriter("output.sql") { AutoFlush = true };
            SQLExportWorker.ReportProgress(0);

            var parameterList = new List<string>();
            var structure = new List<string>();
            ClientFieldInfo[] fileInfo = BaseDbcFormat.GetStructure(Storage.RecordType);

            output.WriteLine("DROP TABLE IF EXISTS `{0}`;", tableName);
            foreach (var info in fileInfo)
            {
                bool isString = (info.FieldType == typeof(string)); // BaseDbcFormat.IsFieldString(Storage.RecordType, info.Name);
                if (info.ArraySize != 0)
                {
                    for (int i = 0; i < info.ArraySize; ++i)
                    {
                        parameterList.Add(String.Format("`{0}_{1}`", info.Name, i));
                        structure.Add(String.Format("`{0}_{1}` {2}", info.Name, i, isString ? "TEXT" : "BIGINT(20)"));
                    }
                }
                else
                {
                    parameterList.Add(String.Format("`{0}`", info.Name));
                    structure.Add(String.Format("`{0}` {1}", info.Name, isString ? "TEXT" : "BIGINT(20)"));
                }
            }

            string insertHeader = String.Format("INSERT INTO `{0}` ({1}) VALUES", tableName, String.Join(", ", parameterList.ToArray()));

            output.WriteLine("CREATE TABLE `{0}` (", tableName);
            int recordItr = 0; // Actually used as the field iterator first
            foreach (var field in structure)
            {
                output.WriteLine("  {0}{1}", field, (recordItr + 1) == structure.Count ? "" : ",");
                ++recordItr;
            }
            output.WriteLine(");" + Environment.NewLine);
            output.WriteLine(insertHeader);

            SQLExportWorker.ReportProgress(50);

            recordItr = 1;
            foreach (var record in Storage.Store)
            {
                var fieldList = new List<string>();
                foreach (var info in fileInfo)
                {
                    FieldInfo fi = record.GetType().GetField(info.Name);

                    if (info.ArraySize != 0)
                    {
                        dynamic value = fi.GetValue(record);
                        foreach (object item in value)
                        {
                            string itemVal = item.ToString();
                            if (info.FieldType == typeof(string))
                                fieldList.Add("\"" + itemVal.Escape() + "\"");
                            else
                                fieldList.Add(itemVal.Replace(",", "."));
                        }
                    }
                    else
                    {
                        string value = fi.GetValue(record).ToString();
                        if (info.FieldType == typeof(string))
                            fieldList.Add(" \"" + value.Escape() + "\"");
                        else
                            fieldList.Add(value.Replace(",", "."));
                    }
                }
                output.WriteLine("({0})" + (recordItr % 250 == 0 || recordItr == Storage.Store.Count ? ";" : ","), String.Join(",", fieldList.ToArray()));

                if (recordItr % 250 == 0)
                    output.WriteLine(Environment.NewLine + insertHeader);
                ++recordItr;
            }

            output.Close();
            SQLExportWorker.ReportProgress(100);
        }

        #endregion SQL Background Worker

        #region File loaders (DBC, DB2)

        protected void InternalFileSelectionHandler(string fileName, string fileType, Type target)
        {
            BackgroundLoader.CancelAsync();
            BackgroundLoader.RunWorkerAsync(new BackgroundWorkerSettings(fileName, fileType, target, SelectedBuild));
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
            Storage = new FileInfoStorage((BackgroundWorkerSettings)e.Argument, ref _lvRecordList);

            try
            {
                BackgroundLoader.ReportProgress(0);

                ClientFieldInfo[] columnsArray = BaseDbcFormat.GetStructure(Storage.RecordType);

                if (columnsArray.Length == 0)
                    throw new Exception(String.Format("No definition found for {0}.{2} ({1})", Storage.GetFileName(),
                                                      SelectedBuild, Storage.GetFileType().ToLower()));

                BackgroundLoader.ReportProgress(10);

                // Load the file
                Type currentStorageType = Storage.StorageType.MakeGenericType(new[] { Storage.RecordType });
                var currentStorage = Activator.CreateInstance(currentStorageType);

                using (var strm = new FileStream(String.Format(@"{0}\{1}.{0}", Storage.GetFileType().ToLower(), Storage.GetFileName()), FileMode.Open))
                {
                    MethodInfo mi = currentStorageType.GetMethod("Load", new[] { typeof(FileStream) });
                    mi.Invoke(currentStorage, new[] { (object)strm });
                }

                BackgroundLoader.ReportProgress(40);

                #region Columns loading, set to hidden
                Storage.ClearStore();

                foreach (var col in columnsArray)
                    if (col.ArraySize != 0)
                        for (var i = 0; i < col.ArraySize; ++i)
                            Storage.AddColumnHeader(col.Name, 0, HorizontalAlignment.Left, i);
                    else
                        Storage.AddColumnHeader(col.Name, 0, HorizontalAlignment.Left);

                #endregion Columns loading, set to hidden

                BackgroundLoader.ReportProgress(60);

                // Get the records
                using (dynamic dbcRecords = currentStorageType.GetProperty("Records").GetValue(currentStorage))
                    foreach (var record in dbcRecords)
                        Storage.AddRecord(record);

                SetSelectedFile(Storage.GetFileName(), Storage.GetFileType().ToLower());
                BackgroundLoader.ReportProgress(90);

                await Task.Run(() => Storage.Render(ref _lvRecordList, ref RecordsCountLabel, ref BackgroundWorkProgressBar));
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                e.Result = ex.Message;
                e.Cancel = true;
                Utils.BoxError(ex.Message);
            }
        }

        #endregion Background DBC/DB2 loader

        private void OnFilterMenuSelect(object sender, EventArgs e)
        {

        }
    }

    public class FileInfoStorage
    {
        // ReSharper disable InconsistentNaming
        private BackgroundWorkerSettings InitialSettings;
        public List<ColumnHeader> Headers;
        public ListView Renderer;
        public Type RecordType
        {
            get { return InitialSettings.RecordType; }
        }
        public Type StorageType
        {
            get { return InitialSettings.StorageType; }
        }
        public List<ListViewItem> Rows;
        public List<object> Store;
        // ReSharper restore InconsistentNaming

        public FileInfoStorage(BackgroundWorkerSettings settings, ref ListView renderer)
        {
            Rows = new List<ListViewItem>();
            Headers = new List<ColumnHeader>();
            Store = new List<object>();
            Renderer = renderer;

            InitialSettings = settings;
        }

        public void AddColumnHeader(string text, int width, HorizontalAlignment alignment, int arrayOffset = -1)
        {
            var header = new ColumnHeader { Name = text, Text = text, Width = width, TextAlign = alignment };
            if (arrayOffset != -1)
                header.Text = String.Format("{0}[{1}]", text, arrayOffset);
            Headers.Add(header);
        }

        public void AddRecord(dynamic record)
        {
            Store.Add(record);
        }

        public void ClearStore()
        {
            Store.Clear();
            Rows.Clear();
            Headers.Clear();
        }

        public void AddRenderingRow(ListViewItem row)
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

        public void PopulateRows()
        {
            foreach (dynamic record in Store)
                AddRenderingRow(BaseDbcFormat.CreateTableRow(record, RecordType));
        }

        public void Filter(FilterInfo[] filters)
        {
            foreach (var filter in filters)
            {

            }
        }

        public void Render(ref ListView renderer, ref ToolStripStatusLabel recCount, ref ToolStripProgressBar loadingBar)
        {
            if (Rows.Count == 0)
                PopulateRows();

            renderer.Clear();
            renderer.BeginUpdate();
            renderer.Columns.AddRange(Headers.ToArray());
            renderer.Items.AddRange(Rows.ToArray());

            foreach (ColumnHeader col in renderer.Columns)
                col.Width = BaseDbcFormat.IsFieldString(RecordType, col.Name) ? 120 : -2;

            renderer.EndUpdate();

            loadingBar.Value = 100;
            recCount.Text = String.Format(@"Records: {0}", Rows.Count);
            loadingBar.Visible = false;
        }
    }

    public class BackgroundWorkerSettings
    {
        public string FileName;
        public string FileType;
        public string SelectedBuild;
        public Type RecordType;
        public Type StorageType;

        public BackgroundWorkerSettings(string name, string type, Type store, string build)
        {
            FileName = name;
            FileType = type;
            SelectedBuild = build;
            StorageType = store;
            RecordType = Assembly.GetExecutingAssembly().GetFormatType("FileStructures.{0}.{1}.{2}Entry", FileType, SelectedBuild, FileName.AsReflectionTypeIdentifier());
        }
    }

    public enum FilterOperator
    {
        FilterEqual = 0,
        FilterDiffer = 1,
        FilterBitAnd = 2,
        FilterContains = 3,
    }

    public class FilterInfo
    {
        public string Field;
        public int ArrayOffset;
        public string Value;
        public FilterOperator Operator;

        public FilterInfo(string field, string compare, FilterOperator op)
        {
            Field = field.Substring(0, field.IndexOf('[') - 1);
            Value = compare;
            Operator = op;
            ArrayOffset = -1;

            if (field != Field)
                ArrayOffset = Convert.ToInt32(field.Substring(field.IndexOf('['), 1));
        }
    }
}
