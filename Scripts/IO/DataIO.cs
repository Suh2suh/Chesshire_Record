using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class DataIO
{
    public static string ReadData(string dataPath)
    {
        string loadedJson = "";
        using (StreamReader streamReader = new(dataPath))
        {
            loadedJson = streamReader.ReadToEnd();
        }

        return loadedJson;
    }
    public static T ReadDataObject<T>(string dataPath)
    {
        string loadedJson = ReadData(dataPath);
        T loadedDataObject = JsonConvert.DeserializeObject<T>(loadedJson);

        return loadedDataObject;
    }


    public static void WriteData(string dataContent, string dataPath)
    {
        CreateDirectoryIfMissing(dataPath);

        using (StreamWriter streamWriter = new(dataPath))
        {
            streamWriter.Write(dataContent);
        }
    }
    public static IEnumerator WriteDataAsync(string dataContent, string dataPath)
    {
        CreateDirectoryIfMissing(dataPath);

        using (StreamWriter streamWriter = new(dataPath))
        {
            yield return streamWriter.WriteAsync(dataContent);
        }
    }
    public static IEnumerator WriteDataObjectAsync(object dataObejct, string dataPath)
	{
        string savingJson = JsonConvert.SerializeObject(dataObejct);

        CreateDirectoryIfMissing(dataPath);

        using (StreamWriter streamWriter = new(dataPath))
		{
            yield return streamWriter.WriteAsync(savingJson);
        }
    }


    public static List<string> GetAllFilePathsInDirectory(string directory)
    {
        if (!Directory.Exists(directory))  return null;

        string[] filePaths = Directory.GetFiles(directory);
        List<string> filteredFilePaths = filePaths
                                                          .Where(file => Path.GetExtension(file) != ".meta")
                                                          .Select(file => file.Replace('\\', '/'))
                                                          .ToList<string>();

        return filteredFilePaths;
    }
    public static List<string> GetAllFileNamesInDirectory(string directory)
	{
        var filePaths = GetAllFilePathsInDirectory(directory);
        var fileNames = new List<string>();

        if (filePaths != null && filePaths.Count > 0)
        {
            foreach (var filePath in filePaths)
                fileNames.Add(Path.GetFileNameWithoutExtension(filePath));
        }

        return fileNames;
    }


    private static void CreateDirectoryIfMissing(string fullDataPath)
	{
        string directory = Path.GetDirectoryName(fullDataPath);
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
    }


}