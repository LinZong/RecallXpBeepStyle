using System;
using System.IO;
using System.Windows;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace EventExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int ClickCounter = 0;
        public MainWindow()
        {
            InitializeComponent();
            CheckDriverStatus();
        }

        public string ExecuteCMD(string ExecuteCommand)
        {

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c C:\\Windows\\System32\\cmd.exe";
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.Verb = "RunAs";
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.StandardInput.WriteLine(ExecuteCommand+" &exit");
            process.StandardInput.AutoFlush = true;
            string strRst = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Close();
            return strRst;
            
        }
        public bool FileExists(string path)
        {
            if (File.Exists(path)) return true;
            IntPtr oldValue = IntPtr.Zero;
            try
            {
                if (Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432") == null)
                    return false;

                Wow64DisableWow64FsRedirection(ref oldValue);
                if (File.Exists(path)) return true;

                return false;
            }
            finally
            {

            }
        }

        public void CheckDriverStatus()
        {
            
            string BeepDriverPath = Environment.GetFolderPath(Environment.SpecialFolder.System)+@"\drivers\beepxp.sys";
            if (FileExists(BeepDriverPath))
            {
                DriverStatus.Content = "看上去你的电脑已经安装BeepXP驱动.";
                InstallDrv.IsEnabled = false;
            }
            else
            {
                DriverStatus.Content = "未检测到BeepXP驱动.";
                UninstDrv.IsEnabled = false;
            }
        }

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern void Beep(int dwFreq, int dwDuration);
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr oldValue);

        private void TestABeep(object sender, RoutedEventArgs e)
        {
            Beep(800, 1000);
        }

        private void InstallADrv(object sender, RoutedEventArgs e)
        {
            this.InstallDrv.Content = "装个驱动!";
            if (ClickCounter == 1)
            {
                this.DriverStatus.Content = "正在装驱动!";
                var process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/c C:\\Windows\\System32\\InfDefaultInstall.exe " + Environment.CurrentDirectory + "\\beepxp.inf"; // where driverPath is path of .inf file
                process.Start();
                process.WaitForExit();
                process.Dispose();
                this.CMDLINE.Content = ExecuteCMD("sc config Beep start= demand");
                MessageBox.Show("驱动应该装完了,重启电脑试试吧!");
                
            }
            ClickCounter++;
        }

        private void UninstallADrv(object sender, RoutedEventArgs e)
        {
            string DriverPath = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\drivers\beepxp.sys";
            this.CMDLINE.Content = ExecuteCMD("RUNDLL32.EXE SETUPAPI.DLL,InstallHinfSection DefaultUninstall 132 "+Environment.CurrentDirectory+"\\beepxp.inf && sc config Beep start= auto");
            
            this.DriverStatus.Content = "看到2个SUCCESS就可以重启系统了";
        }
    }
}
