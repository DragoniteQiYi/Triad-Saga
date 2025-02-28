using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogues", menuName = "�Զ������/�Ի���", order = 1)]
public class DialogueResource : ScriptableObject
{
    public string Key;

    public List<Dialogue> Dialogues;

    // �������Ҫ�ݳ������������Ի�
    public bool IsSkippable;

    public float ContinueDelay = 2f;

    public async Task LoadDialogues()
    {
        Dialogues = await TextLoader.Instance.LoadDialoguesAsync(Key);
    }
}