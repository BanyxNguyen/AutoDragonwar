using ADW.Application.DTO;
using ADW.Application.Implements;
using ADW.Application.Options;
using ADW.Application.RequestFilters;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace ADW.App
{

    public partial class FormMain : Form
    {
        private readonly PuppeteerBrowserOption _OPtion;
        private readonly PuppeteerBrowser _Browser;
        private CaptureNetwork _CaptureNetwork;
        private bool _IsStop = false;
        public bool IsToolRuning
        {
            get => _IsStop; set
            {
                _IsStop = value;
                MethodInvoker action2 = delegate { btnStart.Enabled = value; };
                btnStart.Invoke(action2);
            }
        }
        public string UserDataChrome { get; set; }
        public FormMain()
        {
            InitializeComponent();
            UserDataChrome = @"C:\Users\BenNguyen\AppData\Local\Google\Chrome\User Data";

            _OPtion = new PuppeteerBrowserOption
            {
                MetaMaskExtensionId = "nkbihfbeogaeaoehlefnkodbefgpgknn",
                ChromePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
                UserData = Path.Combine(Directory.GetCurrentDirectory(), @"MetaMash"),
                MetaMaskExtension = @"C:\Users\BenNguyen\AppData\Local\Google\Chrome\User Data\Default\Extensions\nkbihfbeogaeaoehlefnkodbefgpgknn\10.9.3_1"
            };

            CheckProfile(_OPtion.UserData);

            var dirMetaMash = GetExtensionMetaMash(_OPtion.UserData, _OPtion.MetaMaskExtensionId);

            _OPtion.MetaMaskExtension = dirMetaMash;

            _Browser = new PuppeteerBrowser(_OPtion);
            AppInfo = new AppInfo();
        }
        public string GetExtensionMetaMash(string PathProfile, string ExtensionId)
        {
            var pathExtension = Path.Combine(PathProfile, "Default", "Extensions", ExtensionId);

            var dir = Directory.GetDirectories(pathExtension);
            return dir[0];
        }
        public void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, Func<string, bool> filter = null)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);
            Console.WriteLine($"Folder - {destDirName.Replace(Directory.GetCurrentDirectory(), "...")}");

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                Console.WriteLine($"File - {tempPath.Replace(Directory.GetCurrentDirectory(), "...")}");
                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    if (filter != null && !filter.Invoke(subdir.FullName))
                    {
                        continue;
                    }
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
        public void CopyFile(string Source, string target)
        {
            DirectoryInfo dir = new DirectoryInfo(Source);
            foreach (var file in dir.GetFiles())
            {
                string tempPath = Path.Combine(target, file.Name);
                //Console.WriteLine($"File - {tempPath.Replace(Directory.GetCurrentDirectory(), "...")}");
                file.CopyTo(tempPath, true);
            }
        }
        public void CheckProfile(string PathProfile)
        {
            if (!Directory.Exists(PathProfile))
            {
                Directory.CreateDirectory(PathProfile);
                CopyFile(UserDataChrome, PathProfile);

                var Profile = Directory.GetDirectories(UserDataChrome).FirstOrDefault(x => x.Contains("Default", StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrWhiteSpace(Profile))
                {
                    var pth = Path.Combine(PathProfile, "Default");
                    Directory.CreateDirectory(pth);
                    CopyFile(Profile, pth);
                    DirectoryCopy(Path.Combine(Profile, "Extensions"), Path.Combine(pth, "Extensions"), true);
                }
            }

        }
        private RequestFilterMyDragonPage _FilterMyGragon;
        private RequestFilterAdventure _FilterAdventure;
        private async void btnUnlock_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtPAssWord.Text))
            {
                MessageBox.Show(this, "Password can not empty!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var nrow = await _Browser.InitialAsync();

            var res = await _Browser.LoginAsync(txtPAssWord.Text);
            if (res)
            {
                pnContent.Enabled = true;
                pnUnlock.Enabled = false;
                _CaptureNetwork = new CaptureNetwork(_Browser.RootPage);

                _FilterMyGragon = new RequestFilterMyDragonPage();
                _FilterMyGragon.OnResponseListener += onResponseFilter;

                _FilterAdventure = new RequestFilterAdventure();
                _FilterAdventure.OnResponseListener += onResponseAdventure;
                _CaptureNetwork.AddFilter(_FilterMyGragon);
                _CaptureNetwork.AddFilter(_FilterAdventure);
            }
        }

        public AppInfo AppInfo { get; set; }
        private void onResponseAdventure(ResponseCreatedEventArgs e, AdventureDTO myPageDragonDTO)
        {
            if (!myPageDragonDTO.Success) return;
            lock (AppInfo)
            {
                AppInfo.AdventureInfo = myPageDragonDTO;
            }

            if (IsToolRuning && (AppInfo.AdventureInfo?.Payload?.Count ?? 0) > 0)
            {
                btnStart_Click(null, null);
            }
            Uri myUri = new Uri(e.Response.Url);
            MethodInvoker action2 = delegate { txtAdventure.Text = myPageDragonDTO.Payload.Count.ToString(); };
            txtAdventure.Invoke(action2);
        }

        private async void onResponseFilter(ResponseCreatedEventArgs e, MyPageDragonDTO myPageDragonDTO)
        {
            lock (AppInfo)
            {
                AppInfo.DragonPageInfo = myPageDragonDTO;
            }

            if (AppInfo.PayloadDragonPages == null)
            {
                _OPtion.Wallet = e.Response.Request.Headers["wallet"].ToString();
                AppInfo.PayloadDragonPages = await _Browser.GetPageInfosAsync(AppInfo.DragonPageInfo);
            }
            Uri myUri = new Uri(e.Response.Url);
            string page = HttpUtility.ParseQueryString(myUri.Query).Get("page");
            MethodInvoker action1 = delegate { txtCurrentPage.Text = page; };
            txtCurrentPage.Invoke(action1);

            MethodInvoker action2 = delegate { txtDGTotal.Text = myPageDragonDTO.Payload.TotalItems.ToString(); };
            txtDGTotal.Invoke(action2);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _Browser.Dispose();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                IsToolRuning = false;
                await _Browser.RunAdventureClick(AppInfo);
            }
            finally
            {
                IsToolRuning = true;
            }
        }
    }
}
