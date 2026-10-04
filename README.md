# ConfigLib
A bad configuration file system for C#

## NO AI WAS USED WHILE MAKING THIS

## How can I use ConfigLib in my project?
### Option A:
  copy `ConfigLib.cs` to your project

### Option B: Adding a project reference to ConfigLib
  If you're using Visual Studio, you can right click Depedencies in your project in the solution explorer and then click `Add Project Reference`.
  If you prefer using commands, you can do `dotnet add reference [ConfigLib's location]/ConfigLib.csproj` in the project's directory.

After doing either option A or option B, you can add `using ConfigLib;` into your file.

The documentation is inside `ConfigLib.cs`.

## Examples
### Hello, world!
`
using ConfigLib;

internal class Program
{
    private static void Main(string[] args)
    {
        //custom logger function
        Config.ErrorHandlingStyle = ErrorHandlingStyle.LoggerFunction;
        Config.ErrorLoggingFunction = (string msg) =>
        {
            Console.WriteLine(msg);
        };

        //load values from config.txt to a configuration called test
        Config config = Config.CreateFromFile("test", "config.txt");

        Console.WriteLine(config.GetString("hello_world"));
    }
}
`

### Get sum of all numbers in configuration
`
using ConfigLib;

internal class Program
{
    private static void Main(string[] args)
    {
        //custom logger function
        Config.ErrorHandlingStyle = ErrorHandlingStyle.LoggerFunction;
        Config.ErrorLoggingFunction = (string msg) =>
        {
            Console.WriteLine(msg);
        };

        //load values from config.txt to a configuration called test
        Config config = Config.CreateFromFile("test", "config.txt");

        while (true)
        {
            Console.Clear();

            //reload values
            config.LoadValues();

            Stack<double> vals = [];

            //get every number
            foreach (var value in config.Values)
            {
                if (config.TryGetDouble(value.Key, out double v))
                {
                    vals.Push(v);
                }
            }

            //get sum of all numbers
            double sum = 0;
            while (vals.Count > 0)
            {
                sum += vals.Pop();
            }

            Console.WriteLine(sum);

            Thread.Sleep(100);
        }
    }
}
`