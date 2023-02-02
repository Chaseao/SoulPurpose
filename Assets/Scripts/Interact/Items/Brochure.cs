using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brochure : MonoBehaviour, IInteractable
{
    [SerializeField] private SOConversationData dialogue;
    public void ExecuteDialogue()
    {
        print("Starting Brochure Dialogue");
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    public void OpenDoor()
    {
        //Should never activate this method
    }
}
