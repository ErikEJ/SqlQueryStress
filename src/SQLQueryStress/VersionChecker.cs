using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SQLQueryStress
{
    public static class VersionChecker
    {
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
        private sealed record Release(string tag_name);
#pragma warning restore CA1812

        public static async Task CheckForPackageUpdateAsync()
        {
            try
            {
                var fileVersion = FileVersionInfo.GetVersionInfo(Environment.ProcessPath);

                if (fileVersion is null || string.IsNullOrWhiteSpace(fileVersion.FileVersion))
                {
                    return;
                }

                var timeout = TimeSpan.FromSeconds(2);

                using var cts = new CancellationTokenSource(timeout);

                var cacheFile = Path.Join(Path.GetTempPath(), "SQLQeryStress.tag.txt");

                Version latestVersion = null;

                if (File.Exists(cacheFile) && File.GetLastWriteTimeUtc(cacheFile) > DateTime.UtcNow.AddDays(-7))
                {
                    var cache = await File.ReadAllTextAsync(cacheFile, cts.Token).ConfigureAwait(false);
                    latestVersion = Version.Parse(cache);
                }
                else
                {
                    using var httpClient = new HttpClient
                    {
                        DefaultRequestHeaders = { { "User-Agent", "SQLQueryStress" } },
                        Timeout = timeout,
                    };

                    var response = await httpClient.GetFromJsonAsync<Release>("https://api.github.com/repos/ErikEJ/SQLQueryStress/releases/latest").ConfigureAwait(false);
                    if (response is null || response.tag_name is null)
                    {
                        return;
                    }

                    latestVersion = Version.Parse(response.tag_name);

                    await File.WriteAllTextAsync(cacheFile, latestVersion.ToString(), cts.Token).ConfigureAwait(false);
                }

                if (latestVersion is null)
                {
                    return;
                }

                if (latestVersion > new Version(fileVersion.ProductVersion))
                {
                    MessageBox.Show($"A new version of SQLQueryStress is available: {latestVersion}. Download from https://github.com/ErikEJ/SQLQueryStress/releases/latest.", "Update Available", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
            {
                // Ignore
            }
#pragma warning restore CA1031
        }
    }
}
