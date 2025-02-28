#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using static GameVariable;

/*
 * ����ű�������Inspector�л��ƴ�������
 */
[CustomPropertyDrawer(typeof(VariableCondition))]
public class EventConditionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var eventProp = property.FindPropertyRelative("_targetEvent");
        var compProp = property.FindPropertyRelative("_comparison");
        var valueProp = property.FindPropertyRelative("_compareValue");

        Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        // �����¼�ѡ��
        EditorGUI.PropertyField(rect, eventProp);
        rect.y += EditorGUIUtility.singleLineHeight + 2;

        // ���ƱȽ�����
        EditorGUI.PropertyField(rect, compProp);
        rect.y += EditorGUIUtility.singleLineHeight + 2;

        // �����¼����ͻ��������ֶ�
        if (eventProp.objectReferenceValue is GameVariable gameEvent)
        {
            switch (gameEvent.Type)
            {
                case VariableType.Bool:
                    DrawBoolField(rect, valueProp);
                    break;
                case VariableType.Int:
                    DrawIntField(rect, valueProp);
                    break;
                case VariableType.Float:
                    DrawFloatField(rect, valueProp);
                    break;
                case VariableType.String:
                    DrawStringField(rect, valueProp);
                    break;
            }
        }
        else
        {
            EditorGUI.TextField(rect, "Value", "����ѡ���¼�");
        }

        EditorGUI.EndProperty();
    }

    private void DrawBoolField(Rect rect, SerializedProperty prop)
    {
        bool currentValue = bool.TryParse(prop.stringValue, out bool b) && b;
        bool newValue = EditorGUI.Toggle(rect, "Value", currentValue);
        prop.stringValue = newValue.ToString();
    }

    private void DrawIntField(Rect rect, SerializedProperty prop)
    {
        int currentValue = int.TryParse(prop.stringValue, out int i) ? i : 0;
        int newValue = EditorGUI.IntField(rect, "Value", currentValue);
        prop.stringValue = newValue.ToString();
    }

    private void DrawFloatField(Rect rect, SerializedProperty prop)
    {
        float currentValue = float.TryParse(prop.stringValue, out float f) ? f : 0f;
        float newValue = EditorGUI.FloatField(rect, "Value", currentValue);
        prop.stringValue = newValue.ToString();
    }

    private void DrawStringField(Rect rect, SerializedProperty prop)
    {
        prop.stringValue = EditorGUI.TextField(rect, "Value", prop.stringValue);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 3 + 6;
    }
}
#endif