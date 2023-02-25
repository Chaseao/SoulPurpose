using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using static DialogueHelperClass;

public class InteractMartynSystem : SerializedMonoBehaviour
{
    [SerializeField] private SOConversationData dialogue;
    [SerializeField] private List<SOConversationData> conversationsToTrack;
    [SerializeField] private GameObject martynSignal;

    private Dictionary<ConversationData, bool> conversations;
    private bool shouldEnable = false;

    private void Awake()
    {
        martynSignal.SetActive(false);
        conversations = new Dictionary<ConversationData, bool>();
        foreach(var conversation in conversationsToTrack)
        {
            conversations.TryAdd(conversation.Data, false);
        }
    }

    private void OnEnable()
    {
        Controller.OnMartynInteract += InteractMartyn;
        DialogueManager.OnDialogueStarted += UpdateTracking;
        DialogueManager.OnDialogueEnded += ToggleSignal;
    }

    private void UpdateTracking(ConversationData conversation)
    {
        if (conversations.ContainsKey(conversation))
        {
            shouldEnable = !conversations[conversation];
            conversations[conversation] = true;
        }
    }

    private void ToggleSignal()
    {
        martynSignal.SetActive(shouldEnable);
    }

    private void OnDisable()
    {
        Controller.OnMartynInteract -= InteractMartyn;
    }

    void InteractMartyn()
    {
        martynSignal.SetActive(false);
        shouldEnable = false;
        print("Starting Martyn Dialogue");
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    private void OnDestroy()
    {
        Controller.OnMartynInteract -= InteractMartyn;
        DialogueManager.OnDialogueStarted -= UpdateTracking;
        DialogueManager.OnDialogueEnded -= ToggleSignal;
    }
}
