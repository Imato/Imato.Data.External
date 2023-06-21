using Microsoft.Extensions.Logging;
using System.Text.Json;
using Imato.Reflection;

namespace Imato.Data.External
{
    public static class ConsoleOutput
    {
        public static LogLevel LogLevel { get; set; } = LogLevel.Error;

        public static void WriteCsv<T>(IEnumerable<T>? data,
            IEnumerable<string>? columnsList = null)
        {
            if (data == null)
            {
                Console.WriteLine("null");
                return;
            }
            if (!data.Any())
            {
                Console.WriteLine("Empty");
                return;
            }

            string? columns = null;
            var isDictionary = data.First() is IDictionary<string, object?>;
            foreach (var row in data)
            {
                var dic = isDictionary
                    ? row as IDictionary<string, object?>
                    : row.GetFields();

                if (dic == null)
                {
                    Console.WriteLine("Empty");
                    return;
                }

                if (columns == null)
                {
                    columns = string.Join(";", dic.Keys.Select(x => $"\"{x}\""));
                    Console.WriteLine(columns);
                }

                if (columnsList?.Count() > 0)
                {
                    foreach (var key in dic.Keys)
                    {
                        if (!columnsList.Contains(key))
                        {
                            dic.Remove(key);
                        }
                    }
                }

                Console.WriteLine(Strings.ToCsv(dic, false));
            }
        }

        public static void WriteCsv<T>(IEnumerable<T> data, LogLevel level)
        {
            if (level < LogLevel)
                return;

            WriteCsv(data);
        }

        public static void WriteJson(object data)
        {
            Console.WriteLine(JsonSerializer.Serialize(data, Constants.JsonOptions));
        }

        public static void WriteJson(object data, LogLevel level)
        {
            if (level < LogLevel)
                return;
            WriteJson(data);
        }

        public static void Log(string message, LogLevel level)
        {
            if (level < LogLevel)
                return;
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} {level}: {message}");
        }

        public static void Log(Exception ex, LogLevel level)
        {
            if (level < LogLevel)
                return;
            Console.WriteLine(ex.ToString());
        }

        public static void Log(object obj, LogLevel level)
        {
            if (level < LogLevel)
                return;
            WriteJson(obj);
        }

        public static void LogDebug(string message)
        {
            Log(message, LogLevel.Debug);
        }

        public static void LogDebug(object obj)
        {
            Log(obj, LogLevel.Debug);
        }

        public static void LogWarning(string message)
        {
            Log(message, LogLevel.Warning);
        }

        public static void LogWarning(object obj)
        {
            Log(obj, LogLevel.Warning);
        }

        public static void LogInformation(string message)
        {
            Log(message, LogLevel.Information);
        }

        public static void LogInformation(object obj)
        {
            Log(obj, LogLevel.Information);
        }

        public static void LogError(string message)
        {
            Log(message, LogLevel.Error);
        }

        public static void LogError(object obj)
        {
            Log(obj, LogLevel.Error);
        }

        public static void LogError(Exception obj)
        {
            Log(obj, LogLevel.Error);
        }
    }
}