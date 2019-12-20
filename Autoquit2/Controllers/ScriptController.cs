using Autoquit2.Models;
using Autoquit2.Services;
using HttpService;
using HttpService.Core;
using HttpService.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinAPI;

namespace Autoquit2.Controllers {
    [Route("script")]
    public class ScriptController: Controller {
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
                dialog.Filter = "Autoquit Script Files (*.script)|*.script";
                var result = dialog.ShowDialog();
                if ( result == DialogResult.OK ) {
                    path = dialog.FileName;
                }
            }));
            if ( path != null ) {
                Script script = Compressor.ReadObject<Script>(path);
                if (script.Scripts.Count>0 && 
                    script.Scripts
                        .FirstOrDefault(x=>x.EventType.StartsWith("LEFT") || x.EventType.StartsWith("RIGHT")) !=null)
                    Migrator.MigrateScript(ref script);
                return Response.WithSuccess(new { path, script });
            }
            return Response.NoContent;
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
