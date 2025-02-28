using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }

    // 事件定义
    public event Action OnDialogueStart;
    public event Action OnDialogueEnd;
    public event Action<Dialogue> OnDialogueDisplay;
    public event Action<List<Choice>> OnChoicesPresented;

    // 当前对话状态
    private Queue<Dialogue> _dialogueQueue = new();
    private Dialogue _currentDialogue;
    private bool _isTyping;

    // 自动播放间隔
    [SerializeField] private float autoContinueDelay = 2f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartDialogue(List<Dialogue> dialogues)
    {
        _dialogueQueue.Clear();
        foreach (var d in dialogues) _dialogueQueue.Enqueue(d);

        OnDialogueStart?.Invoke();
        DisplayNextDialogue();
    }

    public void DisplayNextDialogue()
    {
        if (_dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        _currentDialogue = _dialogueQueue.Dequeue();
        StartCoroutine(ProcessDialogue(_currentDialogue));
    }

    private IEnumerator ProcessDialogue(Dialogue dialogue)
    {
        // 触发对话显示前事件
        OnDialogueDisplay?.Invoke(dialogue);

        // 等待打字效果完成
        _isTyping = true;
        yield return new WaitUntil(() => !_isTyping);

        // 处理分支选项
        if (dialogue.Choices != null && dialogue.Choices.Count > 0)
        {
            OnChoicesPresented?.Invoke(GetValidChoices(dialogue.Choices));
        }
        else
        {
            // 自动继续配置
            if (dialogue.AutoContinue)
            {
                yield return new WaitForSeconds(autoContinueDelay);
                DisplayNextDialogue();
            }
        }
    }

    private List<Choice> GetValidChoices(List<Choice> choices)
    {
        // 使用之前的条件检查逻辑
        return choices.FindAll(c => CheckChoiceConditions(c));
    }

    public void SelectChoice(int choiceIndex)
    {
        var choice = _currentDialogue.Choices[choiceIndex];
        HandleChoiceEvents(choice);
        // TODO: 分支对话选择
    }

    public void SkipTyping()
    {
        _isTyping = false;
    }

    private void EndDialogue()
    {
        OnDialogueEnd?.Invoke();
    }

    private bool CheckChoiceConditions(Choice choice)
    {
        foreach (var condition in choice.Conditions)
        {
            if (!condition.IsConditionMet())
            {
                return false;
            }
        }
        return true;
    }

    private void HandleChoiceEvents(Choice choice)
    {
        foreach (var evt in choice.OnSelectEvents)
        {
            evt.TargetEvent?.Invoke();
        }
    }
}