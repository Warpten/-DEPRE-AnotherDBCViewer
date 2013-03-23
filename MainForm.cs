using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using FileStructures.DBC;
using MyDBCViewer.Extensions;
using DBFilesClient.NET;

namespace MyDBCViewer
{
    public partial class MainForm : Form
    {
        public string SelectedBuild;
        public static object CurrentDBCStore;
        public static Type DBCStoreType;
        public static string SelectedFile;

        public MainForm()
        {
            InitializeComponent();
        }

        private void OnApplicationLoad(object sender, EventArgs e)
        {
            // Select the build - Pick the last one
            OnBuildSelection(_dbcVersionSelector.DropDownItems[_dbcVersionSelector.DropDownItems.Count - 1], null);

            // Bind event hook on the build enum
            foreach (var build in _dbcVersionSelector.DropDownItems)
                (build as ToolStripMenuItem).Click += new EventHandler(OnBuildSelection);

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
                            Type fileInfo = Assembly.GetExecutingAssembly().GetFormatType("FileStructures.DBC.{0}.{1}Entry", SelectedBuild, dbcFileName);
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

            // Load DB2 files
            try
            {
                string[] db2Names = Directory.GetFiles("db2/", "*.db2", SearchOption.TopDirectoryOnly);

                foreach (string file in db2Names)
                    loadDB2ToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem(file.Replace(".db2", "").Replace("db2/", "")));

                foreach (ToolStripMenuItem item in loadDB2ToolStripMenuItem.DropDownItems)
                    item.Click += new EventHandler(OnDb2FileSelection);
            }
            catch (Exception /*ex*/)
            {
                MessageBox.Show("You need to put your .db2 files into the /db2/ subdirectory!");
            }
        }

        private void OnBuildSelection(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in _dbcVersionSelector.DropDownItems)
                item.Image = null;
            (sender as ToolStripMenuItem).Image = Properties.Resources.CheckBox;
            SelectedBuild = (sender as ToolStripMenuItem).Name.Substring(1).Replace("Build", "");

            foreach (ToolStripMenuItem alphabetical in loadDBCToolStripMenuItem.DropDownItems)
                foreach (ToolStripMenuItem dbc in alphabetical.DropDownItems)
                    if (Assembly.GetExecutingAssembly().GetFormatType("FileStructures.DBC.{0}.{1}Entry", SelectedBuild, dbc.Text) == null)
                        dbc.Image = null;
                    else
                        dbc.Image = Properties.Resources.CheckBox;
        }

        private void OnDb2FileSelection(object sender, EventArgs e)
        {
            SelectedFile = (sender as ToolStripMenuItem).Text;
            var assembly = Assembly.GetExecutingAssembly();

            try
            {
                Type classType = Assembly.GetExecutingAssembly().GetFormatType("FileStructures.DB2.{0}.{1}Entry", SelectedBuild, SelectedFile);
                ClientFieldInfo[] columnsArray = BaseDbcFormat.GetStructure(classType);
                if (columnsArray.Length == 0)
                    throw new Exception("Missing definition!");

                // Load the DBC file
                Type[] typeArgs = { classType };
                DBCStoreType = typeof(DBCStorage<>).MakeGenericType(typeArgs);
                CurrentDBCStore = Activator.CreateInstance(DBCStoreType);
                using (var strm = new FileStream(String.Format("db2\\{0}.db2", SelectedFile), FileMode.Open))
                    typeof(DBCStorage<>)
                        .MakeGenericType(typeArgs)
                        .GetMethod("Load", new Type[] { typeof(FileStream) })
                        .Invoke(CurrentDBCStore, new object[] { strm });

                // Now that this is populated, build our columns
                LoadTableStructure(columnsArray, "DB2");
            }
            catch (Exception /*ex*/)
            {
                Utils.BoxError("No definition found for {0}.db2 ({1})", SelectedFile, SelectedBuild);
            }
        }

        private void OnDbcFileSelection(object sender, EventArgs e)
        {
            SelectedFile = (sender as ToolStripMenuItem).Text;
            var assembly = Assembly.GetExecutingAssembly();

            try
            {
                Type classType = Assembly.GetExecutingAssembly().GetFormatType("FileStructures.DBC.{0}.{1}Entry", SelectedBuild, SelectedFile);
                ClientFieldInfo[] columnsArray = BaseDbcFormat.GetStructure(classType);
                if (columnsArray.Length == 0)
                    throw new Exception("Missing definition!");

                // Load the DBC file
                Type[] typeArgs = { classType };
                DBCStoreType = typeof(DBCStorage<>).MakeGenericType(typeArgs);
                CurrentDBCStore = Activator.CreateInstance(DBCStoreType);
                using (var strm = new FileStream(String.Format("dbc\\{0}.dbc", SelectedFile), FileMode.Open))
                    typeof(DBCStorage<>)
                        .MakeGenericType(typeArgs)
                        .GetMethod("Load", new Type[] { typeof(FileStream) })
                        .Invoke(CurrentDBCStore, new object[] { strm });

                // Now that this is populated, build our columns
                LoadTableStructure(columnsArray, "DBC");
            }
            catch (Exception /*ex*/)
            {
                Utils.BoxError("No definition found for {0}.dbc ({1})", SelectedFile, SelectedBuild);
            }
        }

        private void LoadTableStructure(ClientFieldInfo[] columnsArray, string fileType)
        {
            _lvRecordList.Columns.Clear();
            _lvRecordList.Items.Clear();

            // Load the header
            foreach (var col in columnsArray)
            {
                int colWidth = (col.Name.Length + 5) * 11 + 10; // -2 is buggy (autowidth)

                if (col.ArraySize != 0)
                    for (int i = 0; i < col.ArraySize; ++i)
                        _lvRecordList.Columns.Add(col.Name + "[ " + i + " ]", colWidth, HorizontalAlignment.Left);
                else
                    _lvRecordList.Columns.Add(col.Name, colWidth, HorizontalAlignment.Left); // Autowidth based off items
            }

            // Get the records
            Type[] storageType = { Assembly.GetExecutingAssembly().GetFormatType("FileStructures.{0}.{1}.{2}Entry", fileType, SelectedBuild, SelectedFile) };
            Type genType = DBCStoreType = typeof(DBCStorage<>).MakeGenericType(storageType);
            dynamic dbcRecords = genType.GetProperty("Records").GetValue(CurrentDBCStore);
            foreach (var record in dbcRecords)
                _lvRecordList.Items.Add(BaseDbcFormat.CreateTableRow(record, storageType[0], record.GetType()));
        }

        private void BackgroundLoadFile(object sender, DoWorkEventArgs e)
        {

        }

        private void BackgroundLoaderProgressInform(object sender, ProgressChangedEventArgs e)
        {

        }

        private void BackgroundLoaderProgressCompleteInform(object sender, RunWorkerCompletedEventArgs e)
        {

        }
    }
}
