using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVLocalizationHelper
{
    /// <summary>
    /// 读取CSV文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string[] ReadAllLines(string path)
    {
        using (StreamReader reader = new(path))
        {
            var lines = new List<string>();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }

            return lines.ToArray();
        }
    }

    /// <summary>
    /// 根据键值读取特定行数（根据规范第一列只能为键）
    /// </summary>
    /// <param name="path"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string ReadSingleLine(string path, string key)
    {
        using (StreamReader reader = new(path))
        {
            string line;
            while ((line = reader.ReadLine() )!= null)
            {
                if (line.Contains(key))
                {
                    return line;
                }
            }
        }
        Debug.LogError($"CSVHelper：未能找到指定行数，检查一下CSV文件？({path})");
        return "";
    }

    /// <summary>
    /// 根据键值读取特定行数的特定翻译
    /// </summary>
    /// <param name="path"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string ReadSingleLine(string path, string key, Language language)
    {
        using (StreamReader reader = new(path))
        {
            string line;
            Dictionary<string, int> columnMap = new();
            int index = 0;

            while ((line = reader.ReadLine()) != null)
            {
                // CSV文件以逗号作为分隔符
                string[] parts = line.Split(",");

                if (index == 0)
                {
                    for (int i = 0; i < parts.Length; i++)
                    {
                        columnMap.Add(parts[i].Trim('"'), i);
                    }
                    index++;
                    continue;
                }
                
                // 找到对应行，根据语言获取翻译
                if (parts[0].Trim('"') == key)
                {
                    int column = columnMap[language.ToString()];

                    return parts[column].Trim('"');
                }
            }
        }

        Debug.LogError($"CSVLocalizationHelper：未能找到指定行数，检查一下CSV文件？({path})");
        return "[null]";
    }
}
