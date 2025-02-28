using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanelController : MonoBehaviour
{
    // 配置参数
    [SerializeField] private float _typingSpeed = 0.05f;
    [SerializeField] private GameObject _choiceButtonPrefab;
    [SerializeField] private Dialogue _currentDialogue;

    // UI组件
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _speakerText;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private Image _portraitImage;
    [SerializeField] private Transform _choicesPanel;
    [SerializeField] private GameObject _continueIndicator;

    private Coroutine _typingCoroutine;

    [SerializeField] private Animator _panelAnimator;
    private static readonly int ShowHash = Animator.StringToHash("Show");
    private static readonly int HideHash = Animator.StringToHash("Hide");

    private void PlayShowAnimation()
    {
        _panelAnimator.SetTrigger(ShowHash);
    }

    private void PlayHideAnimation()
    {
        _panelAnimator.SetTrigger(HideHash);
    }

    // 使用对象池管理选项按钮
    private Queue<GameObject> _buttonPool = new();

    private GameObject GetChoiceButton()
    {
        return _buttonPool.Count > 0 ?
            _buttonPool.Dequeue() :
            Instantiate(_choiceButtonPrefab);
    }

    private void ReturnButton(GameObject button)
    {
        button.SetActive(false);
        _buttonPool.Enqueue(button);
    }

    private void OnEnable()
    {
        DialogueSystem.Instance.OnDialogueDisplay += HandleDialogueDisplay;
        DialogueSystem.Instance.OnChoicesPresented += ShowChoices;
        DialogueSystem.Instance.OnDialogueEnd += HidePanel;
    }

    private void OnDisable()
    {
        DialogueSystem.Instance.OnDialogueDisplay -= HandleDialogueDisplay;
        DialogueSystem.Instance.OnChoicesPresented -= ShowChoices;
        DialogueSystem.Instance.OnDialogueEnd -= HidePanel;
    }

    private void HandleDialogueDisplay(Dialogue dialogue)
    {
        SetupDialogueUI(dialogue);
        StartTypingEffect(dialogue.Text);
    }

    private void SetupDialogueUI(Dialogue dialogue)
    {
        _speakerText.text = dialogue.Speaker;
        _portraitImage.sprite = dialogue.Image;
        _continueIndicator.SetActive(false);
        ClearChoices();
    }

    private void StartTypingEffect(string text)
    {
        if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);
        _typingCoroutine = StartCoroutine(TypeText(text));
    }

    private IEnumerator TypeText(string text)
    {
        _dialogueText.text = "";
        foreach (char c in text)
        {
            _dialogueText.text += c;
            yield return new WaitForSeconds(_typingSpeed);
        }
        ShowContinueIndicator();
        DialogueSystem.Instance.SkipTyping();
    }

    private void ShowContinueIndicator()
    {
        _continueIndicator.SetActive(true);
    }

    private void ShowChoices(List<Choice> choices)
    {
        ClearChoices();
        for (int i = 0; i < choices.Count; i++)
        {
            CreateChoiceButton(choices[i], i);
        }
    }

    private void CreateChoiceButton(Choice choice, int index)
    {
        var button = Instantiate(_choiceButtonPrefab, _choicesPanel);
        button.GetComponentInChildren<Text>().text = choice.Text;
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            DialogueSystem.Instance.SelectChoice(index);
        });
    }

    private void ClearChoices()
    {
        foreach (Transform child in _choicesPanel)
        {
            Destroy(child.gameObject);
        }
    }

    private void HidePanel()
    {
        gameObject.SetActive(false);
    }

    // 输入检测（可在Update中添加）
    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
                _dialogueText.text = _currentDialogue.Text;
                ShowContinueIndicator();
                DialogueSystem.Instance.SkipTyping();
            }
            else
            {
                DialogueSystem.Instance.DisplayNextDialogue();
            }
        }
    }
}