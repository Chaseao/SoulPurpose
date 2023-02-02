using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWindow : MonoBehaviour, IInteractable
{
    [SerializeField] private SOConversationData dialogue;
    public void ExecuteDialogue()
    {
        print("Starting BrokenWindow Dialogue");
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    public void OpenDoor()
    {
        //Should never activate this method
    }
}
