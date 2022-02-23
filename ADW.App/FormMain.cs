using ADW.Application.DTO;
using ADW.Application.Implements;
using ADW.Application.Options;
using ADW.Application.RequestFilters;
using Newtonsoft.Json;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private readonly string PatAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
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
            UserDataChrome = $@"{PatAppData}\Google\Chrome\User Data";

            _OPtion = GetOptionFromFile();
            _OPtion.UserData = Path.Combine(Directory.GetCurrentDirectory(), @"MetaMash");
            CheckProfile(_OPtion.UserData);

            var dirMetaMash = GetExtensionMetaMash(_OPtion.UserData, _OPtion.MetaMaskExtensionId);

            _OPtion.MetaMaskExtension = dirMetaMash;

            _Browser = new PuppeteerBrowser(_OPtion);
            AppInfo = new AppInfo();
        }



        private void Application_ApplicationExit(object sender, EventArgs e)
        {
        }

        public PuppeteerBrowserOption GetOptionFromFile()
        {
            var content = File.ReadAllText("PuppeteerConfig.json");
            return JsonConvert.DeserializeObject<PuppeteerBrowserOption>(content);
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
            try
            {
                await _Browser.InitialAsync();

            }
            catch
            {
                MessageBox.Show(this, "Close all browser and try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            await _Browser.InitialTabAsync();
            _CaptureNetwork = new CaptureNetwork(_Browser.RootPage);
            _FilterMyGragon = new RequestFilterMyDragonPage();
            _FilterMyGragon.OnResponseListener += onResponseFilter;
            _FilterAdventure = new RequestFilterAdventure();
            _FilterAdventure.OnResponseListener += onResponseAdventure;
            _CaptureNetwork.AddFilter(_FilterMyGragon);
            _CaptureNetwork.AddFilter(_FilterAdventure);

            pnContent.Enabled = true;
            btnStart.Enabled = true;
            btnUnlock.Enabled = false;

            await _Browser.GotoNavigateMetaMashAsync();
            await _Browser.GotoNavigatePageAsync(1);

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
        private void onResponseFilter(ResponseCreatedEventArgs e, MyPageDragonDTO myPageDragonDTO)
        {
            lock (AppInfo)
            {
                AppInfo.DragonPageInfo = myPageDragonDTO;
            }

            //if (AppInfo.PayloadDragonPages == null && !isLoockLoadPage)
            //{
            //    isLoockLoadPage = true;
            //    _OPtion.Wallet = e.Response.Request.Headers["wallet"].ToString();

            //    _ = Task.Run(async () =>
            //    {
            //        try
            //        {
            //            AppInfo.PayloadDragonPages = await _Browser.GetPageInfosAsync(AppInfo.DragonPageInfo);
            //        }
            //        catch (Exception ex)
            //        {

            //        }
            //        finally
            //        {
            //            isLoockLoadPage = false;
            //        }

            //    });

            //}
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

                await _Browser.GotoNavigateMetaMashAsync();
                await _Browser.GotoNavigatePageAsync(1);
                while (AppInfo.DragonPageInfo == null)
                {
                    await Task.Delay(1000);
                }
                await Task.Delay(2000);
                await _Browser.RunAdventureClick(AppInfo);
            }
            finally
            {
                //IsToolRuning = true;
            }
        }
    }
}
