using System;
using UnityEngine;
using static GameVariable;

[Serializable]
public class VariableCondition
{
    // 比较类型：等于/不等于，blabla
    public enum ComparisonType
    {
        Equal,
        NotEqual,
        GreaterThan,
        GreaterOrEqual,
        LessThan,
        LessOrEqual
    }

    [SerializeField] private GameVariable _targetVar;
    [SerializeField] private ComparisonType _comparison;
    [SerializeField] private string _compareValue;

    public bool IsConditionMet()
    {
        if (_targetVar == null) return false;

        object currentValue = GlobalVariableManager.Instance.GetVarValue(_targetVar.Key);
        object targetValue = ConvertValue(_targetVar.Type, _compareValue);

        return CompareValues(currentValue, targetValue, _comparison);
    }

    /// <summary>
    /// 用于转换游戏变量的值，便于比较
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private object ConvertValue(VariableType type, string value)
    {
        try
        {
            return type switch
            {
                VariableType.Int => int.Parse(value),
                VariableType.Float => float.Parse(value),
                VariableType.Bool => bool.Parse(value),
                VariableType.String => value,
                _ => null
            };
        }
        catch
        {
            Debug.LogError($"值转换失败: {value} -> {type}");
            return null;
        }
    }

    private bool CompareValues(object current, object target, ComparisonType comparison)
    {
        if (current == null || target == null) return false;

        // 处理不同数值类型的比较
        if (current is IConvertible c && target is IConvertible t)
        {
            try
            {
                decimal dc = Convert.ToDecimal(c);
                decimal dt = Convert.ToDecimal(t);

                return comparison switch
                {
                    ComparisonType.Equal => dc == dt,
                    ComparisonType.NotEqual => dc != dt,
                    ComparisonType.GreaterThan => dc > dt,
                    ComparisonType.GreaterOrEqual => dc >= dt,
                    ComparisonType.LessThan => dc < dt,
                    ComparisonType.LessOrEqual => dc <= dt,
                    _ => false
                };
            }
            catch { }
        }

        // 处理布尔值比较
        if (current is bool bCurrent && target is bool bTarget)
        {
            return comparison switch
            {
                ComparisonType.Equal => bCurrent == bTarget,
                ComparisonType.NotEqual => bCurrent != bTarget,
                _ => false
            };
        }

        // 处理字符串比较
        if (current is string sCurrent && target is string sTarget)
        {
            return comparison switch
            {
                ComparisonType.Equal => sCurrent == sTarget,
                ComparisonType.NotEqual => sCurrent != sTarget,
                _ => false
            };
        }

        return false;
    }
}