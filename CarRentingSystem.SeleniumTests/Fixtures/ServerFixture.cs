using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CarRentingSystem.SeleniumTests
{
    /// <summary>Starts the ASP.NET app once for the whole test run.</summary>
    [SetUpFixture]
    public sealed class ServerFixture
    {
        public static string BaseUrl { get; } = "https://localhost:7237";
        private static Process? _server;

        [OneTimeSetUp]
        public void StartServer()
        {
            // If already running, do nothing.
            if (IsUp(BaseUrl, TimeSpan.FromSeconds(2)).GetAwaiter().GetResult())
                return;

            var slnRoot = FindSolutionRoot();
            var webProj = Path.Combine(slnRoot, "CarRentingSystem", "CarRentingSystem.csproj");
            if (!File.Exists(webProj))
                throw new FileNotFoundException("Could not find web project", webProj);

            var psi = new ProcessStartInfo("dotnet")
            {
                Arguments = $"run --project \"{webProj}\" --urls \"{BaseUrl};http://localhost:5010\"",
                WorkingDirectory = slnRoot,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            // Ensure dev environment
            psi.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";

            _server = Process.Start(psi) ?? throw new InvalidOperationException("Failed to start web app.");

            _server.OutputDataReceived += (_, e) => { if (e.Data != null) TestContext.Progress.WriteLine(e.Data); };
            _server.ErrorDataReceived += (_, e) => { if (e.Data != null) TestContext.Progress.WriteLine(e.Data); };
            _server.BeginOutputReadLine();
            _server.BeginErrorReadLine();

            // Wait until it responds
            var ok = WaitUntilAsync(() => IsUp(BaseUrl, TimeSpan.FromSeconds(2)),
                                    TimeSpan.FromSeconds(60)).GetAwaiter().GetResult();
            if (!ok)
            {
                try { _server.Kill(entireProcessTree: true); } catch { }
                throw new InvalidOperationException("Web app did not start within 60s. Check Test Output for logs.");
            }
        }

        [OneTimeTearDown]
        public void StopServer()
        {
            try { if (_server is { HasExited: false }) _server.Kill(entireProcessTree: true); } catch { }
            _server?.Dispose();
        }

        static async Task<bool> IsUp(string baseUrl, TimeSpan timeout)
        {
            try
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (_, __, ___, ____) => true
                };
                using var http = new HttpClient(handler) { Timeout = timeout };
                using var resp = await http.GetAsync($"{baseUrl}/");
                return resp.IsSuccessStatusCode || (int)resp.StatusCode == 404;
            }
            catch { return false; }
        }

        static async Task<bool> WaitUntilAsync(Func<Task<bool>> probe, TimeSpan max)
        {
            var start = DateTime.UtcNow;
            while (DateTime.UtcNow - start < max)
            {
                if (await probe()) return true;
                await Task.Delay(1000);
            }
            return false;
        }

        static string FindSolutionRoot()
        {
            var dir = AppContext.BaseDirectory;
            var d = new DirectoryInfo(dir);
            while (d != null && !File.Exists(Path.Combine(d.FullName, "CarRentingSystem.sln")))
                d = d.Parent!;
            return d?.FullName ?? AppContext.BaseDirectory;
        }
    }
}
