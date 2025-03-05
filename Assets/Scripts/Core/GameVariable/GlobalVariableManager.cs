using System;
using System.Collections.Generic;
using UnityEngine;
using static GameVariable;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GlobalVariableManager : MonoBehaviour
{
    private static GlobalVariableManager _instance;
    public static GlobalVariableManager Instance => _instance;

    [SerializeField] private List<GameVariable> _varList = new();

    private Dictionary<string, GameVariable> _varDictionary = new();

    public event Action<GameVariable> OnValueChanged;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        UpdateVarDictionary();
    }

#if UNITY_EDITOR
    [ContextMenu("自动收集所有变量")]
    private void CollectAllVariables()
    {
        _varList.Clear();
        string[] guids = AssetDatabase.FindAssets("t:GameVariable");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameVariable e = AssetDatabase.LoadAssetAtPath<GameVariable>(path);
            if (e != null) _varList.Add(e);
        }
        EditorUtility.SetDirty(this);
    }
#endif

    /// <summary>
    /// 更新事件名映射，便于查找
    /// </summary>
    private void UpdateVarDictionary()
    {
        _varDictionary.Clear();
        foreach (GameVariable e in _varList)
        {
            _varDictionary.Add(e.Key, e);
        }
    }

    public object GetVarValue(string varKey)
    {
        if (!VarExists(varKey))
        {
            Debug.LogError($"未知游戏变量：{varKey}");
            return null;
        }
        return _varDictionary[varKey].Value;
    }

    public void SetVarValue(string varKey, object value)
    {
        if (!_varDictionary.TryGetValue(varKey, out GameVariable e))
        {
            Debug.LogError($"{varKey}: 游戏变量不存在");
            return;
        }
        if (!CheckTypeMatch(e.Type, value))
        {
            // 处理int到float的转换
            if (e.Type == VariableType.Float && value is int intVal)
                e.Value = (float)intVal;
            else
                e.Value = value;
            OnValueChanged?.Invoke(e);
            return;
        }
        Debug.LogError("游戏变量的类型与所给值不匹配");
    }

    public void AddValue(string varKey, object value)
    {
        if (!_varDictionary.TryGetValue(varKey, out GameVariable e))
        {
            Debug.LogError($"{varKey}: 游戏变量不存在");
            return;
        }
        if (!CheckTypeMatch(e.Type, value))
        {
            Debug.LogError("游戏变量的类型与所给值不匹配");
            return;
        }
        // 根据类型进行加法操作
        switch (e.Type)
        {
            case VariableType.Int:
                e.Value = (int)e.Value + (int)value;
                break;
            case VariableType.Float:
                e.Value = (float)e.Value + Convert.ToSingle(value);
                break;
            default:
                Debug.LogError("该类型不支持加法操作");
                break;
        }
        OnValueChanged?.Invoke(e); // 触发事件
    }

    private bool CheckTypeMatch(VariableType type, object value)
    {
        if (type == VariableType.Float && value is int)
        {
            return true; // 允许int转为float
        }
        return type switch
        {
            VariableType.Int => value is int,
            VariableType.Float => value is float,
            VariableType.Bool => value is bool,
            VariableType.String => value is string,
            _ => false
        };
    }

    private bool VarExists(string varName)
    {
        return _varDictionary.ContainsKey(varName);
    }
}