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

    public object GetVarValue(string varKey)
    {
        if (!VarExists(varKey))
        {
            Debug.LogError($"δ֪��Ϸ������{varKey}");
            return null;
        }
        return _varDictionary[varKey].Value;
    }

    public void SetVarValue(string varKey, object value)
    {
        if (!_varDictionary.TryGetValue(varKey, out GameVariable e))
        {
            Debug.LogError($"{varKey}: ��Ϸ����������");
            return;
        }
        if (!CheckTypeMatch(e.Type, value))
        {
            // ����int��float��ת��
            if (e.Type == VariableType.Float && value is int intVal)
                e.Value = (float)intVal;
            else
                e.Value = value;
            OnValueChanged?.Invoke(e);
            return;
        }
        Debug.LogError("��Ϸ����������������ֵ��ƥ��");
    }

    public void AddValue(string varKey, object value)
    {
        if (!_varDictionary.TryGetValue(varKey, out GameVariable e))
        {
            Debug.LogError($"{varKey}: ��Ϸ����������");
            return;
        }
        if (!CheckTypeMatch(e.Type, value))
        {
            Debug.LogError("��Ϸ����������������ֵ��ƥ��");
            return;
        }
        // �������ͽ��мӷ�����
        switch (e.Type)
        {
            case VariableType.Int:
                e.Value = (int)e.Value + (int)value;
                break;
            case VariableType.Float:
                e.Value = (float)e.Value + Convert.ToSingle(value);
                break;
            default:
                Debug.LogError("�����Ͳ�֧�ּӷ�����");
                break;
        }
        OnValueChanged?.Invoke(e); // �����¼�
    }

    private bool CheckTypeMatch(VariableType type, object value)
    {
        if (type == VariableType.Float && value is int)
        {
            return true; // ����intתΪfloat
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