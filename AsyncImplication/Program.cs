using System.Diagnostics.Metrics;
using System.Net;
using System.Runtime.CompilerServices;

namespace AsyncImplication
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Enter the file link: ");
            string? link = Console.ReadLine();

            Console.Write("Enter output file: ");
            string? fileName = Console.ReadLine();

            if (link == null || fileName == null)
            {
                Console.WriteLine("Necessary input missing!");
                Environment.Exit(0);
            }

            Task result = DownloadFileAsync(link, fileName);
            ShortProcess();

            await result;
        }
        static async Task DownloadFileAsync(string url, string path)
        {
            Console.WriteLine("Beggining the download operation");
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead)) { 
                    response.EnsureSuccessStatusCode();
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
            }
            Console.WriteLine("Download finished successfully! your file is saved as {0}", path);
        }
        public static void ShortProcess()
        {
            Console.WriteLine("Your file is downloading...");
        }
    }
}