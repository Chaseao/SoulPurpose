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

    private void Start()
    {
        foreach(var conversation in conversationsToTrack)
        {
            conversations.TryAdd(conversation.Data, false);
        }
    }

    private void OnEnable()
    {
        Controller.OnMartynInteract += InteractMartyn;
        DialogueManager.OnDialogueStarted += UpdateTracking;
    }

    private void UpdateTracking(ConversationData conversation)
    {
        if (conversations.ContainsKey(conversation))
        {
            martynSignal.SetActive(!conversations[conversation]);
            conversations[conversation] = true;
        }
    }

    private void OnDisable()
    {
        Controller.OnMartynInteract -= InteractMartyn;
    }

    void InteractMartyn()
    {
        martynSignal.SetActive(false);
        print("Starting Martyn Dialogue");
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
