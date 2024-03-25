using Microsoft.Extensions.DependencyInjection;
using NameSorterCore.Interfaces;
using NameSorterApp.Configuration;

namespace NameSorterApp;

internal class Program
{
    public static async Task Main(string[] args)
    {
        // Validate input arguments
        if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
        {
            Console.WriteLine("Please provide the path to the unsorted names file.");
            return;
        }

        var host = HostConfiguration.CreateHostBuilder(args).Build();

        // Retrieve the file processor service
        var fileProcessor = host.Services.GetRequiredService<IFileProcessor>();

        // Process the file
        try
        {
            string inputFilePath = args[0];
            await fileProcessor.ProcessFileAsync(inputFilePath);
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"File not found: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
