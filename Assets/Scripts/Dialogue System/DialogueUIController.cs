using System;
using UnityEngine;

public class DialogueUIController : MonoBehaviour
{
    [SerializeField] PortraitDisplay leftPortrait, rightPortrait;
    [SerializeField] TextBoxDisplay textBoxDisplay;
    [SerializeField] ChoicesDisplay choicesDisplay;

    private void OnEnable()
    {
        DialogueManager.OnDialogueStarted += DisplayUI;
        DialogueManager.OnDialogueEnded += HideUI;
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
        choicesDisplay.Display(conversation);
        DialogueManager.OnTextUpdated += textBoxDisplay.UpdateDialogueText;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueStarted -= DisplayUI;
        DialogueManager.OnDialogueEnded -= HideUI;
    }
}