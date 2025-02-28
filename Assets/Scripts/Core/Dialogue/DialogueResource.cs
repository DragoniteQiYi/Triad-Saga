using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogues", menuName = "自定义对象/对话组", order = 1)]
public class DialogueResource : ScriptableObject
{
    public string Key;

    public List<Dialogue> Dialogues;

    // 如果是重要演出，不可跳过对话
    public bool IsSkippable;

    public float ContinueDelay = 2f;

    public async Task LoadDialogues()
    {
        Dialogues = await TextLoader.Instance.LoadDialoguesAsync(Key);
    }
}