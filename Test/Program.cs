using ADW.Application.Implements;
using ADW.Application.Options;
using PuppeteerSharp;
using System;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var browser = new PuppeteerBrowser(new PuppeteerBrowserOption
            {
                MetaMaskExtensionId = "nkbihfbeogaeaoehlefnkodbefgpgknn",
                ChromePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
                Profile = "Default",
                UserData = "C:\\Users\\BenNguyen\\AppData\\Local\\Google\\Chrome\\User Data",
                MetaMaskExtension = "C:\\Users\\BenNguyen\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Extensions\\nkbihfbeogaeaoehlefnkodbefgpgknn\\10.9.3_0"
            });
            using (browser)
            {
                var scope = await browser.InitialAsync();

                var res = await browser.LoginAsync("leduyen332");
                var pages = await scope.PagesAsync();
                var page = pages[0];
                await page.GoToAsync("https://dragonwars.game/#/train");

                await browser.ComfirmMetaMashAsync();
            }
        }
    }
}
