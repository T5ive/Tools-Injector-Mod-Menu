using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools_Injector_Mod_Menu.Patch_Manager;

namespace Tools_Injector_Mod_Menu
{
    public partial class FrmMain
    {
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

            if (_apkTarget != TEMP_APK_TARGET)
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
            var openFile = new OpenFileDialog
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
                _apkName = apkTarget;
                _apkType = Path.GetExtension(_apkName);
                txtApkTarget.Text = apkTarget;
                if (!re)
                {
                    WriteOutput($"Set Apk Target: {apkTarget}", Enums.LogsType.Success);
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
                        entryApks.ExtractToFile(TEMP_APK_TARGET, true);
                        _apkTarget = TEMP_APK_TARGET;
                    }

                    if (type == (int)Enums.TypeAbi.Arm)
                    {
                        if (entryApks.FullName == "split_config.armeabi_v7a.apk")
                        {
                            entryApks.ExtractToFile(Path.Combine(TEMP_PATH_T_FIVE, entryApks.FullName), true);
                        }
                    }
                    else if (entryApks.FullName == "split_config.arm64_v8a.apk")
                    {
                        entryApks.ExtractToFile(Path.Combine(TEMP_PATH_T_FIVE, entryApks.FullName), true);
                    }
                }
                archive.Dispose();
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
                    else if (entryApks.FullName == "config.arm64_v8a.apk")
                    {
                        entryApks.ExtractToFile(Path.Combine(TEMP_PATH_T_FIVE, entryApks.FullName), true);
                    }

                    var apkFile = Path.Combine(Path.GetTempPath(), entryApks.FullName);
                    using var entryBase = ZipFile.OpenRead(apkFile);
                    var classes = entryBase.Entries.FirstOrDefault(f => f.Name.Contains("classes.dex"));
                    if (classes != null)
                    {
                        entryApks.ExtractToFile(TEMP_APK_TARGET, true);
                        _apkTarget = TEMP_APK_TARGET;
                        _baseName = entryApks.FullName;
                    }
                    entryBase.Dispose();
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
                WriteOutput($"{TEMP_APK_TARGET} Not found", Enums.LogsType.Error, "016");
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

        private void DumpWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcessRun($"/c aapt dump badging \"{_apkTarget}\" PAUSE > \"{TEMP_PATH_T_FIVE}\\result.txt\"",
                BUILD_TOOL_PATH, "202");
        }

        private void DumpWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            WriteOutput("Dump successful", Enums.LogsType.Success);

            if (_errorCount > 0 && !_mySettings.debugMode)
            {
                MyMessage.MsgShowError("Failed to Dump");
                WriteOutput("Failed to Dump", Enums.LogsType.Error, "031");
                SaveLogs();
            }

            if (_type is not (Enums.ProcessType.ApkFull1Decompile or Enums.ProcessType.ApkFull2Decompile))
            {
                FormState(State.Idle);
            }
            
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
                ChangeTab(3);
                DecompileWorker.RunWorkerAsync();
            }
            catch (Exception exception)
            {
                WriteOutput(exception.Message, Enums.LogsType.Error, "020");
            }
        }

        private void DecompileWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcessRun($"/c java -jar {_apkTool}.jar d -f --only-main-classes -o \"{APK_DECOMPILED_PATH}\" \"{_apkTarget}\"", BUILD_TOOL_PATH, "203");
        }

        private void DecompileWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
                archive.Dispose();
            }

            DeleteDecompiledOtherABI();
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

        private void DeleteDecompiledOtherABI()
        {
            if (!_mySettings.chkRemoveOther) return;
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
            ChangeTab(3);
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
            if (!MoveDirectory(TEMP_PATH_T_FIVE + "\\com", $"{destinationPath}\\smali\\com", true, true))
            {
                FormState(State.Idle);
                return false;
            }
            return true;
        }

        private void MenuWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcessRun($"/c {_mySettings.txtNDK}\\build\\ndk-build", $"{TEMP_PATH_T_FIVE}\\jni", "201");
        }

        private void MenuWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CompileMenuDone();

            if (_type is Enums.ProcessType.MenuFull)
            {
                FormState(State.Idle);
                ProcessType(Enums.ProcessType.None);
                if (_mySettings.chkOpenOutput)
                {
                    Process.Start(_outputDir);
                }
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
                return;
            }

            var tempOutputDir = $"{TEMP_PATH_T_FIVE}\\libs";

            MoveDirectory(tempOutputDir, _outputLibDir);
            if (_mySettings.chkMergeApk && _apkType is ".apks" or ".xapk")
            {
                var folderName = comboType.SelectedIndex == (int)Enums.TypeAbi.Arm ? "\\armeabi-v7a" : "\\arm64-v8a";
                MoveDirectory($"{TEMP_PATH_T_FIVE}\\lib\\", _outputLibDir + folderName, false);
            }

            if (_type == Enums.ProcessType.MenuFull)
            {
                DeleteTemp();
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
                text = _mySettings.chkNoMenu ?
                    text.Replace(@"return env->NewStringUTF(OBFUSCATE(""(yourImage)""));", "return NULL;")
                    : text.Replace("(yourImage)", ImageCode);
                text = _mySettings.chkTFiveCredit ? text.Replace("//(TFiveEndCredit)", @"OBFUSCATE(""0_RichWebView_<html><body><marquee style=\""color: white; font-weight:bold;\"" direction=\""left\"" scrollamount=\""5\"" behavior=\""scroll\"">TFive Tools</marquee></body></html>"")") : text;
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
                for (var i = 1; !File.Exists(launch); i++)
                {
                    launch = $"{APK_DECOMPILED_PATH}\\{Utility.SmaliCountToName(_smaliCount - i)}\\" + _launch.Replace(".", "\\") + ".smali";
                }
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
            var path = _outputDir + txtNameGame.Text;
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
                var smaliSource = $"{_outputSmaliDir}com";
                var smaliDes = $"{APK_DECOMPILED_PATH}\\{Utility.SmaliCountToName(_smaliCount, true)}\\com";
                if (!MoveDirectory(smaliSource, smaliDes, false))
                {
                    WriteOutput($"Cannot Move {smaliSource}\nTo => {smaliDes}", Enums.LogsType.Error, "021");
                    return;
                }

                if (_apkType == ".apk" || _mySettings.chkMergeApk)
                {
                    var folderName = comboType.SelectedIndex == (int)Enums.TypeAbi.Arm ? "armeabi-v7a" : "arm64-v8a";
                    var libSource = _outputLibDir + folderName;
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

        private void CompileWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcessRun($"/c java -jar {_apkTool}.jar b -f -o \"{_outputApk}\" \"{APK_DECOMPILED_PATH}\"", BUILD_TOOL_PATH, "204");
        }

        private void CompileWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

        private void SignWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!_compiled)
            {
            }

            _compiled = false;
            WriteOutput($"Compiled {_outputApk}", Enums.LogsType.Success);

            ProcessRun($"/c java -jar ApkSigner.jar sign --key \"{BUILD_TOOL_PATH}tfive.pk8\" --cert \"{BUILD_TOOL_PATH}tfive.pem\" --v4-signing-enabled false --out \"{_outputApkSign}\" \"{_outputApk}\"", BUILD_TOOL_PATH, "205");
        }

        private void SignWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            WriteOutput($"Signed {_outputApkSign}", Enums.LogsType.Success);

            if (_apkType != ".apk" && !_mySettings.chkMergeApk)
            {
                ArchiveApk();
            }

            try
            {
                Directory.Delete(_outputLibDir, true);
                Directory.Delete(_outputSmaliDir, true);
                DeleteTemp();
            }
            catch
            {
                //
            }

            FormState(State.Idle);
            ProcessType(Enums.ProcessType.None);
            if (_mySettings.chkOpenOutput)
            {
                Process.Start(_outputDir);
            }
        }

        #endregion Sign Apk

        #region Merge Apk

        private void ArchiveApk()
        {
            Lib2Config();
            Apk2Apks();
        }

        private void Lib2Config()
        {
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
            var outputConfig = _outputDir + configName;

            File.Copy(_apkName, _outputApkName, true);
            File.Copy(_apkName, _outputApkNameSign, true);
            File.Copy(TEMP_PATH_T_FIVE + "\\" + configName, outputConfig, true);

            var folderName = comboType.SelectedIndex == (int)Enums.TypeAbi.Arm ? "armeabi-v7a\\" : "arm64-v8a\\";

            DirectoryInfo dir = new(_outputLibDir + folderName);

            using var zipToOpen = new FileStream(outputConfig, FileMode.Open);
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
            var unsignList = new List<(string, string)> { (_outputApk, baseName), (_outputDir + configName, configName) };
            var signedList = new List<(string, string)> { (_outputApkSign, baseName), (_outputDir + configName, configName) };
            _outputApkName.AddFiles(unsignList);
            _outputApkNameSign.AddFiles(signedList);
            File.Delete(_outputDir + configName);
            File.Delete(_outputApk);
            File.Delete(_outputApkSign);
        }

        #endregion

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
            _outputDir = $"{AppPath}\\Output\\{txtNameGame.Text}\\";
            _outputLibDir = $"{AppPath}\\Output\\{txtNameGame.Text}\\lib\\";
            _outputSmaliDir = $"{AppPath}\\Output\\{txtNameGame.Text}\\smali\\";
            _outputApk = $"{_outputDir}{txtNameGame.Text}.apk";
            _outputApkSign = $"{_outputDir}{txtNameGame.Text}-Signed.apk";

            _outputApkName = _outputDir + txtNameGame.Text + _apkType;
            _outputApkNameSign = _outputDir + txtNameGame.Text + "-Signed" + _apkType;
        }

        private bool CheckEmpty()
        {
            if (txtLibName.IsEmpty())
            {
                MyMessage.MsgShowWarning("Library Name is Empty, Please Check it again!!!");
                ChangeTab(0);
                WriteOutput("Library Name is Empty", Enums.LogsType.Warning);
                return false;
            }
            if (txtNDK.IsEmpty())
            {
                MyMessage.MsgShowWarning("NDK Path is Empty, Please Check it again!!!");
                ChangeTab(5);
                WriteOutput("NDK Path is Empty", Enums.LogsType.Warning);
                return false;
            }

            if (!_mySettings.chkNoMenu && ImageCode.IsEmpty())
            {
                MyMessage.MsgShowWarning("Image Code is Empty, Please Check it again!!!");
                ChangeTab(0);
                WriteOutput("Image Code is Empty", Enums.LogsType.Warning);
                return false;
            }
            if (txtNameGame.IsEmpty())
            {
                MyMessage.MsgShowWarning("Name Game is Empty, Please Check it again!!!");
                ChangeTab(1);
                WriteOutput("Name Game is Empty", Enums.LogsType.Warning);
                return false;
            }
            if (txtTargetLib.IsEmpty())
            {
                MyMessage.MsgShowWarning("Target Library Name is Empty, Please Check it again!!!");
                ChangeTab(1);
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
                ChangeTab(1);
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
                    Directory.Delete(destinationPath, true);
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

        private void DeleteTemp()
        {
            if (_mySettings.chkRemoveTemp)
            {
                DeleteAll(TEMP_PATH_T_FIVE);
            }
        }

        private static void ProcessType(Enums.ProcessType type)
        {
            _type = type;
        }

        #endregion Utility
    }
}