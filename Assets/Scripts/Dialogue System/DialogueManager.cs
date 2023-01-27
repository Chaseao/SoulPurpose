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
    public static Action<List<string>> OnChoiceMenuOpen;
    public static Action OnChoiceMenuClose;

    [SerializeField] float dialogueSpeed;
    [SerializeField] float dialogueFastSpeed;
    [SerializeField] List<SOConversationData> conversationGroup;
    [SerializeField] List<string> dialogueUnlocks;
    [SerializeField, TextArea(10, 40)] string box;

    Dictionary<string, DialogueBranchData> choiceToPath = new Dictionary<string, DialogueBranchData>();

    float currentDialogueSpeed;
    bool inDialogue;
    bool continueInputRecieved;
    string choiceSelected;
    public bool InDialogue => inDialogue;

    [Button]
    public void TryToConvert()
    {
        JsonDialogueConverter.ConvertToJson(box);
    }

    [Button]
    public void StartDialogue(DialogueSystemValidData.DIALOGUE_ID id)
    {
        StartDialogue(id.ToString());
    }

    public void StartDialogue(string dialogueId)
    {
        if(dialogueId == null || dialogueId.Equals("Exit"))
        {
            ExitDialogue();
            return;
        }
        else if (!inDialogue)
        {
            inDialogue = true;
            Controller.Instance.SwapToUI();
        }

        var SOConversationData = conversationGroup.Find(data => data.Data.ID.ToLower().Equals(dialogueId.ToLower()));
        if (SOConversationData == null)
        {
            Debug.LogError("Could not find " + dialogueId + " in database");
            return;
        }

        StartCoroutine(HandleConversation(SOConversationData.Data));
    }

    private void ExitDialogue()
    {
        inDialogue = false;
        OnDialogueEnded?.Invoke();
        Controller.Instance.SwapToGameplay();
    }

    private IEnumerator HandleConversation(ConversationData data)
    {
        OnDialogueStarted?.Invoke(data);

        foreach (var dialogue in data.Dialogues)
        {
            yield return ProcessDialogue(dialogue, data.Conversant);
        }

        HandleUnlock(data.Unlocks);
        GenerateChoiceToPath(data);
        yield return HandleChoices();
        string nextDialogue = HandleLeadsTo(data.LeadsTo);
        StartDialogue(nextDialogue);
    }

    private string HandleLeadsTo(List<DialogueBranchData> leadsTo)
    {
        if (choiceToPath.Count != 0) return choiceToPath[choiceSelected].BranchText;

        foreach(var route in leadsTo)
        {
            if(route.Requirements.Count == 0 || CheckIfMeetsRequirements(route))
            {
                return route.BranchText;
            }
        }

        return null;
    }

    public void SelectChoice(string choice) => choiceSelected = choice;

    private IEnumerator HandleChoices()
    {
        choiceSelected = null;
        if (choiceToPath.Count == 0) yield break;

        OnChoiceMenuOpen?.Invoke(choiceToPath.Keys.ToList());
        yield return new WaitUntil(() => choiceSelected != null);
        OnChoiceMenuClose?.Invoke();
    }

    private void GenerateChoiceToPath(ConversationData conversation)
    {
        choiceToPath.Clear();
        for (int i = 0; i < conversation.Choices.Count; i++)
        {
            if (CheckIfMeetsRequirements(conversation.Choices[i]))
            {
                choiceToPath.Add(conversation.Choices[i].BranchText, conversation.LeadsTo[i]);
            }
        }
    }

    private void HandleUnlock(string whatIsUnlocked)
    {
        if (!whatIsUnlocked.IsNullOrWhitespace() && !dialogueUnlocks.Contains(whatIsUnlocked.ToLower()))
        {
            dialogueUnlocks.Add(whatIsUnlocked.ToLower());
        }
    }

    private bool CheckIfMeetsRequirements(DialogueBranchData branchData)
    {
        return branchData.Requirements.Find(x => !dialogueUnlocks.Contains(x.ToLower())) == null;
    }

    private void OnContinueInput() => continueInputRecieved = true;

    private IEnumerator ProcessDialogue(DialogueData dialogue, string conversant)
    {
        continueInputRecieved = false;
        string name = (dialogue.WickIsSpeaker ? "Wick" : conversant) + ": ";

        yield return TypewriterDialogue(name, dialogue.Dialogue, dialogue.WickIsSpeaker);

        Controller.OnSelect += OnContinueInput;

        yield return new WaitUntil(() => continueInputRecieved);

        Controller.OnSelect -= OnContinueInput;
    }

    private IEnumerator TypewriterDialogue(string name, string line, bool isWickSpeaker)
    {
        currentDialogueSpeed = dialogueSpeed;
        string loadedText = name;
        Controller.OnSelect += SpeedUpText;
        foreach(char letter in line)
        {
            loadedText += letter;
            OnTextUpdated?.Invoke(loadedText, isWickSpeaker);
            yield return new WaitForSeconds(1 / currentDialogueSpeed);
        }
        Controller.OnSelect -= SpeedUpText;
    }

    private void SpeedUpText() => currentDialogueSpeed = dialogueFastSpeed;
}
