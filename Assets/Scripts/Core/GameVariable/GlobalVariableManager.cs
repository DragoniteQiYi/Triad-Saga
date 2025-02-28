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
    [ContextMenu("�Զ��ռ����б���")]
    private void CollectAllEvents()
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
    /// �����¼���ӳ�䣬���ڲ���
    /// </summary>
    private void UpdateVarDictionary()
    {
        _varDictionary.Clear();
        foreach (GameVariable e in _varList)
        {
            _varDictionary.Add(e.Key, e);
        }
    }

    public object GetVarValue(string eventName)
    {
        if (!_varDictionary.ContainsKey(eventName))
        {
            Debug.LogError($"δ֪�¼�������{eventName}");
            return null;
        }
        return _varDictionary[eventName].Value;
    }

    public void SetVarValue(string eventName, object value)
    {
        if (!_varDictionary.TryGetValue(eventName, out GameVariable e))
        {
            Debug.LogError($"{eventName}: �¼�����������");
        }
        if (CheckTypeMatch(e.Type, value))
        {
            e.Value = value;
            return;
        }
        Debug.LogError("�¼�����������������ֵ��ƥ��");
    }

    private bool CheckTypeMatch(VariableType type, object value)
    {
        return type switch
        {
            VariableType.Int => value is int,
            VariableType.Float => value is float,
            VariableType.Bool => value is bool,
            VariableType.String => value is string,
            _ => false
        };
    }
}