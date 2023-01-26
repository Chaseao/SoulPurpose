using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUIController : MonoBehaviour
{
    [SerializeField] PortraitDisplay leftPortrait, rightPortrait;
    [SerializeField] TextBoxDisplay textBoxDisplay;
    [SerializeField] ChoicesDisplay choicesDisplay;

    List<string> choices = new List<string>();
    int currentChoice;
    int totalChoices;

    private void OnEnable()
    {
        DialogueManager.OnDialogueStarted += DisplayUI;
        DialogueManager.OnDialogueEnded += HideUI;
        DialogueManager.OnChoiceMenuOpen += OnChoiceMenuStart;
        DialogueManager.OnChoiceMenuClose += OnChoiceMenuEnd;
        HideUI();
    }

    private void HideUI()
    {
        leftPortrait.Hide();
        rightPortrait.Hide();
        textBoxDisplay.Hide();
        choicesDisplay.Hide();
        DialogueManager.OnTextUpdated -= textBoxDisplay.UpdateDialogueText;
    }

    private void DisplayUI(ConversationData conversation)
    {
        leftPortrait.Display(conversation.Conversant);
        rightPortrait.Display("wick");
        textBoxDisplay.Display();
        DialogueManager.OnTextUpdated += textBoxDisplay.UpdateDialogueText;
    }

    private void UpdateChoiceSelection(Vector2 navigation)
    {
        currentChoice += Mathf.RoundToInt(navigation.normalized.x);
        currentChoice = Mathf.Clamp(currentChoice, 0, totalChoices - 1);
        choicesDisplay.SelectChoice(currentChoice);
    }

    private void SelectChoice()
    {
        DialogueManager.Instance.SelectChoice(choices[currentChoice]);
    }

    private void OnChoiceMenuStart(List<string> validChoices)
    {
        choicesDisplay.Display(validChoices);
        choices = validChoices;
        totalChoices = validChoices.Count;
        Controller.OnSelect += SelectChoice;
        Controller.OnNavigateMenu += UpdateChoiceSelection;
    }

    private void OnChoiceMenuEnd()
    {
        choicesDisplay.Hide();
        Controller.OnSelect -= SelectChoice;
        Controller.OnNavigateMenu -= UpdateChoiceSelection;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueStarted -= DisplayUI;
        DialogueManager.OnDialogueEnded -= HideUI;
        DialogueManager.OnChoiceMenuOpen -= OnChoiceMenuStart;
        DialogueManager.OnChoiceMenuClose -= OnChoiceMenuEnd;
    }
}