using Jenx.Bluetooth.GattServer.Common;
using System.Threading.Tasks;

namespace App3
{
    public class ConsoleLogger : ILogger
    {
        Task ILogger.LogMessageAsync(string message)
        {
            System.Console.WriteLine(message);
            return Task.CompletedTask;
        }
    }
}