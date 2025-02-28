using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public abstract class ResourceLoader : MonoBehaviour
{
    // 允许通过字典查找资源路径（如：CHAR_Kobe->Resources/Text/Characters）
    protected static readonly Dictionary<string, string> resourcesPath = new()
    {
        { "TERM", "Text/Terms/" },
        { "DIA", "Text/Dialogues/" },
        { "POT", "Images/Portraits/" },
        { "VO", "Audios/Voices/" },
        { "BGM", "Audios/BGM/" },
        { "BGS", "Audios/BGS/" },
        { "SE", "Audios/SE/" },
        { "ME", "Audios/ME/" },
    };

    /// <summary>
    /// 读取文件
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<string> LoadFile(string path)
    {
        string text = await File.ReadAllTextAsync(path);
        return text;
    }
}