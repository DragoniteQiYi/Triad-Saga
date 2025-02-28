using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class TextLoader : ResourceLoader
{
    private static readonly string DIALOGUE_PATH = "Text/Dialogues/";
    private static readonly string TERM_PATH = "Text/Terms/";

    private static readonly Dictionary<int, Language> languageMap = new()
    {
        {0, Language.zh_CN },
        {1, Language.zh_TW },
        {2, Language.en },
        {3, Language.ja },
        {4, Language.ko }
    };

    private static readonly Dictionary<string, string> textResourceMap = new()
    {
        {"DIA", "Text/Dialogues/" },
        {"CHAR", "Text/Terms/Characters.csv" },
        {"ITEM", "Text/Terms/Items.csv" },
        {"ATR", "Text/Terms/Attributes.csv" },
    };

    public static TextLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public async Task<List<Dialogue>> LoadDialoguesAsync(string key)
    {
        List<Dialogue> dialogues = new();
        string path = DIALOGUE_PATH + ParseDialogueKey(key);
        if (!File.Exists(path))
        {
            Debug.LogError("δ�ҵ�ָ���ļ���" + path + "!");
        }

        try
        {
            await Task.Run(() =>
            {
                string[] lines = ReadAllLines(path);
                Dictionary<int, string> columnMap = new();
                int index = 0;

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        index++;
                        continue;
                    }
                    string[] parts = line.Split(',');

                    // ��ȡ��һ��ʱ�����������ݼ��������ֵ��б��ڲ���
                    if (index == 0)
                    {
                        for (int i = 0; i < parts.Length; i++)
                        {
                            columnMap.Add(i, parts[i].Trim('"'));
                        }
                        continue;
                    }

                    // �������Ͷ�ȡ��Ӧ��Դ
                    Dialogue dialogue = new();
                    for (int i = 0; i < parts.Length; i++)
                    {
                        string type = columnMap[i];
                        switch (type)
                        {
                            case "dialogue_id":
                                dialogue.Key = parts[i];
                                break;
                            case "speaker_id":
                                dialogue.Speaker = LoadText(parts[i]);
                                break;
                            case "voice_id":
                                break;
                            default:
                                break;
                        }
                    }
                    dialogues.Add(dialogue);
                }
                return dialogues;
            });
        }
        catch (IOException ex)
        {
            Debug.LogError(ex);
        }
        return null;
    }

    private string[] ReadAllLines(string path)
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

    private string ParseDialogueKey(string key)
    {
        // �����е� "_" �滻Ϊ "/"
        string result = key.Replace('_', '/');

        // ���ַ���ĩβ��� ".csv"
        result += ".csv";

        return result;
    }
    
    private string LoadText(string key)
    {
        return "";
    }
}