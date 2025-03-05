using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVLocalizationHelper
{
    /// <summary>
    /// ��ȡCSV�ļ�
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
    /// ���ݼ�ֵ��ȡ�ض����������ݹ淶��һ��ֻ��Ϊ����
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
        Debug.LogError($"CSVHelper��δ���ҵ�ָ�����������һ��CSV�ļ���({path})");
        return "";
    }

    /// <summary>
    /// ���ݼ�ֵ��ȡ�ض��������ض�����
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
                // CSV�ļ��Զ�����Ϊ�ָ���
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
                
                // �ҵ���Ӧ�У��������Ի�ȡ����
                if (parts[0].Trim('"') == key)
                {
                    int column = columnMap[language.ToString()];

                    return parts[column].Trim('"');
                }
            }
        }

        Debug.LogError($"CSVLocalizationHelper��δ���ҵ�ָ�����������һ��CSV�ļ���({path})");
        return "[null]";
    }
}
