using MaterialSkin;
using MaterialSkin.Controls;
using ModernFolderBrowserDialog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Tools_Injector_Mod_Menu.Patch_Manager;

namespace Tools_Injector_Mod_Menu
{
    //Fix button hook - unsolvable
    //Add String https://github.com/geokar2006/il2cpp_mono_string/
    public partial class FrmMain : MaterialForm
    {
        public FrmMain()
        {
            InitializeComponent();
            LoadTheme();
        }

        #region Variable

        public static readonly string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static readonly string BUILD_TOOL_PATH = $"{AppPath}\\BuildTools\\";

        public static string ImageCode;

        private static MySettings _mySettings = new();

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
            chkOpenOutput.Checked = _mySettings.chkOpenOutput;
            chkRemoveOther.Checked = _mySettings.chkRemoveOther;

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
                if (_mySettings.chkCheckUpdate)
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
            var openFile = new OpenFileDialog
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
                Filter = "XML|*.xml|All files|*.*",
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
            var openFile = new OpenFileDialog
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
                        TextToLogs(time + str + Environment.NewLine, $"[Error:{errorNum}] ", Color.Red);
                        ErrorBreaker();
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
            SystemSounds.Beep.Play();
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
                    _mySettings.chkOpenOutput = chkOpenOutput.Checked;
                    _mySettings.chkRemoveOther = chkRemoveOther.Checked;
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
                            ChangeTab(2);
                            break;

                        case ".xml":
                            LoadXml(file);
                            ChangeTab(1);
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

        private bool MoveDirectory(string sourceDirectory, string destinationPath, bool overwrite = true, bool deleteSource = false)
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

        private void ErrorBreaker()
        {
            DumpWorker.CancelAsync();
            DecompileWorker.CancelAsync();
            MenuWorker.CancelAsync();
            CompileWorker.CancelAsync();
            SignWorker.CancelAsync();
            SaveLogs();
            FormState(State.Idle);
        }

        private void ChangeTab(int page)
        {
            materialTabControl1.SelectedTab = materialTabControl1.TabPages[page];
        }

        private void FormState(State state)
        {
            EnableController(this, state != State.Running);
            if (state == State.Idle && _mySettings.chkSound)
            {
                SystemSounds.Beep.Play();
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