using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace ado_connect_winauth
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            if (args?.Length < 1)
            {
                Console.Error.WriteLine("You must provide an Azure DevOps organization URL as a command-line parameter.");
                return -1;
            }

            Console.WriteLine($"Connecting to '{args[0]}' using Windows Authentication...");
            var baseUrl = new Uri(args[0]);

            // Attempt #1 - This fails with VssUnauthorizedException
            /*
            var credentials = new VssClientCredentials(
                new WindowsCredential(true),
                new VssFederatedCredential(true),
                CredentialPromptType.PromptIfNeeded,
                TaskScheduler.Default);
            */

            // Attempt #2 - This does not compile because VssClientCredentialStorage does not have a parameterless constructor
            /*
            var credentials = new VssClientCredentials();
            credentials.Storage = new VssClientCredentialStorage();
            */

            // Attempt #3 - This fails with VssUnauthorizedException
            var credentials = new VssClientCredentials(
                new WindowsCredential(false),
                new VssFederatedCredential(false),
                CredentialPromptType.PromptIfNeeded);

            using (var connection = new VssConnection(baseUrl, credentials))
            {
                await connection.ConnectAsync().ConfigureAwait(true);
            }

            Console.WriteLine("Hello World!");
            return 0;
        }
    }
}