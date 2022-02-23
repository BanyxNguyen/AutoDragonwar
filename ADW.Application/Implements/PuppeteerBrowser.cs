using ADW.Application.DTO;
using ADW.Application.Options;
using Microsoft.Extensions.Options;
using PuppeteerSharp;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ADW.Application.Implements
{
    public class PuppeteerBrowser : IDisposable
    {
        private readonly PuppeteerBrowserOption _Option;
        private Browser _Browser;
        public Page RootPage { get; private set; }
        public PuppeteerBrowser(IOptions<PuppeteerBrowserOption> options) : this(options.Value)
        {
        }
        public PuppeteerBrowser(PuppeteerBrowserOption puppeteerBrowserOption)
        {
            _Option = puppeteerBrowserOption;
        }

        public void Dispose()
        {
            _Browser?.Dispose();
        }
        public async Task<Browser> InitialAsync(Action<LaunchOptions> config = null)
        {
            var options = new LaunchOptions
            {
                Headless = _Option.Headless,
                //UserDataDir = _Option.UserData,
                Args = new string[]
                {
                    $"--user-data-dir=\"{ _Option.UserData}\"",
                    //$"--profile-directory=\"{_Option.Profile}\"",
                    $"--disable-extensions-except=\"{_Option.MetaMaskExtension}\"",
                    $"--load-extension=\"{_Option.MetaMaskExtension}\"",
                    "--disable-features=\"site-per-process\""
                },
                ExecutablePath = _Option.ChromePath,
            };
            config?.Invoke(options);
            _Browser = await Puppeteer.LaunchAsync(options);
            return _Browser;
        }
        public async Task<Page> GetMetaMashPageAsync(string HtmlName, int delayMsec = 2000, int timeOutMsec = 30000)
        {
            var current = DateTime.Now;
            while (true)
            {
                var lstTarget = _Browser.Targets();
                var lstTemp = lstTarget.Where(x => x.Url.Contains($"chrome-extension://{_Option.MetaMaskExtensionId}"));
                var target = lstTemp.FirstOrDefault(x => x.Url.ToLower().Contains(HtmlName.ToLower()));
                target = lstTemp.FirstOrDefault(x => x.Url.ToLower().Contains(HtmlName.ToLower()));
                if (target != null) return await target.PageAsync();
                await Task.Delay(delayMsec);
                if ((DateTime.Now - current).TotalMilliseconds > timeOutMsec)
                {
                    return null;
                }
            }
        }
        public async Task ComfirmMetaMashAsync()
        {
            await GotoNavigateMetaMashAsync();

            ElementHandle buttonConfirm = null;
            int time = 0;
            while (buttonConfirm == null && ++time < 4)
            {
                var lstTemp = await MetaMaskPage.QuerySelectorAllAsync("button");

                var regexbtn = new Regex(@"^\s*Confirm\s*$", RegexOptions.IgnoreCase);
                foreach (var item in lstTemp)
                {
                    var content = await MetaMaskPage.EvaluateFunctionAsync<string>("e => e.textContent", item);
                    if (regexbtn.IsMatch(content))
                    {
                        buttonConfirm = item;
                        break;
                    }
                }
                await Task.Delay(3000);
            }
            if (buttonConfirm != null)
            {
                await buttonConfirm?.ClickAsync();
            }
        }

        public Page MetaMaskPage { get; set; }
        public async Task<bool> LoginAsync(string PassWord)
        {
            var pages = await _Browser.PagesAsync();
            RootPage = pages[0];

            await Task.Delay(5000);
            MetaMaskPage = await _Browser.NewPageAsync();
            await MetaMaskPage.BringToFrontAsync();
            await MetaMaskPage.GoToAsync(string.Format("chrome-extension://{0}/home.html#unlock", _Option.MetaMaskExtensionId));
            var pwd = await MetaMaskPage.WaitForSelectorAsync("*[type='password']");
            await pwd.TypeAsync(PassWord, new PuppeteerSharp.Input.TypeOptions { Delay = 100 });
            await Task.Delay(1000);

            var button = await MetaMaskPage.QuerySelectorAsync("button");
            await button.ClickAsync();

            var resCheck = MetaMaskPage.WaitForSelectorAsync("*[class='selected-account__clickable']");

            return resCheck != null;
        }
        public string _UrlGameApp = "https://dragonwars.game/#/train";

        public string _UrlMetaMashPopup { get => string.Format("chrome-extension://{0}/popup.html", _Option.MetaMaskExtensionId); }
        public string _UrlMetaMashHome { get => string.Format("chrome-extension://{0}/home.html#unlock", _Option.MetaMaskExtensionId); }
        public async Task InitialTabAsync()
        {
            var pages = await _Browser.PagesAsync();
            RootPage = pages[0];
            MetaMaskPage = await _Browser.NewPageAsync();
        }

        //public async Task<ElementHandle[]> GetHanleNavigateButtonsAsync()
        //{
        //    await RootPage.WaitForSelectorAsync("ul.pagination li a");
        //    return await RootPage.QuerySelectorAllAsync("ul.pagination li a");
        //}

        //public async Task<ElementHandle> GetHandleNavigateButtonAsync(string page)
        //{
        //    var lstBtn = await GetHanleNavigateButtonsAsync();
        //    return lstBtn.First(x => x.GetPropertyAsync("innerText").Result.JsonValueAsync().ToString().Equals(page));
        //}

        public async Task GotoNavigatePageAsync(int page)
        {
            await RootPage.BringToFrontAsync();
            await RootPage.GoToAsync($"https://dragonwars.game/#/train?page={page}");
            await Task.Delay(2000);
        }
        public async Task GotoNavigateMetaMashAsync()
        {
            await MetaMaskPage.BringToFrontAsync();
            await MetaMaskPage.GoToAsync(_UrlMetaMashPopup);
            await Task.Delay(2000);
        }
        public async Task<IList<PayloadDragonPage>> GetPageInfosAsync(MyPageDragonDTO myPageDragonDTO)
        {
            var lstData = new List<PayloadDragonPage>();
            for (int i = 1; i <= myPageDragonDTO.Payload.TotalPage; i++)
            {
                var client = new RestClient($"https://devabcde-api.dragonwars.game/v1/dragons/my-dragon?page={i}&rowPerPage=12&type=DRAGON");
                var request = new RestRequest();
                request.Method = Method.Get;
                request.AddHeader("wallet", _Option.Wallet);
                var response = await client.ExecuteAsync<MyPageDragonDTO>(request);
                lstData.Add(response.Data.Payload);
            }
            return lstData;
        }
        public async Task<IList<CardInfo>> GetCardItemsAsync(int page)
        {
            await Task.Delay(2000);
            await GotoNavigatePageAsync(page);
            var lstData = new List<CardInfo>();
            var lstTemp = await RootPage.QuerySelectorAllAsync(".grid > div.relative");
            var regexSpan = new Regex(@"^\s*#\d+\s*$");
            var regexbtn = new Regex(@"^\s*Adventure\s*$", RegexOptions.IgnoreCase);

            foreach (var item in lstTemp)
            {

                var spans = await item.QuerySelectorAllAsync("span");
                var card = new CardInfo();
                foreach (var span in spans)
                {
                    var content = await span.EvaluateFunctionAsync<string>("e => e.textContent", item);

                    if (regexSpan.IsMatch(content))
                    {
                        card.Id = content;

                        break;
                    }
                }
                //var buttons = await item.QuerySelectorAllAsync("button");
                //foreach (var button in buttons)
                //{
                //    var content = await button.EvaluateFunctionAsync<string>("e => e.textContent", item);

                //    if (regexbtn.IsMatch(content))
                //    {
                //      await  item.ClickAsync();
                //        card.AdventureButton = item;

                //        break;
                //    }
                //}

                lstData.Add(card);


            }


            return lstData;
        }

        public IList<HashSet<string>> GetAdventureForPage(AppInfo appInfo)
        {
            return appInfo.PayloadDragonPages.Select(x =>
            {

                var temp = (from d in x.Data
                            join a in appInfo.AdventureInfo.Payload
                            on d.Id equals a
                            select $"#{a}");
                return temp.ToHashSet();
            }).ToList();
        }
        public async Task AdventureClickAsync(int index)
        {
            await RootPage.BringToFrontAsync();
            await RootPage.ClickAsync($".grid > div.relative:nth-child({index + 1}) button:nth-child(2)");
        }
        public async Task RunAdventureClick(AppInfo appInfo)
        {

            await Task.Delay(2000);
            //var lstPage = GetAdventureForPage(appInfo);
            var lstAdventure = appInfo.AdventureInfo.Payload.Select(x => $"#{x}").ToHashSet();
            var AmountPages = appInfo.DragonPageInfo.Payload.TotalPage;
            for (int page = 0; page < AmountPages; page++)
            {
                //foreach (var item in lstPage)
                //{
                var lstTemp = await GetCardItemsAsync(page + 1);
                if (lstTemp.Count < 1) break;
                for (int i = 0; i < lstTemp.Count; i++)
                {
                    var card = lstTemp[i];
                    try
                    {
                        if (card != null && lstAdventure.Contains(card.Id.Trim()))
                        {
                            await Task.Delay(2000);
                            await AdventureClickAsync(i);
                            lstAdventure.Remove(card.Id.Trim());
                            await Task.Delay(1000);
                            await ComfirmMetaMashAsync();
                            await Task.Delay(2000);
                        }
                    }
                    catch
                    {
                    }
                }

                //}
            }

        }
    }
}
