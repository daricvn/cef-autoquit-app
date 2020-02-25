using Autoquit2._App_Data;
using Autoquit2.Models;
using Autoquit2.Services;
using Autoquit2.Views;
using CefCore;
using HttpService;
using HttpService.Core;
using HttpService.Interfaces;
using HttpService.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinAPI;
using WinAPI.Handler;
using WinAPI.Hook;
using WinAPI.Utility;

namespace Autoquit2.Controllers {
    [Route("script")]
    public class ScriptController: Controller {
        private const string ScriptFileExtension = "Autoquit Script Files (*.script)|*.script";
        private readonly Random rnd;
        private readonly Dictionary<string, string> fileCache;
        public ScriptController() {
            rnd = new Random();
            fileCache = new Dictionary<string, string>();
        }

        [Get("processes")]
        public IResponse GetProcess(bool deep=false) {
            var notepad = Process.GetProcessesByName("notepad")?.AsEnumerable();
            var procs=Process.GetProcesses().Concat(notepad).ToList();
            procs.Sort(( a, b ) => string.Compare(a.ProcessName, b.ProcessName));
            HashSet<AppProcess> result = new HashSet<AppProcess>();
            foreach ( Process p in procs ) {
                try {
                    if ( !p.HasExited && (deep || p.MainWindowHandle != IntPtr.Zero)
                    && p.Id != Process.GetCurrentProcess().Id ) {
                        AppProcess process = new AppProcess();
                        Icon icon = null;
                        try {
                            icon = Icon.ExtractAssociatedIcon(p.MainModule.FileName);
                            process.Icon = string.Concat("data:image/png;base64,", Convert.ToBase64String(GetPngFromIcon(icon)));
                        }
                        catch ( Exception e ) {
                        }
                        string name = !string.IsNullOrEmpty(p.MainWindowTitle) ? p.MainWindowTitle : p.ProcessName;
                        process.FileName = p.ProcessName;
                        process.Pid = p.Id;
                        process.Name = string.Format("{0} ({1})", name, p.Id);
                        result.Add(process);
                    }
                }
                catch ( Exception e ) { }
            }
            return Response.WithSuccess(result);
        }

        [Get("process/details")]
        public IResponse GetChildHandle(long pid) {
            try {
                var process = Process.GetProcessById((int)pid);
                var childs = WinProcess.GetAllChildHandles(process.MainWindowHandle);
                List<AppProcess> result = new List<AppProcess>(childs.Count + 1);
                result.Add(new AppProcess() {
                    Pid = (int)process.MainWindowHandle,
                    FileName = process.ProcessName,
                    Name = process.ProcessName
                });
                foreach ( var item in childs )
                    if ( item != IntPtr.Zero ) {
                        var proc = new AppProcess();
                        proc.Name = string.Format("{0} ({1})", WinProcess.GetWindowName(item), item);
                        proc.Pid = (int)item;
                        proc.FileName = process.ProcessName;
                        try {
                            var icon = Icon.ExtractAssociatedIcon(process.MainModule.FileName);
                            proc.Icon = string.Concat("data:image/png;base64,", Convert.ToBase64String(GetPngFromIcon(icon)));
                        }
                        catch ( Exception e ) {
                        }
                        result.Add(proc);
                    }   
                return Response.WithSuccess(result);
            }
            catch ( Exception e ) { 
            }
            return Response.BadRequest;
        }

        [Put("process/top")]
        public IResponse BringTop( int pid ) {
            try {
                var proc = Process.GetProcessById(pid);
                if ( proc != null && !proc.HasExited) {
                    WinProcess.BringProcessToFront(proc);
                    return Response.Success;
                }
            }
            catch (Exception e ) {

            }
            return Response.BadRequest;
        }

        [Get("browse")]
        public IResponse Browse() {
            string path = null;
            Master.Form.Invoke((MethodInvoker)(()=>{
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.CheckFileExists = true;
                dialog.Multiselect = false;
                dialog.RestoreDirectory = true;
                dialog.Filter = ScriptFileExtension;
                var result = dialog.ShowDialog();
                if ( result == DialogResult.OK ) {
                    path = dialog.FileName;
                }
            }));
            if ( path != null ) {
                Script script = Compressor.ReadObject<Script>(path);
                fileCache.Clear();
                if (script.Version == null || script.Version < Constant.Build || (
                    script.Scripts.Count>0 && 
                    script.Scripts
                        .FirstOrDefault(x=>x.EventType.StartsWith("LEFT") || x.EventType.StartsWith("RIGHT")) !=null))
                    Migrator.MigrateScript(ref script);
                return Response.WithSuccess(new { path, script });
            }
            return Response.NoContent;
        }

        [Post("save")]
        public IResponse Save(string path, Script script) {
            if (script != null ) {
                if ( string.IsNullOrEmpty(path) ) {
                    return this.SaveAs(script);
                }
                if (script.Version == null || script.Version < Constant.Build) {
                    var result = MessageBox.Show(Program.GetString("confirm_override_old_script"), Program.GetString("title_override_old_script"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No ) {
                        return Response.NotModified;
                    }
                    Migrator.MigrateScript(ref script);
                }
                script.Version = Constant.Build;
                Compressor.WriteObject(path, JsonConvert.SerializeObject(script));
                return Response.Success;
            }
            return Response.BadRequest;
        }
        [Post("saveas")]
        public IResponse SaveAs( Script script ) {
            if ( script != null ) {
                if ( script.Version == null || script.Version < Constant.Build ) {
                    Migrator.MigrateScript(ref script);
                }
                script.Version = Constant.Build;
                string fileName = null;
                Master.Form.Invoke((MethodInvoker)(() => {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = ScriptFileExtension;
                    dialog.CheckPathExists = true;
                    dialog.DefaultExt = ".script";
                    var result = dialog.ShowDialog();
                    if ( result == DialogResult.OK ) {
                        fileName = dialog.FileName;
                    }
                }));
                if (!string.IsNullOrEmpty(fileName)) {
                    Compressor.WriteObject(fileName, JsonConvert.SerializeObject(script));
                    return Response.WithSuccess(fileName);
                }
                return Response.NotModified;
            }
            return Response.BadRequest;
        }
        [Post("record")]
        public IResponse StartRecord(int pid) {
            var process = pid == 0 ? null : Process.GetProcessById(pid);
            if ( process == null && pid != 0)
                return Response.NotFound;
            else {
                BringTop(pid);  
            }
            var lastOperation = DateTime.Now;
            Recorder.Start(result => {
                var item = new ScriptItem();
                item.Active = true;
                item.TimeOffset = (long) Math.Round(DateTime.Now.ToUnixTimestamp() - lastOperation.ToUnixTimestamp());
                lastOperation = DateTime.Now;
                if (result is MouseHookEventArgs) {
                    var mouseArgs = (MouseHookEventArgs)result;
                    var name = Enum.GetName(typeof(MouseKey), mouseArgs.Key);
                    if ( name.EndsWith("_CLICK") ) {
                        item.EventType= Constant.ENV_MOUSE_CLICK;
                    }
                    if ( name.EndsWith("_DBCLICK") )
                        item.EventType = Constant.ENV_MOUSE_DBCLICK;
                    if ( name.EndsWith("_UP") )
                        item.EventType = Constant.ENV_MOUSE_UP;
                    if ( name.EndsWith("_DOWN") )
                        item.EventType = Constant.ENV_MOUSE_DOWN;
                    item.KeyName = name[0].ToString().ToUpper();
                    if (process!=null) {
                        var coord = WinAPI.Core.RetrieveCoord(process.MainWindowHandle).Split(':');
                        item.Coord = new Coord(int.Parse(coord[0]), int.Parse(coord[1]));
                    }
                    else {
                        var point = MouseHook.GetCursorPosition();
                        item.Coord = new Coord(point.x, point.y);
                    }
                }
                if (result is KeyHookEventArgs ) {
                    var keyArgs = (KeyHookEventArgs)result;
                    var name = Enum.GetName(typeof(WinAPI.Handler.KeyType), keyArgs.Type);
                    item.EventType = name;
                    var keyCode = ChromiumKeyConverter.GetKeyCode((System.Windows.Forms.Keys)Enum.ToObject(typeof(System.Windows.Forms.Keys), (int)keyArgs.Key));
                    item.KeyName = Enum.GetName(typeof(KeyCode), keyCode);
                }
                if ( !string.IsNullOrEmpty(item.EventType) ) {
                    Chromium.RunScript($"window.addItem('{item.ToJson()}')");
                }
            });
            return Response.Success;
        }
        [Post("stoprecord")]
        public IResponse StopRecord() {
            Recorder.Stop();
            return Response.Success;
        }

        [Post("getcoord")]
        public IResponse GetMouseCoord(int pid) {
            if ( MouseCoord.Instance == null ) {
                var form = new MouseCoord();

                var process = pid == 0 ? null : Process.GetProcessById(pid);
                form.Show(process?.MainWindowHandle ?? IntPtr.Zero);
                return Response.WithSuccess(new {
                    x = form.LastX,
                    y = form.LastY
                });
            }
            else {
                var form = (MouseCoord)MouseCoord.Instance;
                return Response.WithSuccess(new {
                    x = form.LastX,
                    y = form.LastY
                });
            }
        }

        [Post("play")]
        public IResponse Run(IntPtr[] pids, ScriptItem item ) {
            try {
                foreach ( var pid in pids ) {
                    if ( IsMouseEvent(item.EventType) ) {
                        var mouseKey = MouseKey.NONE;
                        var strName = "";
                        if ( item.KeyName.StartsWith("R") )
                            strName = "RIGHT";
                        if ( item.KeyName.StartsWith("M") )
                            strName = "MIDDLE";
                        if ( item.KeyName.StartsWith("L") )
                            strName = "LEFT";
                        var index = item.EventType.IndexOf("_") + 1;
                        Enum.TryParse($"{strName}_{item.EventType.Substring(index)}", out mouseKey);
                        if ( mouseKey != MouseKey.NONE && item.Coord != null ) {
                            if ( item.SendInput )
                                Recorder.SendMouse(pid, mouseKey, item.Coord.X, item.Coord.Y);
                            else
                                WinAPI.Core.SendMouseEx(pid, mouseKey, item.Coord.X, item.Coord.Y);
                        }
                    }
                    else if ( item.EventType.StartsWith("KEY") ) {
                        if ( Enum.TryParse(item.KeyName, out KeyCode keyCode) && Enum.TryParse(item.EventType, out KeyType keyType) ) {
                            var key = ChromiumKeyConverter.GetKey(item.KeyName);
                            SendKey(pid, keyType, key, item.SendInput);
                        }
                    }
                    else if ( item.EventType == Constant.ENV_ENTER_TEXT || item.EventType == Constant.ENV_ENTER_SECRET
                        || item.EventType == Constant.ENV_RANDOM_TEXT || item.EventType == Constant.ENV_FROM_FILE ) {
                        var keyName = item.KeyName;
                        if ( item.EventType == Constant.ENV_RANDOM_TEXT && keyName.Contains(";") ) {
                            var keys = keyName.Split(';');
                            if ( keys.Length == 1 )
                                keyName = keys[0];
                            else {
                                keyName = keys[rnd.Next(0, keys.Length - 1)];
                            }
                        }
                        else if (item.EventType == Constant.ENV_FROM_FILE ) {
                            keyName = "";
                            if ( !fileCache.ContainsKey(item.KeyName)  ) {
                                if ( File.Exists(item.KeyName) ) {
                                    keyName = File.ReadAllText(item.KeyName);
                                    fileCache.Add(item.KeyName, keyName);
                                }
                            }
                            else {
                                keyName = fileCache[item.KeyName];
                            }
                        }
                        Task.Run(async () => {
                            int wordCount = 0;
                            foreach ( char c in keyName ) {
                                WinAPI.Keys key;
                                if ( (key = KeyMapper.Get(c, out bool shift)) != WinAPI.Keys.None ) {
                                    wordCount++;
                                    if ( shift && item.SendInput )
                                        SendKey(pid, KeyType.KEY_DOWN, System.Windows.Forms.Keys.Shift, true);
                                    SendKey(pid, KeyType.KEY_PRESS, key.ToWindowsKey(), item.SendInput);
                                    if ( shift && item.SendInput )
                                        SendKey(pid, KeyType.KEY_UP, System.Windows.Forms.Keys.Shift, true);
                                    if ( Program.Settings.TypeSpeed > 0 && wordCount >= 20 ) {
                                        wordCount = 0;
                                        await Task.Delay(Program.Settings.TypeSpeed / 2);
                                    }
                                }
                            }
                        }).Wait();
                    }
                }
                return Response.Success;
            }
            catch (Exception e ) {
                return Response.WithError(500, e.Message);
            }
        }

        private void SendKey(IntPtr pid,KeyType keyType, System.Windows.Forms.Keys key, bool sendInput) {
            if ( !sendInput ) {
                WinAPI.Core.SendKeyboard(pid, keyType, (WinAPI.Keys)Enum.ToObject(typeof(WinAPI.Keys), (int)key));
            }
            else {
                Recorder.SendKeys(keyType, key);
            }
        }

        private bool IsMouseEvent(string text ) {
            return text.Contains("CLICK") 
                || (!text.Contains("KEY") && (text.Contains("UP") || text.Contains("DOWN")));
        }

        private byte[] GetPngFromIcon(Icon icon ) {
            using (var bmp= icon.ToBitmap() )
            using (var ms= new MemoryStream() ) {
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private byte[] GetBytesFromIcon(Icon icon ) {
            using (var ms= new MemoryStream() ) {
                icon.Save(ms);
                return ms.ToArray();
            }
        }
    }
}
