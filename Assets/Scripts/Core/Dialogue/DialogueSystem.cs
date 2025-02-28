using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }

    // �¼�����
    public event Action OnDialogueStart;
    public event Action OnDialogueEnd;
    public event Action<Dialogue> OnDialogueDisplay;
    public event Action<List<Choice>> OnChoicesPresented;

    // ��ǰ�Ի�״̬
    private Queue<Dialogue> _dialogueQueue = new();
    private Dialogue _currentDialogue;
    private bool _isTyping;

    // �Զ����ż��
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
        // �����Ի���ʾǰ�¼�
        OnDialogueDisplay?.Invoke(dialogue);

        // �ȴ�����Ч�����
        _isTyping = true;
        yield return new WaitUntil(() => !_isTyping);

        // �����֧ѡ��
        if (dialogue.Choices != null && dialogue.Choices.Count > 0)
        {
            OnChoicesPresented?.Invoke(GetValidChoices(dialogue.Choices));
        }
        else
        {
            // �Զ���������
            if (dialogue.AutoContinue)
            {
                yield return new WaitForSeconds(autoContinueDelay);
                DisplayNextDialogue();
            }
        }
    }

    private List<Choice> GetValidChoices(List<Choice> choices)
    {
        // ʹ��֮ǰ����������߼�
        return choices.FindAll(c => CheckChoiceConditions(c));
    }

    public void SelectChoice(int choiceIndex)
    {
        var choice = _currentDialogue.Choices[choiceIndex];
        HandleChoiceEvents(choice);
        // TODO: ��֧�Ի�ѡ��
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