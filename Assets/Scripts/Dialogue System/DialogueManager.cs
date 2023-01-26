using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueManager : SingletonMonoBehavior<DialogueManager>
{
    public static Action<ConversationData> OnDialogueStarted;
    public static Action OnDialogueEnded;
    public static Action<string, bool> OnTextUpdated;

    [Button]
    public void TESTCONVERSTION(SOConversationData conversationData) => JsonDialogueConverter.ConvertToJson(conversationData.Data);

    [SerializeField] float dialogueSpeed;
    [SerializeField] float dialogueFastSpeed;
    [SerializeField] List<SOConversationData> conversationGroup;
    [SerializeField] List<string> dialogueUnlocks;

    float currentDialogueSpeed;
    bool inDialogue;
    bool continueInputRecieved;
    public bool InDialogue => inDialogue;

    [Button]
    public void StartDialogue(DialogueSystemValidData.DIALOGUE_ID id)
    {
        StartDialogue(id.ToString());
    }

    public void StartDialogue(string dialogueId)
    {
        if(dialogueId == null || dialogueId.Equals("Exit"))
        {
            inDialogue = false;
            OnDialogueEnded?.Invoke();
            return;
        }

        var SOConversationData = conversationGroup.Find(data => data.Data.ID.ToLower().Equals(dialogueId.ToLower()));
        if (SOConversationData == null)
        {
            Debug.LogError("Could not find " + dialogueId + " in database");
            return;
        }

        StartCoroutine(HandleConversation(SOConversationData.Data));
    }

    private IEnumerator HandleConversation(ConversationData data)
    {
        OnDialogueStarted?.Invoke(data);

        foreach (var dialogue in data.Dialogues)
        {
            yield return ProcessDialogue(dialogue, data.Conversant);
        }

        HandleUnlock(data.Unlocks);
        int choiceSelection = HandleChoices(data.Choices);
        string nextDialogue = HandleLeadsTo(data.LeadsTo, choiceSelection);
        StartDialogue(nextDialogue);
    }

    private string HandleLeadsTo(List<DialogueBranchData> leadsTo, int choiceSelection)
    {
        if (choiceSelection != -1) return leadsTo[choiceSelection].BranchText;

        foreach(var route in leadsTo)
        {
            if(route.Requirements.Count == 0 || route.Requirements.Find(x => !dialogueUnlocks.Contains(x.ToLower())) == null)
            {
                return route.BranchText;
            }
        }

        return null;
    }

    private int HandleChoices(List<DialogueBranchData> choices)
    {
        if (choices.Count == 0) return -1;
        return 0;
    }

    private void HandleUnlock(string whatIsUnlocked)
    {
        if (!whatIsUnlocked.IsNullOrWhitespace() && !dialogueUnlocks.Contains(whatIsUnlocked.ToLower()))
        {
            dialogueUnlocks.Add(whatIsUnlocked.ToLower());
        }
    }

    private void OnContinueInput() => continueInputRecieved = true;

    private IEnumerator ProcessDialogue(DialogueData dialogue, string conversant)
    {
        continueInputRecieved = false;
        string name = (dialogue.WickIsSpeaker ? "Wick" : conversant) + ": ";

        yield return TypewriterDialogue(name, dialogue.Dialogue, dialogue.WickIsSpeaker);

        Controller.OnInteract += OnContinueInput;

        yield return new WaitUntil(() => continueInputRecieved);

        Controller.OnInteract -= OnContinueInput;
    }

    private IEnumerator TypewriterDialogue(string name, string line, bool isWickSpeaker)
    {
        currentDialogueSpeed = dialogueSpeed;
        string loadedText = name;
        Controller.OnInteract += SpeedUpText;
        foreach(char letter in line)
        {
            loadedText += letter;
            OnTextUpdated?.Invoke(loadedText, isWickSpeaker);
            yield return new WaitForSeconds(1 / currentDialogueSpeed);
        }
        Controller.OnInteract -= SpeedUpText;
    }

    private void SpeedUpText() => currentDialogueSpeed = dialogueFastSpeed;
}
