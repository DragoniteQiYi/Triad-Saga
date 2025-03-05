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
            Debug.LogError("未找到指定文件：" + path + "!");
        }

        try
        {
            await Task.Run(() =>
            {
                string[] lines = CSVLocalizationHelper.ReadAllLines(path);
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

                    // 读取第一行时，将所有数据键储存在字典中便于查找
                    if (index == 0)
                    {
                        for (int i = 0; i < parts.Length; i++)
                        {
                            columnMap.Add(i, parts[i].Trim('"'));
                        }
                        index++;
                        continue;
                    }

                    // TODO: 根据类型读取相应资源
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

    private string ParseDialogueKey(string key)
    {
        // 将所有的 "_" 替换为 "/"
        string result = key.Replace('_', '/');

        // 在字符串末尾添加 ".csv"
        result += ".csv";

        return result;
    }
    
    /// <summary>
    /// 用于读取用语、术语、名词类文本的本地化翻译
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private string LoadText(string key)
    {
        string type = key[..key.IndexOf('_')];
        string target = key[(key.IndexOf("_") + 1)..];
        
        if (!textResourceMap.ContainsKey(type))
        {
            Debug.LogError("未知资源类型：" + type);
            return null;
        }
        string path = textResourceMap[type];

        return CSVLocalizationHelper.ReadSingleLine(path, key, 
            LocalizationManager.Instance.GetCurrentLanguage());
    }
}