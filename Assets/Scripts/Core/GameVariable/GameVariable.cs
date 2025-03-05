using UnityEngine;

[CreateAssetMenu(fileName = "New Variable", menuName = "��Ϸ/��Ϸ����", order = 0)]
public class GameVariable : ScriptableObject
{
    public string Key;

    public string Description;

    public VariableType Type;

    public object Value;

    public enum VariableType
    {
        Int,
        Float,
        String,
        Bool
    }
}