using System;
using UnityEngine;

public class DialogueUIController : MonoBehaviour
{
    [SerializeField] PortraitDisplay leftPortrait, rightPortrait;
    [SerializeField] TextBoxDisplay textBoxDisplay;

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
        DialogueManager.OnTextUpdated -= textBoxDisplay.UpdateDialogueText;
    }

    private void DisplayUI(ConversationData conversation)
    {
        leftPortrait.Display(conversation.Conversant);
        rightPortrait.Display("wick");
        textBoxDisplay.Display();
        DialogueManager.OnTextUpdated += textBoxDisplay.UpdateDialogueText;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueStarted -= DisplayUI;
        DialogueManager.OnDialogueEnded -= HideUI;
    }
}