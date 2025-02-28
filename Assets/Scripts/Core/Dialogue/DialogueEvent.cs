using System;
using UnityEngine.Events;

[Serializable]
public class DialogueEvent
{
    public EventTriggerTiming Timing;
    public UnityEvent TargetEvent;

    public enum EventTriggerTiming
    {
        OnDialogueDisplay, // �Ի���ʾʱ
        OnDialogueEnd      // �Ի�����ʱ
    }
}