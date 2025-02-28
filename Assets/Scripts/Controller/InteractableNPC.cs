using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNPC : MonoBehaviour
{
    [SerializeField] private List<DialogueCondition> _dialogueConditions;

    [Serializable]
    public class DialogueCondition
    {
        public List<VariableCondition> requiredConditions;
        public Dialogue dialogue;
    }

    public void CheckDialogue()
    {
        foreach (var condition in _dialogueConditions)
        {
            bool allMet = true;
            foreach (var c in condition.requiredConditions)
            {
                if (!c.IsConditionMet())
                {
                    allMet = false;
                    break;
                }
            }

            if (allMet)
            {
                ShowDialogue(condition.dialogue);
                return;
            }
        }
    }

    private void ShowDialogue(Dialogue dialogue)
    {
        // ÏÔÊ¾¶Ô»°Âß¼­
    }
}