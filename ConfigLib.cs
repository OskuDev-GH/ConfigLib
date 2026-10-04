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

    public string? FilePath { get; set; }

    public Dictionary<string, string> Values { get; set; } = [];

    public static string[] TrueStrings = ["1", "true", "yes"];
    public static string[] FalseStrings = ["0", "false", "no"];

    public Config(string id)
    {
        ID = id;
    }

    public void LoadValues(string? filePath = null)
    {
        FilePath = filePath ?? FilePath;

        if (FilePath == null || !File.Exists(FilePath))
        {
            HandleError($"Couldn't reload values of '{ID}' from file!");
            return;
        }
        
        Values = GetValuesFromFile(FilePath, out Dictionary<string, string> newValues) ? newValues : Values;
    }

    public bool Contains(string id) => Values.ContainsKey(id);

    public void AddValue(string valueId, object rawValue)
    {
        string value = rawValue.ToString()!;

        if (!Values.TryAdd(valueId, value))
        {
            Values[valueId] = value;
        }
    }

    public string GetRawValue(string valueId)
    {
        if (!Values.TryGetValue(valueId, out string? value))
        {
            HandleError($"Value '{valueId}' was not found in config '{ID}'!");
            return "";
        }

        return value;
    }

    public bool TryGetRawValue(string valueId, out string? value, string defaultValue = "")
    {
        bool success = Values.TryGetValue(valueId, out value);
        value = success ? value : defaultValue;
        return success;
    } 

    public int GetInt(string valueId)
    {
        string raw = GetRawValue(valueId);

        if (!int.TryParse(raw, out int value))
        {
            HandleError($"Invalid int: {value}");
            return 0;
        }

        return value;
    }

    public bool TryGetInt(string valueId, out int value, int defaultValue = 0)
    {
        if (!TryGetRawValue(valueId, out string? raw))
        {
            value = defaultValue;
            return false;
        }

        bool success = int.TryParse(raw, out value);
        value = success ? value : defaultValue;
        return success;
    } 

    public float GetFloat(string valueId)
    {
        string raw = GetRawValue(valueId);

        if (!float.TryParse(raw, out float value))
        {
            HandleError($"Invalid float: {value}");
            return 0;
        }

        return value;
    }

    public bool TryGetFloat(string valueId, out float value, float defaultValue = 0)
    {
        if (!TryGetRawValue(valueId, out string? raw))
        {
            value = defaultValue;
            return false;
        }

        bool success = float.TryParse(raw, out value);
        value = success ? value : defaultValue;
        return success;
    }

    public double GetDouble(string valueId)
    {
        string raw = GetRawValue(valueId);

        if (!double.TryParse(raw, out double value))
        {
            HandleError($"Invalid double: {value}");
            return 0;
        }

        return value;
    }

    public bool TryGetDouble(string valueId, out double value, double defaultValue = 0)
    {
        if (!TryGetRawValue(valueId, out string? raw))
        {
            value = defaultValue;
            return false;
        }

        bool success = double.TryParse(raw, out value);
        value = success ? value : defaultValue;
        return success;
    } 

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

    public string GetString(string valueId)
    {
        string raw = GetRawValue(valueId);

        return raw.StartsWith('"') && raw.EndsWith('"') ? raw[1..^1] : raw;
    }

    public static implicit operator string(Config config) => config.ID;

    public static ErrorHandlingStyle ErrorHandlingStyle = ErrorHandlingStyle.Print;

    public static Action<string>? ErrorLoggingFunction = null;

    private static Dictionary<string, Config> _allConfigs = [];

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

    public static Config Create(string id)
    {
        Config config = new(id);
        _allConfigs.Add(id, config);

        return config;
    }

    public static bool GetValuesFromFile(string filePath, out Dictionary<string, string> values)
    {
        values = [];

        string[] lines = [.. File.ReadLines(filePath)];

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

    public static Config CreateFromFile(string id, string filePath)
    {
        Config config = new(id)
        {
            FilePath = filePath
        };
        
        config.LoadValues(filePath);

        _allConfigs.Add(id, config);

        return config;
    }

    public static Config? Get(string id)
    {
        if (!_allConfigs.TryGetValue(id, out Config? config))
        {
            HandleError($"No config called '{id}' was found!");
        }
        
        return config;
    }
}
