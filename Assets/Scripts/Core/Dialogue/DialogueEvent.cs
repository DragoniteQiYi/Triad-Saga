using System;
using UnityEngine.Events;

[Serializable]
public class DialogueEvent
{
    public EventTriggerTiming Timing;
    public UnityEvent TargetEvent;

    public enum EventTriggerTiming
    {
        OnDialogueDisplay, // 对话显示时
        OnDialogueEnd      // 对话结束时
    }
}