using System.Globalization;

namespace ConfigLib;

public enum ErrorHandlingStyle
{
    Exception,
    Print,
    LoggerFunction,
}

public class Config
{
    public string ID { get; private set; }

    /// <summary>
    /// The path to the file a configuration last read the values from
    /// </summary>
    public string? FilePath { get; set; }

    /// <summary>
    /// All values are stored as strings, use TryGetInt(), TryGetFloat(), etc. to get values as different types
    /// </summary>
    public Dictionary<string, string> Values { get; set; } = [];

    /// <summary>
    /// GetBool() uses this array to determine if the values is true
    /// </summary>
    public static string[] TrueStrings = ["1", "true", "yes"];
    /// <summary>
    /// GetBool() uses this array to determine if the values is false
    /// </summary>
    public static string[] FalseStrings = ["0", "false", "no"];

    public Config(string id)
    {
        ID = id;
    }

    /// <summary>
    /// This function loads values from a file
    /// </summary>
    /// <param name="filePath"></param>
    public void LoadValues(string? filePath = null)
    {
        FilePath = filePath ?? FilePath;

        if (FilePath == null || !File.Exists(FilePath))
        {
            HandleError($"Couldn't load to '{ID}' from file!");
            return;
        }
        
        Values = GetValuesFromFile(FilePath, out Dictionary<string, string> newValues) ? newValues : Values;
    }

    /// <summary>
    /// This function returns true if the configuration's values contain an ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool Contains(string id) => Values.ContainsKey(id);

    /// <summary>
    /// Adds a value to the configuration
    /// </summary>
    /// <param name="valueId"></param>
    /// <param name="rawValue"></param>
    public void AddValue(string valueId, object rawValue)
    {
        string value = rawValue.ToString()!;

        if (!Values.TryAdd(valueId, value))
        {
            Values[valueId] = value;
        }
    }

    /// <summary>
    /// Returns a raw value
    /// </summary>
    /// <param name="valueId"></param>
    /// <returns></returns>
    public string GetRawValue(string valueId)
    {
        if (!Values.TryGetValue(valueId, out string? value))
        {
            HandleError($"Value '{valueId}' was not found in config '{ID}'!");
            return "";
        }

        return value;
    }

    /// <summary>
    /// Safely gets a raw value
    /// </summary>
    /// <param name="valueId"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGetRawValue(string valueId, out string? value)
    {
        bool success = Values.TryGetValue(valueId, out value);
        return success;
    } 

    /// <summary>
    /// Returns an integer value
    /// </summary>
    /// <param name="valueId"></param>
    /// <returns></returns>
    public int GetInt(string valueId)
    {
        string raw = GetRawValue(valueId);

        if (!int.TryParse(raw.Replace(',', '.'), out int value))
        {
            HandleError($"Invalid int: {value}");
            return 0;
        }

        return value;
    }

    /// <summary>
    /// Safely gets an integer value
    /// </summary>
    /// <param name="valueId"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGetInt(string valueId, out int value)
    {
        value = 0;

        if (!TryGetRawValue(valueId, out string? raw))
        {
            return false;
        }

        bool success = int.TryParse(raw!.Replace(',', '.'), out value);
        return success;
    } 

    /// <summary>
    /// Returns a float value
    /// </summary>
    /// <param name="valueId"></param>
    /// <returns></returns>
    public float GetFloat(string valueId)
    {
        string raw = GetRawValue(valueId);

        if (!float.TryParse(raw.Replace(',', '.'), out float value))
        {
            HandleError($"Invalid float: {value}");
            return 0;
        }

        return value;
    }

    /// <summary>
    /// Safely gets an float value
    /// </summary>
    /// <param name="valueId"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGetFloat(string valueId, out float value)
    {
        value = 0;

        if (!TryGetRawValue(valueId, out string? raw))
        {
            return false;
        }

        bool success = float.TryParse(raw!.Replace(',', '.'), out value);
        return success;
    }

    /// <summary>
    /// Returns a double value
    /// </summary>
    /// <param name="valueId"></param>
    /// <returns></returns>
    public double GetDouble(string valueId)
    {
        string raw = GetRawValue(valueId);

        if (!double.TryParse(raw.Replace(',', '.'), out double value))
        {
            HandleError($"Invalid double: {value}");
            return 0;
        }

        return value;
    }

    /// <summary>   
    /// Safely gets an double value
    /// </summary>
    /// <param name="valueId"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGetDouble(string valueId, out double value)
    {
        value = 0;

        if (!TryGetRawValue(valueId, out string? raw))
        {
            return false;
        }

        bool success = double.TryParse(raw!.Replace(',', '.'), out value);
        return success;
    } 

    /// <summary>
    /// Returns a boolean value
    /// </summary>
    /// <param name="valueId"></param>
    /// <param name="caseSensitive"></param>
    /// <returns></returns>
    public bool GetBool(string valueId, bool caseSensitive = false)
    {
        string raw = GetRawValue(valueId);
        if (!caseSensitive) raw = raw.ToLower();

        bool isTrue = TrueStrings.Contains(raw);
        bool isFalse = FalseStrings.Contains(raw);

        if (!isTrue && !isFalse)
        {
            HandleError($"Invalid bool: {raw}");
            return false;
        }

        return isTrue || !isFalse;
    }

    /// <summary>
    /// Safely gets a boolean value
    /// </summary>
    /// <param name="valueId"></param>
    /// <param name="value"></param>
    /// <param name="caseSensitive"></param>
    /// <returns></returns>
    public bool TryGetBool(string valueId, out bool value, bool caseSensitive = false)
    {
        value = false;

        if (!TryGetRawValue(valueId, out string? raw))
        {
            return false;
        }

        if (!caseSensitive) raw = raw!.ToLower();

        bool isTrue = TrueStrings.Contains(raw);
        bool isFalse = FalseStrings.Contains(raw);

        if (!isTrue && !isFalse)
        {
            HandleError($"Invalid bool: {raw}");
            return false;
        }

        value = isTrue || !isFalse;
        return true;
    } 

    /// <summary>
    /// Returns a string value
    /// </summary>
    /// <param name="valueId"></param>
    /// <returns></returns>
    public string GetString(string valueId)
    {
        string raw = GetRawValue(valueId);

        return raw.StartsWith('"') && raw.EndsWith('"') ? raw[1..^1] : raw;
    }

    /// <summary>
    /// Safely gets a string value
    /// </summary>
    /// <param name="valueId"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGetString(string valueId, out string value)
    {
        if (!TryGetRawValue(valueId, out string? raw))
        {
            value = "";
            return false;
        }

        value = raw!.StartsWith('"') && raw.EndsWith('"') ? raw[1..^1] : raw;
        return true;
    } 

    public static implicit operator string(Config config) => config.ID;

    public static ErrorHandlingStyle ErrorHandlingStyle = ErrorHandlingStyle.Print;

    /// <summary>
    /// This is called when an error happens if ErrorHandlingStyle is set to LoggerFunction
    /// </summary>
    public static Action<string>? ErrorLoggingFunction = null;

    public static Dictionary<string, Config> AllConfigs { get; set; } = [];

    private static void HandleError(string errorMessage)
    {
        switch (ErrorHandlingStyle)
        {
            default:
            case ErrorHandlingStyle.Exception:
                throw new Exception(errorMessage);

            case ErrorHandlingStyle.Print:
                Console.WriteLine(errorMessage);
                break;

            case ErrorHandlingStyle.LoggerFunction:
                if (ErrorLoggingFunction != null)
                {
                    throw new Exception($"No error logging function given!\n{errorMessage}");
                }
                ErrorLoggingFunction!.Invoke(errorMessage);
                break;
        }
    }

    /// <summary>
    /// Creates an empty configuration
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Config Create(string id)
    {
        Config config = new(id);
        AllConfigs.Add(id, config);

        return config;
    }

    /// <summary>
    /// Reads values from a file
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool GetValuesFromFile(string filePath, out Dictionary<string, string> values)
    {
        return GetValuesFromString(File.ReadAllText(filePath), out values);
    }

    /// <summary>
    /// Reads values from a string
    /// </summary>
    /// <param name="file"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool GetValuesFromString(string file, out Dictionary<string, string> values)
    {
        values = [];

        string[] lines = file.Split(["\n\r", "\n", "\r"], StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            //comment or empty line
            if (line.Trim().StartsWith('#') || line.All(char.IsWhiteSpace))
            {
                continue;
            }

            if (!line.Contains(':'))
            {
                HandleError($"Invalid line: '{line}' Every line must go like this: 'id: value'");
                return false;
            }

            string[] lineParts = line.Split(':', 2);

            if (lineParts.Length < 2)
            {
                HandleError($"Line '{line}' has too few parts!");
                return false;
            }

            //id, value
            values.TryAdd(lineParts[0], lineParts[1].Trim());
        }

        return true;
    }

    /// <summary>
    /// Creates a new configuration and loads its values from a file
    /// </summary>
    /// <param name="id"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static Config CreateFromFile(string id, string filePath)
    {
        Config config = new(id)
        {
            FilePath = filePath
        };
        
        config.LoadValues(filePath);

        AllConfigs.Add(id, config);

        return config;
    }

    /// <summary>
    /// Checks if a configuration exists
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool Exists(string id)
    {
        return AllConfigs.ContainsKey(id);
    }

    /// <summary>
    /// Returns a configuration 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool Get(string id, out Config? config)
    {
        if (!AllConfigs.TryGetValue(id, out config))
        {
            HandleError($"No config called '{id}' was found!");
            return false;
        }
        
        return true;
    }
}
