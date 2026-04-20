using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace frmcocuni
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //run timer - chạy đồng hồ
            timer1.Start();
            //check if coccoc is installed through registry - kiểm tra xem coccoc đã được cài đặt chưa thông qua registry

            if (IsCocCocInstalled())
            {
                lbldetect.Text = "Coccoc is installed on this computer.";
                btnremove.Enabled = true;
            }
            else
            {
                lbldetect.Text = "Coccoc is not installed on this computer.";
                btnremove.Enabled = false;
                btnremove1.Enabled = true;
            }

            string arch = RuntimeInformation.OSArchitecture.ToString();
            string osName = GetWindowsName();
            lblos.Text = $"{osName} - {arch}";

        }


        public bool IsCocCocInstalled()
        {
            string[] registryPaths =
            [
                @"Software\Microsoft\Windows\CurrentVersion\Uninstall\CocCocBrowser",
                @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\CocCocBrowser"
            ];

            // Kiểm tra HKEY_CURRENT_USER
            foreach (var path in registryPaths)
            {
                using RegistryKey? key = Registry.CurrentUser.OpenSubKey(path);
                var uninstallString = key?.GetValue("UninstallString");
                if (uninstallString != null)
                {
                    txtdetail.Text = uninstallString.ToString() ?? "";
                    return true;
                }
            }

            // Kiểm tra HKEY_LOCAL_MACHINE
            string hklmPath = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\CocCocBrowser";
            using RegistryKey? hklmKey = Registry.LocalMachine.OpenSubKey(hklmPath);
            var hklmUninstallString = hklmKey?.GetValue("UninstallString");
            if (hklmUninstallString != null)
            {
                txtdetail.Text = hklmUninstallString.ToString() ?? "";
                return true;
            }

            txtdetail.Text = "";
            return false;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.Text = $"Coccoc Uninstaller Tool - {DateTime.Now:hh:mm:ss tt}";
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            //chạy lệnh cmd ngầm để gỡ cài đặt coccoc - run hidden cmd command to uninstall coccoc
            try
            {
                if (string.IsNullOrEmpty(txtdetail.Text))
                {
                    MessageBox.Show("CocCoc installation path not found.");
                    return;
                }

                btnremove.Enabled = false;
                var setupExePath = txtdetail.Text;

                var processInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", $"/c \"{setupExePath}\" --uninstall --force-uninstall --multi-install --noshow-uninstall-survey")
                {
                    UseShellExecute = false,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                };

                using var process = System.Diagnostics.Process.Start(processInfo);
                process?.WaitForExit();

                if (!IsCocCocInstalled())
                {
                    MessageBox.Show("Coccoc has been uninstalled successfully.");
                    lbldetect.Text = "Coccoc is not installed on this computer.";
                    btnremove1.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Coccoc is still installed. Uninstall may have failed.");
                    btnremove.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to uninstall Coccoc: {ex.Message}");
                btnremove.Enabled = true;
            }
        }

        private void BtnRemove1_Click(object sender, EventArgs e)
        {
            //xóa tàn dư của cốc trong toàn bộ registry, services, ổ hệ thống
            DialogResult result = MessageBox.Show("This will remove all CocCoc remnants from your system. Continue?", "Remove CocCoc Remnants", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    btnremove1.Enabled = false;

                    // 0. Kill tất cả process CocCoc
                    KillCocCocProcesses();

                    // 1. Scan và xóa tàn dư trong Registry
                    RemoveAllRegistryTraces();

                    // 2. Dừng và xóa Services
                    RemoveServices();

                    // 3. Scan toàn hệ thống và xóa CocCoc files/folders
                    RemoveAllCocCocFiles();

                    MessageBox.Show("CocCoc remnants have been removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnremove1.Enabled = true;
                }
            }
        }

        private static void RemoveAllRegistryTraces()
        {
            // Scan các hive quan trọng để tìm và xóa mọi thứ liên quan CocCoc
            RegistryKey[] hivesToScan =
            [
                Registry.CurrentUser,
                Registry.LocalMachine,
                Registry.ClassesRoot
            ];

            string[] rootPaths =
            [
                @"Software",
                @"SOFTWARE",
                @"SOFTWARE\Wow6432Node",
                @"Software\Classes"
            ];

            foreach (var hive in hivesToScan)
            {
                foreach (var rootPath in rootPaths)
                {
                    try
                    {
                        using RegistryKey? root = hive.OpenSubKey(rootPath, true);
                        if (root != null)
                            ScanAndRemoveRegistryByKeyword(root, "CocCoc");
                    }
                    catch { }
                }

                // Xóa MuiCache values chứa CocCoc
                RemoveMuiCacheValues(hive);
            }
        }

        private static void ScanAndRemoveRegistryByKeyword(RegistryKey key, string keyword)
        {
            // Lấy danh sách subkeys trước để tránh collection-modified error
            string[] subKeyNames;
            try { subKeyNames = key.GetSubKeyNames(); }
            catch { return; }

            foreach (var subKeyName in subKeyNames)
            {
                // Nếu tên key chứa keyword → xóa toàn bộ key đó
                if (subKeyName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    try { key.DeleteSubKeyTree(subKeyName, false); }
                    catch { }
                    continue;
                }

                // Recursive vào subkey
                try
                {
                    using RegistryKey? subKey = key.OpenSubKey(subKeyName, true);
                    if (subKey == null) continue;

                    // Xóa values có tên hoặc data chứa keyword
                    string[] valueNames;
                    try { valueNames = subKey.GetValueNames(); }
                    catch { continue; }

                    foreach (var valueName in valueNames)
                    {
                        try
                        {
                            bool nameMatch = valueName.Contains(keyword, StringComparison.OrdinalIgnoreCase);
                            bool dataMatch = subKey.GetValue(valueName)?.ToString()
                                ?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false;

                            if (nameMatch || dataMatch)
                                subKey.DeleteValue(valueName, false);
                        }
                        catch { }
                    }

                    // Tiếp tục recursive
                    ScanAndRemoveRegistryByKeyword(subKey, keyword);
                }
                catch { }
            }
        }

        private static void RemoveMuiCacheValues(RegistryKey? hive = null)
        {
            try
            {
                hive ??= Registry.CurrentUser;
                string muiCachePath = @"Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\MuiCache";
                using RegistryKey? muiKey = hive.OpenSubKey(muiCachePath, true);
                if (muiKey == null) return;

                foreach (var valueName in muiKey.GetValueNames())
                {
                    try
                    {
                        bool nameMatch = valueName.Contains("CocCoc", StringComparison.OrdinalIgnoreCase);
                        bool dataMatch = muiKey.GetValue(valueName)?.ToString()
                            ?.Contains("CocCoc", StringComparison.OrdinalIgnoreCase) ?? false;

                        if (nameMatch || dataMatch)
                            muiKey.DeleteValue(valueName, false);
                    }
                    catch { }
                }
            }
            catch { }
        }

        private static void RemoveAllCocCocFiles()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

            // Scan và xóa CocCoc folders
            string[] foldersToCheck =
            [
                Path.Combine(localAppData, "CocCoc"),
                Path.Combine(appData, "CocCoc"),
                Path.Combine(programFiles, "CocCoc"),
                Path.Combine(programFilesX86, "CocCoc"),
                @"C:\Program Files\CocCoc",
                @"C:\Program Files (x86)\CocCoc"
            ];

            foreach (var folder in foldersToCheck)
            {
                RemoveDirectory(folder);
            }

            // Scan và xóa shortcuts
            RemoveShortcuts();
        }

        private static void RemoveDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            catch { }
        }

        private static void RemoveShortcuts()
        {
            try
            {
                // Desktop shortcuts
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                RemoveFilesInDirectory(desktopPath, "*CocCoc*.lnk");

                // Start Menu shortcuts
                string startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
                RemoveFilesInDirectory(startMenuPath, "*CocCoc*", SearchOption.AllDirectories);

                // Quick Launch
                string quickLaunch = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                    @"Microsoft\Internet Explorer\Quick Launch");
                if (Directory.Exists(quickLaunch))
                    RemoveFilesInDirectory(quickLaunch, "*CocCoc*.lnk");
            }
            catch { }
        }

        private static void RemoveFilesInDirectory(string directory, string searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                if (!Directory.Exists(directory)) return;

                string[] files = Directory.GetFiles(directory, searchPattern, searchOption);
                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch { }
                }
            }
            catch { }
        }

        private static void RemoveServices()
        {
            string[] serviceNames = ["CocCoc", "CocCocService"];

            foreach (var serviceName in serviceNames)
            {
                try
                {
                    ServiceController service = new(serviceName);

                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        service.Stop();
                        service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                    }

                    // Xóa service từ registry
                    using RegistryKey? key = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{serviceName}", true);
                    if (key != null)
                    {
                        Registry.LocalMachine.DeleteSubKeyTree($@"SYSTEM\CurrentControlSet\Services\{serviceName}", false);
                    }
                }
                catch { }
            }
        }

        private static void KillCocCocProcesses()
        {
            string[] processNames = ["browser", "CocCoc", "coccoc"];
            foreach (var name in processNames)
            {
                try
                {
                    foreach (var proc in System.Diagnostics.Process.GetProcessesByName(name))
                    {
                        proc.Kill();
                        proc.WaitForExit(3000);
                    }
                }
                catch { }
            }
        }

        private static void RemoveFiles()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Các đường dẫn thường chứa CocCoc
            string[] paths =
            [
                Path.Combine(localAppData, "CocCoc"),
                Path.Combine(appData, "CocCoc"),
                @"C:\Program Files\CocCoc",
                @"C:\Program Files (x86)\CocCoc",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "CocCoc"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "CocCoc")
            ];

            foreach (var path in paths)
            {
                try
                {
                    if (Directory.Exists(path))
                        Directory.Delete(path, true);
                }
                catch { }
            }

            // Xóa shortcuts (legacy - giữ lại để compatibility)
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string[] shortcuts = Directory.GetFiles(desktopPath, "*CocCoc*.lnk");
                foreach (var shortcut in shortcuts)
                {
                    File.Delete(shortcut);
                }

                string startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
                shortcuts = Directory.GetFiles(startMenuPath, "*CocCoc*", SearchOption.AllDirectories);
                foreach (var shortcut in shortcuts)
                {
                    File.Delete(shortcut);
                }
            }
            catch { }
        }

        private static string GetWindowsName()
        {
            string osDescription = RuntimeInformation.OSDescription;

            // Parse version từ "Microsoft Windows 10.0.19045"
            if (osDescription.Contains("Windows"))
            {
                // Extract version number (10.0.19045 -> 10, 22621 -> 11)
                var parts = osDescription.Split(' ');
                if (parts.Length >= 3 && Version.TryParse(parts[2], out var version))
                {
                    // Windows 11 detection: Build >= 22000
                    if (osDescription.Contains("10.0") && osDescription.Contains("22"))
                        return "Windows 11";

                    return version.Major switch
                    {
                        5 when version.Minor == 1 => "Windows XP",
                        5 when version.Minor == 2 => "Windows Server 2003",
                        6 when version.Minor == 0 => "Windows Vista",
                        6 when version.Minor == 1 => "Windows 7",
                        6 when version.Minor == 2 => "Windows 8",
                        6 when version.Minor == 3 => "Windows 8.1",
                        10 => "Windows 10",
                        _ => osDescription
                    };
                }
            }

            return osDescription;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "https://ko-fi.com/thanhnguyen150993",
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open link: {ex.Message}");
            }
        }
    }
}
