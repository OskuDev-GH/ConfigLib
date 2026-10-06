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
Clone ConfigLib by running this command: `git clone https://github.com/OskuDev-GH/ConfigLib.git`

If you're using Visual Studio, you can right click Depedencies in your project in the solution explorer and then click `Add Project Reference`.
  
If you prefer using commands, you can do `dotnet add reference [ConfigLib's location]/ConfigLib.csproj` in the project's directory.



After doing either option A or option B, add `using ConfigLib;` into your file.

Now you should be ready to use ConfigLib.

Functions are documented (somewhat sloppily) in `ConfigLib.cs`.

## Value formatting
### Ints
Int values are just integers.

Examples:
 - 7
 - 69
 - 420
 - 1337

### Floats & doubles
Floats and doubles values are just numbers with decimal points in them.

Doubles are more precise than floats.

Examples:
 - 0.7
 - 6.9
 - 42.0
 - 133.7

### Booleans
Booleans are can be true or false.

By default there's 3 ways to write a boolean:
 - 1/0
 - true/false
 - yes/no

### Strings
All values are automatically trimmed before getting stored, so if you want to preserve whitespace characters before or after the string, use double quotes on both ends.

If you want your string to start and end with quotes after formating, just use double double quotes.

Examples:
 - Hello, world! => Hello, world!
 - "Hello, world!" => Hello, world!
 - ""Hello, world!"" => "Hello, world!"

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