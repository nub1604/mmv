using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class JsonFileLoader
{
    public static List<T> LoadAllJsonFiles<T>(string folderPath, JsonSerializerOptions? options = null)
    {
        List<T> objects = new List<T>();

        if (!Directory.Exists(folderPath))
        {
            throw new DirectoryNotFoundException($"The folder '{folderPath}' was not found.");
        }

        foreach (string filePath in Directory.GetFiles(folderPath, "*.json"))
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath);
                T? obj = JsonSerializer.Deserialize<T>(jsonContent, options);
                if (obj != null)
                {
                    objects.Add(obj);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading {filePath}: {ex.Message}");
            }
        }

        return objects;
    }
}
