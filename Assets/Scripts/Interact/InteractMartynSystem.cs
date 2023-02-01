using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMartynSystem : MonoBehaviour
{
    [SerializeField] private SOConversationData dialogue;

    private void OnEnable()
    {
        Controller.OnMartynInteract += InteractMartyn;
    }

    private void OnDisable()
    {
        Controller.OnMartynInteract -= InteractMartyn;
    }

    void InteractMartyn()
    {
        print("Starting Martyn Dialogue");
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
