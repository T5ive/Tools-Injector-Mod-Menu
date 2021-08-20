using MaterialSkin;
using MaterialSkin.Controls;
using ModernFolderBrowserDialog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Tools_Injector_Mod_Menu.Patch_Manager;
using Application = System.Windows.Forms.Application;

namespace Tools_Injector_Mod_Menu
{
    //Fix Dump Compile Decompile Sign
    //Fix button hook
    //Wire Frame & Color Chams
    //Telekill?
    //String, Decimal
    public partial class FrmMain : MaterialForm
    {
        public FrmMain()
        {
            InitializeComponent();
            LoadTheme();
        }

        #region Variable

        public static readonly string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string ImageCode;

        private static Enums.ProcessType _type = Enums.ProcessType.None;

        private static int _errorCount, _smaliCount;

        private static MySettings _mySettings = new();

        private static readonly string TEMP_PATH_T_FIVE = Path.GetTempPath() + "TFiveMenu";

        private static readonly string APK_DECOMPILED_PATH = $"{TEMP_PATH_T_FIVE}\\Decompile";

        private static readonly string BUILD_TOOL_PATH = $"{AppPath}\\BuildTools\\";

        private static string _launch, _apkTarget, _apkTool, _apkName, _apkType, _baseName, _outputDir;

        private static string[] _menuFiles;

        private static bool _compiled;

        public enum State
        {
            Idle,
            Running
        }

        #endregion Variable

        #region Load

        private void FrmMain_Load(object sender, EventArgs e)
        {
            CheckFolder();
            LoadFiles();
            LoadSettings();
            Text = $"Tools Injector Mod Menu - {Assembly.GetEntryAssembly()?.GetName().Version}";
        }

        private void LoadTheme()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Blue700, TextShade.WHITE);
        }

        private static void CheckFolder()
        {
            Checker.CheckFolder(TEMP_PATH_T_FIVE, false);
            Checker.CheckFolder("Theme");
            Checker.CheckFolder("Menu");
            Checker.CheckFolder("Output");
            Checker.CheckFolder("Save");
            Checker.CheckFolder("Logs");
            Checker.CheckFolder("BuildTools");
        }

        private void LoadFiles()
        {
            var themeFiles = Directory.GetFiles(AppPath + "\\Theme", "*.zip");
            _menuFiles = Directory.GetFiles(AppPath + "\\Menu", "*.zip");
            var apktoolFiles = Directory.GetFiles(BUILD_TOOL_PATH, "Apktool_*.jar");

            if (themeFiles.Length == 0)
            {
                MyMessage.MsgShowError("Not found Theme files .zip!!");
                Application.Exit();
            }

            if (_menuFiles.Length == 0)
            {
                MyMessage.MsgShowError("Not found Menu files .zip!!");
                Application.Exit();
            }

            if (apktoolFiles.Length == 0)
            {
                MyMessage.MsgShowError("Not found Apktool files .jar!!");
                Application.Exit();
            }

            if (!Checker.CheckFiles("Theme", "Default.zip"))
            {
                MyMessage.MsgShowError("Theme file Default.zip is missing!!");
                Application.Exit();
            }

            if (!Checker.CheckFiles("Menu", "Default.zip"))
            {
                MyMessage.MsgShowError("Menu file Default.zip is missing!!");
                Application.Exit();
            }

            var themeFile = themeFiles.Select(Path.GetFileName).ToList();
            var menuFile = _menuFiles.Select(Path.GetFileName).ToList();
            var apktoolFile = apktoolFiles.Select(Path.GetFileName).ToList();

            if (!themeFile.IsEqual(menuFile))
            {
                MyMessage.MsgShowError("Theme files not equal Menu files");
                Application.Exit();
            }

            foreach (var t in menuFile)
            {
                WriteOutput("Loaded: " + t, Enums.LogsType.Success);
                comboMenu.Items.Add(t.Replace(".zip", ""));
            }

            foreach (var t in apktoolFile)
            {
                WriteOutput("Loaded: " + t, Enums.LogsType.Success);
                comboApktool.Items.Add(t.Replace(".jar", ""));
            }
        }

        private void LoadSettings()
        {
            _mySettings = MySettings.Load();
            txtLibName.Text = _mySettings.txtLibName;
            if (!_mySettings.txtToast.IsEmpty() && _mySettings.txtToast.Contains('|'))
            {
                foreach (var t in _mySettings.txtToast.Split('|'))
                {
                    listToast.Items.Add(t);
                }
            }

            if (!_mySettings.txtToast.IsEmpty() && !_mySettings.txtToast.Contains('|'))
            {
                listToast.Items.Add(_mySettings.txtToast);
            }

            txtName.Text = _mySettings.txtName;
            txtSite.Text = _mySettings.txtSite;
            txtText.Text = _mySettings.txtText;
            txtEndCredit.Text = _mySettings.txtEndCredit;
            ImageCode = _mySettings.ImageCode;
            chkNoMenu.Checked = _mySettings.chkNoMenu;

            chkRemoveTemp.Checked = _mySettings.chkRemoveTemp;
            chkTFiveCredit.Checked = _mySettings.chkTFiveCredit;
            chkSound.Checked = _mySettings.chkSound;
            chkCheckUpdate.Checked = _mySettings.chkCheckUpdate;
            chkAlwaysOverwrite.Checked = _mySettings.chkAlwaysOverwrite;
            chkMergeApk.Checked = _mySettings.chkMergeApk;

            txtNDK.Text = _mySettings.txtNDK;

            txtService.Text = _mySettings.txtService;
            txtOnCreate.Text = _mySettings.txtOnCreate;
            txtActionMain.Text = _mySettings.txtActionMain;

            LoadImg();

            try
            {
                comboMenu.SelectedIndex = _mySettings.menuStyle;
            }
            catch
            {
                comboMenu.SelectedIndex = 0;
            }

            try
            {
                comboApktool.SelectedIndex = _mySettings.apkTools;
            }
            catch
            {
                comboApktool.SelectedIndex = 0;
            }

            _ = CheckUpdateAsync();
        }

        private async Task CheckUpdateAsync()
        {
            try
            {
                if (chkCheckUpdate.Checked)
                {
                    await UpdateService.CheckGitHubNewerVersion(true).ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                WriteOutput(exception.Message, Enums.LogsType.Error, "001");
            }
        }

        private void LoadImg()
        {
            try
            {
                if (!ImageCode.IsEmpty())
                {
                    picImg.Image = ImageCode.Base64ToImage();
                }
            }
            catch (Exception exception)
            {
                picImg.Image = null;
                ImageCode = "";
                WriteOutput(exception.Message, Enums.LogsType.Error, "002");
            }
        }

        #endregion Load

        #region Main Page

        private void btnAddToast_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtToast.Text.IsEmpty("Toast")) return;
                listToast.Items.Add(txtToast.Text);
                WriteOutput("Add Toast " + txtToast.Text, Enums.LogsType.Success);
            }
            catch (Exception exception)
            {
                WriteOutput(exception.Message, Enums.LogsType.Error, "003");
            }
        }

        private void btnRemoveToast_Click(object sender, EventArgs e)
        {
            try
            {
                if (listToast.SelectedIndex >= 0)
                {
                    WriteOutput("Remove Toast " + listToast.GetItemText(listToast.SelectedItem), Enums.LogsType.Success);
                    listToast.Items.RemoveAt(listToast.SelectedIndex);
                }
            }
            catch (Exception exception)
            {
                WriteOutput(exception.Message, Enums.LogsType.Error, "004");
            }
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            var openFile = new OpenFileDialog()
            {
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.png) | *.jpg; *.jpeg; *.jpe; *.png;",
                Title = Text,
                DefaultExt = ".jpg"
            };
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var imgFormat = Path.GetExtension(openFile.FileName) == ".png" ? ImageFormat.Png : ImageFormat.Jpeg;
                    ImageCode = openFile.FileName.CompressImage(1).ImageToBase64(imgFormat);
                    LoadImg();
                }
                catch (Exception exception)
                {
                    WriteOutput(exception.Message, Enums.LogsType.Error, "005");
                }
            }
        }

        private void btnImageCode_Click(object sender, EventArgs e)
        {
            var frmImage = new FrmImageText();
            Hide();
            frmImage.ShowDialog();
            frmImage.Dispose();
            Show();
            LoadImg();
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyMessage.MsgOkCancel("Save Settings.\n\n" +
                                          "Click \"OK\" to confirm.\n\n" +
                                          "Click \"Cancel\" to cancel."))
                {
                    _mySettings.txtLibName = txtLibName.Text;
                    var toast = listToast.Items.Cast<object>().Aggregate("", (current, t) => current + t + "|");
                    if (listToast.Items.Count > 0)
                        toast = toast.Substring(0, toast.Length - 1);
                    _mySettings.txtToast = toast;
                    _mySettings.txtName = txtName.Text;
                    _mySettings.txtSite = txtSite.Text;
                    _mySettings.txtText = txtText.Text;
                    _mySettings.txtEndCredit = txtEndCredit.Text;
                    _mySettings.ImageCode = ImageCode;
                    _mySettings.chkNoMenu = chkNoMenu.Checked;
                    _mySettings.menuStyle = comboMenu.SelectedIndex;
                    _mySettings.Save();
                    WriteOutput("Saved Settings", Enums.LogsType.Success);
                }
            }
            catch (Exception exception)
            {
                WriteOutput(exception.Message, Enums.LogsType.Error, "006");
            }
        }

        #endregion Main Page

        #region Menu Page

        #region Add Function & Data List

        private void AddFunction()
        {
            Hide();
            var frmAdd = new FrmAddFunction();
            frmAdd.ShowDialog();
            frmAdd.Dispose();
            Show();
            if (Values.Save)
            {
                AddDataList();
                Values.Save = false;
            }
        }

        private void btnAddFunction_Click(object sender, EventArgs e)
        {
            AddFunction();
        }

        private void AddDataList()
        {
            var cheatName = OffsetPatch.ConvertNameList();
            var functionType = OffsetPatch.FunctionList[OffsetPatch.FunctionList.Count - 1].FunctionType;
            AddListValues(cheatName[cheatName.Count - 1], functionType.FunctionTypeToString());
            OffsetPatch.OffsetList.Clear();
        }

        private void AddAllDataList(bool reValues = false, int selected = 0)
        {
            dataList.DataSource = null;
            dataList.Rows.Clear();
            if (reValues)
            {
                WriteOutput("Clear Function List", Enums.LogsType.Success);
            }
            for (var i = 0; i < OffsetPatch.FunctionList.Count; i++)
            {
                var cheatName = OffsetPatch.ConvertNameList();
                var functionType = OffsetPatch.FunctionList[i].FunctionType;
                AddListValues(cheatName[i], functionType.FunctionTypeToString());
                OffsetPatch.OffsetList.Clear();
            }

            try
            {
                dataList.Rows[0].Selected = false;
                dataList.Rows[selected].Selected = true;
                dataList.FirstDisplayedScrollingRowIndex = dataList.SelectedRows[0].Index;
            }
            catch
            {
                //
            }
        }

        private void AddListValues(string cheatName, string functionType)
        {
            dataList.Rows.Add(cheatName, functionType);
            WriteOutput("Added Function: " + cheatName, Enums.LogsType.Success);
        }

        private void dataList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataList.Rows.GetRowCount(DataGridViewElementStates.Selected) > 0)
            {
                var index = dataList.SelectedRows[0].Index;
                var frmFunction = new FrmAddFunction(dataList.SelectedRows[0].Cells[1].Value.ToString().StringToFunctionType(), index);
                Hide();
                frmFunction.ShowDialog();
                frmFunction.Dispose();
                Show();
                if (Values.Save)
                {
                    AddAllDataList(true, index);
                }
            }
        }

        private void dataList_SelectionChanged(object sender, EventArgs e)
        {
            var selectedRowCount = dataList.Rows.GetRowCount(DataGridViewElementStates.Selected);
            btnRemove.HighEmphasis = selectedRowCount != 0;
            btnRemove.Enabled = selectedRowCount != 0;
        }

        private void dataList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveRows();
            }
        }

        private void RemoveRows()
        {
            try
            {
                var selectedRowCount = dataList.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount != 0)
                {
                    var listRemove = new List<int>();
                    for (var i = 0; i < selectedRowCount; i++)
                    {
                        listRemove.Add(dataList.SelectedRows[i].Index);
                    }

                    foreach (var row in listRemove)
                    {
                        WriteOutput("Remove Function: " + dataList.Rows[row].Cells[0].Value, Enums.LogsType.Success);
                        OffsetPatch.FunctionList.RemoveAt(row);
                        dataList.Rows.RemoveAt(row);
                    }

                    dataList.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message, Enums.LogsType.Error, "007");
            }
        }

        private void dataList_MouseDown(object sender, MouseEventArgs e)
        {
            int selectedRows = 0;
            int itemCount = 1;
            if (e.Button == MouseButtons.Right)
            {
                try
                {
                    var hti = dataList.HitTest(e.X, e.Y);
                    dataList.ClearSelection();
                    dataList.Rows[hti.RowIndex].Selected = true;
                    selectedRows = dataList.SelectedRows[0].Index;
                    itemCount = dataList.RowCount - 1;
                }
                catch
                {
                    //
                }
            }

            if (dataList.SelectedRows.Count > 0)
            {
                addToolStripMenuItem.Enabled = false;
                removeToolStripMenuItem.Enabled = true;
                moveUpToolStripMenuItem.Enabled = selectedRows != 0;
                moveDownToolStripMenuItem.Enabled = selectedRows != itemCount;
            }
            else
            {
                addToolStripMenuItem.Enabled = true;
                removeToolStripMenuItem.Enabled = false;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRowCount = dataList.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount != 0)
                {
                    if (MyMessage.MsgOkCancel("You sure you want remove this function in the List?"))
                    {
                        RemoveRows();
                    }
                }
                else
                {
                    MyMessage.MsgShowWarning("There is no Items Functions in the List!!");
                    WriteOutput("[Waring] There is no Items Functions to Remove", Enums.LogsType.Warning);
                }
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message, Enums.LogsType.Error, "008");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyMessage.MsgOkCancel("You sure you want remove this function in the List?"))
                {
                    dataList.DataSource = null;
                    dataList.Rows.Clear();
                    OffsetPatch.FunctionList.Clear();
                    WriteOutput("Clear Function", Enums.LogsType.Success);
                }
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message, Enums.LogsType.Error, "009");
            }
        }

        private void MoveUpDown(int type)
        {
            try
            {
                var selectedRowCount = dataList.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount != 0)
                {
                    var selectedRows = dataList.SelectedRows[0].Index;
                    var newIndex = type == 0 ? selectedRows - 1 : selectedRows + 1;
                    var item = OffsetPatch.FunctionList[selectedRows];
                    OffsetPatch.FunctionList.RemoveAt(selectedRows);
                    OffsetPatch.FunctionList.Insert(newIndex, item);
                    AddAllDataList(true, newIndex);
                }
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message, Enums.LogsType.Error, "042");
            }
        }

        #endregion Add Function & Data List

        #region ToolStripMenuItem

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveRows();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFunction();
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveUpDown(0);
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveUpDown(1);
        }

        #endregion ToolStripMenuItem

        #region Load/Save Group

        private void btnSaveCheat_Click(object sender, EventArgs e)
        {
            if (OffsetPatch.FunctionList.Count <= 0)
            {
                MyMessage.MsgShowWarning(@"Function list is Empty, Please Check it again!!!");
                WriteOutput("[Waring] Function list is Empty", Enums.LogsType.Warning);
                return;
            }

            var saveFile = new SaveFileDialog
            {
                Filter = @"XML|*.xml|All files|*.*",
                Title = Text,
                DefaultExt = ".xml",
                FileName = txtNameGame.Text
            };
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                FormState(State.Running);

                OffsetPatch.T5Menu.FunctionList = OffsetPatch.FunctionList;
                OffsetPatch.T5Menu.GameName = txtNameGame.Text;
                OffsetPatch.T5Menu.Target = txtTargetLib.Text;
                OffsetPatch.T5Menu.TypeAbi = (Enums.TypeAbi)comboType.SelectedIndex;

                SaveXml(OffsetPatch.T5Menu, saveFile.FileName);
                FormState(State.Idle);
            }
        }

        private void btnLoadCheat_Click(object sender, EventArgs e)
        {
            var openFile = new OpenFileDialog()
            {
                Filter = "XML|*.xml|All files|*.*",
                Title = Text,
                DefaultExt = ".xml"
            };
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                LoadXml(openFile.FileName);
            }
        }

        private void LoadXml(string fileName)
        {
            try
            {
                FormState(State.Running);
                OffsetPatch.T5Menu = LoadXml<TFiveMenu>(fileName);

                OffsetPatch.FunctionList = OffsetPatch.T5Menu.FunctionList;
                txtNameGame.Text = OffsetPatch.T5Menu.GameName;
                txtTargetLib.Text = OffsetPatch.T5Menu.Target;
                comboType.SelectedIndex = (int)OffsetPatch.T5Menu.TypeAbi;

                FormState(State.Idle);
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message, Enums.LogsType.Error, "010");
            }
            finally
            {
                AddAllDataList();
                txtNameGame.Text = Path.GetFileNameWithoutExtension(fileName);
            }
        }

        //https://stackoverflow.com/a/6115782/8902883

        private void SaveXml<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                var xmlDocument = new XmlDocument();
                var serializer = new XmlSerializer(serializableObject.GetType());
                using (var stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                }
                WriteOutput("Saved: " + Path.GetFileName(fileName), Enums.LogsType.Success);
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message, Enums.LogsType.Error, "011");
            }
        }

        private T LoadXml<T>(string fileName)
        {
            if (fileName.IsEmpty()) { return default; }

            var objectOut = default(T);

            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                var xmlString = xmlDocument.OuterXml;

                using (var read = new StringReader(xmlString))
                {
                    var outType = typeof(T);

                    var serializer = new XmlSerializer(outType);
                    using XmlReader reader = new XmlTextReader(read);
                    objectOut = (T)serializer.Deserialize(reader);
                }
                WriteOutput("Loaded: " + Path.GetFileName(fileName), Enums.LogsType.Success);
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message, Enums.LogsType.Error, "012");
            }

            return objectOut;
        }

        #endregion Load/Save Group

        private void txtNameGame_Leave(object sender, EventArgs e)
        {
            txtNameGame.Text = txtNameGame.Text.RemoveSpecialCharacters();
        }

        #endregion Menu Page

        #region Compile Page

        #region Events

        private void btnOutput_Click(object sender, EventArgs e)
        {
            Process.Start(AppPath + "\\Output");
        }

        private void btnTempDir_Click(object sender, EventArgs e)
        {
            Process.Start(TEMP_PATH_T_FIVE);
        }

        private void btnCompileMenu_Click(object sender, EventArgs e)
        {
            ProcessType(Enums.ProcessType.MenuFull);
            SetCompileMenu();
        }

        private void btnCompileApk1_Click(object sender, EventArgs e)
        {
            SetFullApk(Enums.ProcessType.ApkFull1Decompile);
        }

        private void btnCompileApk2_Click(object sender, EventArgs e)
        {
            SetFullApk(Enums.ProcessType.ApkFull2Decompile);
        }

        private void SetFullApk(Enums.ProcessType type)
        {
            if (!CheckEmpty()) return;

            if (_apkName.IsEmpty())
            {
                MyMessage.MsgShowWarning("Apk Target is Empty, Please Check it again!!!");
                WriteOutput("Apk Target is Empty", Enums.LogsType.Warning);
                return;
            }

            if (_apkTarget != _apkName)
            {
                SetApkPath(txtApkTarget.Text, true);
            }

            UpdatePath();
            if (!CheckOutputGame(_outputDir)) return;
            SetDecompileApk(type);
        }

        #endregion Events

        #region Get Apk

        private void btnBrowseApk_Click(object sender, EventArgs e)
        {
            var openFile = new OpenFileDialog()
            {
                Filter = "APK File(*.apk;*apks;*xapk)|*.apk;*apks;*xapk|All files(*.*)|*.*",
                Title = Text
            };

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SetApkPath(openFile.FileName);
                }
                catch (Exception ex)
                {
                    WriteOutput(ex.Message, Enums.LogsType.Error, "013");
                }
            }
        }

        private void SetApkPath(string apkTarget, bool re = false)
        {
            try
            {
                _apkTarget = apkTarget;
                _apkName = apkTarget;
                _apkType = Path.GetExtension(_apkName);
                txtApkTarget.Text = _apkTarget;
                if (!re)
                {
                    WriteOutput($"Set Apk Target: {_apkTarget}", Enums.LogsType.Success);
                }
                SetDumpApk();
            }
            catch (Exception exception)
            {
                WriteOutput(exception.Message, Enums.LogsType.Error, "014");
            }
        }

        #endregion Get Apk

        #region Dump Apk

        private void SetDumpApk()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_apkName))
                {
                    MyMessage.MsgShowWarning("Apk Target is Empty, Please Check it again!!!");
                    WriteOutput("Apk Target is Empty", Enums.LogsType.Warning);
                    return;
                }

                File.Copy(_apkName, $"{TEMP_PATH_T_FIVE}\\ApkTarget{_apkType}", true);

                _apkTarget = $"{TEMP_PATH_T_FIVE}\\ApkTarget{_apkType}";

                FormState(State.Running);
                SplitApk();
                if (_apkType is ".apk")
                {
                    DumpApk();
                }
                else
                {
                    while (_apkTarget == $"{TEMP_PATH_T_FIVE}\\ApkTarget{_apkType}")
                    {
                    }

                    DumpApk();
                }
            }
            catch (Exception exception)
            {
                WriteOutput(exception.Message, Enums.LogsType.Error, "015");
            }
        }

        private async void SplitApk()
        {
            try
            {
                var type = 0;
                Invoke(new MethodInvoker(delegate
                {
                    type = comboType.SelectedIndex;
                }));

                if (_apkType is ".apks")
                {
                    await APKsDump(type).ConfigureAwait(false);
                }
                if (_apkType is ".xapk")
                {
                    await XAPKDump(type).ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                WriteOutput(exception.Message, Enums.LogsType.Error, "018");
            }
        }

        private static async Task APKsDump(int type)
        {
            await Task.Factory.StartNew(() =>
            {
                using var archive = ZipFile.OpenRead(_apkTarget);
                foreach (var entryApks in archive.Entries)
                {
                    if (entryApks.FullName == "base.apk")
                    {
                        entryApks.ExtractToFile($"{TEMP_PATH_T_FIVE}\\ApkTarget.apk", true);
                        _apkTarget = $"{TEMP_PATH_T_FIVE}\\ApkTarget.apk";
                    }

                    if (type == (int)Enums.TypeAbi.Arm)
                    {
                        if (entryApks.FullName == "split_config.armeabi_v7a.apk")
                        {
                            entryApks.ExtractToFile(Path.Combine(TEMP_PATH_T_FIVE, entryApks.FullName), true);
                        }
                    }
                    else
                    {
                        if (entryApks.FullName == "split_config.arm64_v8a.apk")
                        {
                            entryApks.ExtractToFile(Path.Combine(TEMP_PATH_T_FIVE, entryApks.FullName), true);
                        }
                    }
                }
            }).ConfigureAwait(false);
        }

        private static async Task XAPKDump(int type)
        {
            await Task.Factory.StartNew(() =>
            {
                using var archive = ZipFile.OpenRead(_apkTarget);
                foreach (var entryApks in archive.Entries)
                {
                    if (type == (int)Enums.TypeAbi.Arm)
                    {
                        if (entryApks.FullName == "config.armeabi_v7a.apk")
                        {
                            entryApks.ExtractToFile(Path.Combine(TEMP_PATH_T_FIVE, entryApks.FullName), true);
                        }
                    }
                    else
                    {
                        if (entryApks.FullName == "config.arm64_v8a.apk")
                        {
                            entryApks.ExtractToFile(Path.Combine(TEMP_PATH_T_FIVE, entryApks.FullName), true);
                        }
                    }

                    var apkFile = Path.Combine(Path.GetTempPath(), entryApks.FullName);
                    using var entryBase = ZipFile.OpenRead(apkFile);
                    var classes = entryBase.Entries.FirstOrDefault(f => f.Name.Contains("classes.dex"));
                    if (classes != null)
                    {
                        entryApks.ExtractToFile($"{TEMP_PATH_T_FIVE}\\ApkTarget.apk", true);
                        _apkTarget = $"{TEMP_PATH_T_FIVE}\\ApkTarget.apk";
                        _baseName = entryApks.FullName;
                    }
                }
                archive.Dispose();
            }).ConfigureAwait(false);
        }

        private void DumpApk()
        {
            if (!File.Exists($"{_apkTarget}"))
            {
                MyMessage.MsgShowError($"{_apkTarget} Not found!!" +
                                       "\nPlease select the Apk again");
                WriteOutput($"{TEMP_PATH_T_FIVE}\\ApkTarget.apk Not found", Enums.LogsType.Error, "016");
                return;
            }

            try
            {
                DumpWorker.RunWorkerAsync();
            }
            catch (Exception exception)
            {
                WriteOutput(exception.Message, Enums.LogsType.Error, "017");
            }
        }

        private void DumpWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            ProcessRun($"/c aapt dump badging \"{_apkTarget}\" PAUSE > \"{TEMP_PATH_T_FIVE}\\result.txt\"",
                BUILD_TOOL_PATH, "202");
        }

        private void DumpWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            DumpApkDone();
        }

        private void DumpApkDone()
        {
            var activity = File.ReadAllText($"{TEMP_PATH_T_FIVE}\\result.txt");

            _launch = activity.GetBetween("launchable-activity: name='", "'");
            var appName = activity.GetBetween("application-label:'", "'");
            var appVersion = activity.GetBetween("versionName='", "'");

            lbApk.Text = $"App Name: {appName}\n\n" +
                         $"Version: {appVersion}\n\n" +
                         $"Launch: {_launch}";

            WriteOutput($"Dump successful", Enums.LogsType.Success);

            if (_errorCount > 0 && !_mySettings.debugMode)
            {
                MyMessage.MsgShowError("Failed to Dump");
                WriteOutput("Failed to Dump", Enums.LogsType.Error, "031");
                SaveLogs();
            }
            FormState(State.Idle);
        }

        #endregion Dump Apk

        #region Decompile Apk

        private void SetDecompileApk(Enums.ProcessType type)
        {
            if (Directory.Exists(APK_DECOMPILED_PATH))
            {
                if (_mySettings.chkAlwaysOverwrite)
                {
                    Directory.Delete(APK_DECOMPILED_PATH, true);
                }
                else if (MyMessage.MsgOkCancel(APK_DECOMPILED_PATH + " Found.\n\n" +
                                               "Click \"OK\" to Continue if you want to overwrite!" +
                                               "\n\nClick \"Cancel\" to cancel it if not!"))
                {
                    Directory.Delete(APK_DECOMPILED_PATH, true);
                }
                else
                {
                    return;
                }
            }

            try
            {
                ProcessType(type);
                FormState(State.Running);
                materialTabControl1.SelectedTab = materialTabControl1.TabPages[3];
                DecompileWorker.RunWorkerAsync();
            }
            catch (Exception exception)
            {
                WriteOutput(exception.Message, Enums.LogsType.Error, "020");
            }
        }

        private void DecompileWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            ProcessRun($"/c java -jar {_apkTool}.jar d -f --only-main-classes -o \"{APK_DECOMPILED_PATH}\" \"{_apkTarget}\"", BUILD_TOOL_PATH, "203");
        }

        private void DecompileWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            DecompileApkDone();
        }

        private void DecompileApkDone()
        {
            GetSmailiCount();
            WriteOutput("Decompile successful", Enums.LogsType.Success);

            if (_type is Enums.ProcessType.ApkFull1Decompile)
            {
                ProcessType(Enums.ProcessType.ApkFull1);
            }
            if (_type is Enums.ProcessType.ApkFull2Decompile)
            {
                ProcessType(Enums.ProcessType.ApkFull2);
            }

            if (_mySettings.chkMergeApk && _apkType != ".apk")
            {
                if (!DeleteAll(TEMP_PATH_T_FIVE + "\\lib")) return;
                string fileName;
                if (comboType.SelectedIndex == (int)Enums.TypeAbi.Arm)
                {
                    fileName = _apkType == ".apks" ? "\\split_config.armeabi_v7a.apk" : "\\config.armeabi_v7a.apk";
                }
                else
                {
                    fileName = _apkType == ".apks" ? "\\split_config.arm64_v8a.apk" : "\\config.arm64_v8a.apk";
                }

                using var archive = ZipFile.OpenRead(TEMP_PATH_T_FIVE + fileName);
                foreach (var entry in archive.Entries.Where(cur => Path.GetDirectoryName(cur.FullName).StartsWith("lib")))
                {
                    var path = Path.Combine(TEMP_PATH_T_FIVE, "lib");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    entry.ExtractToFile(Path.Combine(path, entry.Name));
                }
            }

            DeleteDecompiledLib();
            SetCompileMenu();
        }

        private void GetSmailiCount()
        {
            _smaliCount = 0;
            var directory = new DirectoryInfo($"{APK_DECOMPILED_PATH}");
            foreach (var dir in directory.GetDirectories())
            {
                if (dir.Name.StartsWith("smali") && !dir.Name.StartsWith("smali_assets"))
                {
                    _smaliCount++;
                }
            }
        }

        private void DeleteDecompiledLib()
        {
            var sourcePath = $"{APK_DECOMPILED_PATH}\\lib\\";
            var folderName = comboType.SelectedIndex == (int)Enums.TypeAbi.Arm ? "arm64-v8a" : "armeabi-v7a";
            if (comboType.SelectedIndex == (int)Enums.TypeAbi.Arm)
            {
                try
                {
                    if (Directory.Exists(sourcePath + folderName))
                    {
                        Directory.Delete(sourcePath + folderName, true);
                    }
                }
                catch (Exception exception)
                {
                    WriteOutput($"Can not Delete {sourcePath + folderName} " + exception.Message, Enums.LogsType.Error, "032");
                }
            }
        }

        #endregion Decompile Apk

        #region Compile Menu

        private void SetCompileMenu()
        {
            if (!CheckEmpty()) return;

            UpdatePath();
            if (!CheckOutputGame(_outputDir)) return;

            FormState(State.Running);
            materialTabControl1.SelectedTab = materialTabControl1.TabPages[3];
            if (!DeleteAll(TEMP_PATH_T_FIVE + "\\jni")) return;
            if (!DeleteAll(TEMP_PATH_T_FIVE + "\\libs")) return;
            if (!DeleteAll(TEMP_PATH_T_FIVE + "\\obj")) return;

            if (!ExtractZip(AppPath + $"\\Menu\\{comboMenu.SelectedItem}.zip", TEMP_PATH_T_FIVE)) return;
            if (!ExtractZip(AppPath + $"\\Theme\\{comboMenu.SelectedItem}.zip", TEMP_PATH_T_FIVE)) return;

            if (!Replacer())
            {
                MyMessage.MsgShowError("Failed to Replace Something");
                WriteOutput("Failed to Replace Something", Enums.LogsType.Error, "019");
                return;
            }
            //FormState(State.Idle); // Test Mode
            //return;
            if (!MoveSmali(_outputDir)) return;
            MenuWorker.RunWorkerAsync();
        }

        private bool MoveSmali(string destinationPath)
        {
            if (!MoveDirectory(TEMP_PATH_T_FIVE + "\\com", $"{destinationPath}\\smali\\com"))
            {
                FormState(State.Idle);
                return false;
            }
            return true;
        }

        private void MenuWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            ProcessRun($"/c {_mySettings.txtNDK}\\build\\ndk-build", $"{TEMP_PATH_T_FIVE}\\jni", "201");
        }

        private void MenuWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            CompileMenuDone();

            if (_type is Enums.ProcessType.MenuFull)
            {
                FormState(State.Idle);
                ProcessType(Enums.ProcessType.None);
            }

            if (_type is Enums.ProcessType.ApkFull1 or Enums.ProcessType.ApkFull2)
            {
                SetCompileApk();
            }
        }

        private void CompileMenuDone()
        {
            if (_errorCount > 0 && !_mySettings.debugMode)
            {
                MyMessage.MsgShowError("Failed to Compile");
                WriteOutput("Failed to Compile", Enums.LogsType.Error, "030");
                SaveLogs();
                ProcessType(Enums.ProcessType.None);
                FormState(State.Idle);
                return;
            }

            var tempOutputDir = $"{TEMP_PATH_T_FIVE}\\libs";
            var desDir = _outputDir + "\\lib";
            var deleteTemp = chkRemoveTemp.Checked;

            MoveDirectory(tempOutputDir, desDir, true, deleteTemp);
            if (_mySettings.chkMergeApk && _apkType is ".apks" or ".xapk")
            {
                var folderName = comboType.SelectedIndex == (int)Enums.TypeAbi.Arm ? "\\armeabi-v7a" : "\\arm64-v8a";
                MoveDirectory($"{TEMP_PATH_T_FIVE}\\lib\\", desDir + folderName, false, false);
            }
        }

        #endregion Compile Menu

        #region Modify Files

        private bool Replacer()
        {
            if (!MainActivity()) return false;
            if (!AndroidMk()) return false;
            if (!ApplicationMk()) return false;
            if (!MenuString()) return false;
            if (_type is not Enums.ProcessType.MenuFull && !ApkMainActivity()) return false;
            if (_type is Enums.ProcessType.ApkFull1 && !OnCreate()) return false;
            return MainHack();
        }

        #region Menu

        private bool MainActivity()
        {
            try
            {
                var text = File.ReadAllText(TEMP_PATH_T_FIVE + "\\com\\tfive\\MainActivity.smali");
                text = text.Replace("MyLibName", txtLibName.Text);
                if (_type is Enums.ProcessType.ApkFull2)
                {
                    text = text.Replace("com.unity3d.player.UnityPlayerActivity", _launch);
                }
                File.WriteAllText(TEMP_PATH_T_FIVE + "\\com\\tfive\\MainActivity.smali", text);
                WriteOutput("Replaced MainActivity.smali", Enums.LogsType.Success);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "023");
                return false;
            }
        }

        private bool AndroidMk()
        {
            try
            {
                var text = File.ReadAllText(TEMP_PATH_T_FIVE + "\\jni\\Android.mk");
                text = text.Replace("MyLibName", txtLibName.Text);
                File.WriteAllText(TEMP_PATH_T_FIVE + "\\jni\\Android.mk", text);
                WriteOutput("Replaced Android.mk", Enums.LogsType.Success);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "024");
                return false;
            }
        }

        private bool ApplicationMk()
        {
            try
            {
                var text = File.ReadAllText(TEMP_PATH_T_FIVE + "\\jni\\Application.mk");

                var type = "";
                switch (comboType.SelectedIndex)
                {
                    case (int)Enums.TypeAbi.Arm:
                        type = "armeabi-v7a";
                        break;

                    case (int)Enums.TypeAbi.Arm64:
                        type = "arm64-v8a";
                        break;
                }

                text = text.Replace("(ChangeABIHere)", type);
                File.WriteAllText(TEMP_PATH_T_FIVE + "\\jni\\Application.mk", text);
                WriteOutput("Replaced Application.mk", Enums.LogsType.Success);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "025");
                return false;
            }
        }

        private bool MenuString()
        {
            try
            {
                var text = File.ReadAllText(TEMP_PATH_T_FIVE + "\\jni\\Menu.h");
                text = text.Replace("(yourName)", txtLibName.Text)
                    .Replace("(yourSite)", txtSite.Text)
                    .Replace("(yourText)", txtText.Text);
                text = chkNoMenu.Checked ?
                    text.Replace(@"return env->NewStringUTF(OBFUSCATE(""(yourImage)""));", "return NULL;")
                    : text.Replace("(yourImage)", ImageCode);
                text = chkTFiveCredit.Checked ? text.Replace("//(TFiveEndCredit)", @"OBFUSCATE(""0_RichWebView_<html><body><marquee style=\""color: white; font-weight:bold;\"" direction=\""left\"" scrollamount=\""5\"" behavior=\""scroll\"">TFive Tools</marquee></body></html>"")") : text;
                File.WriteAllText(TEMP_PATH_T_FIVE + "\\jni\\Menu.h", text);
                WriteOutput("Replaced Menu.h (Credit)", Enums.LogsType.Success);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "026");
                return false;
            }
        }

        #endregion Menu

        #region Apk

        private bool ApkMainActivity()
        {
            try
            {
                var text = File.ReadAllText($"{APK_DECOMPILED_PATH}\\AndroidManifest.xml");

                text = text.Contains("<uses-permission") ?
                    text.ReplaceFirst("<uses-permission", $"{txtPermission.Text}\n    <uses-permission") :
                    text.ReplaceFirst("<uses-feature", $"{txtPermission.Text}\n    <uses-feature");

                if (_type is Enums.ProcessType.ApkFull2)
                {
                    text = text.Replace(txtFind.Text, "")
                        .Replace("<action android:name=\"android.intent.action.MAIN\" />", "")

                        .Replace("</application>", $"    {_mySettings.txtActionMain}\n    </application>");
                }
                text = text.Replace("</application>", $"    {_mySettings.txtService}\n    </application>");
                File.WriteAllText($"{APK_DECOMPILED_PATH}\\AndroidManifest.xml", text);
                WriteOutput("Replaced AndroidManifest.xml", Enums.LogsType.Success);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "027");
                return false;
            }
        }

        private bool OnCreate()
        {
            try
            {
                var launch = $"{APK_DECOMPILED_PATH}\\{Utility.SmaliCountToName(_smaliCount)}\\" + _launch.Replace(".", "\\") + ".smali";

                var text = File.ReadAllText(launch);
                var changed = false;
                for (var i = 0; i < 9; i++)
                {
                    if (text.Contains($@".method protected onCreate(Landroid/os/Bundle;)V
    .locals {i}"))
                    {
                        text = text.Replace($@".method protected onCreate(Landroid/os/Bundle;)V
    .locals {i}",
                            ".method protected onCreate(Landroid/os/Bundle;)V" +
                            $"\n    .locals {i}" +
                            $"\n\n    {_mySettings.txtOnCreate}");
                        changed = true;
                        break;
                    }
                }

                if (!changed)
                {
                    MyMessage.MsgShowError("Error Not Found onCreate Pattern");
                    WriteOutput("Error Not Found onCreate Pattern", Enums.LogsType.Error, "043");
                    return false;
                }

                File.WriteAllText(launch, text);
                WriteOutput("Replaced OnCreate", Enums.LogsType.Success);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "028");
                return false;
            }
        }

        #endregion Apk

        private bool MainHack()
        {
            try
            {
                var text = File.ReadAllText(TEMP_PATH_T_FIVE + "\\jni\\Main.cpp");
                var memoryPatch = ModMenuPattern.MemoryPatch();
                var newVariable = ModMenuPattern.NewVariable();
                var newMethod = ModMenuPattern.NewMethod();
                var hackThread64 = "";
                var hackThread = "";
                var toastHere = ModMenuPattern.ToastHere(listToast);
                var featuresList = ModMenuPattern.FeaturesList();
                var newFeatures = ModMenuPattern.NewFeatures();

                if (comboType.SelectedIndex == (int)Enums.TypeAbi.Arm)
                {
                    hackThread = ModMenuPattern.HackThread();
                }
                else
                {
                    hackThread64 = ModMenuPattern.HackThread();
                }

                if (!string.IsNullOrWhiteSpace(toastHere))
                {
                    toastHere = toastHere.Remove(toastHere.LastIndexOf(Environment.NewLine, StringComparison.Ordinal));
                }

                text = text.Replace("//VariableHere", memoryPatch)
                    .Replace("//NewVariableHere", newVariable)
                    .Replace("//NewMethodHere", newMethod)
                    .Replace("(yourTargetLibName)", txtTargetLib.Text)
                    .Replace("//(hackThread64)", hackThread64)
                    .Replace("//(hackThread)", hackThread)
                    .Replace("//ToastHere", toastHere)
                    .Replace("//(yourFeaturesList)", featuresList)
                    .Replace("(yourEndCredit)", txtEndCredit.Text)
                    .Replace("//(yourFeatures)", newFeatures);
                File.WriteAllText(TEMP_PATH_T_FIVE + "\\jni\\Main.cpp", text);
                WriteOutput("Replaced Main.cpp", Enums.LogsType.Success);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "029");
                return false;
            }
        }

        #endregion Modify Files

        #region Compile Apk

        private void SetCompileApk()
        {
            var path = $"{_outputDir}\\{txtNameGame.Text}";
            string[] outputFile = { path + _apkType, path + "-Signed" + _apkType };

            foreach (var t in outputFile)
            {
                if (File.Exists(t))
                {
                    File.Delete(t);
                }
            }

            if (_type is Enums.ProcessType.ApkFull1 or Enums.ProcessType.ApkFull2)
            {
                var smaliSource = $"{_outputDir}\\smali\\com";
                var smaliDes = $"{APK_DECOMPILED_PATH}\\{Utility.SmaliCountToName(_smaliCount, true)}\\com";
                if (!MoveDirectory(smaliSource, smaliDes, false))
                {
                    WriteOutput($"Cannot Move {smaliSource}\nTo => {smaliDes}", Enums.LogsType.Error, "021");
                    return;
                }

                if (_apkType == ".apk" || _mySettings.chkMergeApk)
                {
                    var folderName = comboType.SelectedIndex == (int)Enums.TypeAbi.Arm ? "armeabi-v7a" : "arm64-v8a";
                    var libSource = $"{_outputDir}\\lib\\{folderName}";
                    var libDes = $"{APK_DECOMPILED_PATH}\\lib\\{folderName}";

                    if (!MoveDirectory(libSource, libDes, false))
                    {
                        WriteOutput($"Cannot Move {libSource}\nTo => {libDes}", Enums.LogsType.Error, "022");
                        return;
                    }
                }
            }

            ProcessType(Enums.ProcessType.CompileApk);
            FormState(State.Running);
            CompileWorker.RunWorkerAsync();
        }

        private void CompileWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var outputFile = "";
            Invoke(new MethodInvoker(delegate
            {
                outputFile = $"{_outputDir}\\{txtNameGame.Text}.apk";
            }));
            ProcessRun($"/c java -jar {_apkTool}.jar b -f -o \"{outputFile}\" \"{APK_DECOMPILED_PATH}\"", BUILD_TOOL_PATH, "204");
        }

        private void CompileWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            CompileApkDone();
        }

        private void CompileApkDone()
        {
            SetSignApk();
        }

        #endregion Compile Apk

        #region Sign Apk

        private void SetSignApk()
        {
            SignWorker.RunWorkerAsync();
        }

        private void SignWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (!_compiled)
            {
            }

            _compiled = false;
            var outputFile = "";
            Invoke(new MethodInvoker(delegate
            {
                outputFile = $"{_outputDir}\\{txtNameGame.Text}";
            }));
            WriteOutput($"Compiled {outputFile}.apk", Enums.LogsType.Success);

            ProcessRun($"/c java -jar ApkSigner.jar sign --key \"{BUILD_TOOL_PATH}tfive.pk8\" --cert \"{BUILD_TOOL_PATH}tfive.pem\" --v4-signing-enabled false --out \"{outputFile}-Signed.apk\" \"{outputFile}.apk\"", BUILD_TOOL_PATH, "205");
        }

        private void SignWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            var outputFile = "";
            Invoke(new MethodInvoker(delegate
            {
                outputFile = $"{_outputDir}\\{txtNameGame.Text}-Signed.apk";
            }));
            WriteOutput($"Signed {outputFile}", Enums.LogsType.Success);

            if (_apkType != ".apk" && !_mySettings.chkMergeApk)
            {
                ArchiveApk();
            }

            var outputDir = $"{_outputDir}\\";

            try
            {
                Directory.Delete(outputDir + "lib", true);
                Directory.Delete(outputDir + "smali", true);
            }
            catch
            {
                //
            }

            FormState(State.Idle);
            ProcessType(Enums.ProcessType.None);
        }

        private void ArchiveApk()
        {
            Lib2Config();
            Apk2Apks();
        }

        private void Lib2Config()
        {
            var outputDir = $"{_outputDir}\\";
            var sourceDir = $"{TEMP_PATH_T_FIVE}\\";
            var outputFile = $"{_outputDir}\\{txtNameGame.Text}{_apkType}";
            var outputSignedFile = $"{_outputDir}\\{txtNameGame.Text}-signed{_apkType}";
            var fileName = _apkType switch
            {
                ".apks" => comboType.SelectedIndex == (int)Enums.TypeAbi.Arm
                    ? "split_config.armeabi_v7a.apk"
                    : "split_config.arm64_v8a.apk",
                ".xapk" => comboType.SelectedIndex == (int)Enums.TypeAbi.Arm
                    ? "config.armeabi_v7a.apk"
                    : "config.arm64_v8a.apk",
                _ => ""
            };

            File.Copy(_apkName, outputFile, true);
            File.Copy(_apkName, outputSignedFile, true);
            File.Copy(sourceDir + fileName, outputDir + fileName, true);

            var folderName = comboType.SelectedIndex == (int)Enums.TypeAbi.Arm ? "armeabi-v7a\\" : "arm64-v8a\\";

            DirectoryInfo dir = new(outputDir + "lib\\" + folderName);

            using var zipToOpen = new FileStream(outputDir + fileName, FileMode.Open);
            using var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
            foreach (var file in dir.GetFiles("*"))
            {
                archive.CreateEntryFromFile(file.FullName, "lib\\" + folderName + file.Name);
            }
            archive.Dispose();
            zipToOpen.Dispose();
        }

        private void Apk2Apks()
        {
            var mainFile = $"{_outputDir}\\{txtNameGame.Text}{_apkType}";
            var mainSignedFile = $"{_outputDir}\\{txtNameGame.Text}-signed{_apkType}";
            var apkFile = $"{_outputDir}\\{txtNameGame.Text}.apk";
            var apkSignedFile = $"{_outputDir}\\{txtNameGame.Text}-signed.apk";
            var outputDir = $"{_outputDir}\\";

            var baseName = _apkType is ".apks" ? "base.apk" : _baseName;
            var configName = _apkType switch
            {
                ".apks" => comboType.SelectedIndex == (int)Enums.TypeAbi.Arm
                    ? "split_config.armeabi_v7a.apk"
                    : "split_config.arm64_v8a.apk",
                ".xapk" => comboType.SelectedIndex == (int)Enums.TypeAbi.Arm
                    ? "config.armeabi_v7a.apk"
                    : "config.arm64_v8a.apk",
                _ => ""
            };
            var unsignList = new List<(string, string)> { (apkFile, baseName), (outputDir + configName, configName) };
            var signedList = new List<(string, string)> { (apkSignedFile, baseName), (outputDir + configName, configName) };
            mainFile.AddFiles(unsignList);
            mainSignedFile.AddFiles(signedList);
            File.Delete(outputDir + configName);
            File.Delete(apkFile);
            File.Delete(apkSignedFile);
        }

        #endregion Sign Apk

        #region Process

        private void ProcessRun(string args, string workDir, string error)
        {
            try
            {
                _errorCount = 0;
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = "cmd.exe",
                        Arguments = args,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        ErrorDialog = false,
                        CreateNoWindow = true,
                        WorkingDirectory = workDir
                    },
                    EnableRaisingEvents = true
                };
                process.OutputDataReceived += OutputDataReceived;
                process.ErrorDataReceived += ErrorDataReceived;
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit(50000);
                process.Close();
            }
            catch (Exception exception)
            {
                DumpWorker.CancelAsync();
                DecompileWorker.CancelAsync();
                MenuWorker.CancelAsync();
                CompileWorker.CancelAsync();
                SignWorker.CancelAsync();
                WriteOutput(exception.Message, Enums.LogsType.Error, error);
            }
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data.IsEmpty()) return;
            WriteOutput(e.Data, _type.ProcessTypeToLogsType());
            if (e.Data == "I: Built apk...")
            {
                _compiled = true;
            }
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data.IsEmpty()) return;
            if (e.Data == "fcntl(): Bad file descriptor") return;
            _errorCount++;
            WriteOutput(e.Data, Enums.LogsType.Error, "033");
        }

        #endregion Process

        #region Utility

        private void UpdatePath()
        {
            _outputDir = $"{AppPath}\\Output\\{txtNameGame.Text}";
        }

        private bool CheckEmpty()
        {
            if (txtLibName.IsEmpty())
            {
                MyMessage.MsgShowWarning("Library Name is Empty, Please Check it again!!!");
                WriteOutput("Library Name is Empty", Enums.LogsType.Warning);
                return false;
            }
            if (txtNDK.IsEmpty())
            {
                MyMessage.MsgShowWarning("NDK Path is Empty, Please Check it again!!!");
                WriteOutput("NDK Path is Empty", Enums.LogsType.Warning);
                return false;
            }

            if (!chkNoMenu.Checked && ImageCode.IsEmpty())
            {
                MyMessage.MsgShowWarning("Image Code is Empty, Please Check it again!!!");
                WriteOutput("Image Code is Empty", Enums.LogsType.Warning);
                return false;
            }
            if (txtNameGame.IsEmpty())
            {
                MyMessage.MsgShowWarning("Name Game is Empty, Please Check it again!!!");
                WriteOutput("Name Game is Empty", Enums.LogsType.Warning);
                return false;
            }
            if (txtTargetLib.IsEmpty())
            {
                MyMessage.MsgShowWarning("Target Library Name is Empty, Please Check it again!!!");
                WriteOutput("Target Library Name is Empty", Enums.LogsType.Warning);
                return false;
            }
            if (_type is Enums.ProcessType.ApkFull1 or Enums.ProcessType.ApkFull2 && txtApkTarget.IsEmpty())
            {
                MyMessage.MsgShowWarning("Apk Target is Empty, Please Check it again!!!");
                WriteOutput("Image Code is Empty", Enums.LogsType.Warning);
                return false;
            }
            if (OffsetPatch.FunctionList.Count == 0)
            {
                MyMessage.MsgShowWarning("Function list is Empty, Please Check it again!!!");
                WriteOutput("Function list is Empty", Enums.LogsType.Warning);
                return false;
            }

            return true;
        }

        private static bool CheckOutputGame(string destinationPath)
        {
            if (Directory.Exists(destinationPath))
            {
                if (_mySettings.chkAlwaysOverwrite)
                {
                    Directory.Delete(APK_DECOMPILED_PATH, true);
                    return true;
                }

                if (MyMessage.MsgOkCancel(destinationPath + " Found.\n\n" +
                                          "Click \"OK\" to Continue if you want to delete!" +
                                          "\n\nClick \"Cancel\" to cancel it if not!"))
                {
                    Directory.Delete(destinationPath, true);
                    return true;
                }

                return false;
            }
            return true;
        }

        private static void ProcessType(Enums.ProcessType type)
        {
            _type = type;
        }

        #endregion Utility

        #endregion Compile Page

        #region Log Page

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rbLog.Clear();
        }

        private void btnSaveLog_Click(object sender, EventArgs e)
        {
            SaveLogs();
        }

        private void SaveLogs()
        {
            var date = DateTime.Now.ToString("yyyy-M-d HH-mm-ss");
            var path = $"{AppPath}\\Logs\\{date}.txt";
            File.WriteAllText(path, rbLog.Text);
            WriteOutput($"Log saved successfully. {path}", Enums.LogsType.Logs);
        }

        private static void AppendText(RichTextBox box, string text, string strColor, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(strColor);
            box.SelectionColor = box.ForeColor;
            box.AppendText(text);
            box.ScrollToCaret();
        }

        private void TextToLogs(string str, string strColor, Color color)
        {
            Invoke(new MethodInvoker(delegate
            {
                AppendText(rbLog, str, strColor, color);
            }));
        }

        private void WriteOutput(string str, Enums.LogsType type, string errorNum = null)
        {
            var time = "{" + DateTime.Now.ToString("HH:mm:ss tt") + "} ";
            Invoke(new MethodInvoker(delegate
            {
                switch (type)
                {
                    case Enums.LogsType.CompileMenu:
                        TextToLogs(time + str + Environment.NewLine, "[Compile Menu] ", Color.DodgerBlue);
                        break;

                    case Enums.LogsType.Success:
                        TextToLogs(time + str + Environment.NewLine, "[Success] ", Color.Green);
                        break;

                    case Enums.LogsType.Warning:
                        TextToLogs(time + str + Environment.NewLine, "[Warning] ", Color.Orange);
                        break;

                    case Enums.LogsType.Error:
                        FormState(State.Idle);
                        TextToLogs(time + str + Environment.NewLine, $"[Error:{errorNum}] ", Color.Red);
                        break;

                    case Enums.LogsType.Dump:
                        TextToLogs(time + str + Environment.NewLine, "[Dump Apk] ", Color.SlateBlue);
                        break;

                    case Enums.LogsType.Decompile:
                        TextToLogs(time + str + Environment.NewLine, "[Decompile Apk] ", Color.DarkSalmon);
                        break;

                    case Enums.LogsType.CompileApk:
                        TextToLogs(time + str + Environment.NewLine, "[Compile Apk] ", Color.DeepPink);
                        break;

                    case Enums.LogsType.Logs:
                        TextToLogs(time + str + Environment.NewLine, "[Logs] ", Color.Gold);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }));
        }

        #endregion Log Page

        #region Dev Page

        #region Permission

        private void btnCopyPermission_Click(object sender, EventArgs e)
        {
            CopyText(txtPermission.Text);
        }

        private void btnCopyService_Click(object sender, EventArgs e)
        {
            CopyText(txtService.Text);
        }

        private void btnSavePermission_Click(object sender, EventArgs e)
        {
            if (!MyMessage.MsgOkCancel("Do you want to save?\n\n" +
                                       "Click \"OK\" to confirm.\n\n" +
                                       "Click \"Cancel\" to cancel.")) return;
            _mySettings.txtService = txtService.Text;
        }

        #endregion Permission

        #region Method 1

        private void btnOnCreate_Click(object sender, EventArgs e)
        {
            CopyText(txtOnCreate.Text);
        }

        private void btnSaveMethod1_Click(object sender, EventArgs e)
        {
            if (!MyMessage.MsgOkCancel("Do you want to save?\n\n" +
                                       "Click \"OK\" to confirm.\n\n" +
                                       "Click \"Cancel\" to cancel.")) return;
            _mySettings.txtOnCreate = txtOnCreate.Text;
        }

        #endregion Method 1

        #region Method 2

        private void btnCopyFind_Click(object sender, EventArgs e)
        {
            CopyText(txtFind.Text);
        }

        private void btnCopyActionMain2_Click(object sender, EventArgs e)
        {
            CopyText(txtActionMain.Text);
        }

        private void btnSaveMethod2_Click(object sender, EventArgs e)
        {
            if (!MyMessage.MsgOkCancel("Do you want to save?\n\n" +
                                       "Click \"OK\" to confirm.\n\n" +
                                       "Click \"Cancel\" to cancel.")) return;
            _mySettings.txtActionMain = txtActionMain.Text;
        }

        #endregion Method 2

        private void CopyText(string str)
        {
            Clipboard.SetText(str);
            if (!_mySettings.chkSound) return;
            System.Media.SystemSounds.Beep.Play();
        }

        #endregion Dev Page

        #region About Page

        private void comboApktool_SelectedIndexChanged(object sender, EventArgs e)
        {
            _apkTool = comboApktool.SelectedItem.ToString();
        }

        private void btnSaveSettings2_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyMessage.MsgOkCancel("Save Settings.\n\n" +
                                          "Click \"OK\" to confirm.\n\n" +
                                          "Click \"Cancel\" to cancel."))
                {
                    _mySettings.txtNDK = txtNDK.Text;
                    _mySettings.apkTools = comboApktool.SelectedIndex;

                    _mySettings.chkRemoveTemp = chkRemoveTemp.Checked;
                    _mySettings.chkTFiveCredit = chkTFiveCredit.Checked;
                    _mySettings.chkSound = chkSound.Checked;
                    _mySettings.chkCheckUpdate = chkCheckUpdate.Checked;
                    _mySettings.chkAlwaysOverwrite = chkAlwaysOverwrite.Checked;
                    _mySettings.chkMergeApk = chkMergeApk.Checked;
                    _mySettings.Save();
                    WriteOutput("Saved Settings", Enums.LogsType.Success);
                }
            }
            catch (Exception exception)
            {
                WriteOutput(exception.Message, Enums.LogsType.Error, "034");
            }
        }

        private void btnBrowseNDK_Click(object sender, EventArgs e)
        {
            var folderBrowser = new FolderBrowser();
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (folderBrowser.SelectedPath.IsPathSpecialChar())
                {
                    MyMessage.MsgShowWarning("Ndk path must without any special character");
                    WriteOutput("Ndk path must without any special character", Enums.LogsType.Error, "035");
                    return;
                }
                txtNDK.Text = folderBrowser.SelectedPath;
                WriteOutput("Change NDK Path To: " + txtNDK.Text, Enums.LogsType.Success);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            await UpdateService.CheckGitHubNewerVersion().ConfigureAwait(false);
        }

        #endregion About Page

        #region Form Drag

        private void FrmMain_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                FormState(State.Running);

                if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

                foreach (var file in (string[])e.Data.GetData(DataFormats.FileDrop))
                {
                    switch (Path.GetExtension(file))
                    {
                        case ".apk":
                        case ".apks":
                        case ".xapk":
                            SetApkPath(file);
                            materialTabControl1.SelectedTab = materialTabControl1.TabPages[2];
                            break;

                        case ".xml":
                            LoadXml(file);
                            materialTabControl1.SelectedTab = materialTabControl1.TabPages[1];
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteOutput($"{ex.Message}", Enums.LogsType.Error, "036");
            }
        }

        private void FrmMain_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            foreach (var file in (string[])e.Data.GetData(DataFormats.FileDrop))
            {
                switch (Path.GetExtension(file))
                {
                    case ".apk":
                    case ".apks":
                    case ".xapk":
                    case ".xml":
                        e.Effect = DragDropEffects.Copy;
                        break;
                }
            }
        }

        #endregion Form Drag

        #region Utility

        private bool DeleteAll(string path)
        {
            try
            {
                if (!Directory.Exists(path)) return true;
                var directory = new DirectoryInfo(path);
                foreach (var file in directory.GetFiles())
                {
                    file.Delete();
                }
                foreach (var dir in directory.GetDirectories())
                {
                    dir.Delete(true);
                }
                WriteOutput("Deleted " + path, Enums.LogsType.Success);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "037");
                return false;
            }
        }

        private bool ExtractZip(string sourceFileName, string destinationPath)
        {
            try
            {
                using (var zipArchive = ZipFile.OpenRead(sourceFileName))
                {
                    zipArchive.ExtractToDirectory(destinationPath, true);
                }
                WriteOutput($"Extract {Path.GetFileName(sourceFileName)}{Environment.NewLine}To => {destinationPath}", Enums.LogsType.Success);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "038");
                return false;
            }
        }

        private bool MoveDirectory(string sourceDirectory, string destinationPath, bool overwrite = true, bool deleteSource = true)
        {
            try
            {
                if (Directory.Exists(destinationPath) && overwrite)
                {
                    if (_mySettings.chkAlwaysOverwrite)
                    {
                        Directory.Delete(destinationPath, true);
                    }
                    else
                    {
                        if (MyMessage.MsgOkCancel(destinationPath + " Found.\n\n" +
                                                  "Click \"OK\" to Continue if you want to overwrite!" +
                                                  "\n\nClick \"Cancel\" to cancel it if not!"))
                        {
                            Directory.Delete(destinationPath, true);
                        }
                        else
                        {
                            FormState(State.Idle);
                            return false;
                        }
                    }
                }

                Checker.CheckFolder("Output\\" + txtNameGame.Text);

                if (!DirectoryCopy(sourceDirectory, destinationPath, true)) return false;

                if (Directory.Exists(sourceDirectory) && deleteSource)
                {
                    Directory.Delete(sourceDirectory, true);
                }

                if (Directory.Exists(destinationPath))
                {
                    WriteOutput($"Move {sourceDirectory}{Environment.NewLine}To => {destinationPath}", Enums.LogsType.Success);
                    return true;
                }

                WriteOutput($"Can not Move {sourceDirectory}{Environment.NewLine}To => {destinationPath}", Enums.LogsType.Error, "039");
                return false;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "040");
                return false;
            }
        }

        private bool DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            try
            {
                var dir = new DirectoryInfo(sourceDirName);

                if (!dir.Exists)
                {
                    return false;
                }

                var dirs = dir.GetDirectories();
                Directory.CreateDirectory(destDirName);
                foreach (var file in dir.GetFiles())
                {
                    var tempPath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(tempPath, true);
                }
                if (copySubDirs)
                {
                    foreach (var subDir in dirs)
                    {
                        var tempPath = Path.Combine(destDirName, subDir.Name);
                        DirectoryCopy(subDir.FullName, tempPath, true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "041");
                return false;
            }
        }

        private void FormState(State state)
        {
            EnableController(this, state != State.Running);
            if (state == State.Idle && _mySettings.chkSound)
            {
                System.Media.SystemSounds.Beep.Play();
            }
        }

        private static void EnableController(Form form, bool value)
        {
            foreach (var obj in form.Controls)
            {
                var control = (Control)obj;
                switch (control.GetType().Name)
                {
                    case "Button":
                    case "TextBox":
                    case "RadioButton":
                    case "ListView":
                    case "Label":
                    case "LinkLabel":
                    case "ListBox":
                    case "MaterialButton":
                    case "MaterialCheckBox":
                    case "MaterialComboBox":
                    case "MaterialTabControl":
                    case "MaterialTextBox":
                    case "RichTextBox":
                        control.Enabled = value;
                        break;

                    case "GroupBox":
                    case "MaterialCard":
                    case "MaterialForm":
                    case "Panel":
                    case "TableLayoutPanel":
                    case "TabPage":
                        EnableController(control, value);
                        break;
                }
            }
        }

        private static void EnableController(Control control, bool value)
        {
            foreach (var obj in control.Controls)
            {
                var control2 = (Control)obj;
                switch (control.GetType().Name)
                {
                    case "Button":
                    case "TextBox":
                    case "RadioButton":
                    case "ListView":
                    case "Label":
                    case "LinkLabel":
                    case "ListBox":
                    case "MaterialButton":
                    case "MaterialCheckBox":
                    case "MaterialComboBox":
                    case "MaterialTabControl":
                    case "MaterialTextBox":
                    case "RichTextBox":
                        control2.Enabled = value;
                        break;
                }
                switch (control2.GetType().Name)
                {
                    case "GroupBox":
                    case "MaterialCard":
                    case "MaterialForm":
                    case "Panel":
                    case "TableLayoutPanel":
                    case "TabPage":
                        EnableController(control, value);
                        break;
                }
            }
        }

        #endregion Utility
    }
}