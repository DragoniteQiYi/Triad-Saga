using System;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    [SerializeField] private Language _currentLanguage = Language.zh_CN;

    public event Action<Language> OnLanguageChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 默认语言为简体中文
        InitializeLanguage(Language.zh_CN);
        OnLanguageChanged += InitializeLanguage;
    }

    private void InitializeLanguage(Language language)
    {
        _currentLanguage = language;
    }

    public Language GetCurrentLanguage()
    {
        return _currentLanguage;
    }

    public void SetLanguage(int index)
    {
        OnLanguageChanged.Invoke((Language)index);
    }

    /// <summary>
    /// 根据ID找到对应文本文件，获取其翻译
    /// </summary>
    /// <param name="strKey"></param>
    /// <returns></returns>
    public string GetTranslated(string strKey)
    {
        var request = Resources.LoadAsync<TextAsset>(strKey);

        return string.Empty;
    }
}

public enum Language
{
    zh_CN,
    zh_TW,
    en,
    ja,
    ko
}