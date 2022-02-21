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
            await Task.Delay(3000);
            await MetaMarkPage.BringToFrontAsync();
            await MetaMarkPage.GoToAsync(string.Format("chrome-extension://{0}/popup.html", _Option.MetaMaskExtensionId));
            await Task.Delay(1000);
            var lstTemp = await MetaMarkPage.QuerySelectorAllAsync("button");

            var regexbtn = new Regex(@"^\s*Confirm\s*$", RegexOptions.IgnoreCase);
            ElementHandle buttonConfirm = null;
            foreach (var item in lstTemp)
            {
                var content = await MetaMarkPage.EvaluateFunctionAsync<string>("e => e.textContent", item);
                if (regexbtn.IsMatch(content))
                {
                    buttonConfirm = item;
                    break;
                }
            }
            await Task.Delay(2000);
            await buttonConfirm.ClickAsync();

            //var page = this.GetMetaMashPageAsync("")
        }

        public Page MetaMarkPage { get; set; }
        public async Task<bool> LoginAsync(string PassWord)
        {
            var pages = await _Browser.PagesAsync();
            RootPage = pages[0];

            await Task.Delay(5000);
            MetaMarkPage = await _Browser.NewPageAsync();
            await MetaMarkPage.BringToFrontAsync();
            await MetaMarkPage.GoToAsync(string.Format("chrome-extension://{0}/home.html#unlock", _Option.MetaMaskExtensionId));
            var pwd = await MetaMarkPage.WaitForSelectorAsync("*[type='password']");
            await pwd.TypeAsync(PassWord, new PuppeteerSharp.Input.TypeOptions { Delay = 100 });
            await Task.Delay(1000);

            var button = await MetaMarkPage.QuerySelectorAsync("button");
            await button.ClickAsync();

            var resCheck = MetaMarkPage.WaitForSelectorAsync("*[class='selected-account__clickable']");

            await RootPage.GoToAsync("https://dragonwars.game/#/train");

            return resCheck != null;
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
            await RootPage.GoToAsync($"https://dragonwars.game/#/train?page={page}");
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
            await RootPage.BringToFrontAsync();
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
                var buttons = await item.QuerySelectorAllAsync("button");
                foreach (var button in buttons)
                {
                    var content = await button.EvaluateFunctionAsync<string>("e => e.textContent", item);

                    if (regexbtn.IsMatch(content))
                    {
                        card.AdventureButton = item;

                        break;
                    }
                }

                lstData.Add(card);
                //content = await item.EvaluateFunctionAsync<string>("e => e.textContent", item);
                //if (regexbtn.IsMatch(content))
                //{
                //    lstData[index++].AdventureButton = item;
                //}

            }
            //foreach (var item in lstTemp)
            //{

            //}

            //lstTemp = await RootPage.QuerySelectorAllAsync("button");

            //foreach (var item in lstTemp)
            //{
            //    var content = await RootPage.EvaluateFunctionAsync<string>("e => e.textContent", item);
            //    if (regexbtn.IsMatch(content))
            //    {
            //        lstData[index++].AdventureButton = item;
            //    }
            //}

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

        public async Task RunAdventureClick(AppInfo appInfo)
        {
            await MetaMarkPage.GoToAsync(string.Format("chrome-extension://{0}/popup.html", _Option.MetaMaskExtensionId));

            await RootPage.BringToFrontAsync();
            await Task.Delay(2000);
            var lstPage = GetAdventureForPage(appInfo);

            int page = 1;
            foreach (var item in lstPage)
            {
                while (true)
                {
                    await RootPage.BringToFrontAsync();
                    var lstTemp = await GetCardItemsAsync(page++);
                    if (lstTemp.Count < 1) break;

                    var card = lstTemp.First();
                    try
                    {
                        if (card != null && item.Contains(card.Id.Trim()) && card.AdventureButton != null)
                        {
                            await Task.Delay(2000);
                            await card.AdventureButton.ClickAsync();
                            await Task.Delay(5000);
                            await ComfirmMetaMashAsync();
                            await Task.Delay(2000);
                        }
                    }
                    catch
                    {
                    }
                }
            }

        }
    }
}
