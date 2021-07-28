using Octokit;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Tools_Injector_Mod_Menu
{
    public static class UpdateService
    {
        public static async Task CheckGitHubNewerVersion(bool msg = false)
        {
            var client = new GitHubClient(new ProductHeaderValue("Tools-Injector-Mod-Menu"));
            var releases = await client.Repository.Release.GetAll("T5ive", "Tools-Injector-Mod-Menu").ConfigureAwait(false);

            var latestGitHubVersion = new Version(releases[0].TagName);
            var localVersion = Assembly.GetEntryAssembly()?.GetName().Version;

            if (localVersion < latestGitHubVersion)
            {
                var url = releases[0].Assets[0].BrowserDownloadUrl;
                if (MyMessage.MsgOkCancel($"You are using version {localVersion}\n" +
                                          $"The latest version is {latestGitHubVersion}\n\n" +
                                          "Do you want to update? "))
                {
                    Process.Start(url);
                }
            }
            else
            {
                if(msg) return;
                MyMessage.MsgShowInfo($"You are using version {localVersion}\n" +
                                      $"The latest version is {latestGitHubVersion }\n\n" +
                                      "You are using the latest version");
            }
        }
    }
}