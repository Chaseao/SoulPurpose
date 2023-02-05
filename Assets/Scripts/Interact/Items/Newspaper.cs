using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Newspaper : MonoBehaviour, IInteractable
{
    [SerializeField] private SOConversationData dialogue;
    public bool ExecuteDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
        return true;
    }

    public void OpenDoor()
    {
        //Should never activate this method
    }
}
