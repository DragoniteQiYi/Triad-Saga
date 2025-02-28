using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public string Key { get; set; }

    public string Speaker { get; set; }

    public string Text { get; set; }

    public Sprite Image { get; set; } = null;

    public AudioClip Voice { get; set; } = null;

    public bool AutoContinue { get; set; } = false;

    public List<Choice> Choices = null;

    public string Symbol;

    [Header("�Ի�����")]
    public List<VariableCondition> Conditions = new();

    [Header("�Ի��¼�")]
    public List<DialogueEvent> Events = new();
}

[Serializable]
public class Choice
{
    public string Key;

    public int Next;
    public string Text;

    [Header("ѡ���������")]
    public List<VariableCondition> Conditions = new();

    [Header("ѡ����¼�")]
    public List<DialogueEvent> OnSelectEvents = new();
}