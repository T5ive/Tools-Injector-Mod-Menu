using Octokit;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Tools_Injector_Mod_Menu
{
    public static class UpdateService
    {
        public static async Task CheckGitHubNewerVersion()
        {
            var client = new GitHubClient(new ProductHeaderValue("Tools-Injector-Mod-Menu"));
            var releases = await client.Repository.Release.GetAll("T5ive", "Tools-Injector-Mod-Menu");

            var latestGitHubVersion = new Version(releases[0].TagName);
            var localVersion = Assembly.GetEntryAssembly()?.GetName().Version;

            if (localVersion < latestGitHubVersion)
            {
                var url = releases[0].Assets[0].BrowserDownloadUrl;
                if (MyMessage.MsgOkCancel($"You are using version {localVersion}\nThe latest version is {latestGitHubVersion}\nDo you want to update? "))
                {
                    Process.Start(url);
                }
            }
            else
            {
                MyMessage.MsgShowInfo("You are using the latest version");
            }
        }
    }
}