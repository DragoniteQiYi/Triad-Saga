using UnityEngine;

[CreateAssetMenu(fileName = "New Variable", menuName = "游戏/游戏变量", order = 0)]
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