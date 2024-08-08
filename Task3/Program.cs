using System.Threading.Tasks;
using Task3.Algorithms;
using Task3.UI;

namespace Task3;

internal class Program
{
  private static async Task Main(string[] args)
  {
    var shell = new ConsoleShell();
    await shell.StartAsync(PhoneBook.Instance);
  }
}