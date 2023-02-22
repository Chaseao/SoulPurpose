using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using static DialogueHelperClass;

public class DialogueManager : SingletonMonoBehavior<DialogueManager>
{
    public static Action<ConversationData> OnDialogueStarted;
    public static Action OnDialogueEnded;
    public static Action<string, bool> OnTextUpdated;
    public static Action<List<string>> OnChoiceMenuOpen;
    public static Action OnChoiceMenuClose;

    [SerializeField] AudioControls audioControls;
    [SerializeField] float dialogueSpeed;
    [SerializeField] float dialogueFastSpeed;
    [SerializeField] List<SOConversationData> conversationGroup;
    [SerializeField] public List<string> dialogueUnlocks;

    Dictionary<string, DialogueBranchData> choiceToPath = new Dictionary<string, DialogueBranchData>();

    float currentDialogueSpeed;
    bool inDialogue;
    bool continueInputRecieved;
    string choiceSelected;
    public bool InDialogue => inDialogue;

    private void Start()
    {
        SceneManager.activeSceneChanged += ClearManager;
    }


    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= ClearManager;
    }

    private void ClearManager(Scene arg0, Scene arg1)
    {
        audioControls = FindObjectOfType<AudioControls>();
        dialogueUnlocks.Clear();
    }

    [Button]
    public void StartDialogue(SOConversationData conversation)
    {
        StartDialogue(conversation.Data.ID);
    }

    public void StartDialogue(string dialogueId)
    {
        if(dialogueId == null || dialogueId.Equals("Exit"))
        {
            ExitDialogue();
            audioControls?.SetAudio(new int[6] { 40, 0, 0, 0, 0, 0}, true);
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

        AdjustAudio(data.EmotionsValue);

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

    private void AdjustAudio(int[] emotionsValue)
    {
        if (audioControls == null) return;

        audioControls.SetAudio(emotionsValue, false);
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
        branchData.Requirements.ForEach(x => Debug.Log(x));
        branchData.Requirements.Where(x => !dialogueUnlocks.Contains(x.ToLower())).ForEach(x => Debug.Log(x.ToString()));
        return branchData.Requirements.Find(x => !dialogueUnlocks.Contains(x.ToLower())) == null;
    }

    private void OnContinueInput() => continueInputRecieved = true;

    private IEnumerator ProcessDialogue(DialogueData dialogue, string conversant)
    {
        OnTextUpdated?.Invoke("", dialogue.WickIsSpeaker);
        yield return new WaitUntil(() => FadeToBlackSystem.FadeOutComplete);

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
        bool atSpecialCharacter = false;
        foreach(char letter in line)
        {
            loadedText += letter;
            atSpecialCharacter = letter == '<' || atSpecialCharacter;
            if (atSpecialCharacter && letter != '>') continue;
            atSpecialCharacter = false;
            OnTextUpdated?.Invoke(loadedText, isWickSpeaker);
            yield return new WaitForSeconds(1 / currentDialogueSpeed);
        }
        Controller.OnSelect -= SpeedUpText;
    }

    private void SpeedUpText() => currentDialogueSpeed = dialogueFastSpeed;
}
