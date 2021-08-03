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
    //TODO Support apks, xapk
    //Vector
    //Error Num
    public partial class FrmMain : MaterialForm
    {
        private const bool Debug = false;

        public FrmMain()
        {
            InitializeComponent();
            LoadTheme();
        }

        #region Variable

        public static readonly string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string ImageCode;

        private static Enums.ProcessType _type = Enums.ProcessType.None;

        private static int _compile, _smaliCount;

        private static MySettings _mySettings = new();

        private static readonly string _apkTargetPath = $"{AppPath}\\BuildTools\\ApkTarget";

        private static readonly string _tempPathMenu = Path.GetTempPath() + "TFiveMenu";

        private static string _launch, _apkTarget, _apkTool, _apkName, _apkType;

        private static string[] _menuFiles;

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
        }

        private void LoadTheme()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Blue700, TextShade.WHITE);
        }

        private void CheckFolder()
        {
            if (!Directory.Exists(_tempPathMenu))
            {
                Directory.CreateDirectory(_tempPathMenu);
            }

            Utility.CheckFolder("Theme");
            Utility.CheckFolder("Menu");
            Utility.CheckFolder("Output");
            Utility.CheckFolder("Save");
            Utility.CheckFolder("Logs");
            Utility.CheckFolder("BuildTools");
        }

        private void LoadFiles()
        {
            var themeFiles = Directory.GetFiles(AppPath + "\\Theme", "*.zip");
            _menuFiles = Directory.GetFiles(AppPath + "\\Menu", "*.zip");
            var apktoolFiles = Directory.GetFiles(AppPath + "\\BuildTools", "Apktool_*.jar");

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

            if (!Utility.CheckFiles("Theme", "Default.zip"))
            {
                MyMessage.MsgShowError("Theme file Default.zip is missing!!");
                Application.Exit();
            }

            if (!Utility.CheckFiles("Menu", "Default.zip"))
            {
                MyMessage.MsgShowError("Menu file Default.zip is missing!!");
                Application.Exit();
            }

            var themeFile = themeFiles.Select(Path.GetFileName).ToList();
            var menuFile = _menuFiles.Select(Path.GetFileName).ToList();
            var apktoolFile = apktoolFiles.Select(Path.GetFileName).ToList();

            if (!Utility.IsEqual(themeFile, menuFile))
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
            if (!Utility.IsEmpty(_mySettings.txtToast, false) && _mySettings.txtToast.Contains('|'))
            {
                foreach (var t in _mySettings.txtToast.Split('|'))
                {
                    listToast.Items.Add(t);
                }
            }

            if (!Utility.IsEmpty(_mySettings.txtToast, false) && !_mySettings.txtToast.Contains('|'))
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
            chkLogsComplie.Checked = _mySettings.chkLogsComplie;
            chkLogsSuccess.Checked = _mySettings.chkLogsSuccess;
            chkLogsError.Checked = _mySettings.chkLogsError;
            chkLogsWarning.Checked = _mySettings.chkLogsWarning;
            chkSound.Checked = _mySettings.chkSound;
            chkCheckUpdate.Checked = _mySettings.chkCheckUpdate;

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
            catch
            {
            }
        }

        private void LoadImg()
        {
            try
            {
                if (!Utility.IsEmpty(ImageCode, false))
                {
                    picImg.Image = Base64ToImage(ImageCode);
                }
            }
            catch (Exception exception)
            {
                picImg.Image = null;
                ImageCode = "";
                WriteOutput(exception.Message, Enums.LogsType.Error, "001");
            }
        }

        #endregion Load

        #region Main Page

        private void btnAddToast_Click(object sender, EventArgs e)
        {
            try
            {
                if (Utility.IsEmpty(txtToast.Text)) return;
                listToast.Items.Add(txtToast.Text);
                WriteOutput("Add Toast " + txtToast.Text, Enums.LogsType.Success);
            }
            catch (Exception exception)
            {
                WriteOutput(exception.Message, Enums.LogsType.Error, "002");
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
                WriteOutput(exception.Message, Enums.LogsType.Error, "003");
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
                    ImageCode = ImageToBase64(CompressImage(openFile.FileName, 1), imgFormat);
                    LoadImg();
                }
                catch
                {
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
                    var toast = listToast.Items.Cast<object>().Aggregate("", (current, t) => current + (t + "|"));
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
                WriteOutput(exception.Message, Enums.LogsType.Error, "004");
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
            AddListValues(cheatName[cheatName.Count - 1], Utility.FunctionTypeToString(functionType));
            OffsetPatch.OffsetList.Clear();
        }

        private void AddAllDataList(bool reValues = false)
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
                AddListValues(cheatName[i], Utility.FunctionTypeToString(functionType));
                OffsetPatch.OffsetList.Clear();
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
                var frmFunction = new FrmAddFunction(Utility.StringToFunctionType(dataList.SelectedRows[0].Cells[1].Value.ToString()), index);
                Hide();
                frmFunction.ShowDialog();
                frmFunction.Dispose();
                Show();
                if (Values.Save)
                {
                    AddAllDataList(true);
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
                WriteOutput(ex.Message, Enums.LogsType.Error, "005");
            }
        }

        private void dataList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                try
                {
                    var hti = dataList.HitTest(e.X, e.Y);
                    dataList.ClearSelection();
                    dataList.Rows[hti.RowIndex].Selected = true;
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
                WriteOutput(ex.Message, Enums.LogsType.Error, "006");
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
                WriteOutput(ex.Message, Enums.LogsType.Error, "007");
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
                try
                {
                    FormState(State.Running);
                    OffsetPatch.T5Menu = LoadXml<TFiveMenu>(openFile.FileName);

                    OffsetPatch.FunctionList = OffsetPatch.T5Menu.FunctionList;
                    txtNameGame.Text = OffsetPatch.T5Menu.GameName;
                    txtTargetLib.Text = OffsetPatch.T5Menu.Target;
                    comboType.SelectedIndex = (int)OffsetPatch.T5Menu.TypeAbi;

                    FormState(State.Idle);
                }
                catch (Exception ex)
                {
                    FormState(State.Idle);
                    WriteOutput(ex.Message, Enums.LogsType.Error, "008");
                }
                finally
                {
                    AddAllDataList();
                    txtNameGame.Text = Path.GetFileNameWithoutExtension(openFile.FileName);
                }
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
                WriteOutput(ex.Message, Enums.LogsType.Error, "009");
            }
        }

        private T LoadXml<T>(string fileName)
        {
            if (Utility.IsEmpty(fileName, false)) { return default; }

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
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                    }
                }
                WriteOutput("Loaded: " + Path.GetFileName(fileName), Enums.LogsType.Success);
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message, Enums.LogsType.Error, "010");
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
            Process.Start(_tempPathMenu);
        }

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
                    WriteOutput(ex.Message, Enums.LogsType.Error, "011");
                }
            }
        }

        private void btnCompileMenu_Click(object sender, EventArgs e)
        {
            CompileType(Enums.ProcessType.MenuFull);
            FullCompile();
        }

        private void btnCompileApk1_Click(object sender, EventArgs e)
        {
            SetDumpApk();
            CompileType(Enums.ProcessType.ApkFull1);
            FullCompile();
        }

        private void btnCompileApk2_Click(object sender, EventArgs e)
        {
            SetDumpApk();
            CompileType(Enums.ProcessType.ApkFull2);
            FullCompile();
        }

        private void btnDecompileApk_Click(object sender, EventArgs e)
        {
            CompileType(Enums.ProcessType.DecompileApk);
            FormState(State.Running);
            Worker.RunWorkerAsync();
        }

        private void btnCompileApk_Click(object sender, EventArgs e)
        {
            if (Utility.IsEmpty(txtNameGame, false))
            {
                MyMessage.MsgShowWarning("Name Game is Empty, Please Check it again!!!");
                WriteOutput("Name Game is Empty", Enums.LogsType.Warning);
                return;
            }

            if (!Directory.Exists(_apkTargetPath))
            {
                MyMessage.MsgShowWarning($"Not Found {_apkTargetPath}, Please Check it again!!!");
                WriteOutput($"Not Found {_apkTargetPath}", Enums.LogsType.Warning);
                return;
            }

            CompileType(Enums.ProcessType.CompileApk);
            FormState(State.Running);
            CompileApk(false);
        }

        #endregion Events

        #region Apk Manager

        private void SetApkPath(string apkTarget)
        {
            _apkTarget = apkTarget;
            txtApkTarget.Text = _apkTarget;
            WriteOutput($"Set Apk Target: {_apkTarget}", Enums.LogsType.Success);
            _apkName = Utility.GetApkName(_apkTarget);
            _apkType = Path.GetExtension(_apkTarget);
            SetDumpApk();
            //lbApk.Text = $"Apk Name: {_apkName}\n\n" +
            //             "App Name: \n" +
            //             "Version: \n" +
            //             "Launch: ";
        }

        private void SetDumpApk()
        {
            if (string.IsNullOrWhiteSpace(_apkTarget))
            {
                MyMessage.MsgShowWarning("Apk Target is Empty, Please Check it again!!!");
                WriteOutput("Apk Target is Empty", Enums.LogsType.Warning);
                return;
            }
            File.Copy(_apkTarget, $"{_tempPathMenu}\\ApkTarget{_apkType}", true);
            _apkTarget = $"{_tempPathMenu}\\ApkTarget{_apkType}";

            FormState(State.Running);
            SplitApk().GetAwaiter();
            CompileType(Enums.ProcessType.DumpApk);
            DumpApk();
        }

        private void DumpApk()
        {
            if (!File.Exists($"{_apkTarget}"))
            {
                MyMessage.MsgShowError($"{_apkTarget} Not found!!" +
                                       "\nPlease select the Apk again");
                WriteOutput($"{_tempPathMenu}\\ApkTarget.apk Not found", Enums.LogsType.Error, "000");
                FormState(State.Idle);
                Worker.CancelAsync();
                return;
            }

            if (Directory.Exists(_apkTargetPath))
            {
                var result = MyMessage.MsgYesNoCancel(_apkTargetPath + " Found.\n\n" +
                                                      "Click \"Yes\" to Continue if you want to overwrite!" +
                                                      "\n\nClick \"No\" to keep old files!" +
                                                      "\n\nClick \"Cancel\" to cancel it if not!");
                switch (result)
                {
                    case DialogResult.Yes:
                        Directory.Delete(_apkTargetPath, true);
                        break;

                    case DialogResult.Cancel:
                        FormState(State.Idle);
                        Worker.CancelAsync();
                        return;
                }
            }

            Worker.RunWorkerAsync();
        }

        private async Task SplitApk()
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

        private static async Task APKsDump(int type)
        {
            await Task.Factory.StartNew(() =>
            {
                using var archive = ZipFile.OpenRead(_apkTarget);
                foreach (var entryApks in archive.Entries)
                {
                    if (entryApks.FullName == "base.apk")
                    {
                        entryApks.ExtractToFile($"{_tempPathMenu}\\ApkTarget.apk", true);
                        _apkTarget = $"{_tempPathMenu}\\ApkTarget.apk";
                    }

                    if (type == (int)Enums.TypeAbi.Arm)
                    {
                        if (entryApks.FullName == "split_config.armeabi_v7a.apk")
                        {
                            entryApks.ExtractToFile(Path.Combine(_tempPathMenu, entryApks.FullName), true);
                        }
                    }
                    else
                    {
                        if (entryApks.FullName == "split_config.arm64_v8a.apk")
                        {
                            entryApks.ExtractToFile(Path.Combine(_tempPathMenu, entryApks.FullName), true);
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
                            entryApks.ExtractToFile(Path.Combine(_tempPathMenu, entryApks.FullName), true);
                        }
                    }
                    else
                    {
                        if (entryApks.FullName == "config.arm64_v8a.apk")
                        {
                            entryApks.ExtractToFile(Path.Combine(_tempPathMenu, entryApks.FullName), true);
                        }
                    }

                    var apkFile = Path.Combine(Path.GetTempPath(), entryApks.FullName);
                    using var entryBase = ZipFile.OpenRead(apkFile);
                    var classes = entryBase.Entries.FirstOrDefault(f => f.Name.Contains("classes.dex"));
                    if (classes != null)
                    {
                        entryApks.ExtractToFile($"{_tempPathMenu}\\ApkTarget.apk", true);
                        _apkTarget = $"{_tempPathMenu}\\ApkTarget.apk";
                    }
                }
                archive.Dispose();
            }).ConfigureAwait(false);
        }

        #endregion Apk Manager

        private void FullCompile()
        {
            if (Utility.IsEmpty(txtLibName, false))
            {
                MyMessage.MsgShowWarning("Library Name is Empty, Please Check it again!!!");
                WriteOutput("Library Name is Empty", Enums.LogsType.Warning);
                return;
            }
            if (Utility.IsEmpty(txtNDK, false))
            {
                MyMessage.MsgShowWarning("NDK Path is Empty, Please Check it again!!!");
                WriteOutput("NDK Path is Empty", Enums.LogsType.Warning);
                return;
            }

            if (!chkNoMenu.Checked && Utility.IsEmpty(ImageCode, false))
            {
                MyMessage.MsgShowWarning("Image Code is Empty, Please Check it again!!!");
                WriteOutput("Image Code is Empty", Enums.LogsType.Warning);
                return;
            }
            if (Utility.IsEmpty(txtNameGame, false))
            {
                MyMessage.MsgShowWarning("Name Game is Empty, Please Check it again!!!");
                WriteOutput("Name Game is Empty", Enums.LogsType.Warning);
                return;
            }
            if (Utility.IsEmpty(txtTargetLib, false))
            {
                MyMessage.MsgShowWarning("Target Library Name is Empty, Please Check it again!!!");
                WriteOutput("Target Library Name is Empty", Enums.LogsType.Warning);
                return;
            }
            if (_type is Enums.ProcessType.ApkFull1 or Enums.ProcessType.ApkFull2 && Utility.IsEmpty(txtApkTarget, false))
            {
                MyMessage.MsgShowWarning("Apk Target is Empty, Please Check it again!!!");
                WriteOutput("Image Code is Empty", Enums.LogsType.Warning);
                return;
            }
            if (OffsetPatch.FunctionList.Count == 0)
            {
                MyMessage.MsgShowWarning("Function list is Empty, Please Check it again!!!");
                WriteOutput("Function list is Empty", Enums.LogsType.Warning);
                return;
            }
            FormState(State.Running);
            materialTabControl1.SelectedTab = materialTabControl1.TabPages[3];
            if (!DeleteAll(_tempPathMenu)) return;
            if (!ExtractZip(AppPath + $"\\Menu\\{comboMenu.SelectedItem}.zip", _tempPathMenu)) return;
            if (!ExtractZip(AppPath + $"\\Theme\\{comboMenu.SelectedItem}.zip", _tempPathMenu)) return;
            if (!Replacer())
            {
                MyMessage.MsgShowError("Failed to Replace Something");
                WriteOutput("Failed to Replace Something", Enums.LogsType.Error, "012");
                FormState(State.Idle);
                return;
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (Debug)
            {
#pragma warning disable 162
                FormState(State.Idle);
#pragma warning restore 162
                return;
            }

            var destinationPath = $"{AppPath}\\Output\\{txtNameGame.Text}";
            if (Directory.Exists(destinationPath) && MyMessage.MsgOkCancel(destinationPath + " Found.\n\n" +
                                                                        "Click \"OK\" to Continue if you want to delete!" +
                                                                        "\n\nClick \"Cancel\" to cancel it if not!"))
            {
                Directory.Delete(destinationPath, true);
            }

            if (_type is Enums.ProcessType.MenuFull)
            {
                if (!MoveDirectory(_tempPathMenu + "\\com", $"{destinationPath}\\smali\\com")) return;
            }
            else if (!MoveDirectory(_tempPathMenu + "\\com",
                $"{destinationPath}\\{Utility.SmaliCountToName(_smaliCount, true)}\\com"))
            {
                return;
            }

            Worker.RunWorkerAsync();
        }

        private static void CompileType(Enums.ProcessType type)
        {
            _type = type;
        }

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

        private bool MainActivity()
        {
            try
            {
                var text = File.ReadAllText(_tempPathMenu + "\\com\\tfive\\MainActivity.smali");
                text = text.Replace("MyLibName", txtLibName.Text);
                if (_type is Enums.ProcessType.ApkFull2)
                {
                    text = text.Replace("com.unity3d.player.UnityPlayerActivity", _launch);
                }
                File.WriteAllText(_tempPathMenu + "\\com\\tfive\\MainActivity.smali", text);
                WriteOutput("Replaced MainActivity.smali", Enums.LogsType.Success);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "013");
                FormState(State.Idle);
                return false;
            }
        }

        private bool AndroidMk()
        {
            try
            {
                var text = File.ReadAllText(_tempPathMenu + "\\jni\\Android.mk");
                text = text.Replace("MyLibName", txtLibName.Text);
                File.WriteAllText(_tempPathMenu + "\\jni\\Android.mk", text);
                WriteOutput("Replaced Android.mk", Enums.LogsType.Success);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "014");
                FormState(State.Idle);
                return false;
            }
        }

        private bool ApplicationMk()
        {
            try
            {
                var text = File.ReadAllText(_tempPathMenu + "\\jni\\Application.mk");

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
                File.WriteAllText(_tempPathMenu + "\\jni\\Application.mk", text);
                WriteOutput("Replaced Application.mk", Enums.LogsType.Success);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "015");
                FormState(State.Idle);
                return false;
            }
        }

        private bool MenuString()
        {
            try
            {
                var text = File.ReadAllText(_tempPathMenu + "\\jni\\Menu.h");
                text = text.Replace("(yourName)", txtLibName.Text)
                    .Replace("(yourSite)", txtSite.Text)
                    .Replace("(yourText)", txtText.Text);
                text = chkNoMenu.Checked ?
                    text.Replace(@"return env->NewStringUTF(OBFUSCATE(""(yourImage)""));", "return NULL;")
                    : text.Replace("(yourImage)", ImageCode);
                text = chkTFiveCredit.Checked ? text.Replace("//(TFiveEndCredit)", @"OBFUSCATE(""0_RichWebView_<html><body><marquee style=\""color: white; font-weight:bold;\"" direction=\""left\"" scrollamount=\""5\"" behavior=\""scroll\"">TFive Tools</marquee></body></html>"")") : text;
                File.WriteAllText(_tempPathMenu + "\\jni\\Menu.h", text);
                WriteOutput("Replaced Menu.h (Credit)", Enums.LogsType.Success);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "016");
                FormState(State.Idle);
                return false;
            }
        }

        private bool MainHack()
        {
            try
            {
                var text = File.ReadAllText(_tempPathMenu + "\\jni\\Main.cpp");
                var memoryPatch = ModMenuPattern.MemoryPatch();
                var newVariable = ModMenuPattern.NewVariable();
                var newMethod = ModMenuPattern.NewMethod();
                var hackThread64 = ModMenuPattern.HackThread64((Enums.TypeAbi)comboType.SelectedIndex);
                var hackThread = ModMenuPattern.HackThread((Enums.TypeAbi)comboType.SelectedIndex);
                var toastHere = ModMenuPattern.ToastHere(listToast);
                var featuresList = ModMenuPattern.FeaturesList();
                var newFeatures = ModMenuPattern.NewFeatures();

                text = text.Replace("//VariableHere", memoryPatch)
                    .Replace("//NewVariableHere", newVariable)
                    .Replace("//NewMethodHere", newMethod)
                    .Replace("(yourTargetLibName)", txtTargetLib.Text)
                    .Replace("//(hackThread64)", hackThread64)
                    .Replace("//(hackThread)", hackThread)
                    .Replace("//ToastHere", toastHere.Remove(toastHere.LastIndexOf(Environment.NewLine, StringComparison.Ordinal)))
                    .Replace("//(yourFeaturesList)", featuresList)
                    .Replace("(yourEndCredit)", txtEndCredit.Text)
                    .Replace("//(yourFeatures)", newFeatures);
                File.WriteAllText(_tempPathMenu + "\\jni\\Main.cpp", text);
                WriteOutput("Replaced Main.cpp", Enums.LogsType.Success);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "017");
                FormState(State.Idle);
                return false;
            }
        }

        private bool ApkMainActivity()
        {
            try
            {
                var text = File.ReadAllText(AppPath + "\\BuildTools\\ApkTarget\\AndroidManifest.xml");
                text = text.Replace("<uses-permission", $"{txtPermission.Text}\n    <uses-permission");
                if (_type is Enums.ProcessType.ApkFull2)
                {
                    text = text.Replace(txtFind.Text, "")
                        .Replace("<action android:name=\"android.intent.action.MAIN\" />", "")

                        .Replace("</application>", $"    {_mySettings.txtActionMain}\n    </application>");
                }
                text = text.Replace("</application>", $"    {_mySettings.txtService}\n    </application>");
                File.WriteAllText(AppPath + "\\BuildTools\\ApkTarget\\AndroidManifest.xml", text);
                WriteOutput("Replaced AndroidManifest.xml", Enums.LogsType.Success);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "018");
                FormState(State.Idle);
                return false;
            }
        }

        private bool OnCreate()
        {
            try
            {
                var launch = $"{AppPath}\\BuildTools\\ApkTarget\\{Utility.SmaliCountToName(_smaliCount)}\\" + _launch.Replace(".", "\\") + ".smali";

                var text = File.ReadAllText(launch);
                text = text.Replace(@".method protected onCreate(Landroid/os/Bundle;)V
    .locals 2",
                    ".method protected onCreate(Landroid/os/Bundle;)V" +
                    "\n    .locals 2" +
                    $"\n\n    {_mySettings.txtOnCreate}");

                File.WriteAllText(launch, text);
                WriteOutput("Replaced OnCreate", Enums.LogsType.Success);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "019");
                FormState(State.Idle);
                return false;
            }
        }

        #endregion Modify Files

        #region Worker

        private void compilerWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (_type is Enums.ProcessType.MenuFull or Enums.ProcessType.ApkFull1 or Enums.ProcessType.ApkFull2)
            {
                ProcessRun($"/c {_mySettings.txtNDK}\\build\\ndk-build", $"{_tempPathMenu}\\jni", "020");
            }

            if (_type is Enums.ProcessType.DumpApk)
            {
                ProcessRun($"/c aapt dump badging {_apkTarget} PAUSE > {_tempPathMenu}\\result.txt",
                    $"{AppPath}\\BuildTools\\", "024");
            }

            if (_type is Enums.ProcessType.DecompileApk)
            {
                ProcessRun($"/c {_apkTool}.jar d {_apkTarget}", $"{AppPath}\\BuildTools\\", "026");
            }
        }

        private void compilerWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (_type is Enums.ProcessType.MenuFull or Enums.ProcessType.ApkFull1 or Enums.ProcessType.ApkFull2)
            {
                CompileMenuDone();

                if (_type is Enums.ProcessType.MenuFull)
                {
                    FormState(State.Idle);
                    CompileType(Enums.ProcessType.None);
                }
            }

            if (_type is Enums.ProcessType.ApkFull1 or Enums.ProcessType.ApkFull2 or Enums.ProcessType.CompileApk)
            {
                if (_type is Enums.ProcessType.CompileApk)
                {
                    CompileApk(false);
                }
                else
                {
                    CompileApk();
                }
            }

            if (_type is Enums.ProcessType.DumpApk)
            {
                DumpApkDone();
                FormState(State.Idle);
                CompileType(Enums.ProcessType.None);
            }

            if (_type is Enums.ProcessType.DecompileApk)
            {
                DecompileApkDone();
            }
        }

        private void ProcessRun(string args, string workDir, string error)
        {
            try
            {
                _compile = 0;
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
                        CreateNoWindow = true,
                        WorkingDirectory = workDir
                    }
                };

                process.OutputDataReceived += OutputDataReceived;
                process.ErrorDataReceived += ErrorDataReceived;
                process.EnableRaisingEvents = true;
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit(50000);
                process.Close();
            }
            catch (Exception exception)
            {
                Worker.CancelAsync();
                WriteOutput(exception.Message, Enums.LogsType.Error, error);
                FormState(State.Idle);
            }
        }

        private void CompileMenuDone()
        {
            if (_compile > 0 && !_mySettings.debugMode)
            {
                MyMessage.MsgShowError("Failed to Compile");
                WriteOutput("Failed to Compile", Enums.LogsType.Error, "021");
                SaveLogs();
                FormState(State.Idle);
                return;
            }

            var tempOutputDir = $"{_tempPathMenu}\\libs";
            var desDir = AppPath + "\\Output\\" + txtNameGame.Text + "\\lib";
            var deleteTemp = chkRemoveTemp.Checked;

            if (MoveDirectory(tempOutputDir, desDir, true, deleteTemp))
            {
                WriteOutput($"Move {tempOutputDir}{Environment.NewLine}To => {desDir}", Enums.LogsType.Success);
            }
            else
            {
                WriteOutput("Can not Move", Enums.LogsType.Error, "022");
            }
        }

        private void CompileApk(bool move = true)
        {
            if (move)
            {
                var desDir = AppPath + "\\Output\\" + txtNameGame.Text + "\\lib";
                var deleteTemp = chkRemoveTemp.Checked;

                var apkTarget = $"{_apkTargetPath}";
                var outputDir = $"{AppPath}\\Output\\{txtNameGame.Text}\\";
                if (MoveDirectory(outputDir, $"{apkTarget}", false, deleteTemp))
                {
                    WriteOutput($"Move {desDir}{Environment.NewLine}To => {apkTarget}", Enums.LogsType.Success);
                }
                else
                {
                    WriteOutput("Can not Move", Enums.LogsType.Error, "023");
                    return;
                }
            }
            ApkWorker.RunWorkerAsync();
        }

        private void DumpApkDone()
        {
            var activity = File.ReadAllText($"{_tempPathMenu}\\result.txt");

            _launch = Utility.GetBetween(activity, "launchable-activity: name='", "'");
            var appName = Utility.GetBetween(activity, "application-label:'", "'");
            var appVersion = Utility.GetBetween(activity, "versionName='", "'");

            lbApk.Text = $"Apk Name: {_apkName}\n\n" +
                         $"App Name: {appName}\n" +
                         $"Version: {appVersion}\n" +
                         $"Launch: {_launch}";

            if (_compile > 0 && !_mySettings.debugMode)
            {
                MyMessage.MsgShowError("Failed to Compile");
                WriteOutput("Failed to Compile", Enums.LogsType.Error, "025");

                SaveLogs();
                FormState(State.Idle);
            }
        }

        private void DecompileApkDone()
        {
            GetSmailiCount();
            WriteOutput("Decompiled APK file", Enums.LogsType.Success);
        }

        private void ApkWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (_type is Enums.ProcessType.ApkFull1 or Enums.ProcessType.ApkFull2 or Enums.ProcessType.CompileApk)
            {
                ProcessRun($"/c {_apkTool}.jar b ApkTarget", $"{AppPath}\\BuildTools\\", "026");
            }
        }

        private void ApkWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (_type is Enums.ProcessType.ApkFull1 or Enums.ProcessType.ApkFull2 or Enums.ProcessType.CompileApk)
            {
                CompileApkDone();
            }

            FormState(State.Idle);
            CompileType(Enums.ProcessType.None);
        }

        private void CompileApkDone()
        {
            var apkPath = $"{AppPath}\\BuildTools\\ApkTarget\\dist\\ApkTarget.apk";
            var outputFile = $"{AppPath}\\Output\\{txtNameGame.Text}\\{txtNameGame.Text}.apk";

            if (!File.Exists(apkPath))
            {
                WriteOutput($"Not found {apkPath}", Enums.LogsType.Error, "000");
                return;
            }
            if (File.Exists(apkPath) && File.Exists(outputFile) &&
                !MyMessage.MsgOkCancel(outputFile + " Found.\n\n" +
                                       "Click \"OK\" to Continue if you want to overwrite!" +
                                       "\n\nClick \"Cancel\" to cancel it if not!"))
            {
                return;
            }

            Directory.CreateDirectory($"{AppPath}\\Output\\{txtNameGame.Text}");
            File.Copy(apkPath, outputFile, true);
            WriteOutput($"Compiled {outputFile}", Enums.LogsType.Success);
            SignApk();
        }

        private void SignApk()
        {
            var outputFile = $"{AppPath}\\Output\\{txtNameGame.Text}\\{txtNameGame.Text}";
            if (!File.Exists(outputFile + ".apk"))
            {
                MyMessage.MsgShowWarning($"{outputFile} Not found, Please Check it again!!!");
                return;
            }
            ProcessRun($"/c java -jar ApkSigner.jar sign --key tfive.pk8 --cert tfive.pem --v4-signing-enabled false --out \"{outputFile}-Signed.apk\" \"{outputFile}.apk\"", $"{AppPath}\\BuildTools\\", "026");
            WriteOutput($"Signed {outputFile}-Signed.apk", Enums.LogsType.Success);
        }

        private void GetSmailiCount()
        {
            _smaliCount = 0;
            var directory = new DirectoryInfo($"{_apkTargetPath}");
            foreach (var dir in directory.GetDirectories())
            {
                if (dir.Name.StartsWith("smali"))
                {
                    _smaliCount++;
                }
            }
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (Utility.IsEmpty(e.Data, false)) return;
            WriteOutput(e.Data, Enums.LogsType.Compile);
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (Utility.IsEmpty(e.Data, false)) return;
            if (e.Data == "fcntl(): Bad file descriptor") return;
            _compile++;
            WriteOutput(e.Data, Enums.LogsType.Error, "000");
        }

        #endregion Worker

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
            if (!chkLogsComplie.Checked && type == Enums.LogsType.Compile)
            {
                return;
            }
            if (!chkLogsSuccess.Checked && type == Enums.LogsType.Success)
            {
                return;
            }
            if (!chkLogsWarning.Checked && type == Enums.LogsType.Warning)
            {
                return;
            }
            if (!chkLogsError.Checked && type == Enums.LogsType.Error)
            {
                return;
            }

            var time = "{" + DateTime.Now.ToString("HH:mm:ss tt") + "} ";
            Invoke(new MethodInvoker(delegate
            {
                switch (type)
                {
                    case Enums.LogsType.Compile:
                        TextToLogs(time + str + Environment.NewLine, "[Compile] ", Color.DodgerBlue);
                        break;

                    case Enums.LogsType.Success:
                        TextToLogs(time + str + Environment.NewLine, "[Success] ", Color.Green);
                        break;

                    case Enums.LogsType.Warning:
                        TextToLogs(time + str + Environment.NewLine, "[Warning] ", Color.Orange);
                        break;

                    case Enums.LogsType.Error:
                        TextToLogs(time + str + Environment.NewLine, $"[Error:{errorNum}] ", Color.Red);
                        break;

                    case Enums.LogsType.Logs:
                        TextToLogs(time + str + Environment.NewLine, "[Logs] ", Color.MediumPurple);
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
                    _mySettings.chkLogsComplie = chkLogsComplie.Checked;
                    _mySettings.chkLogsSuccess = chkLogsSuccess.Checked;
                    _mySettings.chkLogsError = chkLogsError.Checked;
                    _mySettings.chkLogsWarning = chkLogsWarning.Checked;
                    _mySettings.chkSound = chkSound.Checked;
                    _mySettings.chkCheckUpdate = chkCheckUpdate.Checked;
                    _mySettings.Save();
                    WriteOutput("Saved Settings", Enums.LogsType.Success);
                }
            }
            catch (Exception exception)
            {
                WriteOutput(exception.Message, Enums.LogsType.Error, "027");
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
                    WriteOutput("Ndk path must without any special character", Enums.LogsType.Error, "028");
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

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteOutput($"{ex.Message}", Enums.LogsType.Error, "000");
            }
            FormState(State.Idle);
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

        #region Image Encoder

        //https://stackoverflow.com/a/45673201/8902883
        private static string ImageToBase64(Image image, ImageFormat imageFormat)
        {
            using var ms = new MemoryStream();
            image.Save(ms, imageFormat);
            var imageBytes = ms.ToArray();
            return Convert.ToBase64String(imageBytes);
        }

        private static Image Base64ToImage(string base64String)
        {
            var imageBytes = Convert.FromBase64String(base64String);
            using var ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            return Image.FromStream(ms, true);
        }

        private static string GetMimeType(string fileName)
        {
            var mimeType = "application/unknown";
            var ext = Path.GetExtension(fileName).ToLower();
            var regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey?.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        //https://stackoverflow.com/a/24651073/8902883
        private static Image CompressImage(string fileName, int newQuality)
        {
            using var image = new Bitmap(fileName);
            using var memImage = new Bitmap(image);

            var myEncoderParameters = new EncoderParameters(1)
            {
                Param = { [0] = new EncoderParameter(Encoder.Quality, newQuality) }
            };

            var memStream = new MemoryStream();
            memImage.Save(memStream, GetEncoderInfo(GetMimeType(fileName)), myEncoderParameters);
            var newImage = Image.FromStream(memStream);
            var imageAttributes = new ImageAttributes();
            using var g = Graphics.FromImage(newImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(newImage, new Rectangle(Point.Empty, newImage.Size), 0, 0, newImage.Width, newImage.Height, GraphicsUnit.Pixel, imageAttributes);
            return newImage;
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            var encoders = ImageCodecInfo.GetImageEncoders();
            return Array.Find(encoders, ici => ici.MimeType == mimeType);
        }

        #endregion Image Encoder

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
                WriteOutput(ex.Message, Enums.LogsType.Error, "029");
                FormState(State.Idle);
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
                WriteOutput(ex.Message, Enums.LogsType.Error, "030");
                FormState(State.Idle);
                return false;
            }
        }

        private bool MoveDirectory(string sourceDirectory, string destinationPath, bool overwrite = true, bool deleteSource = true)
        {
            try
            {
                if (Directory.Exists(destinationPath) && overwrite &&
                    MyMessage.MsgOkCancel(destinationPath + " Found.\n\n" +
                                                         "Click \"OK\" to Continue if you want to overwrite!" +
                                                         "\n\nClick \"Cancel\" to cancel it if not!"))
                {
                    Directory.Delete(destinationPath, true);
                }

                Utility.CheckFolder("Output\\" + txtNameGame.Text);

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

                return false;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput(ex.Message, Enums.LogsType.Error, "031");
                FormState(State.Idle);
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
                WriteOutput(ex.Message, Enums.LogsType.Error, "032");
                FormState(State.Idle);
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