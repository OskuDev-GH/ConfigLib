# ConfigLib
A bad configuration file system for C#

## Important info
Generative AI has **NOT** been and **NEVER** will be used to work on this project.

## Why should I use ConfigLib?
There's no reason.

## How can I use ConfigLib in my project?
### Option A: Copying files
  Copy `ConfigLib.cs` into your project.

### Option B: Adding a project reference
  If you're using Visual Studio, you can right click Depedencies in your project in the solution explorer and then click `Add Project Reference`.
  If you prefer using commands, you can do `dotnet add reference [ConfigLib's location]/ConfigLib.csproj` in the project's directory.

After doing either option A or option B, add `using ConfigLib;` into your file.
Now you should be ready to use ConfigLib.

The documentation is inside `ConfigLib.cs`.

## Examples
### Hello, world!
```cs
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

        //get value of hello_world safely
        config.TryGetString("hello_world", out string helloWorld);
        Console.WriteLine(helloWorld);
    }
}
```

### Get sum of all numbers in configuration
```cs
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
```